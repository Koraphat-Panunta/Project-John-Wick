using UnityEngine;

public interface IThrowAbleObject : IDamageVisitor
{
    public bool _isTriggerThrow { get; set; }
    public Transform _throwAbleObjectTransform { get; set; }
    public ObjectIsBeenThrowNodeLeaf _objectIsBeenThrowNodeLeaf { get; set; }
    public Vector3 _targetThrowAtPosition { get; set; }
    public IThrowObjectAble _throwerObject { get; set; }
    public IBeenThrewObjectAt _targetBeenThrowAbleObjectAt { get; set; }
    public Rigidbody _throwAbleObjectRigidBody { get; set; }
    public float _throwVelocity { get; set; }
    public void Throw(IThrowObjectAble throwerObject,IBeenThrewObjectAt beenThrewObjectAt);
    public void Throw(IThrowObjectAble throwerObject,Vector3 targetPosition);

}
 