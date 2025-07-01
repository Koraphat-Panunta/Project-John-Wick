using System;
using System.Collections.Generic;
using UnityEngine;

public class GunFuExecute_Single_NodeLeaf : PlayerStateNodeLeaf, IGunFuExecuteNodeLeaf
{
    public IWeaponAdvanceUser weaponAdvanceUser;
    public IGunFuAble gunFuAble { get; set; }
    string IGunFuExecuteNodeLeaf._stateName { get => this.stateName ; }
    public IGotGunFuAttackedAble attackedAbleGunFu { get; set; }
    public string stateName => gunFuExecute_Single_ScriptableObject.gunFuStateName;
    public GunFuExecute_Single_ScriptableObject _gunFuExecute_Single_ScriptableObject => gunFuExecute_Single_ScriptableObject;
    public GunFuExecute_Single_ScriptableObject gunFuExecute_Single_ScriptableObject { get; protected set; }
    private Dictionary<float,bool> isShootAlready = new Dictionary<float,bool>();
    private List<float> shootimingNormalized => gunFuExecute_Single_ScriptableObject.firingTimingNormalized;
    public AnimationClip _animationClip { get => gunFuExecute_Single_ScriptableObject.executeClip; set { } }
  
    private float warpingNormalized => gunFuExecute_Single_ScriptableObject.warpingPhaseTimeNormalized;
    private Transform gunFuAttackerTransform => player.transform;
    private Transform gunFuGotAttackedTransform => player.executedAbleGunFu._gunFuAttackedAble;

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
        this._timer = 0;
        curGunFuPhase = GunFuExecuteSinglePhase.Warping;
        CalculateAdjustTransform();
        base.Enter();
    }

    public override void Exit()
    {
        ResetIsShootAlready();
        gunFuAble._gunFuAnimator.applyRootMotion = false;
        base.Exit();
    }

    public override void FixedUpdateNode()
    {

        switch (curGunFuPhase)
        {
            case GunFuExecuteSinglePhase.Warping:
                {
                    if (WarpingComplete())
                    {
                        gunFuAble._gunFuAnimator.applyRootMotion = true;
                        gunFuAble.executedAbleGunFu.TakeGunFuAttacked(this,gunFuAble);
                        curGunFuPhase = GunFuExecuteSinglePhase.Interacting;
                    }
                    break;
                }
            case GunFuExecuteSinglePhase.Interacting:
                {
                    ShootingCheck();
                    ExecuteCheck();
                    break;
                }
        }
        

        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return base.IsComplete();
    }

    public override bool IsReset()
    {
        if(this.IsComplete())
            return true;
        return false;
    }

    public override void UpdateNode()
    {
        _timer += Time.deltaTime;
        
        base.UpdateNode();
    }
    private void ShootingCheck()
    {
        if(shootimingNormalized.Count>0)
        foreach(float shootimingNormal in shootimingNormalized)
        {
            if(_timer >= _animationClip.length * shootimingNormal
                && isShootAlready[shootimingNormal] == false)
                WeaponShootBlank.ShootBlank(weaponAdvanceUser._currentWeapon);
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
        }
    }
    private bool WarpingComplete()
    {
        float t = _timer / (_animationClip.length * warpingNormalized);

        if(t>1)
            return true;

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
        Vector3 enterDir = gunFuAttackerTransform.forward;

        gunFuAttackerEnterPosition = gunFuAttackerTransform.position;
        gunFuAttackerEnterRotation = gunFuAttackerTransform.rotation;
        opponentGunFuEnterPosition = gunFuGotAttackedTransform.position;
        opponentGunFuEnterRotation = gunFuGotAttackedTransform.rotation;

        gunFuAttackerTargetPosition
            = gunFuAttackerTransform.position
            + (gunFuAttackerTransform.forward * gunFuExecute_Single_ScriptableObject.playerForwardRelativePosition)
            + (gunFuAttackerTransform.right * gunFuExecute_Single_ScriptableObject.playerRightwardRelativePosition);

        gunFuAttackerTargetRotation
            = gunFuAttackerTransform.rotation * Quaternion.Euler(0,gunFuExecute_Single_ScriptableObject.playerRotationRelative,0);

        opponentGunFuTargetPosition
            = gunFuGotAttackedTransform.position
            + (gunFuGotAttackedTransform.forward * gunFuExecute_Single_ScriptableObject.opponentForwardRelative)
            + (gunFuGotAttackedTransform.right * gunFuExecute_Single_ScriptableObject.opponentRightwardRelative);

        opponentGunFuTargetRotation
            = gunFuGotAttackedTransform.rotation * Quaternion.Euler(0, gunFuExecute_Single_ScriptableObject.opponentRotationRelative, 0);
    }
}
