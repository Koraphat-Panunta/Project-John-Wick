using System;
using UnityEngine;

public class ReloadStageSelector : WeaponSelector
{
    MagazineType magazineType;

    public ReloadStageSelector(Weapon weapon, Func<bool> preCondition) : base(weapon, preCondition)
    {
        magazineType = weapon as MagazineType;
    }
    //public override bool Precondition()
    //{
    //    bool isReload = Weapon.isReloadCommand;
    //    Weapon.isReloadCommand = false;

    //    return Weapon.bulletStore[BulletStackType.Magazine] < Weapon.bulletCapacity && isReload;
    //}

}
