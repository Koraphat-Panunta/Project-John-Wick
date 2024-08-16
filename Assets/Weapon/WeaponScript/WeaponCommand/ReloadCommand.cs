using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadCommand : WeaponCommand
{
    AmmoProuch ammoProuch;
    public ReloadCommand(Weapon weapon,AmmoProuch ammoProuch) : base(weapon)
    {
        this.ammoProuch = ammoProuch;
    }

    public override void Execute()
    {
        if(weapon == null)
        {
            
        }
       weapon.Reload();
       this.ammoProuch.prochReload.Performed(base.weapon);
    }
}
