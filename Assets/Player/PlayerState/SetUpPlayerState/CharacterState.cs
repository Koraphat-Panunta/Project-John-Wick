using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class CharacterState 
{
    protected Player player;

    public abstract void EnterState();


    public abstract void ExitState();


    public abstract void FrameUpdateState(PlayerStateManager stateManager);

    public abstract void PhysicUpdateState(PlayerStateManager stateManager);
   
    protected virtual void InputPerformed()
    {

    }

   
}
