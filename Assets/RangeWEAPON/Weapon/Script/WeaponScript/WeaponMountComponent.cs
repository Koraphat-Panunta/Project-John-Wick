using UnityEngine;

public class WeaponMountComponent : MountComponent
{
    [SerializeField] RangeWeapon weapon;
    public Transform curWeaponGrip;
    public Transform mainHandGrip => weapon._mainHandGripTransform;
    public Transform secondHandGrip => weapon._SecondHandGripTransform;

    public void Attach(Transform weaponSocket,Transform weaponGrip,Vector3 offsetPosition,Quaternion offsetRotation,float attatchingDuration)
    {
        this.curWeaponGrip = weaponGrip;
        base.Attach(weaponSocket, offsetPosition, offsetRotation,attatchingDuration);
        this.weapon.Notify(weapon, RangeWeaponSubject.WeaponNotifyType.BeenAttatch);
    }
    protected override void LateUpdate()
    {
        if(this._parentAttachTransform != null
            && this.attachRate >= 1
            && _attachAbleObject.parent != this.parentAttachTransform)
        {
            _attachAbleObject.position = Vector3.Lerp(_attachAbleObject.position, GetAttachPosition(), this.attachRate);
            _attachAbleObject.rotation = Quaternion.Lerp(_attachAbleObject.rotation, GetAttachRotation(), this.attachRate);

            _attachAbleObject.SetParent(this._parentAttachTransform, true);
        }
        base.LateUpdate();
    }
    public override void Detach()
    {
        if(this._parentAttachTransform != null)
            this._attachAbleObject.SetParent(null,true);
        this.curWeaponGrip = null;
        base.Detach();
        this.weapon.Notify(weapon, RangeWeaponSubject.WeaponNotifyType.BeenDetatch);
    }
    public override Vector3 GetAttachPosition()
    {
        if (_parentAttachTransform == null || curWeaponGrip == null)
            return base._attachAbleObject.position;

        // Calculate how far the grip is from the weapon’s origin in world space.
        Vector3 gripToWeaponOffset = weapon.transform.position - curWeaponGrip.position;

        // Start with the socket position, then apply the offset in socket's local space.
        Vector3 socketPosition = _parentAttachTransform.position
            + (_parentAttachTransform.right * offsetPosition.x)
            + (_parentAttachTransform.up * offsetPosition.y)
            + (_parentAttachTransform.forward * offsetPosition.z);

        // Apply the offset so that the weapon’s grip aligns perfectly with the socket.
        return socketPosition + gripToWeaponOffset;
    }
    public override Quaternion GetAttachRotation()
    {
        if (_parentAttachTransform == null || curWeaponGrip == null)
            return base._attachAbleObject.rotation;


        // This aligns the grip's rotation to match the socket’s rotation.
        // We first calculate the relative rotation difference between weapon and grip.
        Quaternion gripToWeaponRotationOffset = Quaternion.Inverse(curWeaponGrip.rotation) * weapon.transform.rotation;

        // Then, we align the socket rotation with that offset.
        Quaternion targetRotation = _parentAttachTransform.rotation * gripToWeaponRotationOffset;

        // Finally, apply user-defined offset rotation (if any).
        targetRotation *= offsetRotation;

        return targetRotation;
    }
    private void OnValidate()
    {
        if (this.weapon == null)
        {
            this.weapon = GetComponent<RangeWeapon>();
            base._attachAbleObject = this.weapon.transform;
        }
    }

    public static readonly float attatchingDurationGlobal = .75f;
}
