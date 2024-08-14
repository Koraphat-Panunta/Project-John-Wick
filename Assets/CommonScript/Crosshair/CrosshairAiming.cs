using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairAiming : ICrosshairAction
{
    private CrosshairController _crosshairController;
    public CrosshairAiming(CrosshairController crosshairController)
    {
        this._crosshairController = crosshairController;
    }
    public void Performed(Weapon weapon)
    {
        
    }

    public void Performed(PlayerStateManager playerStateManager)
    {
        
    }

   
}
