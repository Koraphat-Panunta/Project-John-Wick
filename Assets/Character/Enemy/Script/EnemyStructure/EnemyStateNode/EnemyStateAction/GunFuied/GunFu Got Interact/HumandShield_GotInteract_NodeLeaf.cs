using System;
using System.Collections.Generic;
using UnityEngine;
using static HumanShield_GunFu_NodeLeaf;
using static UnityEngine.EventSystems.EventTrigger;

public class HumandShield_GotInteract_NodeLeaf : EnemyStateLeafNode, IGotGunFuAttackNode, INodeLeafTransitionAble
{

    public INodeManager nodeManager { get => enemy.enemyStateManagerNode ; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get ; set ; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }
    private HumanShield_GunFu_NodeLeaf humanShield_GunFuInteraction_NodeLeaf;

    protected Animator animator;
    protected HumanShieldInteractionPhase interactionPhase;
    public HumandShield_GotInteract_NodeLeaf(
        Enemy enemy
        ,Func<bool> preCondition
        ,Animator animator) : base(enemy, preCondition)
    {

        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
        this.animator = animator;
    }


    public override void Enter()
    {
        humanShield_GunFuInteraction_NodeLeaf = enemy.gunFuAbleAttacker.curGunFuNode as HumanShield_GunFu_NodeLeaf;
        interactionPhase = humanShield_GunFuInteraction_NodeLeaf.curIntphase;
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        enemy.friendlyFirePreventingBehavior.DisableFriendlyFirePreventing();
        StateEnter();

        base.Enter();
    }

    public override void Exit()
    {
        enemy.friendlyFirePreventingBehavior.EnableFriendlyFirePreventing();
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }
    public override void UpdateNode()
    {
        switch (interactionPhase)
        {
            case HumanShieldInteractionPhase.Enter:
                {
                    this.interactionPhase = humanShield_GunFuInteraction_NodeLeaf.curIntphase;
                    if (interactionPhase == HumanShieldInteractionPhase.Stay)
                    {
                        StateStay();
                        enemy.enableRootMotion = false;
                    }
                }
                break;
            case HumanShieldInteractionPhase.Stay:
                {
                    this.interactionPhase = humanShield_GunFuInteraction_NodeLeaf.curIntphase;
                    if (interactionPhase == HumanShieldInteractionPhase.Exit)
                    {
                        Debug.Log("interactionPhase == HumanShieldInteractionPhase.Exit");
                        nodeLeafTransitionBehavior.TransitionAbleAll(this);
                        TransitioningCheck();
                        isComplete = true;
                    }
                }
                break;
            
           

        }
        TransitioningCheck();

        base.UpdateNode();
    }

    public void StateEnter()
    {
        enemy.enableRootMotion = true;
        animator.CrossFade(GotGunFuManuverStateName.GotHumanShield_Enter.ToString(),0, 0
            , humanShield_GunFuInteraction_NodeLeaf.animationInteractScriptableObject.animationInteractCharacterDetail[1].enterAnimationOffsetNormalizedTime);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);
        enemy.NotifyObserver(enemy, this);
    }
    public void StateStay()
    {
        animator.CrossFade(GotGunFuManuverStateName.GotHumanShield_Stay.ToString(), AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration, 0);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);
        enemy.NotifyObserver(enemy, this);
    }
   

    public override bool IsReset()
    {
        if (enemy._triggerHitedGunFu)
            return true;

        if (enemy.gunFuAbleAttacker.curGunFuNode 
            is HumanShield_GunFu_NodeLeaf == false)
            return true;

        if (IsComplete())
            return true;

        if (enemy.isDead)
            return true;

        return false;
    }

    public bool TransitioningCheck() => nodeLeafTransitionBehavior.TransitioningCheck(this);

    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);
    
}
