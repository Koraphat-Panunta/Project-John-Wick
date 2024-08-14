using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowReadyCommand : WeaponCommand
{
    public LowReadyCommand(Weapon weapon) : base(weapon)
    {
    }
    public override void Execute()
    {
        weapon.LowWeapon();
    }
}
