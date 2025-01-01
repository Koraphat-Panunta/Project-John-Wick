using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyStateActionNode : EnemyStateNode
{
    public override List<EnemyStateNode> childNode { get; set; }
    protected override Func<bool> preCondidtion { get; set; }
    protected Action enter;
    protected Action exit;
    protected Action update;
    protected Action fixedUpdate;
    protected Func<bool> isReset;
    protected Func<bool> isComplete;


    public EnemyStateActionNode(Enemy enemy) : base(enemy)
    {
    }
    public EnemyStateActionNode(Enemy enemy
        , Func<bool> preCondition
        , Action Enter
        , Action Exit
        , Action Update
        , Action FixedUpdate
        , Func<bool> isComplete
        , Func<bool> isReset) : base(enemy)
    {
        this.preCondidtion = preCondition;
        this.enter = Enter;
        this.exit = Exit;
        this.update = Update;
        this.fixedUpdate = FixedUpdate;
        this.isComplete = isComplete;
        this.isReset = isReset;
    }
    public EnemyStateActionNode(Enemy enemy
       , Func<bool> preCondition
       , Func<bool> isReset) : base(enemy)
    {
        this.preCondidtion = preCondition;
        this.isReset = isReset;
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
