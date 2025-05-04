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

        userInput.PlayerAction.Firing.performed += playerInputAPI.PullTrigger; ;
        userInput.PlayerAction.Firing.canceled += playerInputAPI.PullTrigger;

        userInput.PlayerAction.Reload.performed += playerInputAPI.Reload;
        userInput.PlayerAction.Reload.canceled += playerInputAPI.Reload;

        //userInput.PlayerAction.SwapShoulder.performed += playerInputAPI.SwapShoulder;
        userInput.PlayerAction.SwapShoulder.canceled += playerInputAPI.SwapShoulder;

        userInput.PlayerAction.SwitchWeapon.performed += playerInputAPI.SwitchWeapon;
        userInput.PlayerAction.SwitchWeapon.canceled += playerInputAPI.SwitchWeapon;

        userInput.PlayerAction.TrggerGunFu.performed += playerInputAPI.TriggerGunFu;

        userInput.PlayerAction.ToggleChangeStance.performed += playerInputAPI.ToggleCrouchStand;
        userInput.PlayerAction.ToggleChangeStance.canceled += playerInputAPI.ToggleCrouchStand;

        userInput.PlayerAction.TriggerDodgeRoll.performed += playerInputAPI.TriggerDodgeRoll;

        userInput.PlayerAction.TriggerPickingUpWeapon.performed += playerInputAPI.TriggerPickingUpWeapon;

        userInput.PlayerAction.TriggerDropWeapon.performed += playerInputAPI.TriggerDropWeapon;
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

    private void OnValidate()
    {
        player = FindAnyObjectByType<Player>();
    }
}
