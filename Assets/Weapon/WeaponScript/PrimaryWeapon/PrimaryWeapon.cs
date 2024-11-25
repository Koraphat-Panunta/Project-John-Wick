using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PrimaryWeapon : Weapon
{
    public abstract Transform forntGrip { get; set; }
    public abstract Transform slingAnchor { get; set; }
   
}
