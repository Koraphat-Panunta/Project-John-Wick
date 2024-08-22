using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : SubjectPlayer
{
    // Start is called before the first frame update
    public Vector2 movementInput;
    public Vector2 lookInput;
    public bool sprintInput;
    public InputAction.CallbackContext sprintInputX;
    public bool isAiming = false;
    [SerializeField] private PlayerWeaponCommand weaponCommand;
    [SerializeField] private PlayerStateManager stateManager;
    [SerializeField] public CrosshairController crosshairController;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {

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
        sprintInputX = Value;
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
            NotifyObserver(this,PlayerAction.SwapShoulder);
        }
    }
}
