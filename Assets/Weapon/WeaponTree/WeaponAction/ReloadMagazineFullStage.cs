using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadMagazineFullStage : WeaponActionNode
{
    private float reloadTime;
    private Coroutine reloadingCoroutine;
    private ReloadType reloadStage;
    private MagazineType mag;
    public ReloadMagazineFullStage(Weapon weapon) : base(weapon)
    {
        this.reloadTime = weapon.reloadSpeed;
        mag = weapon as MagazineType;
    }
    public override void FixedUpdate()
    {
        
    }

    public override bool IsComplete()
    {
        if(reloadStage == ReloadType.MAGAZINE_RELOAD_SUCCESS && (reloadingCoroutine == null ||Weapon.triggerState == TriggerState.IsDown))
        {
            
            return true;
        }
        else
            return false;   

    }
    public override bool IsReset()
    {
        if (IsComplete())
            return true;
        else if (Weapon.isEquip == false
            ||Weapon.isCancelAction == true)
        {
            return true;
        } 
        else return false;
    }

    public override bool PreCondition()
    {
        int chamberCount = Weapon.bulletStore[BulletStackType.Chamber];
        int magCount = Weapon.bulletStore[BulletStackType.Magazine];
        bool isMagIn = mag.isMagIn;
       
        if
            (
             isMagIn == true 
            && chamberCount ==0
            && magCount == 0
            )
            return true;
        else
            return false;
    }

    public override void Update()
    {
        int chamberCount = Weapon.bulletStore[BulletStackType.Chamber];
        int magCount = Weapon.bulletStore[BulletStackType.Magazine];
        bool isMagIn = mag.isMagIn; 

        if (
            chamberCount != 0
            && magCount != 0
            && isMagIn == true
            )
            reloadStage = ReloadType.MAGAZINE_RELOAD_SUCCESS;
    }

    public override void Enter()
    {
        reloadStage = ReloadType.MAGAZINE_RELOAD;
        Weapon.Notify(Weapon, WeaponSubject.WeaponNotifyType.Reloading);
        Weapon.userWeapon.weaponAfterAction.Reload(Weapon, reloadStage);
        reloadingCoroutine = Weapon.StartCoroutine(Reloading());
    }
    private IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadTime);
        reloadStage = ReloadType.MAGAZINE_RELOAD_SUCCESS;
        Weapon.userWeapon.weaponAfterAction.Reload(Weapon, reloadStage);
        //Weapon.bulletStore[BulletStackType.Magazine] = Weapon.userWeapon.weaponBelt.ammoProuch.
        new AmmoProchReload(Weapon.userWeapon.weaponBelt.ammoProuch).Performed(Weapon);
        Weapon.bulletStore[BulletStackType.Chamber] += 1;
        Weapon.bulletStore[BulletStackType.Magazine] -= 1;
        reloadingCoroutine = null;
    }
    public override void Exit()
    {
        if(reloadingCoroutine != null)
        {
            Weapon.StopCoroutine(reloadingCoroutine);
        }
    }
}
