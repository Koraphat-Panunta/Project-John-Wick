using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSightCommand : WeaponCommand
{
    public AimDownSightCommand(Weapon weapon) : base(weapon)
    {
    }
    public override void Execute()
    {
        weapon.Aim();
    }
}
