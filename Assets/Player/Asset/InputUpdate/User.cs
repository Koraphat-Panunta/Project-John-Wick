using UnityEngine;
using UnityEngine.InputSystem;

public class User : MonoBehaviour,IObserverPlayerSpawner
{
    private UserInput userInput;
    [SerializeField] private PlayerSpawner playerSpawner;

    public void GetNotify(Player player)
    {
        userInput.Enable();
        PlayerInputAPI playerInputAPI = player.GetComponent<PlayerInputAPI>();
  
        Debug.Log("playerInputAPI == "+playerInputAPI);

        Debug.Log("userInput = " + this.userInput);

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

        userInput.PlayerAction.SwapShoulder.performed += playerInputAPI.SwapShoulder;
        userInput.PlayerAction.SwapShoulder.canceled += playerInputAPI.SwapShoulder;

        userInput.PlayerAction.SwitchWeapon.performed += playerInputAPI.SwitchWeapon;
        userInput.PlayerAction.SwitchWeapon.canceled += playerInputAPI.SwitchWeapon;

        userInput.PlayerAction.TrggerGunFu.performed += playerInputAPI.TriggerGunFu;
        userInput.PlayerAction.TrggerGunFu.canceled += playerInputAPI.TriggerGunFu;

        userInput.PlayerAction.ToggleChangeStance.performed += playerInputAPI.ToggleCrouchStand;
        userInput.PlayerAction.ToggleChangeStance.canceled += playerInputAPI.ToggleCrouchStand;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerSpawner = FindAnyObjectByType<PlayerSpawner>();
        playerSpawner.AddObserverPlayerSpawner(this);
        userInput = new UserInput();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
