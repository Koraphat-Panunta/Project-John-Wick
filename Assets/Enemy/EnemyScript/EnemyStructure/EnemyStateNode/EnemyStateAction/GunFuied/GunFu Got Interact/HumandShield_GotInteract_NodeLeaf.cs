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

    HumanShield_GunFuInteraction_NodeLeaf.InteractionPhase interactionPhase;

    public INodeManager nodeManager { get ; set ; }
    public Dictionary<INodeLeaf, bool> transitionAbleNode { get ; set ; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    public HumandShield_GotInteract_NodeLeaf(Enemy enemy,Func<bool> preCondition,Animator animator) : base(enemy, preCondition)
    {
        this.animator = animator;

        this.nodeManager = enemy.enemyStateManagerNode;
        this.transitionAbleNode = new Dictionary<INodeLeaf, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();

    }



    public override void Enter()
    {
        got_threwDown_time = 0;
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        StateEnter();

        base.Enter();
    }

    public override void Exit()
    {
        nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

   

    public override void UpdateNode()
    {
        Transitioning();

        if(interactionPhase == InteractionPhase.Exit)
        {
            got_threwDown_time += Time.deltaTime;
            if(got_threwDown_time >= 2.5f)
            {
                
            }
            else if(got_threwDown_time >= 0.7f)
            {
                enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.ragdollMotionState);
            }

        }
        base.UpdateNode();
    }

    public void StateEnter()
    {
        animator.CrossFade(stateNameEnter, 0.075f, 0);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        interactionPhase = InteractionPhase.Enter;

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuGotInteract);
    }
    public void StateStay()
    {
        animator.CrossFade(stateNameStay, 0.075f, 0);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        interactionPhase = InteractionPhase.Stay;

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuGotInteract);
    }
    public void StateExit()
    {
        animator.CrossFade(stateNameExit, 0.075f, 0);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        interactionPhase = InteractionPhase.Exit;

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuGotInteract);
        enemy._posture = 0;
    }

    public bool Transitioning() => nodeLeafTransitionBehavior.Transitioning(this);

    public void AddTransitionNode(INodeLeaf nodeLeaf) => nodeLeafTransitionBehavior.AddTransistionNode(this, nodeLeaf);
    
}
