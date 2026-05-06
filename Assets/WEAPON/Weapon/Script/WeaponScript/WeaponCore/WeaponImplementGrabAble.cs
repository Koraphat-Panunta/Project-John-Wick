using UnityEngine;

public partial class Weapon : IGrabAbleObject
{
    public Transform grabAbleTransform => this.transform;
    public Transform defaultGrabPoint => this._mainHandGripTransform;
    public MountComponent mountComponent => this._weaponAttacherComponent;
    public Rigidbody grabAbleRigidbody => this.rb;
    public Collider grabAbleCollider => this._collider;

    IObjectGrabbedAble IGrabAbleObject.currentGrabbedAt => this.curAttatch as IObjectGrabbedAble;

    void IGrabAbleObject.SetCurrentGrabbedAt(IObjectGrabbedAble socket)
    {
        this.SetCurAttatchAble(socket as WeaponSocket);
    }
}
