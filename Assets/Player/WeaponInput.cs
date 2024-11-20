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
    public void InputWeaponUpdate(PlayerController.Input input, Player player)
    {
        if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2))
        {
            player.weaponCommand.SwitchWeapon();
        }
        if (input.aiming.phase == InputActionPhase.Performed || input.aiming.phase == InputActionPhase.Started||Input.GetKey(KeyCode.Mouse1))
        {
            player.weaponCommand.AimDownSight();
        }
        else
        {
            player.weaponCommand.LowReady();
        }

        if (input.firing.phase == InputActionPhase.Started || Input.GetKeyDown(KeyCode.Mouse0)|| input.firing.phase == InputActionPhase.Performed
            ||Input.GetKey(KeyCode.Mouse0))
        {
            player.weaponCommand.PullTrigger();
        }
        else
        {
            player.weaponCommand.CancleTrigger();
        }

        if (input.reloading.phase == InputActionPhase.Started || Input.GetKeyDown(KeyCode.R))
        {
            player.weaponCommand.Reload(player.weaponBelt.ammoProuch);
        }
    }
}
