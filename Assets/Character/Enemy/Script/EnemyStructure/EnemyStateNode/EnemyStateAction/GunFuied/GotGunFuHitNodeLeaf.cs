using System;
using System.Collections.Generic;
using UnityEngine;

public class GotGunFuHitNodeLeaf : EnemyStateLeafNode
    ,IGotGunFuAttackNode
    ,INodeLeafTransitionAble
{
    public IGunFuAble gunFuAble => enemy.gunFuAbleAttacker;
    public IGotGunFuAttackedAble gotGunFuAttackedAble => enemy;
    public float _painTime { get; set; }
    public float _timer { get; set; }
    float forceStop => this.enemy.breakDecelerate * .25f;
    public Dictionary<INode, bool> transitionAbleNode { get ; set ; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }
    public INodeManager nodeManager { get; set; }

    public enum GotHitPhase
    {
        Enter,
        Exit,
    }
    public GotHitPhase curGotHitPhase { get;protected set; }


    public GotGunFuHitNodeLeaf(Enemy enemy,INodeManager nodeManager,Func<bool> preCondition) : base(enemy,preCondition)
    {
       
        this.nodeManager = nodeManager;
        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
    }
    public override void Enter()
    {
        this.nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        this._timer = 0;
        this.enemy._movementCompoent.SetRotation(Quaternion.LookRotation((gunFuAble._character.transform.position - enemy.transform.position).normalized
            ,Vector3.up));
        this.curGotHitPhase = GotHitPhase.Enter;

        base.Enter();
    }
    public override void Exit()
    {
        this.curGotHitPhase = GotHitPhase.Exit;
        base.Exit();
    }
    public override void UpdateNode()
    {
        _timer += Time.deltaTime;

        if (_timer >= this._painTime)
        {
            isComplete = true;
            nodeLeafTransitionBehavior.TransitionAbleAll(this);
        }

        this.TransitioningCheck();

        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        this.enemy._movementCompoent.UpdateMoveToDirWorld(Vector3.zero, forceStop, MoveMode.MaintainMomentumDirection);    
        base.FixedUpdateNode();
    }

    public override bool IsReset()
    {
        if (IsComplete())
            return true;

        if(this.enemy.isDead)
            return true;

        if(this.enemy._isPainTrigger
            ||this.enemy._triggerHitedGunFu)
            return true;

        return false;
    }

    public void SetPainTime(float painTime)
    {
        this._painTime = painTime;
    }
    public bool TransitioningCheck()
    {
       return nodeLeafTransitionBehavior.TransitioningCheck(this);
    }

    public void AddTransitionNode(INode node)
    {
        nodeLeafTransitionBehavior.AddTransistionNode(this, node);
    }
}
