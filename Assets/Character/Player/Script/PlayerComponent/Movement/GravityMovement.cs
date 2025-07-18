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

        if (movementCompoent.IsGround(out Vector3 hitGroundPos) == false)
        {
            _velocityY += GRAVITY * gravitySclae;
            _velocityY = Mathf.Clamp(_velocityY, 0, 1.3f);
            movementCompoent.curMoveVelocity_World -= new Vector3(0, _velocityY, 0);
        }
        else
        {
            movementCompoent.SetPosition(new Vector3
                (movementCompoent.transform.position.x, 
                hitGroundPos.y,
                movementCompoent.transform.position.z)
                );
            _velocityY = 0;
        }
    }
    public void EnableGravity() => enableGravity = true;
    public void DisableGravity() => enableGravity = false;

    private float _velocityY = 0;
}
