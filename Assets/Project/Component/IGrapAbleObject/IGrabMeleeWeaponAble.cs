using UnityEngine;

public interface IGrabMeleeWeaponAble : IGrabAbleObject
{
    public IMeleeWeaponUserAble _meleeWeaponUser { get; }
    public MeleeWeapon curMeleeWeapon { get; }
}
