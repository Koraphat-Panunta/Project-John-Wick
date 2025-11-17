using System;
using System.Collections.Generic;
using UnityEngine;

public class HumanShield_Exit_GotInteract_NodeLeaf : EnemyStateLeafNode, IGotGunFuAttackNode, INodeLeafTransitionAble
{

    public INodeManager nodeManager { get => enemy.enemyStateManagerNode; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }
    public AnimationTriggerEventPlayer animationTriggerEventPlayer {get;protected set; }
    private Animator animator;
    public HumanShield_Exit_GotInteract_NodeLeaf(Enemy enemy
        , Func<bool> preCondition
        ,AnimationTriggerEventSCRP animationTriggerEventSCRP
        ,Animator animator) : base(enemy, preCondition)
    {
        this.animator = animator;
        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(animationTriggerEventSCRP);
        this.animationTriggerEventPlayer.SubscribeEvent("Transition ragdoll", TriggerRagdoll);
    }
    public override void Enter()
    {
        this.enemy.enableRootMotion = true;
        this.animationTriggerEventPlayer.Rewind();
        this.nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        this.animator.CrossFade("GotRestrict Exit", AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration, 0);
        base.Enter();
    }
    public override void UpdateNode()
    {
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
        this.TransitioningCheck();
        base.UpdateNode();
    }
    public override void Exit()
    {
        this.enemy.enableRootMotion = false;
        base.Exit();
    }
    private void TriggerRagdoll()
    {
        this.enemy.enableRootMotion = false;
        this.nodeLeafTransitionBehavior.TransitionAbleAll(this);
        isComplete = true;
    }
    public override bool IsComplete()
    {
        return this.animationTriggerEventPlayer.IsPlayFinish();
    }
    public override bool IsReset()
    {
        if(enemy.isDead)
            return true;

        if(IsComplete())
            return true;

        return false;
    }
    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);
    public bool TransitioningCheck() => nodeLeafTransitionBehavior.TransitioningCheck(this);
    
}
