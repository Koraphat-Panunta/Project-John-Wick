using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputWeaponAction : IInputUpdate
{
    public InputWeaponAction()
    {

    }
    public void InputLogicPerforming(PlayerController playerController, Player player)
    {
        PlayerController.Input input = playerController.input;
        if (input.aiming.phase == InputActionPhase.Performed || input.aiming.phase == InputActionPhase.Started)
        {
            Debug.Log("Aim Idle");
            player.playerWeaponCommand.Aim();
        }
        else
        {
            player.playerWeaponCommand.LowWeapon();
        }

        if (input.firing.phase == InputActionPhase.Performed/*||Input.GetKey(KeyCode.Mouse0*/)
        {
            Debug.Log("Pull Trigger Idle");
            player.playerWeaponCommand.Pulltriger();
        }
        else
        {
            player.playerWeaponCommand.CancelTrigger();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.playerWeaponCommand.Reload();
        }
    }
}
