using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCommand : Icommand
{
    protected Weapon weapon;
    public WeaponCommand(Weapon weapon)
    {
        this.weapon = weapon;
    }
    public virtual void Execute()
    {
       
    }

   
}
