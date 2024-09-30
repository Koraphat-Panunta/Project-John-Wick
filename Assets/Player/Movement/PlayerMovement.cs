using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerMovement
{
    public Vector3 inputDirection_World { get; private set; }
    public Vector3 forwardDirection_World { get; private set; }
    public Vector3 velocityDirection_World { get; private set; }
    public Vector3 velocityDirection_Local { get; private set; }

    public float move_MaxSpeed = 2.8f;
    public float move_Acceleration = 0.4f;
    public float sprint_MaxSpeed = 5.6f;
    public float sprint_Acceleration = 0.6f;

    public float rotate_Speed = 6;

    public Vector3 curVelocity_World { get; set; }
    public Vector3 curVelocity_Local { get; private set; }


    private Player player;
    private PlayerController playerController;
    private CharacterController characterController;
    public MovementWarping movementWarping;

    public bool isOverride;

    private List<IMovementComponent> movementComponents = new List<IMovementComponent>();
    public PlayerMovement(Player player)
    {
        this.player = player;
        this.characterController = player.GetComponent<CharacterController>();
        this.playerController = player.playerController;
        this.movementComponents.Add(new GravityMovement(this.characterController));
        curVelocity_World = Vector3.zero;
        move_Acceleration = player.movementTest.move_Acceleration;
        sprint_Acceleration = player.movementTest.sprint_Acceleration;
        movementWarping = new MovementWarping();
    }
    public void MovementUpdate()
    {
        DirectionUpdate();
        foreach (IMovementComponent movement in movementComponents) 
        {
            movement.MovementUpdate(this);
        }
        curVelocity_Local = TransformWorldToLocalVector(curVelocity_World, player.gameObject.transform.forward);
        velocityDirection_Local = curVelocity_Local.normalized;
        if (isOverride == false)
        {
            characterController.Move(curVelocity_World * Time.deltaTime);
        }
        
    }
    private void DirectionUpdate()
    {
        inputDirection_World = TransformLocalToWorldVector(new Vector3(this.playerController.input.movement.ReadValue<Vector2>().x,0,this.playerController.input.movement.ReadValue<Vector2>().y), Camera.main.transform.forward);
        forwardDirection_World = player.transform.forward;
        velocityDirection_World = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).normalized;
        DrawDirLine();
    }
    public void OMNI_DirMovingCharacter()
    {
        curVelocity_World = Vector3.MoveTowards(curVelocity_World, inputDirection_World * move_MaxSpeed, move_Acceleration );
    }
    public void ONE_DirMovingCharacter()
    {
        curVelocity_World = Vector3.MoveTowards(curVelocity_World, forwardDirection_World * sprint_MaxSpeed, sprint_Acceleration);
    }
   
    public void FreezingCharacter()
    {
        curVelocity_World = Vector3.MoveTowards(curVelocity_World, Vector3.zero, move_Acceleration);
    }
    private Vector3 TransformLocalToWorldVector(Vector3 dirChild, Vector3 dirParent)
    {
        float zeta;

        Vector3 Direction;
        zeta = Mathf.Atan2(dirParent.z, dirParent.x) - Mathf.Deg2Rad * 90;
        Direction.x = dirChild.x * Mathf.Cos(zeta) - dirChild.z * Mathf.Sin(zeta);
        Direction.z = dirChild.x * Mathf.Sin(zeta) + dirChild.z * Mathf.Cos(zeta);
        Direction.y = 0;

        return Direction;
    }
    private Vector3 TransformWorldToLocalVector(Vector3 dirChild,Vector3 dirParent)
    {
        Vector3 Direction = Vector3.zero;
        float zeta;
        zeta = Mathf.Atan2(dirParent.z, dirParent.x) - Mathf.Deg2Rad * 90;
        zeta = -zeta;
        Direction.x = dirChild.x * Mathf.Cos(zeta) - dirChild.z * Mathf.Sin(zeta);
        Direction.z = dirChild.x * Mathf.Sin(zeta) + dirChild.z * Mathf.Cos(zeta);
        Direction.y = 0;

        return Direction;
    }
    public void RotateCharacter(Vector3 dir, float rotateSpeed)
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
    private void DrawDirLine()
    {
        Debug.DrawLine(player.transform.position, player.transform.position + inputDirection_World,Color.green);
        Debug.DrawLine(player.transform.position, player.transform.position + forwardDirection_World, Color.blue);
        Debug.DrawLine(player.transform.position, player.transform.position + velocityDirection_World, Color.yellow);

       
    }

    
}
