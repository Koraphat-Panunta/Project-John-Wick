using UnityEngine;

public partial class RangeWeapon : IObjectGrabbedAble
{
    public Transform _grabAbleTransform => this.transform;
    public Transform _defaultGrabPoint => this._mainHandGripTransform;
    public MountComponent _mountComponent => this._weaponAttacherComponent;
    public Rigidbody _grabAbleRigidbody => this.rb;
    public Collider _grabAbleCollider => this._collider;

    IGrabAbleObject IObjectGrabbedAble._currentGrabbedAt => this.curAttatch as IGrabAbleObject;

    void IObjectGrabbedAble.SetCurrentGrabbedAt(IGrabAbleObject socket)
    {
        this.SetCurAttatchAble(socket as IGrabRangeWeaponAble);
    }
}
