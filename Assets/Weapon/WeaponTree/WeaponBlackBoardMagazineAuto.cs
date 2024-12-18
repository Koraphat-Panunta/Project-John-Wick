using UnityEngine;

public class WeaponBlackBoardMagazineAuto : WeaponBlackBoard
{

    private MagazineType weaponMag;
    public bool IsMagin { get => weaponMag.isMagIn; }
    public WeaponBlackBoardMagazineAuto(Weapon weapon) : base(weapon)
    {
        weaponMag = weapon as MagazineType;
    }
}
