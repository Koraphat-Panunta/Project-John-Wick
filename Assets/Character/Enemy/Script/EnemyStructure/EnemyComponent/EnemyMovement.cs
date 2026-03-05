using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MovementCompoent, IMotionImplusePushAble, IMovementSnaping
{
   
    public Enemy enemy { get; set; }
    public MovementCompoent movementCompoent => this;
    public CharacterMovementController characterController { get; set; }
    public MotionImplusePushAbleBehavior motionImplusePushAbleBehavior { get; set; }
    public override Vector3 curPosition => this.characterController.position;
    public override Quaternion curRotation => this.characterController.rotation;
    public EnemyMovement(Enemy enemy,Transform transform, MonoBehaviour myMovement, CharacterMovementController characterController) : base(transform, myMovement)
    {
        this.enemy = enemy;
        this.characterController = characterController;
    }

  
    public MovementNodeLeaf restMovementNodeLeaf { get; set; }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true, "enemy movementComponent startSelector");

        onUpdateMovementNodeLeaf = new OnUpdateMovementNodeLeaf(()=> isOnUpdateEnable 
       
        ,this);
        restMovementNodeLeaf = new MovementNodeLeaf(()=> true);

        startNodeSelector.AddtoChildNode(onUpdateMovementNodeLeaf);
        startNodeSelector.AddtoChildNode(restMovementNodeLeaf);

        _nodeManagerBehavior.SearchingNewNode(this);
    }
    public void AddForcePush(Vector3 force, IMotionImplusePushAble.PushMode pushMode)
    {
        if(motionImplusePushAbleBehavior == null)
            motionImplusePushAbleBehavior = new MotionImplusePushAbleBehavior();

        motionImplusePushAbleBehavior.AddInstantVelocity(this, force, pushMode);
    }
    public void SnapingMovement(Vector3 Destination, Vector3 offset, float speed)
    {
        Vector3 finalDestination = Destination + offset;
        float distacne = Vector3.Distance(curPosition, finalDestination);

        curMoveVelocity_World = Vector3.zero;

        if (Vector3.Distance(curPosition, finalDestination) <= speed * Time.deltaTime)
        {
            Move((finalDestination - curPosition).normalized * speed * (distacne / speed * Time.deltaTime) * Time.deltaTime);
            return;
        }
        Move((finalDestination - curPosition).normalized * speed * Time.deltaTime);
    }

  
    public override void Move(Vector3 position)
    {
        this.characterController.Move(position);
    }
    public override void SetRotation(Quaternion rotation)
    {
        this.characterController.SetRotation(rotation);
    }
}
