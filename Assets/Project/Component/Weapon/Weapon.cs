using UnityEngine;

public abstract class Weapon : MonoBehaviour, IObjectGrabbedAble, IInitializedAble
{
    public Transform _grabAbleTransform { get => this.transform; }
    public abstract Transform _defaultGrabPoint { get; }
    public abstract MountComponent _mountComponent { get; }
    public abstract Rigidbody _grabAbleRigidbody { get; }
    public abstract Collider _grabAbleCollider { get; }
    public abstract IGrabAbleObject _currentGrabbedAt { get; }

    public abstract void Initialized();
    public abstract void SetCurrentGrabbedAt(IGrabAbleObject socket);

   

}
