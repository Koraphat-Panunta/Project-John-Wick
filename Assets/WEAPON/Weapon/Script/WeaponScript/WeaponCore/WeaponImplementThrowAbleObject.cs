using UnityEngine;

[RequireComponent(typeof(Collider))]
public partial class Weapon : IThrowAbleObject
{
    public bool _isTriggerThrow { get; set; }
    public Transform _throwAbleObjectTransform { get; set; }
    public ObjectIsBeenThrowNodeLeaf _objectIsBeenThrowNodeLeaf { get; set; }
    public Vector3 _targetThrowAtPosition { get; set; }
    public IThrowObjectAble _throwerObject { get; set; }
    public IBeenThrewObjectAt _targetBeenThrowAbleObjectAt { get; set; }
    [Range(0,100)]
    [SerializeField] private float throwVelocity;
    public float _throwVelocity { get => this.throwVelocity; set => this.throwVelocity = value; }
    public Rigidbody _throwAbleObjectRigidBody { get => rb; set => rb = value; }

    
    private void OnCollisionEnter(Collision collision)
    {
       _objectIsBeenThrowNodeLeaf.OnColliderHit(collision);
    }

    public void Throw(IThrowObjectAble throwerObject, IBeenThrewObjectAt beenThrewObjectAt)
    {
        this._targetBeenThrowAbleObjectAt = beenThrewObjectAt;
        this.Throw(throwerObject,beenThrewObjectAt._beenThrowObjectAtPosition);
    }

    public void Throw(IThrowObjectAble throwerObject, Vector3 targetPosition)
    {
        WeaponAttachingBehavior.Detach(this, this.userWeapon);

        _isTriggerThrow = true;
        this._throwerObject = throwerObject;

        this._targetThrowAtPosition = targetPosition;
    }
}
