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
        bool isReload = Weapon.isReloadCommand;
        Weapon.isReloadCommand = false;

        return Weapon.bulletStore[BulletStackType.Magazine] < Weapon.Magazine_capacity && isReload;
    }

}
