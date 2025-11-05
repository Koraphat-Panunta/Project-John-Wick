using UnityEngine;

public class WeaponAttacherComponent : AttacherComponent
{
    [SerializeField] Weapon weapon;
    public Transform curWeaponGrip;
    public Transform mainHandGrip => weapon._mainHandGripTransform;
    public Transform secondHandGrip => weapon._SecondHandGripTransform;

    public void Attach(Transform weaponSocket,Transform weaponGrip,Vector3 offsetPosition,Quaternion offsetRotation)
    {
        this.curWeaponGrip = weaponGrip;
        base.Attach(weaponSocket, offsetPosition, offsetRotation);
    }
    public override void Detach()
    {
        this.curWeaponGrip = null;
        base.Detach();
    }
    public override Vector3 GetAttachPosition()
    {
        if (parentAttachTransform == null || curWeaponGrip == null)
            return base._attachAbleObject.position;

        // Calculate how far the grip is from the weapon’s origin in world space.
        Vector3 gripToWeaponOffset = weapon.transform.position - curWeaponGrip.position;

        // Start with the socket position, then apply the offset in socket's local space.
        Vector3 socketPosition = parentAttachTransform.position
            + (parentAttachTransform.right * offsetPosition.x)
            + (parentAttachTransform.up * offsetPosition.y)
            + (parentAttachTransform.forward * offsetPosition.z);

        // Apply the offset so that the weapon’s grip aligns perfectly with the socket.
        return socketPosition + gripToWeaponOffset;
    }
    public override Quaternion GetAttachRotation()
    {
        if (parentAttachTransform == null || curWeaponGrip == null)
            return base._attachAbleObject.rotation;

        // This aligns the grip's rotation to match the socket’s rotation.
        // We first calculate the relative rotation difference between weapon and grip.
        Quaternion gripToWeaponRotationOffset = Quaternion.Inverse(curWeaponGrip.rotation) * weapon.transform.rotation;

        // Then, we align the socket rotation with that offset.
        Quaternion targetRotation = parentAttachTransform.rotation * gripToWeaponRotationOffset;

        // Finally, apply user-defined offset rotation (if any).
        targetRotation *= offsetRotation;

        return targetRotation;
    }
    private void OnValidate()
    {
        if (this.weapon == null)
        {
            this.weapon = GetComponent<Weapon>();
            base._attachAbleObject = this.weapon.transform;
        }
    }
}
