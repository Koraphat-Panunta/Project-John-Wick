using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MovementCompoent, IMotionImplusePushAble, IMovementSnaping
{
   
    public Enemy enemy { get; set; }
    private NavMeshAgent agent;
    public MovementCompoent movementCompoent => this;

    public MotionImplusePushAbleBehavior motionImplusePushAbleBehavior { get; set; }
    public override bool isOnUpdateEnable { get => true; protected set { } }
    public EnemyMovement(Enemy enemy,Transform transform, MonoBehaviour myMovement,NavMeshAgent agent) : base(transform, myMovement)
    {
        this.enemy = enemy;
        this.agent = agent;
    }

  
    public OnUpdateMovementNodeLeaf onUpdateMovementNodeLeaf { get; set; }
    public MovementNodeLeaf restMovementNodeLeaf { get; set; }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true, "enemy movementComponent startSelector");

        onUpdateMovementNodeLeaf = new OnUpdateMovementNodeLeaf(()=> isOnUpdateEnable,this);
        restMovementNodeLeaf = new MovementNodeLeaf(()=> true);

        startNodeSelector.AddtoChildNode(onUpdateMovementNodeLeaf);
        startNodeSelector.AddtoChildNode(restMovementNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }
    public void AddForcePush(Vector3 force, IMotionImplusePushAble.PushMode pushMode)
    {
        if(motionImplusePushAbleBehavior == null)
            motionImplusePushAbleBehavior = new MotionImplusePushAbleBehavior();

        motionImplusePushAbleBehavior.AddForecPush(this, force, pushMode);
    }
    public void SnapingMovement(Vector3 Destination, Vector3 offset, float speed)
    {
        Vector3 finalDestination = Destination + offset;
        float distacne = Vector3.Distance(enemy.transform.position, finalDestination);

        curMoveVelocity_World = Vector3.zero;

        if (Vector3.Distance(enemy.transform.position, finalDestination) <= speed * Time.deltaTime)
        {
            Move((finalDestination - enemy.transform.position).normalized * speed * (distacne / speed * Time.deltaTime) * Time.deltaTime);
            return;
        }
        Move((finalDestination - enemy.transform.position).normalized * speed * Time.deltaTime);
    }

    public override void Move(Vector3 position)
    {
        agent.Move(position);
    }
}
