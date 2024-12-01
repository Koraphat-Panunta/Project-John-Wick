using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadMagazineFullStage : WeaponActionNode
{
    private WeaponBlackBoardMagazineAuto weaponBlackBoard;
    private Weapon weapon;
    private float reloadTime;
    private Coroutine reloadingCoroutine;
    private ReloadType reloadStage;
    public ReloadMagazineFullStage(WeaponTreeManager weaponTree) : base(weaponTree)
    {
        this.weaponBlackBoard = weaponTree.WeaponBlackBoard as WeaponBlackBoardMagazineAuto;
        this.weapon = weaponTree.weapon;
        this.reloadTime = weapon.reloadSpeed;
    }
    public override void FixedUpdate()
    {
        
    }

    public override bool IsComplete()
    {
        if(
            reloadStage == ReloadType.MAGAZINE_RELOAD_SUCCESS 
            &&
            (reloadingCoroutine == null
            ||weaponBlackBoard.TriggerState == TriggerState.IsDown))
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
        else if (weaponBlackBoard.IsEquip == false
            ||weaponBlackBoard.IsCancle == true)
        {
            return true;
        } 
        else return false;
    }

    public override bool PreCondition()
    {
        int chamberCount = weaponBlackBoard.BulletStack[BulletStackType.Chamber];
        int magCount = weaponBlackBoard.BulletStack[BulletStackType.Magazine];
        bool isMagIn = weaponBlackBoard.IsMagin;
       
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
        int chamberCount = weaponBlackBoard.BulletStack[BulletStackType.Chamber];
        int magCount = weaponBlackBoard.BulletStack[BulletStackType.Magazine];
        bool isMagIn = weaponBlackBoard.IsMagin;

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
        weapon.Notify(weapon, WeaponSubject.WeaponNotifyType.Reloading);
        weapon.userWeapon.weaponAfterAction.Reload(weapon, reloadStage);
        reloadingCoroutine = weapon.StartCoroutine(Reloading());
    }
    private IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadTime);
        reloadStage = ReloadType.MAGAZINE_RELOAD_SUCCESS;
        weapon.userWeapon.weaponAfterAction.Reload(weapon,reloadStage);
        //weapon.bulletStore[BulletStackType.Chamber] += 1;
        //weapon.bulletStore[BulletStackType.Magazine] -= 1;
        reloadingCoroutine = null;
    }
    public override void Exit()
    {
        if(reloadingCoroutine != null)
        {
            weapon.StopCoroutine(reloadingCoroutine);
        }
    }
}
