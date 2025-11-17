using UnityEngine;

public interface IRagdollAble 
{
    public Animator _animator { get;}
    public Transform _hipsBone { get; }
    public Transform _root { get; }
    public Transform[] _bones { get; }
    public Rigidbody[] _ragdollRigidbodies { get; }
    public bool _isFallDown { get; }
    public bool _isGetUp { get; }
    public bool _isFacingUp => Vector3.Dot(_hipsBone.forward, Vector3.up) > 0;
}
