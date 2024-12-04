using UnityEngine;

public class ReloadStageSelector : WeaponSelector
{
    MagazineType magazineType;
    public ReloadStageSelector(Weapon weapon) : base(weapon)
    {
        magazineType = weapon as MagazineType;
    }
    public override bool PreCondition()
    {
        Weapon.isReloadCommand = false;
        return Weapon.bulletStore[BulletStackType.Magazine] < Weapon.Magazine_capacity;
    }

}
