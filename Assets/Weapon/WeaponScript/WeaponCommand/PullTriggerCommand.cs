using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullTriggerCommand : WeaponCommand
{
    public PullTriggerCommand(Weapon weapon) : base(weapon)
    {
    }
    public override void Execute()
    {
        if (weapon.weapon_stateManager._currentState != weapon.weapon_stateManager.reloadState)
        {
            if (weapon.triggerPull == Weapon.TriggerPull.Up)
            {
                weapon.triggerPull = Weapon.TriggerPull.IsDown;
            }
            else if (weapon.triggerPull == Weapon.TriggerPull.IsDown)
            {
                weapon.triggerPull = Weapon.TriggerPull.Down;
            }
            weapon.Fire();
        }
    }
    public void TriggerCancel()
    {
        weapon.triggerPull = Weapon.TriggerPull.Up;
    }
}
