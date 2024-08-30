using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 inputDirection;
    private Vector3 forwardDirection;
    private Vector3 velocityDirection;

    private float move_MaxSpeed;
    private float move_Acceleration;
    private float sprint_MaxSpeed;
    private float sprint_Acceleration;
    private float curVelocity;

    private PlayerController playerController;
    private CharacterController characterController;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        DirectionUpdate();
        //
    }
    private void DirectionUpdate()
    {
        inputDirection = TransformDirectionObject(new Vector3(playerController.movementInput.x,0,playerController.movementInput.y), Camera.main.transform.forward);
        forwardDirection = transform.forward;
        velocityDirection = characterController.velocity.normalized;
        curVelocity = characterController.velocity.magnitude;
        DrawDirLine();
    }
    private Vector3 TransformDirectionObject(Vector3 dirWolrd, Vector3 dirObjectLocal)
    {
        float zeta;

        Vector3 Direction;
        zeta = Mathf.Atan2(dirObjectLocal.z, dirObjectLocal.x) - Mathf.Deg2Rad * 90;
        Direction.x = dirWolrd.x * Mathf.Cos(zeta) - dirWolrd.z * Mathf.Sin(zeta);
        Direction.z = dirWolrd.x * Mathf.Sin(zeta) + dirWolrd.z * Mathf.Cos(zeta);
        Direction.y = 0;

        return Direction;
    }
    private void DrawDirLine()
    {
        Debug.DrawLine(transform.position, transform.position + inputDirection,Color.green);
        Debug.DrawLine(transform.position, transform.position + forwardDirection, Color.blue);
        Debug.DrawLine(transform.position, transform.position + velocityDirection, Color.yellow);
    }
}
