using System;
using System.Collections.Generic;
using UnityEngine;

public class GunFuExecute_OnGround_Single_NodeLeaf : PlayerStateNodeLeaf,IGunFuExecuteNodeLeaf
{

    public IWeaponAdvanceUser weaponAdvanceUser => player;
    public IGunFuAble gunFuAble { get => base.player; set { } }
    public IGotGunFuAttackedAble gotGunFuAttackedAble { get => gunFuAble.attackedAbleGunFu; set { } }
    public string _stateName => gunFuExecute_OnGround_Single_ScriptableObject.gunFuStateName;
    public GunFuExecuteScriptableObject _gunFuExecuteScriptableObject => this.gunFuExecute_OnGround_Single_ScriptableObject;
    public GunFuExecute_OnGround_Single_ScriptableObject gunFuExecute_OnGround_Single_ScriptableObject { get; protected set; }
    private Dictionary<float, bool> isShootAlready = new Dictionary<float, bool>();
    private List<float> shootimingNormalized => gunFuExecute_OnGround_Single_ScriptableObject.firingTimingNormalized;
    public AnimationClip _animationClip { get => gunFuExecute_OnGround_Single_ScriptableObject.executeClip; set { } }

    private float warpingNormalized => gunFuExecute_OnGround_Single_ScriptableObject.warpingPhaseTimeNormalized;
    private Transform gunFuAttackerTransform => player.transform;
    private Transform gunFuGotAttackedTransform => player.executedAbleGunFu._character.transform;

    private Vector3 gunFuAttackerEnterPosition;
    private Quaternion gunFuAttackerEnterRotation;

    private Vector3 gunFuAttackerTargetPosition;
    private Quaternion gunFuAttackerTargetRotation;

    public enum GunFuExecuteSinglePhase
    {
        Warping,
        Interacting,
        Execute,
    }
    public GunFuExecuteSinglePhase curGunFuPhase { get; protected set; }
    public float _timer { get; set; }
    bool IGunFuExecuteNodeLeaf._isExecuteAldready { get => isExecuteAlready; set => isExecuteAlready = value; }
    private bool isExecuteAlready;

    public GunFuExecute_OnGround_Single_NodeLeaf(Player player, Func<bool> preCondition, GunFuExecute_OnGround_Single_ScriptableObject gunFuExecute_Single_ScriptableObject) : base(player, preCondition)
    {
        this.gunFuExecute_OnGround_Single_ScriptableObject = gunFuExecute_Single_ScriptableObject;
        PopulateIsShootAlready();
    }

    public override void Enter()
    {

        this._timer = _animationClip.length * gunFuExecute_OnGround_Single_ScriptableObject.executeAnimationOffset;
        curGunFuPhase = GunFuExecuteSinglePhase.Warping;
        gunFuAble.executedAbleGunFu.TakeGunFuAttacked(this, gunFuAble);
        CalculateAdjustTransform();
        (player._movementCompoent as MovementCompoent).CancleMomentum();

        base.Enter();
    }

    public override void Exit()
    {
        isExecuteAlready = false;
        gunFuAble._character.enableRootMotion = false;
        isTriggerSlowMotion = false;
        ResetIsShootAlready();
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }
    public override bool IsComplete()
    {
        if (_timer > _animationClip.length)
            return true;
        return false;
    }

