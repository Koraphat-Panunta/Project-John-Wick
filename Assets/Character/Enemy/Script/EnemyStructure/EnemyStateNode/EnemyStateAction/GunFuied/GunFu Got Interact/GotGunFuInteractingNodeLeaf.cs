using System;
using System.Collections.Generic;
using UnityEngine;

public class GotGunFuInteractingNodeLeaf : EnemyStateLeafNode
    , IGotGunFuAttackNode
    ,INodeLeafTransitionAble
{
    protected AnimationTriggerEventPlayer animationTriggerEventPlayer;

    public INodeManager nodeManager { get => this.enemy.stateManagerNode; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    public bool triggerReset;


    public GotGunFuInteractingNodeLeaf(Enemy enemy,AnimationTriggerEventSCRP animationTriggerEventSCRP, Func<bool> preCondition) : base(enemy, preCondition)
    {
        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(animationTriggerEventSCRP);
    }
    public override void Enter()
    {
        this.triggerReset = false;

        if (enemy.motionControlManager.curMotionState != enemy.motionControlManager.codeDrivenMotionState)
        {
            enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);
        }
        this.isComplete = false;
        this.animationTriggerEventPlayer.Rewind();
        _= SubjectAnimationInteract.DelayRootMotion(this.enemy);
        base.Enter();
    }
    public override void Exit()
    {
        this.triggerReset = false;
        this.enemy.enableRootMotion = false;
        base.Exit();
    }
    public override void UpdateNode()
    {
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
        if (this.animationTriggerEventPlayer.IsPlayFinish())
        {
            this.isComplete = true;
            this.nodeLeafTransitionBehavior.TransitionAbleAll(this);
            this.TransitioningCheck();
        }
        base.UpdateNode();
    }
    public override bool IsComplete()
    {
        return this.isComplete;
    }
    public override bool IsReset()
    {

        if (this.enemy._triggerEnterGotAttacked_OCM)
            return true;

        return IsComplete();
    }

    public void TriggerReset() => this.triggerReset = true;

    public bool TransitioningCheck() => this.nodeLeafTransitionBehavior.TransitioningCheck(this);
    public void AddTransitionNode(INode node) => this.nodeLeafTransitionBehavior.AddTransistionNode(this, node);
    
}
