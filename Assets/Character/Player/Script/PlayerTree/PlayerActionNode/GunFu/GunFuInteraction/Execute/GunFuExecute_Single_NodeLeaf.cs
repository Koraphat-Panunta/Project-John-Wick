using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class GunFuExecute_Single_NodeLeaf : PlayerStateNodeLeaf, IGunFuExecuteNodeLeaf
{
    public IWeaponAdvanceUser weaponAdvanceUser;
    public IGunFuAble gunFuAble { get; set; }
    public IGotGunFuAttackedAble gotGunFuAttackedAble { get => gunFuAble.attackedAbleGunFu; set { } }
    public string _stateName => gunFuExecute_Single_ScriptableObject.gunFuStateName;
    public GunFuExecuteScriptableObject _gunFuExecuteScriptableObject => this.gunFuExecute_Single_ScriptableObject;
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

    IGunFuExecuteNodeLeaf.GunFuExecutePhase IGunFuExecuteNodeLeaf._curGunFuPhase { get => this.curGunFuPhase; set => this.curGunFuPhase = value; }
    private IGunFuExecuteNodeLeaf.GunFuExecutePhase curGunFuPhase { get; set; }
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
        this._timer = _animationClip.length * gunFuExecute_Single_ScriptableObject.executeAnimationOffset;
        curGunFuPhase = IGunFuExecuteNodeLeaf.GunFuExecutePhase.Warping;
        gunFuAble._character._movementCompoent.CancleMomentum();
        gunFuAble.executedAbleGunFu._character._movementCompoent.CancleMomentum();
        CalculateAdjustTransform();
        isWarpingComplete = false;  

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
    private bool isWarpingComplete;

    Vector3 attackerPosAtInteractBegin;
    Vector3 opponentPosAtInteractBegun;
    public override void UpdateNode()
    {
        _timer += Time.deltaTime;
        switch (curGunFuPhase)
        {
            case IGunFuExecuteNodeLeaf.GunFuExecutePhase.Warping:
                {
                    if (isWarpingComplete)
                    {
                        //gunFuAble.executedAbleGunFu._character._movementCompoent.SetRotation(opponentGunFuTargetRotation);

                        gunFuAble.executedAbleGunFu.TakeGunFuAttacked(this, gunFuAble);
                        curGunFuPhase = IGunFuExecuteNodeLeaf.GunFuExecutePhase.Interacting;
                        gunFuAble._character.enableRootMotion = true;

                        gunFuAble._character._movementCompoent.SetPosition(gunFuAttackerTargetPosition);
                        gunFuAble._character._movementCompoent.SetRotation(gunFuAttackerTargetRotation);

                        gunFuAble.executedAbleGunFu._character._movementCompoent.SetPosition(opponentGunFuTargetPosition);
                        gunFuAble.executedAbleGunFu._character._movementCompoent.SetRotation(opponentGunFuTargetRotation);

                        attackerPosAtInteractBegin = gunFuAble._character.transform.position;
                        opponentPosAtInteractBegun = gunFuAble.executedAbleGunFu._character.transform.position;

                        Debug.Log("distance enemyRealPos - enemyAdjustPos = " + Vector3.Distance(gunFuGotAttackedTransform.position, opponentGunFuTargetPosition));
                        Debug.Log("deltaRotate enemyRealRot - enemyAdjustRot = " + Quaternion.Angle(gunFuGotAttackedTransform.rotation,opponentGunFuTargetRotation));
                        //Debug.Log("deltaPos = " + Vector3.Distance(gunFuAble._character.transform.position, gunFuAble.executedAbleGunFu._character.transform.position));
                        //Debug.Log("deltaRotation = " + Quaternion.Angle(gunFuAble._character.transform.rotation, gunFuAble.executedAbleGunFu._character.transform.rotation));
                    }
                    else if (WarpingComplete())
                        isWarpingComplete = true;
                        
                    break;
                }
            case IGunFuExecuteNodeLeaf.GunFuExecutePhase.Interacting:
                {
                    if(isTriggerSlowMotion == false
                        &&_timer >= gunFuExecute_Single_ScriptableObject.slowMotionTriggerNormailzed * _animationClip.length)
                    {
                        TimeControlBehavior.TriggerTimeStop(0, gunFuExecute_Single_ScriptableObject.slowMotionDurarion);
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
            curGunFuPhase = IGunFuExecuteNodeLeaf.GunFuExecutePhase.Execute;
            BulletExecute bulletExecute = new BulletExecute(weaponAdvanceUser._currentWeapon);
            weaponAdvanceUser._currentWeapon.PullTrigger();
            gunFuAble.executedAbleGunFu._damageAble.TakeDamage(bulletExecute);
            isExecuteAlready = true;

            Debug.Log("Distance enemy int-Ex = " + Vector3.Distance(opponentPosAtInteractBegun, gunFuAble.executedAbleGunFu._character.transform.position));
            //Debug.Log("Distance PlayerEnemy Ex = " + Vector3.Distance(gunFuAble._character.transform.position, gunFuAble.executedAbleGunFu._character.transform.position));
            player.NotifyObserver(player, curGunFuPhase);
            player.NotifyObserver(player, this);
        }
    }
    private bool WarpingComplete()
    {
        float lenghtOffset = _animationClip.length * gunFuExecute_Single_ScriptableObject.executeAnimationOffset;

        float t = (_timer - lenghtOffset) / ((_animationClip.length - lenghtOffset)*warpingNormalized);

        t = Mathf.Clamp01(t);

        Debug.Log("player curvelocity before at warping = " + gunFuAble._character._movementCompoent.curMoveVelocity_World);
        MovementWarper.WarpMovement
            (gunFuAttackerEnterPosition
            , gunFuAttackerEnterRotation
            , gunFuAble._character._movementCompoent
            , gunFuAttackerTargetPosition
            , gunFuAttackerTargetRotation
            , t);

        gunFuAble._character._movementCompoent.CancleMomentum();
        Debug.Log("enemy curvelocity at before warping = " + gunFuAble.executedAbleGunFu._character._movementCompoent.curMoveVelocity_World);
        MovementWarper.WarpMovement
            (opponentGunFuEnterPosition
            , opponentGunFuEnterRotation
            , gotGunFuAttackedAble._character._movementCompoent
            , opponentGunFuTargetPosition
            , opponentGunFuTargetRotation
            , t);
        gunFuAble.executedAbleGunFu._character._movementCompoent.CancleMomentum();

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

        Vector3 anchor = gunFuAttackerTransform.position;

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
