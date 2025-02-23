using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : IMovementCompoent,IMovementSnaping,IMotionWarpingAble,IMotionImplusePushAble

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
    public MovementComponentBehavior moveTo { get ; set ; }
    public bool isEnable { get; set; }

    public IMovementCompoent movementCompoent => this;

    public MotionImplusePushAbleBehavior motionImplusePushAbleBehavior { get ; set; }

    public PlayerMovement(Player player)
    {
        this.userMovement = player;
        this.player = player;
        this.characterController = player.GetComponent<CharacterController>();

        curMoveVelocity_World = Vector3.zero;
       
        this.gravityMovement = new GravityMovement();

        curStance = IMovementCompoent.Stance.Stand;

        moveTo = new MovementComponentBehavior();
        motionImplusePushAbleBehavior = new MotionImplusePushAbleBehavior();
        isEnable = true;
        
    }
    public void MovementUpdate()
    {
        if(isEnable == false)
            return;

        moveInputVelocity_Local = TransformWorldToLocalVector(moveInputVelocity_World, player.transform.forward);
        curMoveVelocity_Local = TransformWorldToLocalVector(curMoveVelocity_World, player.gameObject.transform.forward);

        forwardDir = player.transform.forward;
        DrawLine();
    }
    public void MovementFixedUpdate()
    {
        if (isEnable == false)
            return;

        GravityUpdate();
        characterController.Move(curMoveVelocity_World * Time.deltaTime);

        if (Physics.Raycast(player.centreTransform.position, Vector3.down, 1,moveTo.GetGroundLayerMask()))
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
    public void RotateToDirWorld(Vector3 lookDirWorldNomalized, float rotateSpeed_NoMultiplyDeltaTime)
    {
        RotateCharacter(lookDirWorldNomalized, rotateSpeed_NoMultiplyDeltaTime);
    }
    public void RotateToDirWorldSlerp(Vector3 dir,float t)
    {
        Quaternion targetRotation = Quaternion.LookRotation(dir);

        player.gameObject.transform.rotation = Quaternion.Lerp(player.gameObject.transform.rotation, targetRotation, t);
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

    public void SetRotateCharacter(Vector3 dir)
    {
        
        player.gameObject.transform.forward = dir;
    }

    public void MoveToDirWorld(Vector3 dirWorldNormalized, float speed, float maxSpeed, IMovementCompoent.MoveMode moveMode)
    {
       moveTo.MoveToDirWorld(this,dirWorldNormalized, speed, maxSpeed, moveMode);
    }

    public void MoveToDirLocal(Vector3 dirLocalNormalized, float speed, float maxSpeed, IMovementCompoent.MoveMode moveMode)
    {
        moveTo.MoveToDirLocal(this, dirLocalNormalized, speed, maxSpeed, moveMode);
    }
    private void DrawLine()
    {
        //InPutMoveWorld 
        Debug.DrawLine(userMovement.transform.position, userMovement.transform.position + (moveInputVelocity_World), Color.green);

        //CurVelocityWorld
        Debug.DrawLine(userMovement.transform.position, userMovement.transform.position + (curMoveVelocity_World), Color.yellow);

        //Forward
        Debug.DrawLine(userMovement.transform.position, userMovement.transform.position + (forwardDir), Color.blue);
    }

    public void StartWarping(Vector3 start, Vector3 cT1, Vector3 cT2, Vector3 exit, float duration, AnimationCurve animationCurve, IMovementCompoent movementCompoent)
    {
       if(movementMotionWarping == null)
            movementMotionWarping = new MotionWarpingByCharacterController(movementCompoent,this.characterController);

        this.movementMotionWarping.StartMotionWarpingCurve(start, cT1, cT2, exit, duration, animationCurve);
    }

    public void AddForcePush(Vector3 force, IMotionImplusePushAble.PushMode pushMode)=> motionImplusePushAbleBehavior.AddForecPush(this, force, pushMode);


}
