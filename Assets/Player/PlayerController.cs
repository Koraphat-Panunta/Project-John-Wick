using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
  
    public PlayerInput inputPlayer;
    public Player player;

    public struct Input
    {
        public InputAction.CallbackContext movement;
        public InputAction.CallbackContext look;
        public InputAction.CallbackContext sprint;
        public InputAction.CallbackContext aiming;
        public InputAction.CallbackContext firing;
        public InputAction.CallbackContext reloading;
        public InputAction.CallbackContext swapShoulder;
    }
    public Input input;
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

        inputPlayer.PlayerAction.SwapShoulder.started += SwapShoulder;
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
        input.movement = Value;
    }
    public void OnLook(InputAction.CallbackContext Value)
    {
        input.look = Value;
    }
    public void OnSprint(InputAction.CallbackContext Value)
    {
        input.sprint = Value;
    }
    public void Pulltriger(InputAction.CallbackContext Value)
    {
       input.firing = Value;
    }
    public void Aim(InputAction.CallbackContext Value)
    {
        input.aiming = Value; 
    }
    public void Reload(InputAction.CallbackContext Value)
    {
        input.reloading = Value;
    }
    public void SwapShoulder(InputAction.CallbackContext Value)
    {
        input.swapShoulder = Value;
    }
}
