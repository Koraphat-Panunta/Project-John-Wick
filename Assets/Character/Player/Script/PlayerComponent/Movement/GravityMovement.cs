using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityMovement 
{
    private const float GRAVITY = 9.8f;
    private bool enableGravity;
    public GravityMovement() 
    {
        enableGravity = true;
    }
    public  void GravityMovementUpdate(MovementCompoent movementCompoent)
    {
        float gravitySclae = 0.005f;

        if(enableGravity == false)
            return;

        if (movementCompoent.isGround == false)
        {
            _velocityY += GRAVITY * gravitySclae;
            _velocityY = Mathf.Clamp(_velocityY, 0, 1.3f);
            movementCompoent.curMoveVelocity_World -= new Vector3(0, _velocityY, 0);
        }
        else
        {
            _velocityY = 0;
        }
    }
    public void EnableGravity() => enableGravity = true;
    public void DisableGravity() => enableGravity = false;

    private float _velocityY = 0;
}
