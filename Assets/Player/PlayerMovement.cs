using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerMovement 
{
    private Vector3 inputDirection;
    private Vector3 forwardDirection;
    private Vector3 velocityDirection;

    private float move_MaxSpeed = 8;
    private float move_Acceleration = 1;
    private float sprint_MaxSpeed;
    private float sprint_Acceleration;
    private float curVelocity;

    private Player player;
    private PlayerController playerController;
    private CharacterController characterController;
    public PlayerMovement(Player player)
    {
        this.player = player;
        this.characterController = player.GetComponent<CharacterController>();
        this.playerController = player.GetComponent<PlayerController>();
    }
    public void MovementUpdate()
    {
        DirectionUpdate();
    }
    private void DirectionUpdate()
    {
        inputDirection = TransformDirectionObject(new Vector3(playerController.input.movement.ReadValue<Vector2>().x,0,playerController.input.movement.ReadValue<Vector2>().y), Camera.main.transform.forward);
        forwardDirection = player.transform.forward;
        velocityDirection = characterController.velocity.normalized;
        curVelocity = characterController.velocity.magnitude;
        DrawDirLine();
    }
    public void MovingCharacter()
    {
        Vector3 dirMove = Vector3.Lerp(velocityDirection*curVelocity, inputDirection*move_MaxSpeed, move_Acceleration * Time.deltaTime);
        characterController.Move(dirMove);
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
        Debug.DrawLine(player.transform.position, player.transform.position + inputDirection,Color.green);
        Debug.DrawLine(player.transform.position, player.transform.position + forwardDirection, Color.blue);
        Debug.DrawLine(player.transform.position, player.transform.position + velocityDirection, Color.yellow);
    }
}
