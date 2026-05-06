using UnityEngine;

public interface IGrabWeaponAble : IObjectGrabbedAble
{
    Transform weaponAttachingAbleTransform { get; }
    IWeaponAdvanceUser weaponAdvanceUser { get; }
    Weapon curWeaponAtSocket { get; }

    void Attatch(Weapon weapon);
    void Attatch(Weapon weapon,
        Vector3 additionalOffsetPosition,
        Quaternion additionalOffsetRotation,
        float attatchingDuration);
    void Detach();
}
