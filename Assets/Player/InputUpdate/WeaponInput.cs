using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponInput 
{
    public WeaponInput()
    {

    }
    public void InputWeaponUpdate( IWeaponAdvanceUser weaponAdvanceUser)
    {
        if (weaponAdvanceUser.isSwitchWeapon)
        {
            weaponAdvanceUser.weaponCommand.SwitchWeapon();
        }
        if (weaponAdvanceUser.isAiming)
        {
            weaponAdvanceUser.weaponCommand.AimDownSight();
        }
        else
        {
            weaponAdvanceUser.weaponCommand.LowReady();
        }

        if (weaponAdvanceUser.isPullTrigger)
        {
            weaponAdvanceUser.weaponCommand.PullTrigger();
        }
        else
        {
            weaponAdvanceUser.weaponCommand.CancleTrigger();
        }

        if (weaponAdvanceUser.isReload)
        {
            weaponAdvanceUser.weaponCommand.Reload(weaponAdvanceUser.weaponBelt.ammoProuch);
        }
    }
}
