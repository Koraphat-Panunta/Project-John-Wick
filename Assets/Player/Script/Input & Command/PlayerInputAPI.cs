using UnityEngine;
using UnityEngine.InputSystem;
using static Player;
using static SubjectPlayer;

public class PlayerInputAPI : MonoBehaviour
{
    // Start is called once before the first execution of UpdateNode after the MonoBehaviour is created
    public Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
 
    public void Move(InputAction.CallbackContext context)
    {
        player.inputMoveDir_Local = context.ReadValue<Vector2>();
    }
    public void Look(InputAction.CallbackContext context)
    {
        player.inputLookDir_Local = context.ReadValue<Vector2>();
    }
    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
            player.isSprint = true;
        if(context.canceled)
            player.isSprint = false;
    }
    public void Aim(InputAction.CallbackContext context)
    {
        if (context.performed)
            player.isAimingCommand = true;
        if (context.canceled)
            player.isAimingCommand = false;
    }
    public void PullTrigger(InputAction.CallbackContext context)
    {
        if (context.performed)
            player.isPullTriggerCommand = true;
        if (context.canceled)
            player.isPullTriggerCommand = false;
    }
    public void Reload(InputAction.CallbackContext context)
    {
        if (context.performed)
            player.isReloadCommand = true;
        if (context.canceled)
            player.isReloadCommand = false;
    }
    public void SwapShoulder(InputAction.CallbackContext context)
    {


        if (player.curShoulderSide == ShoulderSide.Left)
        { player.curShoulderSide = ShoulderSide.Right; }

        else if (player.curShoulderSide == ShoulderSide.Right)
        { player.curShoulderSide = ShoulderSide.Left; }
        player.NotifyObserver(player, PlayerAction.SwapShoulder);

    }
    public void SwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
            player.isSwitchWeaponCommand = true;
        if (context.canceled)
            player.isSwitchWeaponCommand = false;
    }
    public void TriggerGunFu(InputAction.CallbackContext context)
    {
        if(context.performed)
            player._triggerGunFu = true;
    }
    public void ToggleCrouchStand(InputAction.CallbackContext context)
    {
        Debug.Log("ToggleCrouchStand");

        if (context.performed)
        {
            Debug.Log("ToggleCrouchStand performed");
            

            switch ( player.playerStance) 
            {
                case Player.PlayerStance.stand: { player.playerStance = Player.PlayerStance.crouch; }
                    break;
                case Player.PlayerStance.crouch: { player.playerStance = Player.PlayerStance.stand; }
                    break;
            }
           
        }
    }
    public void TriggerDodgeRoll(InputAction.CallbackContext context)
    {
        if(context.performed)
            player.triggerDodgeRoll = true;
    }

    public void TriggerPickingUpWeapon(InputAction.CallbackContext context)
    {
        Debug.Log("Picking up command");

        if(context.performed)
            player.isPickingUpWeaponCommand = true;
    }
}
