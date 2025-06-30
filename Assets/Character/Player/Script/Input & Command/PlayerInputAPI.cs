using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
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
        //float screenW = 2560;
        //float screenH = 1440;

        player.inputLookDir_Local = context.ReadValue<Vector2>();
        //player.inputLookDir_Local *= ((Screen.width * Screen.height) / (screenW * screenH));
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
            player._isAimingCommand = true;
        if (context.canceled)
            player._isAimingCommand = false;
    }
    public void PullTrigger(InputAction.CallbackContext context)
    {
        if (context.performed)
            player._isPullTriggerCommand = true;
        if (context.canceled)
            player._isPullTriggerCommand = false;
    }
    public void Reload(InputAction.CallbackContext context)
    {
        if (context.performed)
            player._isReloadCommand = true;
        if (context.canceled)
            player._isReloadCommand = false;
    }
    public void SwapShoulder(InputAction.CallbackContext context)
    {


        if (player.curShoulderSide == ShoulderSide.Left)
        { player.curShoulderSide = ShoulderSide.Right; }

        else if (player.curShoulderSide == ShoulderSide.Right)
        { player.curShoulderSide = ShoulderSide.Left; }
        player.NotifyObserver(player, NotifyEvent.SwapShoulder);

    }
    public void TriggerSwitchDrawPrimaryWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
            player._isDrawPrimaryWeaponCommand = true;
        if(context.canceled)
            player._isDrawPrimaryWeaponCommand = false;
    }
    public void TriggerSwitchDrawSecondaryWeapon(InputAction.CallbackContext context)
    {
        if(context.performed)
            player._isDrawSecondaryWeaponCommand = true;
        if(context.canceled)
            player._isDrawSecondaryWeaponCommand = false;
    }
    public void HolsterWeapon(InputAction.CallbackContext context)
    {
        if(context.performed)
            player._isHolsterWeaponCommand = true;
        else if(context.canceled)
            player._isHolsterWeaponCommand= false;
    }
    public void TriggerGunFu(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            player._triggerExecuteGunFu = true;
        }
        else if(context.interaction is PressInteraction)
        {
            player._triggerGunFu = true;
        }

    }
    public void ToggleCrouchStand(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            

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
        if(context.performed)
            player._isPickingUpWeaponCommand = true;
    }
    public void TriggerDropWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
            player._isDropWeaponCommand = true;
    }
    public void TriggerParkour(InputAction.CallbackContext context)
    {
        if(context.performed)
            player._isParkourCommand = true;
    }
}
