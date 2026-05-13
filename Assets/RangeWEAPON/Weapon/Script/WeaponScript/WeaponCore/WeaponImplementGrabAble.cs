using UnityEngine;

public partial class RangeWeapon
{
    public override Transform _defaultGrabPoint => this._mainHandGripTransform;

    public override MountComponent _mountComponent => this.WeaponAttacherComponent;

    public override Rigidbody _grabAbleRigidbody => this.rb;

    public override Collider _grabAbleCollider => this.Collider;

    public override IGrabAbleObject _currentGrabbedAt => this.curAttatch;

    public override void SetCurrentGrabbedAt(IGrabAbleObject socket)
    {
        this.SetCurAttatchAble(socket as IGrabRangeWeaponAble);
    }
}
