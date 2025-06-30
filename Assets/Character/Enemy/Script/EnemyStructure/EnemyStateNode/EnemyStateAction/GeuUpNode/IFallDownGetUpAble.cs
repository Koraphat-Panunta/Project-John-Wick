using UnityEngine;

public interface IFallDownGetUpAble 
{
    public AnimationClip _standUpClip { get;}
    public AnimationClip _pushUpClip { get;}
    public Animator _animator { get;}
    public Transform _hipsBone { get; }
    public Transform _root { get; }
    public Transform[] _bones { get; }
    public Rigidbody[] _ragdollRigidbodies { get; }
    public bool _isFallDown { get; }
    public bool _isGetUp { get; }
}
