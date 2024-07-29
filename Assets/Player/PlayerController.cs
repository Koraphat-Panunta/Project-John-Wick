using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Subject
{
    // Start is called before the first frame update
    public Vector2 movementInput;
    public Vector2 lookInput;
    public bool sprintInput;
    public InputAction.CallbackContext sprintInputX;
    public bool isAiming = false;
    [SerializeField] private PlayerWeaponCommand weaponCommand;
    [SerializeField] private PlayerStateManager stateManager;
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
        if(Value.phase == InputActionPhase.Started)
        {
            weaponCommand.Pulltriger(stateManager);
        }
    }
    public void Aim(InputAction.CallbackContext Value)
    {   
        if (Value.phase.IsInProgress())            
        {
            weaponCommand.Aim(stateManager);
        }            
        else    
        {
            weaponCommand.LowWeapon(stateManager.Current_state);
        }        
    }
}
