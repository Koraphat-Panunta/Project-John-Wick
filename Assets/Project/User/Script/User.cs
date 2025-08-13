using UnityEngine;

public class User : MonoBehaviour
{
    public UserInput userInput { get; protected set; }
    [SerializeField] private Player player;

    public void InitailizedInputAction(Player player)
    {
        PlayerInputAPI playerInputAPI = player.GetComponent<PlayerInputAPI>();

        userInput.PlayerAction.Move.performed += playerInputAPI.Move;
        userInput.PlayerAction.Move.canceled += playerInputAPI.Move;

        userInput.PlayerAction.Look.performed += playerInputAPI.Look;
        userInput.PlayerAction.Look.canceled += playerInputAPI.Look;

        userInput.PlayerAction.Sprint.performed += playerInputAPI.Sprint;
        userInput.PlayerAction.Sprint.canceled += playerInputAPI.Sprint;

        userInput.PlayerAction.Aim.performed += playerInputAPI.Aim;
        userInput.PlayerAction.Aim.canceled += playerInputAPI.Aim;

        userInput.PlayerAction.Attack.performed += playerInputAPI.Attack; ;
        userInput.PlayerAction.Attack.canceled += playerInputAPI.Attack;

        userInput.PlayerAction.Reload.performed += playerInputAPI.Reload;
        userInput.PlayerAction.Reload.canceled += playerInputAPI.Reload;

        //userInput.NotifyEvent.SwapShoulder.performed += playerInputAPI.SwapShoulder;
        userInput.PlayerAction.SwapShoulder.canceled += playerInputAPI.SwapShoulder;

        userInput.PlayerAction.TriggerSwitchDrawPrimary.performed += playerInputAPI.TriggerSwitchDrawPrimaryWeapon;
        userInput.PlayerAction.TriggerSwitchDrawPrimary.canceled += playerInputAPI.TriggerSwitchDrawPrimaryWeapon;

        userInput.PlayerAction.TriggerSwitchDrawSecondary.performed += playerInputAPI.TriggerSwitchDrawSecondaryWeapon;
        userInput.PlayerAction.TriggerSwitchDrawSecondary.canceled += playerInputAPI.TriggerSwitchDrawSecondaryWeapon;

        userInput.PlayerAction.HolsterWeapon.performed += playerInputAPI.HolsterWeapon;
        userInput.PlayerAction.HolsterWeapon.canceled += playerInputAPI.HolsterWeapon;

        userInput.PlayerAction.TrggerGunFuExecute.performed += playerInputAPI.TriggerGunFuExecute;

        userInput.PlayerAction.ToggleChangeStance.performed += playerInputAPI.ToggleCrouchStand;
        userInput.PlayerAction.ToggleChangeStance.canceled += playerInputAPI.ToggleCrouchStand;

        userInput.PlayerAction.TriggerDodgeRoll.performed += playerInputAPI.TriggerDodgeRoll;

        userInput.PlayerAction.TriggerPickingUpWeapon.performed += playerInputAPI.TriggerPickingUpWeapon;

        userInput.PlayerAction.TriggerDropWeapon.performed += playerInputAPI.TriggerDropWeapon;

        userInput.PlayerAction.TriggerParkour.performed += playerInputAPI.TriggerParkour;
    }

    public void EnableInput()
    {
        if(userInput == null)
            userInput = new UserInput();

        userInput.Enable();
    }
    public void DisableInput()
    {
        userInput.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if(userInput == null)
            userInput = new UserInput();
        InitailizedInputAction(this.player);
        EnableInput();
    }
    private void OnDisable()
    {
        DisableInput();
    }
    private void OnValidate()
    {
        player = FindAnyObjectByType<Player>();
    }
}
