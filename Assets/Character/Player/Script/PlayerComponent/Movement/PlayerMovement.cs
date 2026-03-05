using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MovementCompoent
    ,IMovementSnaping
    ,IMotionWarpingAble
    ,IMotionImplusePushAble
    ,IObserverPlayer
{

    public IMovementMotionWarping movementMotionWarping { get; set; }
    public MovementCompoent movementCompoent => this;
    public MotionImplusePushAbleBehavior motionImplusePushAbleBehavior { get; set; }
    public CharacterMovementController characterController { get; protected set; }

    private CharacterMovementControllerScriptableObject standCharControllerSCRP;
    private CharacterMovementControllerScriptableObject crouchCharControllerSCRP;
    private CharacterMovementControllerScriptableObject parkour_CharacterControllerSCRP;

    public float stanceRateMovement { get; protected set; }//0 : idle/Move 1:Sprint

    private Player player;

    public override Vector3 curPosition => this.characterController.position;
    public override Quaternion curRotation => this.characterController.rotation;

    public PlayerMovement(
        Player player
        ,Transform transform
        , MonoBehaviour myMovement
        , CharacterMovementController characterController
        , CharacterMovementControllerScriptableObject standCharControllerSCRP
        , CharacterMovementControllerScriptableObject crouchCharControllerSCRP
        , CharacterMovementControllerScriptableObject parkour_CharacterControllerSCRP) : base(transform, myMovement)
    {
        this.player = player;
        this.player.AddObserver(this);
        this.characterController = characterController;
        motionImplusePushAbleBehavior = new MotionImplusePushAbleBehavior();
        this.standCharControllerSCRP = standCharControllerSCRP;
        this.crouchCharControllerSCRP = crouchCharControllerSCRP;
        this.parkour_CharacterControllerSCRP = parkour_CharacterControllerSCRP;
    }

    public MovementNodeLeaf restMovementNodeLeaf { get; set; }
    public override void UpdateNode()
    {
        this.UpdateProximityInAir();
        base.UpdateNode();
    }
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
    //public void DrawLine()
    //{
    //    //InPutMoveWorld 
    //    Debug.DrawLine(userMovement.transform.position, userMovement.transform.position + (moveInputVelocity_World), Color.green);

    //    //CurVelocityWorld
    //    Debug.DrawLine(userMovement.transform.position, userMovement.transform.position + (curMoveVelocity_World), Color.yellow);

    //    //Forward
    //    Debug.DrawLine(userMovement.transform.position, userMovement.transform.position + (forwardDir), Color.blue);
    //}
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
    public void AddForcePush(Vector3 force, IMotionImplusePushAble.PushMode pushMode)=> motionImplusePushAbleBehavior.AddInstantVelocity(this, force, pushMode);

    public override void Move(Vector3 position)
    {
       characterController.Move(position);
    }

    public void OnNotify<T>(Player player, T node)
    {
        if(isOnUpdateEnable == false)
        {
            characterController.isEnableGravity = false;
        }
        else
            characterController.isEnableGravity = true;
        if( player.playerStateNodeManager != null 
            && (player.playerStateNodeManager as INodeManager).GetCurNodeLeaf() is IParkourNodeLeaf)
        {
            this.characterController.SetCharacterControllerAttribute(this.parkour_CharacterControllerSCRP);
            return;
        }
        switch (player.stance)
        {
            case Stance.stand: 
                {
                    this.characterController.SetCharacterControllerAttribute(this.standCharControllerSCRP);
                }
                break;
            case Stance.crouch:
                {
                    this.characterController.SetCharacterControllerAttribute(this.crouchCharControllerSCRP);
                }
                break;
        }
        
    }

    private void UpdateProximityInAir()
    {
        if(Physics.SphereCast(this.characterController.capsuleColliderCenterPosition
            ,.2f
            , Vector3.down
            ,out RaycastHit hitInfo
            ,2f
            , this.player.playerMovement.characterController.layerMask
            , QueryTriggerInteraction.Ignore)
            )
            this.inAirTimer = 0;
        else
            this.inAirTimer += Time.deltaTime;
        

        if (this.inAirTimer >= this.inAirTime)
            this.isProximityInAir = true;
        else
            this.isProximityInAir = false;

        //if (this.isProximityInAir)
        //    Debug.Log("isProximityInAir");
    }
    public bool isProximityInAir { get; private set; }

    private float inAirTimer;
    private float inAirTime = 0.2f;
    
    public void SetStanceWeight(float weight)
    {
        this.stanceRateMovement = Mathf.Clamp01(weight);
    }

    public override void SetRotation(Quaternion rotation)
    {
        this.characterController.SetRotation(rotation);
    }
}
