using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponInput : MonoBehaviour
{
    public WeaponInput()
    {

    }
    public void InputWeaponUpdate(PlayerController.Input input, Player player)
    {
        if (input.aiming.phase == InputActionPhase.Performed || input.aiming.phase == InputActionPhase.Started)
        {
            player.playerWeaponCommand.Aim();
        }
        else
        {
            player.playerWeaponCommand.LowWeapon();
        }

        if (input.firing.phase == InputActionPhase.Started || Input.GetKeyDown(KeyCode.Mouse0)|| input.firing.phase == InputActionPhase.Performed
            ||Input.GetKey(KeyCode.Mouse0))
        {
            Debug.Log("PullTrigger");
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
