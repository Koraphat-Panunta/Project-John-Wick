using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerMovement
{

    public Vector3 inputVelocity_World { get; private set; }
    public Vector3 inputVelocity_Local { get; private set; }
    public Vector3 curVelocity_World { get; set; }
    public Vector3 curVelocity_Local { get; private set; }

    public float rotateRate { get; private set; }

    public float move_MaxSpeed = 2.8f;
    public float move_Acceleration = 0.4f;
    public float sprint_MaxSpeed = 5.6f;
    public float sprint_Acceleration = 0.08f;
    public float rotate_Speed = 6;

    public bool isGround { get; private set; }


    private Player player;
    //private PlayerController playerController;
    private CharacterController characterController;
    public MovementWarping movementWarping;


    private List<IMovementComponent> movementComponents = new List<IMovementComponent>();
    public PlayerMovement(Player player)
    {
        this.player = player;
        this.characterController = player.GetComponent<CharacterController>();
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
        inputVelocity_Local = TransformWorldToLocalVector(inputVelocity_World, player.transform.forward);
        curVelocity_Local = TransformWorldToLocalVector(curVelocity_World, player.gameObject.transform.forward);
        characterController.Move(curVelocity_World * Time.deltaTime);

        if(Physics.Raycast(player.transform.position,Vector3.down,1))
            isGround = true;
        else isGround = false;
    }
    private void DirectionUpdate()
    {
        DrawDirLine();
    }
    public void OMNI_DirMovingCharacter()
    {
        Vector3 inputDirection_World = TransformLocalToWorldVector(
            new Vector3(player.inputMoveDir_Local.x, 
            0,
            player.inputMoveDir_Local.y),
            Camera.main.transform.forward);

        inputVelocity_World = inputDirection_World * move_MaxSpeed;

        curVelocity_World = Vector3.MoveTowards(curVelocity_World, inputDirection_World * move_MaxSpeed, move_Acceleration );
    }
    public void ONE_DirMovingCharacter()
    {
        Vector3 forwardDirection_World = player.transform.forward;

        inputVelocity_World = TransformLocalToWorldVector(
            new Vector3(player.inputMoveDir_Local.x,
            0, 
            player.inputMoveDir_Local.y),
            Camera.main.transform.forward)*sprint_MaxSpeed;

        curVelocity_World = Vector3.MoveTowards(curVelocity_World, forwardDirection_World * sprint_MaxSpeed, sprint_Acceleration);
    }
    public void WarpingMovementCharacter(Vector3 Destination,Vector3 offset,float speed)
    {
        Vector3 finalDestination = Destination + offset;
        if (Vector3.Distance(player.transform.position, finalDestination) > 0)
        {
            curVelocity_World = Vector3.zero;
            characterController.Move((finalDestination - player.transform.position).normalized*speed*Time.deltaTime);
        }
        
    }
    public void FreezingCharacter()
    {
        inputVelocity_World = Vector3.zero ;
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
        //Debug.DrawLine(playerAnimationManager.transform.position, playerAnimationManager.transform.position + inputDirection_World,Color.green);
        //Debug.DrawLine(playerAnimationManager.transform.position, playerAnimationManager.transform.position + forwardDirection_World, Color.blue);
        //Debug.DrawLine(playerAnimationManager.transform.position, playerAnimationManager.transform.position + velocityDirection_World, Color.yellow);
        //Debug.DrawLine(playerAnimationManager.transform.position, playerAnimationManager.transform.position + curVelocity_World, Color.red);

       
    }

    
}
