using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PrimaryWeapon 
{
    public abstract Transform forntGrip { get; set; }
    public abstract Transform slingAnchor { get; set; }
   
}