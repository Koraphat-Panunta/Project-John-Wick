using System;
using System.Collections.Generic;
using UnityEngine;
using static HumanShield_GunFuInteraction_NodeLeaf;

public class HumandShield_GotInteract_NodeLeaf : GunFu_GotInteract_NodeLeaf,INodeLeafTransitionAble
{
    public Animator animator;
    public string stateNameEnter = "Got HumandShielded Enter";
    public string stateNameStay = "Got HumandShielded Stay";
    public string stateNameExit = "Got HumandShielded Throw";

    float got_threwDown_time = 0;

    HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase interactionPhase => humanShield_GunFuInteraction_NodeLeaf.curIntphase;

    public INodeManager nodeManager { get => enemy.enemyStateManagerNode ; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get ; set ; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }
    private HumanShield_GunFuInteraction_NodeLeaf humanShield_GunFuInteraction_NodeLeaf;

    public HumandShield_GotInteract_NodeLeaf(Enemy enemy,Func<bool> preCondition,Animator animator) : base(enemy, preCondition)
    {
        this.animator = animator;

        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();

    }


    public override void Enter()
    {
        isStayOnEnter = true;
        isExitOnEnter = true;
        humanShield_GunFuInteraction_NodeLeaf = enemy.gunFuAbleAttacker.curGunFuNode as HumanShield_GunFuInteraction_NodeLeaf;
        got_threwDown_time = 0;
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


    private bool isStayOnEnter;
    private bool isExitOnEnter;
    public override void UpdateNode()
    {

        switch (interactionPhase)
        {
            case HumanShieldInteractionPhase.Enter:
                {

                }
                break;
            case HumanShieldInteractionPhase.Stay:
                {
                    if (isStayOnEnter)
                    {
                        StateStay();
                    }
                }
                break;
            case HumanShieldInteractionPhase.Exit:
                {

                }
                break;
            case HumanShieldInteractionPhase.ExitAttacked:
                {
                    if (isExitOnEnter)
                    {
                        StateExit();
                    }
                    enemy._isPainTrigger = true;
                }
                break;

        }
        TransitioningCheck();

        base.UpdateNode();
    }

    public void StateEnter()
    {
        animator.CrossFade(stateNameEnter, 0.075f, 0);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        enemy.NotifyObserver(enemy, this);
    }
    public void StateStay()
    {
        animator.CrossFade(stateNameStay, 0.075f, 0);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);
        enemy._posture = 0;
        enemy.NotifyObserver(enemy, this);
    }
    public void StateExit()
    {
        animator.CrossFade(stateNameExit, 0.075f, 0,.57f);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);
        nodeLeafTransitionBehavior.TransitionAbleAll(this);
        enemy.NotifyObserver(enemy, this);

    }

    public override bool IsReset()
    {
        if (enemy._triggerHitedGunFu)
            return true;

        if (enemy.gunFuAbleAttacker.curGunFuNode 
            is HumanShield_GunFuInteraction_NodeLeaf == false)
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
