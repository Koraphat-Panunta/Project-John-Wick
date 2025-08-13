using System;
using UnityEngine;

public class OnUpdateMovementNodeLeaf : MovementNodeLeaf
{
    private MovementCompoent movementCompoent;
    public GravityMovement gravityMovement { get; private set; }
    public OnUpdateMovementNodeLeaf(Func<bool> preCondition,MovementCompoent movementCompoent) : base(preCondition)
    {
        gravityMovement = new GravityMovement();
        this.movementCompoent = movementCompoent;
    }

  
    public override void FixedUpdateNode()
    {
       
        Vector3 euler = movementCompoent.transform.rotation.eulerAngles;
        movementCompoent.transform.rotation = Quaternion.Euler(0f, euler.y, 0f);

        gravityMovement.GravityMovementUpdate(movementCompoent);    
        
        movementCompoent.Move(movementCompoent.curMoveVelocity_World * Time.deltaTime);

    }
}
