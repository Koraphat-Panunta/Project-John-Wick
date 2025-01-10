using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMovementComponent 
{
    public CharacterController characterController { get;protected set; }
    public bool isEnable = true;
    public abstract void MovementUpdate(PlayerMovement playerMovement);
}
