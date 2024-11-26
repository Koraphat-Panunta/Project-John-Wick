using System.Collections;
using UnityEngine;

public class WeaponCommand
{
    private IWeaponAdvanceUser weaponUser;
    public WeaponCommand(IWeaponAdvanceUser weaponUser)
    {
        this.weaponUser = weaponUser;
    }
    public void PullTrigger()
    {
        Weapon weapon = this.weaponUser.currentWeapon;
        if (weapon.weapon_stateManager._currentState != weapon.weapon_stateManager.reloadState
             && weapon.weapon_StanceManager._currentStance == weapon.weapon_StanceManager.aimDownSight && weapon.weapon_StanceManager.AimingWeight >= 1)
        {
            if (weapon.triggerState == TriggerState.Up)
            {
                weapon.triggerState = TriggerState.IsDown;
            }
            else if (weapon.triggerState == TriggerState.IsDown)
            {
                weapon.triggerState = TriggerState.Down;
            }
            weapon.Fire();
        }
    }
    public void CancleTrigger()
    {
        weaponUser.currentWeapon.triggerState = TriggerState.Up;
    }
    public void Reload(AmmoProuch ammoProuch)
    {
        Weapon weapon = weaponUser.currentWeapon;
        if (ammoProuch.amountOf_ammo[weapon.bullet.myType] > 0)
        {
            weapon.Reload();
        }
    }
    public void LowReady()
    {
        weaponUser.currentWeapon.LowWeapon();
    }
    public void AimDownSight()
    {
        weaponUser.currentWeapon.Aim();
    }

    bool isSwitchingWeapon = false;
    public void SwitchWeapon()
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
        WeaponStateManager weaponStateManager = weaponUser.currentWeapon.weapon_stateManager;
        PrimaryWeapon primaryWeapon = weaponUser.weaponBelt.primaryWeapon;
        SecondaryWeapon secondaryWeapon = weaponUser.weaponBelt.secondaryWeapon;
        Weapon curWeapon = weaponUser.currentWeapon;
        if (weaponStateManager._currentState == weaponStateManager.reloadState)
        {
            weaponUser.currentWeapon.weapon_stateManager.ChangeState(weaponStateManager.none);
        }
        Animator animator = weaponUser.weaponUserAnimator;
        animator.SetTrigger("HolsterPrimary");
        yield return new WaitForSeconds(0.3f);
        curWeapon.AttachWeaponTo(weaponUser.weaponBelt.primaryWeaponSocket);
        animator.SetTrigger("DrawSecondary");
        yield return new WaitForSeconds(0.13f);
        secondaryWeapon.AttatchWeaponTo(weaponUser);
        //
        if(weaponUser is Player) {
            Player player = weaponUser as Player;
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwitchWeapon);
        }
        isSwitchingWeapon = false;
    }
    IEnumerator SwitchSTP()
    {
        isSwitchingWeapon = true;
        WeaponStateManager weaponStateManager = weaponUser.currentWeapon.weapon_stateManager;
        PrimaryWeapon primaryWeapon = weaponUser.weaponBelt.primaryWeapon;
        SecondaryWeapon secondaryWeapon = weaponUser.weaponBelt.secondaryWeapon;
        Weapon curWeapon = weaponUser.currentWeapon;
        if (weaponStateManager._currentState == weaponStateManager.reloadState)
        {
            weaponUser.currentWeapon.weapon_stateManager.ChangeState(weaponStateManager.none);
        }
        Animator animator = weaponUser.weaponUserAnimator;
        animator.SetTrigger("HolsterSecondary");
        yield return new WaitForSeconds(0.4f);
        curWeapon.AttachWeaponTo(weaponUser.weaponBelt.secondaryWeaponSocket);
        animator.SetTrigger("DrawPrimary");
        yield return new WaitForSeconds(0.5f);
        primaryWeapon.AttatchWeaponTo(weaponUser);
        if (weaponUser is Player)
        {
            Player player = weaponUser as Player;
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwitchWeapon);
        }
        isSwitchingWeapon = false;
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
