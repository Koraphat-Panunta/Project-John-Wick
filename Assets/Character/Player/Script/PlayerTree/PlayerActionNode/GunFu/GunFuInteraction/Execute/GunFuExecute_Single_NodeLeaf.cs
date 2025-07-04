using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class GunFuExecute_Single_NodeLeaf : PlayerStateNodeLeaf, IGunFuExecuteNodeLeaf
{
    public IWeaponAdvanceUser weaponAdvanceUser;
    public IGunFuAble gunFuAble { get; set; }
    string IGunFuExecuteNodeLeaf._stateName { get => this.stateName ; }
    public IGotGunFuAttackedAble attackedAbleGunFu { get => gunFuAble.attackedAbleGunFu; set { } }
    public string stateName => gunFuExecute_Single_ScriptableObject.gunFuStateName;
    public GunFuExecute_Single_ScriptableObject _gunFuExecute_Single_ScriptableObject => gunFuExecute_Single_ScriptableObject;
    public GunFuExecute_Single_ScriptableObject gunFuExecute_Single_ScriptableObject { get; protected set; }
    private Dictionary<float,bool> isShootAlready = new Dictionary<float,bool>();
    private List<float> shootimingNormalized => gunFuExecute_Single_ScriptableObject.firingTimingNormalized;
    public AnimationClip _animationClip { get => gunFuExecute_Single_ScriptableObject.executeClip; set { } }
  
    private float warpingNormalized => gunFuExecute_Single_ScriptableObject.warpingPhaseTimeNormalized;
    private Transform gunFuAttackerTransform => player.transform;
    private Transform gunFuGotAttackedTransform => player.executedAbleGunFu._character.transform;

    private Vector3 gunFuAttackerEnterPosition;
    private Quaternion gunFuAttackerEnterRotation;

    private Vector3 opponentGunFuEnterPosition;
    private Quaternion opponentGunFuEnterRotation;

    private Vector3 gunFuAttackerTargetPosition;
    private Quaternion gunFuAttackerTargetRotation;

    private Vector3 opponentGunFuTargetPosition;
    private Quaternion opponentGunFuTargetRotation;
    public enum GunFuExecuteSinglePhase 
    {
        Warping,
        Interacting
    }
    public GunFuExecuteSinglePhase curGunFuPhase { get;protected set; }
    public float _timer { get ; set ; }
    bool IGunFuExecuteNodeLeaf._isExecuteAldready { get => isExecuteAlready; set => isExecuteAlready = value; }
    private bool isExecuteAlready;
    public GunFuExecute_Single_NodeLeaf(Player player, Func<bool> preCondition,GunFuExecute_Single_ScriptableObject gunFuExecute_Single_ScriptableObject) : base(player, preCondition)
    {
        this.gunFuAble = player;
        this.gunFuExecute_Single_ScriptableObject = gunFuExecute_Single_ScriptableObject;
        weaponAdvanceUser = player;
        PopulateIsShootAlready();
    }



    public override void Enter()
    {
        this._timer = _animationClip.length * gunFuExecute_Single_ScriptableObject.playerAnimationOffset;
        curGunFuPhase = GunFuExecuteSinglePhase.Warping;
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
        if(_timer > _animationClip.length)
            return true;
        return false;
    }

    public override bool IsReset()
    {
        if(this.IsComplete())
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
                        gunFuAble.executedAbleGunFu.TakeGunFuAttacked(this, gunFuAble);
                        curGunFuPhase = GunFuExecuteSinglePhase.Interacting;
                        gunFuAble._character.enableRootMotion = true;

                    }
                    break;
                }
            case GunFuExecuteSinglePhase.Interacting:
                {
                    if(isTriggerSlowMotion == false
                        &&_timer >= gunFuExecute_Single_ScriptableObject.slowMotionTriggerNormailzed * _animationClip.length)
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
        if(shootimingNormalized.Count>0)
        foreach(float shootimingNormal in shootimingNormalized)
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
        if (_timer >= _animationClip.length * gunFuExecute_Single_ScriptableObject.executeTimeNormalized
           && isExecuteAlready == false)
        {
            BulletExecute bulletExecute = new BulletExecute(weaponAdvanceUser._currentWeapon);
            weaponAdvanceUser._currentWeapon.PullTrigger();
            gunFuAble.executedAbleGunFu._damageAble.TakeDamage(bulletExecute);
            isExecuteAlready = true;
        }
    }
    private bool WarpingComplete()
    {
        float lenghtOffset = _animationClip.length * gunFuExecute_Single_ScriptableObject.playerAnimationOffset;

        float t = (_timer - lenghtOffset) / ((_animationClip.length - lenghtOffset)*warpingNormalized);

        t = Mathf.Clamp01(t);


        TransformWarper.WarpTransform
            (gunFuAttackerEnterPosition
            , gunFuAttackerEnterRotation
            , gunFuAttackerTransform
            , gunFuAttackerTargetPosition
            , gunFuAttackerTargetRotation
            , t);

        TransformWarper.WarpTransform
            (opponentGunFuEnterPosition
            , opponentGunFuEnterRotation
            , gunFuGotAttackedTransform
            , opponentGunFuTargetPosition
            , opponentGunFuTargetRotation
            , t);

        if (t >= 1)
            return true;

        return false;

    }
    private void PopulateIsShootAlready()
    {
        if(shootimingNormalized.Count >0)
        foreach(float shootimingNor in shootimingNormalized)
        {
            isShootAlready.Add(shootimingNor, false);
        }
    }
    private void ResetIsShootAlready()
    {
        if(shootimingNormalized.Count >0)
        foreach (float shootimingNor in shootimingNormalized)
        {
            isShootAlready[shootimingNor] = false;
        }
    }
    private void CalculateAdjustTransform()
    {
        Vector3 enterDir = (gunFuGotAttackedTransform.position - gunFuAttackerTransform.position).normalized;

        gunFuAttackerEnterPosition = gunFuAttackerTransform.position;
        gunFuAttackerEnterRotation = gunFuAttackerTransform.rotation;
        opponentGunFuEnterPosition = gunFuGotAttackedTransform.position;
        opponentGunFuEnterRotation = gunFuGotAttackedTransform.rotation;

        gunFuAttackerTargetPosition
            = gunFuAttackerTransform.position
            + (enterDir * gunFuExecute_Single_ScriptableObject.playerForwardRelativePosition)
            + (Vector3.Cross(enterDir,Vector3.down) * gunFuExecute_Single_ScriptableObject.playerRightwardRelativePosition);

        gunFuAttackerTargetRotation
            = Quaternion.LookRotation(enterDir,Vector3.up) * Quaternion.Euler(0,gunFuExecute_Single_ScriptableObject.playerRotationRelative,0);

        opponentGunFuTargetPosition
            = gunFuAttackerTransform.position
            + (enterDir * gunFuExecute_Single_ScriptableObject.opponentForwardRelative)
            + (Vector3.Cross(enterDir,Vector3.down) * gunFuExecute_Single_ScriptableObject.opponentRightwardRelative);

        opponentGunFuTargetRotation
            = Quaternion.LookRotation(enterDir, Vector3.up) * Quaternion.Euler(0, gunFuExecute_Single_ScriptableObject.opponentRotationRelative, 0);
    }
}
