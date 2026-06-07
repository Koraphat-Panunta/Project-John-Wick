using UnityEngine;

public class AttachWeaponToLeftHandEvent
{
    private readonly RangeWeapon _weapon;
    private readonly Transform _referenceTransform;
    private readonly TransformOffsetSCRP _offsetSCRP;

    public AttachWeaponToLeftHandEvent(RangeWeapon weapon, Transform referenceTransform, TransformOffsetSCRP offsetSCRP)
    {
        _weapon = weapon;
        _referenceTransform = referenceTransform;
        _offsetSCRP = offsetSCRP;
    }

    public void Attach()
    {
        if (_weapon == null || _referenceTransform == null) return;
        if (_weapon.curAttatch != null)
            _weapon.curAttatch.GrabDetach();

        _weapon.WeaponAttacherComponent.Attach(
            _referenceTransform,
            _weapon._SecondHandGripTransform,
            _offsetSCRP != null ? _offsetSCRP.postitionOffset : Vector3.zero,
            _offsetSCRP != null ? Quaternion.Euler(_offsetSCRP.rotationEulerOffset) : Quaternion.identity,
            WeaponMountComponent.attatchingDurationGlobal);
    }
}
