using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyActionLeafNode : EnemyActionNode
{
    public override List<EnemyActionNode> childNode { get; set; }
    protected override Func<bool> preCondidtion { get; set; }
    protected override Func<float> getCost { get ; set ; }

    protected Action enter;
    protected Action exit;
    protected Action update;
    protected Action fixedUpdate;
    protected Func<bool> isReset;
    protected Func<bool> isComplete;


    public EnemyActionLeafNode(EnemyControllerAPI enemyController) : base(enemyController)
    {
    }
    public EnemyActionLeafNode(EnemyControllerAPI enemyController
        , Func<bool> preCondition
        , Func<float> getCost
        , Action Enter
        , Action Exit
        , Action Update
        , Action FixedUpdate
        , Func<bool> isComplete
        , Func<bool> isReset) : base(enemyController)
    {
        this.preCondidtion = preCondition;
        this.getCost = getCost;
        this.enter = Enter;
        this.exit = Exit;
        this.update = Update;
        this.fixedUpdate = FixedUpdate;
        this.isComplete = isComplete;
        this.isReset = isReset;
    }
    public EnemyActionLeafNode(EnemyControllerAPI enemyController
       , Func<bool> preCondition
       , Func<float> getCost
       , Func<bool> isReset) : base(enemyController)
    {
        this.preCondidtion = preCondition;
        this.getCost = getCost;
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
    public override float GetCost()
    {
        return base.GetCost();
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
