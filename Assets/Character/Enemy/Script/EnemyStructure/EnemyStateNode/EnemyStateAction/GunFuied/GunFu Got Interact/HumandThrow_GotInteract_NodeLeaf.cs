using System;
using System.Collections.Generic;
using UnityEngine;

public class HumandThrow_GotInteract_NodeLeaf : GunFu_GotInteract_NodeLeaf
{
  
    private Animator animator;
    private HumanThrowGunFuInteractionNodeLeaf humanThrowGunFuInteractionNodeLeaf;
    private HumanThrowGunFuInteractionNodeLeaf.HumanThrowPhase throwPhase => humanThrowGunFuInteractionNodeLeaf.curThrowPhase;

    private HumanThrowFallDown_GotInteract_NodeLeaf humanThrowFallDown_GotInteract_NodeLeaf;
    public HumandThrow_GotInteract_NodeLeaf(Enemy enemy, Func<bool> preCondition,Animator animator) : base(enemy, preCondition)
    {
        this.animator = animator;
        humanThrowFallDown_GotInteract_NodeLeaf = new HumanThrowFallDown_GotInteract_NodeLeaf(enemy, enemy, ()=> true);
    }
    public override void Enter()
    {
        Debug.Log("Human Throw Got Interact Enter");
        humanThrowGunFuInteractionNodeLeaf = enemy.gunFuAbleAttacker.curGunFuNode as HumanThrowGunFuInteractionNodeLeaf;
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
        return base.IsComplete();
    }

    public override bool IsReset()
    {
        if(IsComplete())
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        if(throwPhase == HumanThrowGunFuInteractionNodeLeaf.HumanThrowPhase.beforeThrow)
        {

        }
        else if(throwPhase == HumanThrowGunFuInteractionNodeLeaf.HumanThrowPhase.Throwing)
        {
            humanThrowFallDown_GotInteract_NodeLeaf.GotThorwForce(enemy.transform.forward * 100 + (Vector3.up*10));
            enemy.enemyStateManagerNode.nodeManagerBehavior.ChangeNodeManual(enemy.enemyStateManagerNode, humanThrowFallDown_GotInteract_NodeLeaf);
            enemy.NotifyObserver(enemy, this);
        }
        base.UpdateNode();
    }
}
public class HumanThrowFallDown_GotInteract_NodeLeaf : FallDown_EnemyState_NodeLeaf
{
    private Vector3 forceThrow;
    public HumanThrowFallDown_GotInteract_NodeLeaf(Enemy enemy, IFallDownGetUpAble fallDownGetUpAble, Func<bool> preCondition) : base(enemy, fallDownGetUpAble, preCondition)
    {
    }
    public void GotThorwForce(Vector3 forceThrow)
    {
        this.forceThrow = forceThrow;
    }
    public Vector3 GetForceThrow() => this.forceThrow;
    public override void Enter()
    {
        enemy.NotifyObserver(enemy, this);
        base.Enter();
    }
    public override void UpdateNode()
    {
        base.UpdateNode();
    }
    
}
