using UnityEngine;

public interface IGrabAbleObject
{
    Transform grabAbleTransform { get; }
    Transform defaultGrabPoint { get; }
    MountComponent mountComponent { get; }
    Rigidbody grabAbleRigidbody { get; }
    Collider grabAbleCollider { get; }
    IObjectGrabbedAble currentGrabbedAt { get; }
    void SetCurrentGrabbedAt(IObjectGrabbedAble socket);
}
