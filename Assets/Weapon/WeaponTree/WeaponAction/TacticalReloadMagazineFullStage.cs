using System.Collections.Generic;
using UnityEngine;

public class TacticalReloadMagazineFullStage : WeaponActionNode
{
    private WeaponTreeMagazineAuto weaponTreeMagazineAuto;
    private WeaponBlackBoardMagazineAuto blackBoardMagazineAuto;
    private Weapon weapon;
    private MagazineType weaponMag;
    public TacticalReloadMagazineFullStage(WeaponTreeManager weaponTreeManager):base(weaponTreeManager)
    {
        weaponTreeMagazineAuto = weaponTreeManager as WeaponTreeMagazineAuto;
        blackBoardMagazineAuto = weaponTreeMagazineAuto.WeaponBlackBoard as WeaponBlackBoardMagazineAuto;
        weapon = weaponTreeMagazineAuto.weapon;
        weaponMag = weapon as MagazineType;
    }

    public override List<WeaponNode> SubNode { get ; set ; }


    public override void Enter()
    {
        weapon.Notify(weapon, WeaponSubject.WeaponNotifyType.TacticalReload);
        weapon.userWeapon.weaponAfterAction.Tactical_ReloadMagazine(weapon);
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }

    public override bool IsComplete()
    {
        if(
            blackBoardMagazineAuto.IsMagin == true
            && blackBoardMagazineAuto.BulletStack[BulletStackType.Magazine] == weapon.Magazine_capacity
            )
            return true;
        else return false;
    }

    public override bool IsReset()
    {
        if(IsComplete())
            return true;
        else if(
            blackBoardMagazineAuto.IsEquip == false
            ||blackBoardMagazineAuto.IsCancle == true
            )
            return true;
        else return false ;
    }

    public override bool PreCondition()
    {
        bool IsMagIn = blackBoardMagazineAuto.IsMagin;
        int MagCount = blackBoardMagazineAuto.BulletStack[BulletStackType.Magazine];
        if (
            IsMagIn == true
            && MagCount <= 0
            )
            return true;
        else 
            return false;
    }

    public override void Update()
    {
        
    }

   
}
