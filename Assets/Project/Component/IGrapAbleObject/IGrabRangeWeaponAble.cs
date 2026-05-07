using UnityEngine;

public interface IGrabRangeWeaponAble : IGrabAbleObject
{
    Transform weaponAttachingAbleTransform { get; }
    IRangeWeaponAdvanceUser weaponAdvanceUser { get; }
    RangeWeapon curRangeWeaponAtSocket { get; }

  
}
