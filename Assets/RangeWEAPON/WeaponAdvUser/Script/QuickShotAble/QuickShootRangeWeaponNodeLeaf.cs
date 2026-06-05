using System;
using System.Collections;
using UnityEngine;

public class QuickShootRangeWeaponNodeLeaf : WeaponManuverLeafNode
{

    public float aimingWeightQuickShot { get; protected set; }
    public Vector3 targetQuickShot { get => this.target ? this.target.position : this.weaponAdvanceUser._pointingPos; }
    public CastFindingScriptableObject castFindingScriptableObject { get; protected set; }
    private AnimationTriggerEventPlayer animationTriggerEventPlayer;
    public Vector3 shootDir 
    { 
        get => this.target
            ? (this.target.position - this.weaponAdvanceUser._currentWeapon.bulletSpawner.transform.position).normalized
            : (this.weaponAdvanceUser._pointingPos - this.weaponAdvanceUser._currentWeapon.bulletSpawner.transform.position).normalized;
    }
    public Transform target { get; protected set; }

    private float rotateNormalTime;

    public bool isQuickShotAble;
    public float quickShotCoolDownTime = 1;

    public QuickShootRangeWeaponNodeLeaf(
        IRangeWeaponAdvanceUser weaponAdvanceUser
        ,float aimingWeightQuickShot
        ,CastFindingScriptableObject castFindingTargetScriptableObject
        , AnimationTriggerEventSCRP animationTriggerEventSCRP
        , Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        this.aimingWeightQuickShot = aimingWeightQuickShot;
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(animationTriggerEventSCRP);
        this.castFindingScriptableObject = castFindingTargetScriptableObject;

        this.animationTriggerEventPlayer.SubscribeEvent("RotateEnd",this.RotateEnd);
        this.animationTriggerEventPlayer.SubscribeEvent("Shoot", this.Shoot);

        this.animationTriggerEventPlayer.GetNormalizedTimeFromStateName("RotateEnd", out this.rotateNormalTime);
        this.isQuickShotAble = true;
    }
    public override void Enter()
    {
        this.isQuickShotAble = false;
        this.weaponAdvanceUser._character._movementCompoent.CancleMomentum();
        this.triggerReset = false;
        this.isRotate = true;
        this.animationTriggerEventPlayer.Rewind();
        this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction(
            WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive
            ,this
            );

        Vector3 castDir = this.weaponAdvanceUser._shootingPos - this.weaponAdvanceUser._character._movementCompoent.curPosition;
        castDir.Normalize();

        if (CastFinding.FindLiveObjectInConeByComponent<BodyPart>(
            this.weaponAdvanceUser._character._movementCompoent.curPosition
            , castDir
            , this.castFindingScriptableObject.castDistance
            , this.castFindingScriptableObject.casthalfAngleDegrees
            , castFindingScriptableObject.targetLayerMask
            , out BodyPart bulletDamageAble
            ))
        {
            Debug.Log("QuickShot bulletDamagedAble = " + bulletDamageAble);
            this.target = (bulletDamageAble.enemy.humanoidBone.hips);
        }
        else
        {
            this.target = null;
        }

        Debug.Log("quickshot target = " + this.target);
            

        base.Enter();
    }
    public override void Exit()
    {
        this.isQuickShotAble = false;

        this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction(
          WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive
          , this
          );

        this.weaponAdvanceUser._character.StartCoroutine(this.CoolDownQuickShot());

        base.Exit();
    }

    public override bool Precondition()
    {
        if(this.isQuickShotAble == false)
            return false;

        return base.Precondition();
    }

    public override void FixedUpdateNode()
    {
        if (this.isRotate)
        {
            float t = this.animationTriggerEventPlayer.GetRemapNormalizedTimer(0, this.rotateNormalTime);
            this.weaponAdvanceUser._character._movementCompoent.SetRotateToDirWorldSlerp(this.shootDir,t);
        }
    }

    public override bool IsComplete()
    {
        return this.animationTriggerEventPlayer.IsPlayFinish();
    }

    public void TriggerReset()
    {

        triggerReset = true;
    } 
    protected bool triggerReset;
    public override bool IsReset()
    {
        if(this.weaponAdvanceUser._character.isDead)
            return true;

        if (this.IsComplete())
        {
            return true;
        }

        if(this.triggerReset)
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);

        this.weaponAdvanceUser._weaponManuverManager.aimingWeight = Mathf.Lerp(this.weaponAdvanceUser._weaponManuverManager.aimingWeight
            , this.aimingWeightQuickShot, Time.deltaTime * 10);

        this.weaponAdvanceUser._weaponAfterAction.SendFeedBackWeaponAfterAction(
         WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive
         , this
         );
    }

    private bool isRotate;
    public void RotateEnd()
    {
        this.isRotate = false;
    }
    public void Shoot()
    {

        RangeWeaponBehavior.ShootByPassRateOfFire(this.weaponAdvanceUser._currentWeapon);
    }

    protected IEnumerator CoolDownQuickShot()
    {
        yield return new WaitForSeconds(this.quickShotCoolDownTime);
        this.isQuickShotAble = true;
    }
}
