using System;
using System.Collections.Generic;
using UnityEngine;
using static HumanShield_GunFuInteraction_NodeLeaf;

public class HumandShield_GotInteract_NodeLeaf : GunFu_GotInteract_NodeLeaf,INodeLeafTransitionAble
{
    public Animator animator;
    public string stateNameEnter = "HumandShielded Enter";
    public string stateNameStay = "HumandShielded Stay";
    public string stateNameExit = "HumandShielded Throw";

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
        humanShield_GunFuInteraction_NodeLeaf = enemy.gunFuAbleAttacker.curGunFuNode as HumanShield_GunFuInteraction_NodeLeaf;
        got_threwDown_time = 0;
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        enemy.friendlyFirePreventingBehavior.DisableFriendlyFirePreventing();
        StateEnter();

        base.Enter();
    }

    public override void Exit()
    {
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        enemy.friendlyFirePreventingBehavior.EnableFriendlyFirePreventing();
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }


    private bool isStayOnEnter;
    public override void UpdateNode()
    {
        Transitioning();
        if(interactionPhase == HumanShieldInteractionPhase.Enter)
        {

        }
        else if(interactionPhase == HumanShieldInteractionPhase.Stay)
        {
            if (isStayOnEnter)
            {
                StateStay();
                isStayOnEnter = false;
                nodeLeafTransitionBehavior.TransitionAbleAll(this);
            }

        }
        else if(interactionPhase == HumanShieldInteractionPhase.Exit
            || enemy.gunFuAbleAttacker.curGunFuNode is HumandShield_GotInteract_NodeLeaf == false)
            isComplete = true;

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

        enemy.NotifyObserver(enemy, this);
    }
    public void StateExit()
    {
        animator.CrossFade(stateNameExit, 0.075f, 0);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        enemy.NotifyObserver(enemy, this);
        enemy._posture = 0;
    }

    public bool Transitioning() => nodeLeafTransitionBehavior.Transitioning(this);

    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);
    
}
