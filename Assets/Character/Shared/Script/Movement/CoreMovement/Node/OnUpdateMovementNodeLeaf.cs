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

        movementCompoent.Move(movementCompoent.curMoveVelocity_World * Time.fixedDeltaTime);

    }
}
