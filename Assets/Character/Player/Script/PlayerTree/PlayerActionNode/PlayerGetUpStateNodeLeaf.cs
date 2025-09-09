using System;
using UnityEngine;

public class PlayerGetUpStateNodeLeaf : PlayerStateNodeLeaf
{
    private PlayerGetUpStateScriptableObject playerGetUpStateScriptable;
    private AnimationClip animationClip => playerGetUpStateScriptable.animationClip;
    private float _timer;
    public PlayerGetUpStateNodeLeaf(PlayerGetUpStateScriptableObject playerGetUpStateScriptableObject, Player player, Func<bool> preCondition) : base(player, preCondition)
    {
        this.playerGetUpStateScriptable = playerGetUpStateScriptableObject;
    }

    public override void Enter()
    {
        _timer = 0;
        isComplete = false;
        (player._movementCompoent as MovementCompoent).CancleMomentum();
        player.NotifyObserver(player,this);
        base.Enter();
    }

    public override void Exit()
    {
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

        if (this._timer >= this.animationClip.length)
        {
            player.playerStance = Stance.stand;
            isComplete = true;
        }
            
        base.UpdateNode();
    }
}

