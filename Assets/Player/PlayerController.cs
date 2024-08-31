using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 movementInput;
    public Vector2 lookInput;
    public bool sprintInput;
    public bool isAiming = false;
    [SerializeField] private PlayerWeaponCommand weaponCommand;
    [SerializeField] private PlayerStateManager stateManager;
    [SerializeField] public CrosshairController crosshairController;

    public PlayerInput inputPlayer;
    public Player player;
    private void Awake()
    {
        inputPlayer = new PlayerInput();
        inputPlayer.PlayerAction.Look.performed += OnLook;

        inputPlayer.PlayerAction.Move.performed += OnMove;
        inputPlayer.PlayerAction.Move.started += OnMove;
        inputPlayer.PlayerAction.Move.canceled += OnMove;

        inputPlayer.PlayerAction.Sprint.performed += OnSprint;
        inputPlayer.PlayerAction.SwapShoulder.started += SwapShoulder;

        inputPlayer.PlayerAction.PullTrigger.started += Pulltriger;
        inputPlayer.PlayerAction.PullTrigger.performed += Pulltriger;
        inputPlayer.PlayerAction.PullTrigger.canceled += Pulltriger;

        inputPlayer.PlayerAction.AimDownSight.started += Aim;
        inputPlayer.PlayerAction.AimDownSight.performed += Aim;
        inputPlayer.PlayerAction.AimDownSight.canceled += Aim;

        inputPlayer.PlayerAction.Reload.started += Reload;
    }
    private void OnEnable()
    {
        inputPlayer.PlayerAction.Enable();
    }
    private void OnDisable()
    {
        inputPlayer.PlayerAction.Disable();
    }
    public void OnMove(InputAction.CallbackContext Value)
    {
        movementInput = Value.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext Value)
    {
        lookInput = Value.ReadValue<Vector2>();
    }
    public void OnSprint(InputAction.CallbackContext Value)
    {
        sprintInput = Value.phase.IsInProgress();
    }
    public void Pulltriger(InputAction.CallbackContext Value)
    {
        if (Value.phase == InputActionPhase.Started || Value.phase == InputActionPhase.Performed||Value.phase == InputActionPhase.Canceled)
        {
            weaponCommand.Pulltriger(stateManager,Value);
        }
    }
    public void Aim(InputAction.CallbackContext Value)
    {   
        if (Value.phase == InputActionPhase.Performed|| Value.phase == InputActionPhase.Started)            
        {
            weaponCommand.Aim(stateManager);
        }            
        else    
        {
            weaponCommand.LowWeapon(stateManager.Current_state);
        }        
    }
    public void Reload(InputAction.CallbackContext Value)
    {
        weaponCommand.Reload(stateManager.Current_state); 
    }
    public void SwapShoulder(InputAction.CallbackContext Value)
    {
        if(Value.phase == InputActionPhase.Started)
        {
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwapShoulder);
        }
    }
}
