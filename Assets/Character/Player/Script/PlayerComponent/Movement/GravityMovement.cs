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
        float gravitySclae = 0.015f;

        if(enableGravity == false)
            return;

        if (movementCompoent.IsGround(out Vector3 hitGroundPos) == false)
        {
            Debug.Log(movementCompoent + "_velocityY = " + _velocityY);

            _velocityY += GRAVITY * gravitySclae;
            _velocityY = Mathf.Clamp(_velocityY, 0, 60);
            movementCompoent.curMoveVelocity_World = new Vector3(
                movementCompoent.curMoveVelocity_World.x
                ,- _velocityY
                , movementCompoent.curMoveVelocity_World.z
                );
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
