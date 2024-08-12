using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadCommand : WeaponCommand
{
    public ReloadCommand(Weapon weapon) : base(weapon)
    {
    }

    public override void Execute()
    {
       weapon.Reload();
    }
}
