using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectIsBeenThrowNodeLeaf : INodeLeaf
{
    public List<Func<bool>> isReset { get; set ; }
    public NodeLeafBehavior nodeLeafBehavior { get ; set ; }
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }
    public IThrowAbleObject throwAbleObject { get; set; }
    private bool isLockOn;

    private bool isHit;

    public ObjectIsBeenThrowNodeLeaf(Func<bool> preCondition, IThrowAbleObject throwAbleObject)
    {
        this.isReset = new List<Func<bool>>();
        this.nodeLeafBehavior = new NodeLeafBehavior();
        this.preCondition = preCondition;
        this.throwAbleObject = throwAbleObject;
    }
    public void Enter()
    {
        isHit = false;
        if(this.throwAbleObject._targetBeenThrowAbleObjectAt != null)
            isLockOn = true;
        else
        {
            isLockOn = false;
            throwAbleObject._throwAbleObjectRigidBody.AddForce((throwAbleObject._targetThrowAtPosition - throwAbleObject._throwAbleObjectTransform.position).normalized * throwAbleObject._throwVelocity,ForceMode.VelocityChange);
        }
    }

    public void Exit()
    {

    }
    public void UpdateNode()
    {

    }
    public void FixedUpdateNode()
    {
        this.throwAbleObject._throwAbleObjectTransform.rotation *= Quaternion.AngleAxis(10 * Time.fixedDeltaTime, this.throwAbleObject._throwAbleObjectTransform.right);
        if(isLockOn)
        {
            this.throwAbleObject._throwAbleObjectTransform.position = Vector3.MoveTowards(this.throwAbleObject._throwAbleObjectTransform.position
                , throwAbleObject._targetBeenThrowAbleObjectAt._beenThrowObjectAtPosition
                , throwAbleObject._throwVelocity * Time.fixedDeltaTime);
        }
        
    }

    public bool IsComplete()
    {
        return isHit;
    }

    public bool IsReset()
    {
        if(IsComplete())
            return true;

        return false;
    }
   

    public bool Precondition()
    {
        return this.preCondition.Invoke();
    }

    public void OnColliderHit(Collision collision)
    {
        isHit = true;
        if (collision.gameObject.TryGetComponent<IBeenThrewObjectAt>(out IBeenThrewObjectAt targetBeenThrowAble))
        {
            targetBeenThrowAble.TakeDamage(this.throwAbleObject);
            this.throwAbleObject._throwerObject.ObjectBeenHit(this.throwAbleObject, collision.collider);
        }
    }

   
}
