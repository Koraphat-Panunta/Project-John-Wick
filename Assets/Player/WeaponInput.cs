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
        Debug.Log("InputUpdate");
        if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2))
        {
            player.playerWeaponCommand.SwitchWeapon();
        }
        if (input.aiming.phase == InputActionPhase.Performed || input.aiming.phase == InputActionPhase.Started||Input.GetKey(KeyCode.Mouse1))
        {
            player.playerWeaponCommand.Aim();
        }
        else
        {
            Debug.Log("LowReadyInput");
            player.playerWeaponCommand.LowWeapon();
        }

        if (input.firing.phase == InputActionPhase.Started || Input.GetKeyDown(KeyCode.Mouse0)|| input.firing.phase == InputActionPhase.Performed
            ||Input.GetKey(KeyCode.Mouse0))
        {
            player.playerWeaponCommand.Pulltriger();
        }
        else
        {
            player.playerWeaponCommand.CancelTrigger();
        }

        if (input.reloading.phase == InputActionPhase.Started || Input.GetKeyDown(KeyCode.R))
        {
            player.playerWeaponCommand.Reload();
        }
    }
}
