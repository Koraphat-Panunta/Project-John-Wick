using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : IMovementCompoent,IMovementSnaping,IMotionWarpingAble
{

    public Vector3 moveInputVelocity_World { get; set; }
    public Vector3 moveInputVelocity_Local { get; set; }
    public Vector3 curMoveVelocity_World { get; set; }
    public Vector3 curMoveVelocity_Local { get; set; }
    public bool isGround { get; set; }
    public MonoBehaviour userMovement { get; set; }
    public IMovementCompoent.Stance curStance { get; set; }
    public Vector3 forwardDir { get ; set ; }

  

    private Player player;
    private CharacterController characterController;

    private GravityMovement gravityMovement;
    public IMovementMotionWarping movementMotionWarping { get; set; }

    public PlayerMovement(Player player)
    {
        this.userMovement = player;
        this.player = player;
        this.characterController = player.GetComponent<CharacterController>();

        curMoveVelocity_World = Vector3.zero;
       
        this.gravityMovement = new GravityMovement();

        curStance = IMovementCompoent.Stance.Stand;

        player.StartCoroutine(DelayInitialized());
        
    }
    public void MovementUpdate()
    {
        moveInputVelocity_Local = TransformWorldToLocalVector(moveInputVelocity_World, player.transform.forward);
        curMoveVelocity_Local = TransformWorldToLocalVector(curMoveVelocity_World, player.gameObject.transform.forward);

        forwardDir = player.transform.forward;
    }
    public void MovementFixedUpdate()
    {

        GravityUpdate();
        characterController.Move(curMoveVelocity_World * Time.deltaTime);

        if (Physics.Raycast(player.centreTransform.position, Vector3.down, 1))
            isGround = true;
        else isGround = false;
    }
    public void SnapingMovement(Vector3 Destination,Vector3 offset,float speed)
    {
        Vector3 finalDestination = Destination + offset;
        float distacne = Vector3.Distance(player.transform.position, finalDestination);

        curMoveVelocity_World = Vector3.zero;

        if (Vector3.Distance(player.transform.position, finalDestination) <= speed*Time.deltaTime)
        {
            characterController.Move((finalDestination - player.transform.position).normalized * speed * (distacne / speed*Time.deltaTime) * Time.deltaTime);
            return;
        }

        characterController.Move((finalDestination - player.transform.position).normalized * speed  * Time.deltaTime);
    }
   
   
    public void GravityUpdate()
    {
        gravityMovement.GravityMovementUpdate(this);
    }
    public void MoveToDirWorld(Vector3 dirWorldNormalized, float speed, float maxSpeed)
    {
        moveInputVelocity_World = new Vector3(dirWorldNormalized.x, 0, dirWorldNormalized.z);
        curMoveVelocity_World = Vector3.Lerp(curMoveVelocity_World, moveInputVelocity_World.normalized * maxSpeed, speed * Time.deltaTime);
    }
    public void MoveToDirLocal(Vector3 dirLocalNormalized, float speed, float maxSpeed)
    {
        moveInputVelocity_World = TransformLocalToWorldVector(
            new Vector3(dirLocalNormalized.x,0,dirLocalNormalized.y),
            forwardDir);

        curMoveVelocity_World = Vector3.Lerp(curMoveVelocity_World, moveInputVelocity_World.normalized * maxSpeed, speed * Time.deltaTime);
    }
    public void RotateToDirWorld(Vector3 lookDirWorldNomalized, float rotateSpeed_NoMultiplyDeltaTime)
    {
        RotateCharacter(lookDirWorldNomalized, rotateSpeed_NoMultiplyDeltaTime);
    }

    #region TransformLocalWorld
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
    private Vector3 TransformWorldToLocalVector(Vector3 dirChild, Vector3 dirParent)
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
    #endregion

    private IEnumerator DelayInitialized()
    {
        yield return null;
        movementMotionWarping = new MotionWarpingByCharacterController(this, characterController);
    }
    private void RotateCharacter(Vector3 dir, float rotateSpeed)
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

}
