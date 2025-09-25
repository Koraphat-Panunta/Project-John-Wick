using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static Player;
using static SubjectPlayer;

public class PlayerInputAPI : MonoBehaviour,IInitializedAble
{
    // Start is called once before the first execution of UpdateNode after the MonoBehaviour is created
    public Player player;
    public User user;
    public void Initialized()
    {
        player = GetComponent<Player>();
        this.InitailizedInputAction();
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
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (player._isAimingCommand)
            {
                player._isPullTriggerCommand = true;

                if (player._currentWeapon != null
                    && player._currentWeapon.fireMode == Weapon.FireMode.Single
                    && (player._currentWeapon.triggerState == TriggerState.Up
                    || player._currentWeapon.triggerState == TriggerState.IsUp))
                {
                    Debug.Log("player.commandBufferManager.AddCommand(nameof(player._isPullTriggerCommand), 1f);");
                    player.commandBufferManager.AddCommand(nameof(player._isPullTriggerCommand), 1f);
                }
            }
            else
            {
                player._triggerGunFu = true;
                player.commandBufferManager.AddCommand(nameof(player._triggerGunFu), 0.15f);
                player._isPullTriggerCommand = false;
            }
        }
        else if (context.canceled)
        {
            player._isPullTriggerCommand = false;
        }
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
    public void TriggerGunFuExecute(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player._triggerExecuteGunFu = true;
        }
    }
    public void ToggleCrouchStand(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            

            switch ( player.playerStance) 
            {
                case Stance.stand: { player.playerStance = Stance.crouch; }
                    break;
                case Stance.crouch: { player.playerStance = Stance.stand; }
                    break;
            }
           
        }
    }
    public void TriggerSpecialMove(InputAction.CallbackContext context)
    {
        TriggerDodgeRoll(context);
    }
    public void TriggerDodgeRoll(InputAction.CallbackContext context)
    {
        if(context.performed)
            player.triggerDodgeRoll = true;
    }
    public void TriggerInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player._isPickingUpWeaponCommand = true;
            player.commandBufferManager.AddCommand(nameof(player._isPickingUpWeaponCommand), 0.35f);
            player.Interact();
            if(player.currentInteractable == null)
                TriggerParkour(context);
        }
    }
    public void TriggerDropWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
            player._isDropWeaponCommand = true;
    }
    public void TriggerParkour(InputAction.CallbackContext context)
    {
        if (context.performed)
            player._isParkourCommand = true;
        
    }

    private void OnValidate()
    {
        this.user = FindAnyObjectByType<User>();
    }
    public void InitailizedInputAction()
    {


        this.user.userInput.PlayerAction.Move.performed += this.Move;
        this.user.userInput.PlayerAction.Move.canceled += this.Move;

        this.user.userInput.PlayerAction.Look.performed += this.Look;
        this.user.userInput.PlayerAction.Look.canceled += this.Look;

        this.user.userInput.PlayerAction.Sprint.performed += this.Sprint;
        this.user.userInput.PlayerAction.Sprint.canceled += this.Sprint;

        this.user.userInput.PlayerAction.Aim.performed += this.Aim;
        this.user.userInput.PlayerAction.Aim.canceled += this.Aim;

        this.user.userInput.PlayerAction.Attack.performed += this.Attack; ;
        this.user.userInput.PlayerAction.Attack.canceled += this.Attack;

        this.user.userInput.PlayerAction.Reload.performed += this.Reload;
        this.user.userInput.PlayerAction.Reload.canceled += this.Reload;

        //userInput.NotifyEvent.SwapShoulder.performed += playerInputAPI.SwapShoulder;
        this.user.userInput.PlayerAction.SwapShoulder.canceled += this.SwapShoulder;

        this.user.userInput.PlayerAction.TriggerSwitchDrawPrimary.performed += this.TriggerSwitchDrawPrimaryWeapon;
        this.user.userInput.PlayerAction.TriggerSwitchDrawPrimary.canceled += this.TriggerSwitchDrawPrimaryWeapon;

        this.user.userInput.PlayerAction.TriggerSwitchDrawSecondary.performed += this.TriggerSwitchDrawSecondaryWeapon;
        this.user.userInput.PlayerAction.TriggerSwitchDrawSecondary.canceled += this.TriggerSwitchDrawSecondaryWeapon;

        this.user.userInput.PlayerAction.HolsterWeapon.performed += this.HolsterWeapon;
        this.user.userInput.PlayerAction.HolsterWeapon.canceled += this.HolsterWeapon;

        this.user.userInput.PlayerAction.TrggerGunFuExecute.performed += this.TriggerGunFuExecute;

        this.user.userInput.PlayerAction.ToggleChangeStance.performed += this.ToggleCrouchStand;
        this.user.userInput.PlayerAction.ToggleChangeStance.canceled += this.ToggleCrouchStand;

        this.user.userInput.PlayerAction.TriggerDodgeRoll.performed += this.TriggerSpecialMove;

        this.user.userInput.PlayerAction.TriggerPickingUpWeapon.performed += this.TriggerInteract;

        this.user.userInput.PlayerAction.TriggerDropWeapon.performed += this.TriggerDropWeapon;
    }

}
