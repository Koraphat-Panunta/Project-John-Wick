using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Subject
{
    // Start is called before the first frame update
    public InputAction Move;
    public Vector2 Movement;
    private PlayerStateManager playerState;
    
    private void OnEnable()
    {
        Move.Enable();
    }
    private void OnDisable()
    {
        Move.Disable();
    }
    void Start()
    {
       playerState = GetComponent<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement = Move.ReadValue<Vector2>();
       
    }
    private void FixedUpdate()
    {
        if (Movement.x != 0 || Movement.y != 0)
        {
            playerState.ChangeState(playerState.move);
        }
        else
        {
            playerState.ChangeState(playerState.idle);
        }
    }

}
