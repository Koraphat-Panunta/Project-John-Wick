using UnityEngine;

public interface IObjectGrabbedAble
{
    Transform _grabAbleTransform { get; }
    Transform _defaultGrabPoint { get; }
    MountComponent _mountComponent { get; }
    Rigidbody _grabAbleRigidbody { get; }
    Collider _grabAbleCollider { get; }
    IGrabAbleObject _currentGrabbedAt { get; }
    void SetCurrentGrabbedAt(IGrabAbleObject socket);
}
