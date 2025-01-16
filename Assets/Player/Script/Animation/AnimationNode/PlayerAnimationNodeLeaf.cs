using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationNodeLeaf : PlayerAnimationNode
{

    public override List<PlayerAnimationNode> childNode { get; set; }
    protected override Func<bool> preCondidtion { get; set; }
    protected Action enter;
    protected Action exit;
    protected Action update;
    protected Action fixedUpdate;
    protected Func<bool> isReset;
    protected Func<bool> isComplete;



    public PlayerAnimationNodeLeaf(PlayerAnimationManager playerAnimationManager,Animator animator) : base(playerAnimationManager,animator)
    {
    }
    public PlayerAnimationNodeLeaf(PlayerAnimationManager playerAnimationManager, Animator animator
        , Func<bool> preCondition
        , Action Enter
        , Action Exit
        , Action Update
        , Action FixedUpdate
        , Func<bool> isComplete
        , Func<bool> isReset) : base(playerAnimationManager, animator)
    {
        this.preCondidtion = preCondition;
        this.enter = Enter;
        this.exit = Exit;
        this.update = Update;
        this.fixedUpdate = FixedUpdate;
        this.isComplete = isComplete;
        this.isReset = isReset;
    }

    public PlayerAnimationNodeLeaf(PlayerAnimationManager playerAnimationManager, Animator animator, Func<bool> preCondition) : base(playerAnimationManager, animator)
    {
        this.preCondidtion = preCondition;
    }

    public virtual void Enter()
    {
        if (enter != null)
            enter.Invoke();
    }

    public virtual void Exit()
    {
        if (exit != null)
            exit.Invoke();
    }

    public override bool IsReset()
    {
        return isReset.Invoke();
    }

    public override bool PreCondition()
    {
        return preCondidtion.Invoke();
    }

    public override void Update()
    {
        if (update != null)
            update.Invoke();
    }

    public override void FixedUpdate()
    {
        if (fixedUpdate != null)
            fixedUpdate.Invoke();
    }
}
