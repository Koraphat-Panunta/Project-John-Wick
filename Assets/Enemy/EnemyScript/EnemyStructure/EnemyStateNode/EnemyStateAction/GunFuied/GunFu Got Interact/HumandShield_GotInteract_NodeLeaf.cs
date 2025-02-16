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

    float got_threwDown_time;

    HumanShield_GunFuInteraction_NodeLeaf.InteractionPhase interactionPhase => humanShield_GunFuInteraction_NodeLeaf.curIntphase;

    public INodeManager nodeManager { get => enemy.enemyStateManagerNode ; set { } }
    public Dictionary<INodeLeaf, bool> transitionAbleNode { get ; set ; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }
    private HumanShield_GunFuInteraction_NodeLeaf humanShield_GunFuInteraction_NodeLeaf;

    public HumandShield_GotInteract_NodeLeaf(Enemy enemy,Func<bool> preCondition,Animator animator) : base(enemy, preCondition)
    {
        this.animator = animator;

        this.transitionAbleNode = new Dictionary<INodeLeaf, bool>();
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
        Debug.Log("HumanShield Got Interact Exit");
        Debug.Log("Enemy curAttackedNodeleafGunFu = " + enemy.curGotAttackedGunFuNode);
        Debug.Log("trigger GunFu ATK = " + enemy._triggerHitedGunFu);
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
        Debug.Log("HumanShield Got Interact Update");
        Transitioning();
        if(interactionPhase == InteractionPhase.Enter)
        {

        }
        else if(interactionPhase == InteractionPhase.Stay)
        {
            if (isStayOnEnter)
            {
                StateStay();
                isStayOnEnter = false;
                nodeLeafTransitionBehavior.TransitionAbleAll(this);
            }

        }
        else if(interactionPhase == InteractionPhase.Release
            || enemy.gunFuAbleAttacker.curGunFuNode is HumandShield_GotInteract_NodeLeaf == false)
            isComplete = true;

        base.UpdateNode();
    }

    public void StateEnter()
    {
        animator.CrossFade(stateNameEnter, 0.075f, 0);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuGotInteract);
    }
    public void StateStay()
    {
        animator.CrossFade(stateNameStay, 0.075f, 0);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuGotInteract);
    }
    public void StateExit()
    {
        animator.CrossFade(stateNameExit, 0.075f, 0);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuGotInteract);
        enemy._posture = 0;
    }

    public bool Transitioning() => nodeLeafTransitionBehavior.Transitioning(this);

    public void AddTransitionNode(INodeLeaf nodeLeaf) => nodeLeafTransitionBehavior.AddTransistionNode(this, nodeLeaf);
    
}
