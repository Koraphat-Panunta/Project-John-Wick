using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleasTriggerCommand : WeaponCommand
{
    public ReleasTriggerCommand(Weapon weapon) : base(weapon)
    {
    }

    public override void Execute()
    {
        weapon.triggerPull = Weapon.TriggerPull.Up;
    }
}
