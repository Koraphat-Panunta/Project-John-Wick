
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputBlackBoard : MonoBehaviour
{
    //PlayerInput playerInput = new PlayerInput();

    InputAction move;
    InputAction look;
    InputAction sprint;
    InputAction swapShoulder;
    InputAction switchWeapon;
    InputAction aimDownSight;
    InputAction pullTrigger;
    InputAction reload;

    public Vector2 moveInput;
    public Vector2 lookInput;
    public bool sprintInput;
    public bool swapShoulderInput;
    public bool switchWeaponInput;
    public bool aimDownSightInput;
    public bool pullTriggerInput;
    public bool reloadInput;

    private void Awake()
    {
        //playerInput.Enable();
        //move = playerInput.PlayerAction.Move;
        //look = playerInput.PlayerAction.Look;
        //sprint = playerInput.PlayerAction.Sprint;
        //swapShoulder = playerInput.PlayerAction.SwapShoulder;
        //switchWeapon = playerInput.PlayerAction.SwitchWeapon;
        //aimDownSight = playerInput.PlayerAction.AimDownSight;
        //pullTrigger = playerInput.PlayerAction.PullTrigger;
        //reload = playerInput.PlayerAction.Reload;

        InitailizedInput();
    }
    void Start()
    {
       
    }
    private void InitailizedInput()
    {
        move.performed += context => moveInput = move.ReadValue<Vector2>();
        move.canceled += context => moveInput = Vector2.zero;

        look.performed += context => lookInput = look.ReadValue<Vector2>();
        look.canceled += context => lookInput = Vector2.zero;

        sprint.performed += context => sprintInput = true;
        sprint.canceled += context => sprintInput = false;

        swapShoulder.performed += context => swapShoulderInput = true;
        swapShoulder.canceled += context => swapShoulderInput = false;

        switchWeapon.performed += context => switchWeaponInput = true;
        switchWeapon.canceled += context => switchWeaponInput = false;

        aimDownSight.performed += context => aimDownSightInput = true;
        aimDownSight.canceled += context => aimDownSightInput = false;

        pullTrigger.performed += context => pullTriggerInput = true;
        pullTrigger.canceled += context => pullTriggerInput = false;

        reload.performed += context => reloadInput = true;
        reload.canceled += context => reloadInput = false;
    }

   



    // Update is called once per frame
    void Update()
    {
        
    }
}
