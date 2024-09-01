using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelTriggerCommand : WeaponCommand
{
    public CancelTriggerCommand(Weapon weapon) : base(weapon)
    {

    }
    public override void Execute()
    {
        weapon.triggerPull = Weapon.TriggerPull.Up;
    }
}
