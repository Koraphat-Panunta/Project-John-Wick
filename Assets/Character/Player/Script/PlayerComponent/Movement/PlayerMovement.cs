using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MovementCompoent,IMovementSnaping,IMotionWarpingAble,IMotionImplusePushAble
{

    public IMovementMotionWarping movementMotionWarping { get; set; }
    public MovementCompoent movementCompoent => this;
    public MotionImplusePushAbleBehavior motionImplusePushAbleBehavior { get; set; }
    private CharacterController characterController;
   
    private Player player;

    public PlayerMovement(Player player,Transform transform, MonoBehaviour myMovement, CharacterController characterController) : base(transform, myMovement)
    {
        this.player = player;
        this.characterController = characterController;
        motionImplusePushAbleBehavior = new MotionImplusePushAbleBehavior();
    }

    public MovementNodeLeaf restMovementNodeLeaf { get; set; }
    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }
    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(()=> true,"StartNodeSelector PlayerMovement");

        onUpdateMovementNodeLeaf = new OnUpdateMovementNodeLeaf(()=> isOnUpdateEnable,this);
        restMovementNodeLeaf = new MovementNodeLeaf(()=> true);

        startNodeSelector.AddtoChildNode(onUpdateMovementNodeLeaf);
        startNodeSelector.AddtoChildNode(restMovementNodeLeaf);

        _nodeManagerBehavior.SearchingNewNode(this);
    }
   
    public void SnapingMovement(Vector3 Destination,Vector3 offset,float speed)
    {
        Vector3 finalDestination = Destination + offset;
        float distacne = Vector3.Distance(player.transform.position, finalDestination);

        curMoveVelocity_World = Vector3.zero;

        if (Vector3.Distance(player.transform.position, finalDestination) <= speed*Time.deltaTime)
        {
            Move((finalDestination - player.transform.position).normalized * speed * (distacne / speed*Time.deltaTime) * Time.deltaTime);
            return;
        }
        Move((finalDestination - player.transform.position).normalized * speed  * Time.deltaTime);
    }
    public void DrawLine()
    {
        //InPutMoveWorld 
        Debug.DrawLine(userMovement.transform.position, userMovement.transform.position + (moveInputVelocity_World), Color.green);

        //CurVelocityWorld
        Debug.DrawLine(userMovement.transform.position, userMovement.transform.position + (curMoveVelocity_World), Color.yellow);

        //Forward
        Debug.DrawLine(userMovement.transform.position, userMovement.transform.position + (forwardDir), Color.blue);
    }
    public void StartWarpingCurve(Vector3 start, Vector3 cT1, Vector3 cT2, Vector3 exit, float duration, AnimationCurve animationCurve, MovementCompoent movementCompoent)
    {
        curMoveVelocity_World = Vector3.zero;
       if(movementMotionWarping == null)
            movementMotionWarping = new MotionWarpingByCharacterController(movementCompoent,this.characterController);

        this.movementMotionWarping.StartMotionWarpingCurve(start, cT1, cT2, exit, duration, animationCurve);
    }
    public void StartWarpingLinear(Vector3 start,Vector3 end,float duration,AnimationCurve animationCurve, MovementCompoent movementCompoent)
    {
        curMoveVelocity_World = Vector3.zero;
        if (movementMotionWarping == null)
            movementMotionWarping = new MotionWarpingByCharacterController(movementCompoent, this.characterController);

        this.movementMotionWarping.StartMotionWarpingLinear(start,end, duration, animationCurve);
    }
    public void AddForcePush(Vector3 force, IMotionImplusePushAble.PushMode pushMode)=> motionImplusePushAbleBehavior.AddForecPush(this, force, pushMode);

    public override void Move(Vector3 position)
    {
       characterController.Move(position);
    }
}
