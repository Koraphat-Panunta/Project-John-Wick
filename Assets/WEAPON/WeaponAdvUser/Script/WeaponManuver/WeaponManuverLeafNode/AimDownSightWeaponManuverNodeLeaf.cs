using System;
using UnityEngine;

public class AimDownSightWeaponManuverNodeLeaf : WeaponManuverLeafNode
{
    public WeaponNodeManuverManager weaponManuverManager => weaponAdvanceUser._weaponManuverManager;
    WeaponAfterAction weaponAfterAction;
    Weapon curWeapon => weaponAdvanceUser._currentWeapon;

    protected LayerMask blockLayer = LayerMask.GetMask("Default");
    public bool isBlocked 
    {
        get
        {
            Vector3 mainHandToBulletSpanwer = this.curWeapon.bulletSpawner.transform.position - this.curWeapon._mainHandGripTransform.position;

            if(Physics.Raycast(this.curWeapon._mainHandGripTransform.position
                , mainHandToBulletSpanwer.normalized
                ,mainHandToBulletSpanwer.magnitude
                ,this.blockLayer
                ,QueryTriggerInteraction.Ignore
                ))
            {
                return true;
            }
            return false;
        } 
    }
   
    public enum AimDownSightPhase
    {
        Enter,
        Update,
        Exit
    }
    public AimDownSightPhase curPhase { get; protected set; }
    public AimDownSightWeaponManuverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {
        weaponAfterAction = weaponAdvanceUser._weaponAfterAction;
    }

    public override void Enter()
    {
        curPhase = AimDownSightPhase.Enter;
        weaponAfterAction.SendFeedBackWeaponAfterAction
            <AimDownSightWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive,this);
    }

    public override void Exit()
    {
        curPhase = AimDownSightPhase.Exit;
        weaponAfterAction.SendFeedBackWeaponAfterAction
            <AimDownSightWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
    }

    public override void FixedUpdateNode()
    {

        if (weaponManuverManager == null)
            Debug.Log(weaponManuverManager + "is null");

        weaponManuverManager.aimingWeight = Mathf.Clamp01(weaponManuverManager.aimingWeight + Time.deltaTime * curWeapon.aimDownSight_speed);


    }

    public override bool IsComplete()
    {
        if(weaponManuverManager.aimingWeight >= 1)
            return true;

        return false;
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override bool Precondition()
    {
        return base.Precondition();
    }

    public override void UpdateNode()
    {
        //weaponAfterAction.SendFeedBackWeaponAfterAction
        //    <AimDownSightWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);
        curPhase = AimDownSightPhase.Update;
        weaponAfterAction.SendFeedBackWeaponAfterAction
           <AimDownSightWeaponManuverNodeLeaf>(WeaponAfterAction.WeaponAfterActionSending.WeaponStateNodeActive, this);

        //if (this.isBlocked)
        //{
        //    Debug.Log("Weapon Blocked");
        //}

       
        if (this.weaponManuverManager.isPullTriggerManuverAble && this.weaponAdvanceUser._isPullTriggerCommand && this.isBlocked == false)
            curWeapon.PullTrigger();
    }
}
