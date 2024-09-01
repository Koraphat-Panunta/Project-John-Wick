using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityMovement : IMovementComponent
{
    private const float GRAVITY = 9.8f;
    public GravityMovement(CharacterController characterController) 
    {
        base.characterController = characterController;
    }
    public override void MovementUpdate(PlayerMovement playerMovement)
    {
        float gravitySclae = 0.005f;
        if (this.characterController.isGrounded == false)
        {
            _velocityY += GRAVITY * gravitySclae;
            _velocityY = Mathf.Clamp(_velocityY, 0, 1.3f);
            playerMovement.curVelocity_World -= new Vector3(0, _velocityY, 0);
            //Debug.Log("IsGround == false");
            //Debug.Log("Fall v =" + _velocityY);
        }
        else
        {
            _velocityY = 0;
        }
    }
    private float _velocityY = 0;
}
