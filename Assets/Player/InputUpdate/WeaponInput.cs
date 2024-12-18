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
    public void InputWeaponUpdate( Player player)
    {
        if (player.isSwitchWeapon)
        {
            player.weaponCommand.SwitchWeapon();
        }
        if (player.isAiming)
        {
            player.weaponCommand.AimDownSight();
        }
        else
        {
            player.weaponCommand.LowReady();
        }

        if (player.isPullTrigger)
        {
            player.weaponCommand.PullTrigger();
        }
        else
        {
            player.weaponCommand.CancleTrigger();
        }

        if (player.isReload)
        {
            player.weaponCommand.Reload(player.weaponBelt.ammoProuch);
        }
    }
}
