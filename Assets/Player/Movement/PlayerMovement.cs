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

    public float move_MaxSpeed = 3.7f;
    private float move_Acceleration = 0.4f;
    private float sprint_MaxSpeed;
    private float sprint_Acceleration;
    public Vector3 curVelocity;


    private Player player;
    private PlayerController playerController;
    private CharacterController characterController;

    private List<IMovementComponent> movementComponents = new List<IMovementComponent>();
    public PlayerMovement(Player player)
    {
        this.player = player;
        this.characterController = player.GetComponent<CharacterController>();
        this.playerController = player.GetComponent<PlayerController>();
        this.movementComponents.Add(new GravityMovement(this.characterController));
        curVelocity = Vector3.zero;
    }
    public void MovementUpdate()
    {
        DirectionUpdate();
        foreach (IMovementComponent movement in movementComponents) 
        {
            movement.MovementUpdate(this);
        }
        characterController.Move(curVelocity*Time.deltaTime);
    }
    private void DirectionUpdate()
    {
        inputDirection = TransformDirectionObject(new Vector3(playerController.input.movement.ReadValue<Vector2>().x,0,playerController.input.movement.ReadValue<Vector2>().y), Camera.main.transform.forward);
        forwardDirection = player.transform.forward;
        velocityDirection = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).normalized;
        DrawDirLine();
    }
    public void OMNI_DirMovingCharacter()
    {
        curVelocity = Vector3.MoveTowards(curVelocity, inputDirection * move_MaxSpeed, move_Acceleration );
    }
    public void ONE_DirMovingCharacter()
    {
        curVelocity = Vector3.MoveTowards(velocityDirection, forwardDirection, move_Acceleration);
    }
    public void RotateCharacter(Vector3 dir,float rotateSpeed)
    {
        dir.Normalize();

        // Flatten the direction vector to the XZ plane to only rotate around the Y axis
        dir.y = 0;

        // Check if the direction is not zero to avoid setting a NaN rotation
        if (dir != Vector3.zero)
        {
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(dir);

            // Smoothly rotate towards the target rotation
            player.gameObject.transform.rotation = Quaternion.Slerp(player.gameObject.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
    public void FreezingCharacter()
    {
        curVelocity = Vector3.MoveTowards(curVelocity, Vector3.zero, move_Acceleration);
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
