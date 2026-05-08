using UnityEngine;

public interface IGrabMeleeWeaponAble : IGrabAbleObject
{
    public IMeleeWeaponUserAble _meleeWeaponUser { get; }
    public MeleeWeapon curMeleeWeapon { get; }

    public static float grabDuration = .5f;
}
