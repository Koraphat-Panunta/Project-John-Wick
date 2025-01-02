using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyGoalLeaf : EnemyGoal
{
    protected abstract EnemyActionLeafNode enemyActionLeaf { get; set; }
    protected abstract EnemyActionSelectorNode startActionSelector { get; set; }
    public override List<EnemyGoal> childNode { get; set; }
    protected override Func<bool> preCondidtion { get; set; }
    protected override Func<float> getCost { get ; set ; }

    protected Action enter;
    protected Action exit;
    protected Action update;
    protected Action fixedUpdate;
    protected Func<bool> isReset;
    protected Func<bool> isComplete;


    public EnemyGoalLeaf(EnemyControllerAPI enemyController, IEnemyGOAP enemyGOAP) : base(enemyController, enemyGOAP)
    {
    }
    public EnemyGoalLeaf(EnemyControllerAPI enemyController
        , IEnemyGOAP enemyGOAP
        , Func<bool> preCondition
        , Func<float> getCost
        , Action Enter
        , Action Exit
        , Action Update
        , Action FixedUpdate
        , Func<bool> isComplete
        , Func<bool> isReset) : base(enemyController, enemyGOAP)
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

    public override float GetCost()
    {
        return getCost.Invoke();
    }

    public abstract void ActionUpdate();
    public abstract void ActionFixedUpdate();
    protected abstract void InitailizedActionNode();
}
