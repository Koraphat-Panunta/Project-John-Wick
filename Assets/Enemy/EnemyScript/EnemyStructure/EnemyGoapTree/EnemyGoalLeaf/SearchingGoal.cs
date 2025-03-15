using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static CombatOffensiveInstinct;

public class SearchingGoal : EnemyGoalLeaf
{
    private IFindingTarget findingTarget;
    float idleSeachTime;
    private Vector3 oriPos;
    private NavMeshAgent agent;
    public SearchingGoal(EnemyCommandAPI enemyController, IEnemyGOAP enemyGOAP,IFindingTarget findingTarget, Func<bool> preCondition, Action Enter, Action Exit, Action Update, Action FixedUpdate, Func<bool> isComplete, Func<bool> isReset) : base(enemyController, enemyGOAP, preCondition, Enter, Exit, Update, FixedUpdate, isComplete, isReset)
    {
        this.findingTarget = findingTarget;
        this.oriPos = enemyGOAP._enemy.transform.position;
        this.agent = enemy.agent;
        InitailizedActionNode();
    }

    public SearchingGoal(EnemyCommandAPI enemyController, IEnemyGOAP enemyGOAP, IFindingTarget findingTarget) : base(enemyController, enemyGOAP)
    {
        this.findingTarget = findingTarget;
        this.oriPos = enemyGOAP._enemy.transform.position;
        this.agent = enemy.agent;
        InitailizedActionNode();
    }

    public override List<EnemyGoal> childNode { get ; set ; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        idleSeachTime = 0;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (enemy.findingTargetComponent.FindTarget(out GameObject target))
            enemy.targetKnewPos = target.transform.position;

       

        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;

        if (enemy.findingTargetComponent.isSpottingTarget)
            return true;

        if (combatPhase != CombatOffensiveInstinct.CombatPhase.SemiAlert)
            return true;

        if(combatPhase != CombatOffensiveInstinct.CombatPhase.Suspect)
            return true;


        return false;
    }

    public override bool PreCondition()
    {
        CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;

        if(combatPhase == CombatOffensiveInstinct.CombatPhase.SemiAlert)
            return true;

        if(combatPhase == CombatOffensiveInstinct.CombatPhase.Suspect)
            return true;

        if (combatPhase == CombatOffensiveInstinct.CombatPhase.Chill)
            return true;

        return false;
    }
    private MoveToSuspectPos moveToSuspectPos;
    private Idle_and_LowReady idle_And_LowReady;
    protected override EnemyActionLeafNode enemyActionLeaf { get; set; }
    protected override EnemyActionSelectorNode startActionSelector { get; set; }

    protected override void InitailizedActionNode()
    {
        //startActionSelector = new EnemyActionSelectorNode(enemyController,()=>true);

        //moveToSuspectPos = new MoveToSuspectPos(enemyController, findingTarget,
        //    () => 
        //    {
        //        CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;
        //        float distance = (findingTarget.findingTargetComponent.suspectPos - findingTarget.userObj.transform.position).magnitude;

        //        if(combatPhase == CombatOffensiveInstinct.CombatPhase.SemiAlert
        //        && distance > 1.5f)
        //            return true;

        //        return false;
        //    }, //Precondition
        //    () => 
        //    {
        //        CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;
        //        float distance = (findingTarget.findingTargetComponent.suspectPos - findingTarget.userObj.transform.position).magnitude;

        //        if(combatPhase != CombatOffensiveInstinct.CombatPhase.SemiAlert
        //        || combatPhase != CombatOffensiveInstinct.CombatPhase.Suspect)
        //            return true;

        //        if(distance < 1.5f)
        //            return true;

        //        return false;
        //    } //Reset
        //    );

        //idle_And_LowReady = new Idle_and_LowReady(enemyController,
        //    () => true, //Precondition
        //    () =>
        //    {
        //        idleSeachTime += Time.deltaTime;

        //        if(idleSeachTime > 3)
        //            return true;

        //        return false;
        //    } //Reset
        //    );

        //startActionSelector.AddChildNode(moveToSuspectPos);
        //startActionSelector.AddChildNode(idle_And_LowReady);
    }
}