    public override bool IsReset()
    {
        if (this.IsComplete())
            return true;
        return false;
    }
    private bool isTriggerSlowMotion;
    public override void UpdateNode()
    {
        _timer += Time.deltaTime;
        switch (curGunFuPhase)
        {
            case GunFuExecuteSinglePhase.Warping:
                {
                    if (WarpingComplete())
                    {
                        curGunFuPhase = GunFuExecuteSinglePhase.Interacting;
                        gunFuAble._character.enableRootMotion = true;

                    }
                    break;
                }
            case GunFuExecuteSinglePhase.Interacting:
                {
                    if (isTriggerSlowMotion == false
                        && _timer >= gunFuExecute_OnGround_Single_ScriptableObject.slowMotionTriggerNormailzed * _animationClip.length)
                    {
                        TimeControlBehavior.TriggerTimeStop(0, 1.2f);
                        isTriggerSlowMotion = true;
                    }

                    ShootingCheck();
                    ExecuteCheck();
                    break;
                }
        }

        base.UpdateNode();
    }
    private void ShootingCheck()
    {
        if (shootimingNormalized != null && shootimingNormalized.Count > 0)
            foreach (float shootimingNormal in shootimingNormalized)
            {
                if (_timer >= _animationClip.length * shootimingNormal
                    && isShootAlready[shootimingNormal] == false)
                {
                    isShootAlready[shootimingNormal] = true;
                    WeaponShootBlank.ShootBlank(weaponAdvanceUser._currentWeapon);
                }
            }


    }
    private void ExecuteCheck()
    {
        if (_timer >= _animationClip.length * gunFuExecute_OnGround_Single_ScriptableObject.executeTimeNormalized
           && isExecuteAlready == false)
        {
            curGunFuPhase = GunFuExecuteSinglePhase.Execute;
            BulletExecute bulletExecute = new BulletExecute(weaponAdvanceUser._currentWeapon);
            weaponAdvanceUser._currentWeapon.PullTrigger();
            gunFuAble.executedAbleGunFu._damageAble.TakeDamage(bulletExecute);
            isExecuteAlready = true;
            player.NotifyObserver(player, this);
        }
    }
    private bool WarpingComplete()
    {
        float lenghtOffset = _animationClip.length * gunFuExecute_OnGround_Single_ScriptableObject.executeAnimationOffset;

        float t = (_timer - lenghtOffset) / ((_animationClip.length - lenghtOffset) * warpingNormalized);

        t = Mathf.Clamp01(t);


        MovementWarper.WarpMovement
            (gunFuAttackerEnterPosition
            , gunFuAttackerEnterRotation
            , gunFuAble._character._movementCompoent
            , gunFuAttackerTargetPosition
            , gunFuAttackerTargetRotation
            , t);

        if (t >= 1)
            return true;

        return false;

    }
    private void PopulateIsShootAlready()
    {
        if(shootimingNormalized == null)
            return;

        if (shootimingNormalized.Count > 0)
            foreach (float shootimingNor in shootimingNormalized)
            {
                isShootAlready.Add(shootimingNor, false);
            }
    }
    private void ResetIsShootAlready()
    {
        if (shootimingNormalized != null && shootimingNormalized.Count > 0)
            foreach (float shootimingNor in shootimingNormalized)
            {
                isShootAlready[shootimingNor] = false;
            }
    }
    private void CalculateAdjustTransform()
    {
        Vector3 anchorPos = gunFuGotAttackedTransform.position;
        Vector3 anChorDir;

        if (gotGunFuAttackedAble is IFallDownGetUpAble fallDownGetUpAble)
        {
            anChorDir = new Vector3(fallDownGetUpAble._hipsBone.up.x, 0, fallDownGetUpAble._hipsBone.up.z);
        } else
            anChorDir = Vector3.zero;


        gunFuAttackerEnterPosition = gunFuAttackerTransform.position;
        gunFuAttackerEnterRotation = gunFuAttackerTransform.rotation;

        gunFuAttackerTargetPosition
            = anchorPos
            + (anChorDir * gunFuExecute_OnGround_Single_ScriptableObject.playerForwardRelativePosition)
            + (Vector3.Cross(anChorDir, Vector3.down) * gunFuExecute_OnGround_Single_ScriptableObject.playerRightwardRelativePosition);

        gunFuAttackerTargetRotation
            = Quaternion.LookRotation(anChorDir, Vector3.up) * Quaternion.Euler(0, gunFuExecute_OnGround_Single_ScriptableObject.playerRotationRelative, 0);

       
    }
}

