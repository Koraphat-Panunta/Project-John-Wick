using System.Collections;
using UnityEngine;

public class WeaponCommand
{
    private IWeaponAdvanceUser weaponUser;
    public WeaponCommand(IWeaponAdvanceUser weaponUser)
    {
        this.weaponUser = weaponUser;
    }
    public virtual void PullTrigger()
    {
        Weapon weapon = this.weaponUser.currentWeapon;
        switch (weapon.triggerState)
        {
            case (TriggerState.Up):weapon.triggerState = TriggerState.IsDown;
                break;
            case (TriggerState.IsDown):weapon.triggerState = TriggerState.Down;
                break;
             default:weapon.triggerState = TriggerState.Down;
                break;
        }
        weapon.Fire();
    }
    public virtual void CancleTrigger()
    {
        weaponUser.currentWeapon.triggerState = TriggerState.Up;
    }
    public virtual void Reload(AmmoProuch ammoProuch)
    {
        Weapon weapon = weaponUser.currentWeapon;
        if (ammoProuch.amountOf_ammo[weapon.bullet.myType] > 0)
        {
            weapon.Reload();
        }
    }
    public virtual void LowReady()
    {
        weaponUser.currentWeapon.LowWeapon();
    }
    public virtual void AimDownSight()
    {
        weaponUser.currentWeapon.Aim();
    }

    bool isSwitchingWeapon = false;
    public virtual void SwitchWeapon()
    {
        WeaponBelt weaponBelt = weaponUser.weaponBelt;
        if (isSwitchingWeapon == false)
        {
            if (weaponUser.currentWeapon == weaponBelt.primaryWeapon)
            {
                Character user = weaponUser as Character;
                user.StartCoroutine(SwitchPTS());
            }
            else
            {
                Character user = weaponUser as Character;
                user.StartCoroutine(SwitchSTP());
            }
        }
    }
    IEnumerator SwitchPTS()
    {
        isSwitchingWeapon = true;
        if (weaponUser is Player)
        {
            Player player = weaponUser as Player;
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwitchWeapon);
        }
        //WeaponStateManager weaponStateManager = _weaponUser.currentWeapon.weapon_stateManager;
        PrimaryWeapon primaryWeapon = weaponUser.weaponBelt.primaryWeapon;
        SecondaryWeapon secondaryWeapon = weaponUser.weaponBelt.secondaryWeapon;
        Weapon curWeapon = weaponUser.currentWeapon;
        //if (weaponStateManager._currentState == weaponStateManager.reloadState)
        //{
        //    _weaponUser.currentWeapon.weapon_stateManager.ChangeState(weaponStateManager.none);
        //}
        Animator animator = weaponUser.weaponUserAnimator;
        animator.SetTrigger("HolsterPrimary");
        yield return new WaitForSeconds(0.15f);
        curWeapon.AttachWeaponTo(weaponUser.weaponBelt.primaryWeaponSocket);
        animator.SetTrigger("DrawSecondary");
        yield return new WaitForSeconds(0.30f);
        (secondaryWeapon as Weapon).AttatchWeaponTo(weaponUser);
        //
        
        isSwitchingWeapon = false;
    }
    IEnumerator SwitchSTP()
    {
        isSwitchingWeapon = true;
        if (weaponUser is Player)
        {
            Player player = weaponUser as Player;
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwitchWeapon);
        }
        //WeaponStateManager weaponStateManager = _weaponUser.currentWeapon.weapon_stateManager;
        PrimaryWeapon primaryWeapon = weaponUser.weaponBelt.primaryWeapon;
        SecondaryWeapon secondaryWeapon = weaponUser.weaponBelt.secondaryWeapon;
        Weapon curWeapon = weaponUser.currentWeapon;
        //if (weaponStateManager._currentState == weaponStateManager.reloadState)
        //{
        //    _weaponUser.currentWeapon.weapon_stateManager.ChangeState(weaponStateManager.none);
        //}
        Animator animator = weaponUser.weaponUserAnimator;
        animator.SetTrigger("HolsterSecondary");
        yield return new WaitForSeconds(0.36f);
        curWeapon.AttachWeaponTo(weaponUser.weaponBelt.secondaryWeaponSocket);
        animator.SetTrigger("DrawPrimary");
        yield return new WaitForSeconds(0.19f);
        (primaryWeapon as Weapon).AttatchWeaponTo(weaponUser);
       
        isSwitchingWeapon = false;
    }
    public void CancelWeaponActionEvent()
    {
        Weapon weapon = weaponUser.currentWeapon;
        weapon.isCancelAction = true;
    }
    private void QuickDraw()
    {

    }
    public void DropWeapon()
    {

    }
    public void PickUpWeapon()
    {

    }
}
