using System;
using UnityEngine;

public class PlayerGetUpStateNodeLeaf : PlayerStateNodeLeaf
{

    private float _timer;
    public float getUpTime = .5f;
    protected PlayerMovement playerMovement => this.player.playerMovement;
    public PlayerGetUpStateNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {

    }

    public override void Enter()
    {
        this.player.playerStance = Stance.stand;
        _timer = 0;
        isComplete = false;
        (player._movementCompoent as MovementCompoent).CancleMomentum();
        this.player.enableRootMotion = true;
        base.Enter();
    }

    public override void Exit()
    {
        this.player.enableRootMotion = false;
        base.Exit();
    }

    public override void FixedUpdateNode()
    {

        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
       return isComplete;
    }

    public override bool IsReset()
    {
        if(player.isDead)
            return true;

        return IsComplete();
    }

    public override void UpdateNode()
    {
        this._timer += Time.deltaTime;

        if (this._timer >= this.getUpTime)
        {
            player.playerStance = Stance.stand;
            isComplete = true;
        }
            
        base.UpdateNode();
    }

}

