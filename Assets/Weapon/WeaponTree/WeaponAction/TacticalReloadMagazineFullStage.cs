using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalReloadMagazineFullStage : WeaponActionNode
{
    private MagazineType weaponMag;
    private Coroutine reloadCoroutine;
    public TacticalReloadMagazineFullStage(Weapon weapon):base(weapon)
    {
        weaponMag = weapon as MagazineType;
    }



    public override void Enter()
    {
        Weapon.Notify(Weapon, WeaponSubject.WeaponNotifyType.TacticalReload);
        Weapon.userWeapon.weaponAfterAction.Reload(Weapon,ReloadType.MAGAZINE_TACTICAL_RELOAD);
        Weapon.userWeapon.weaponAfterAction.Tactical_ReloadMagazine(Weapon);
        reloadCoroutine = Weapon.StartCoroutine(Reloading());
    }

    public override void Exit()
    {
        if (reloadCoroutine != null){
            //Weapon.userWeapon.weaponAfterAction.Reload(Weapon, ReloadType.MAGAZINE_RELOAD_CANCLE);
            Weapon.StopCoroutine(reloadCoroutine);
        }
    }

    public override void FixedUpdate()
    {
        
    }

    public override bool IsComplete()
    {
        if(weaponMag.isMagIn == true && Weapon.bulletStore[BulletStackType.Magazine] == Weapon.bulletCapacity)
            return true;
        else return false;
    }

    public override bool IsReset()
    {
        if(IsComplete())
            return true;
        else if(
            Weapon.isEquip == false
            ||Weapon.isCancelAction == true
            )
            return true;
        else return false ;
    }

    public override bool PreCondition()
    {
        bool IsMagIn = weaponMag.isMagIn;
        int MagCount = Weapon.bulletStore[BulletStackType.Magazine];
        if (
            IsMagIn == true
            && MagCount >= 0
            )
            return true;
        else 
            return false;
    }

    public override void Update()
    {
        
    }
    private IEnumerator Reloading()
    {
        float reloadTime = Weapon.reloadSpeed;
        ReloadType reloadStage;
        yield return new WaitForSeconds(reloadTime);
        reloadStage = ReloadType.MAGAZINE_RELOAD_SUCCESS;
        Weapon.userWeapon.weaponAfterAction.Reload(Weapon, reloadStage);
        new AmmoProchReload(Weapon.userWeapon.weaponBelt.ammoProuch).Performed(Weapon);
        reloadCoroutine = null;
    }
   
}
