using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MovementCompoent
    , IMotionImplusePushAble
    , IMovementSnaping
    ,IObserverEnemy
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
        this.enemy.AddObserver(this);
        this.characterController = characterController;
    }

  
    public override void UpdateNode()
    {
        base.UpdateNode();
    }

    public void AddForcePushInstantly(Vector3 force, IMotionImplusePushAble.PushMode pushMode)
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

    public override void CancelPhysicsVelocity()
    {
        this.characterController.SetVelocityPhysicBased(Vector3.zero);
    }

    public void AddForcePushVelocityChange(Vector3 force, IMotionImplusePushAble.PushMode pushMode, float velocityChangeDuration)
    {
        if (motionImplusePushAbleBehavior == null)
            motionImplusePushAbleBehavior = new MotionImplusePushAbleBehavior();

        motionImplusePushAbleBehavior.AddChangeVelocity(this, force, pushMode,velocityChangeDuration);
    }

    public void OnNotify<T>(Enemy enemy, T node)
    {
        if (this.enemy._isFallDown
            || this.enemy.isDead)
            this.characterController.GetCharacterCapsuleCollider().isTrigger = true;
        else
            this.characterController.GetCharacterCapsuleCollider().isTrigger = false;
    }

    public override void ForceUpdateTransform()
    {
        this.characterController.UpdateCharacterPosition();
        this.characterController.UpdateCharacterRotation();
    }
}
