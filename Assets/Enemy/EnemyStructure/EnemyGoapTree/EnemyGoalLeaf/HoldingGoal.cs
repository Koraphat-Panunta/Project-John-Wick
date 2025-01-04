using System;
using System.Collections.Generic;
using UnityEngine;

public class HoldingGoal : EnemyGoalLeaf
{
    public HoldingGoal(EnemyControllerAPI enemyController, IEnemyGOAP enemyGOAP,IFindingTarget findingTarget) : base(enemyController, enemyGOAP)
    {

    }
    public override List<EnemyGoal> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }
   

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if(enemy.cost*enemy.strength >  (100/enemy.cost)*enemy.intelligent )
            return true;

        return false;
    }

    public override bool PreCondition()
    {
        CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;
        if (100 / enemy.cost * enemy.intelligent <= enemy.cost * enemy.strength)
            return false;

        if (combatPhase != CombatOffensiveInstinct.CombatPhase.FullAlert)
            return false;

        if (combatPhase != CombatOffensiveInstinct.CombatPhase.Alert)
            return false;


        return true;
    }

    public override void Update()
    {
        if (enemy.findingTargetComponent.FindTarget(out GameObject target))
            enemy.targetKnewPos = target.transform.position;

        base.Update();
    }

    private Idle_and_Shoot idle_And_Shoot;
    private Idle_and_Aim idle_And_Aim;
    private MoveCurve_and_Aim moveCurve_and_Aim;
    protected override EnemyActionLeafNode enemyActionLeaf { get; set; }
    protected override EnemyActionSelectorNode startActionSelector { get; set; }
    protected override void InitailizedActionNode()
    {
        startActionSelector = new EnemyActionSelectorNode(enemyController, () => true);

        idle_And_Shoot = new Idle_and_Shoot(enemyController, 
            () => 
            {
                CombatOffensiveInstinct.CombatPhase combatPhase 
                = enemy.combatOffensiveInstinct.myCombatPhase;

                switch (combatPhase)
                {
                    case CombatOffensiveInstinct.CombatPhase.FullAlert:
                        return true;
                    case CombatOffensiveInstinct.CombatPhase.Alert:
                        return true;
                }
                return false;

            }, //PreCondition
            () => 
            { 
                if(enemy.combatOffensiveInstinct.myCombatPhase == CombatOffensiveInstinct.CombatPhase.SemiAlert)
                    return true;

                return false;
            } //Reset
            );

        idle_And_Aim = new Idle_and_Aim(enemyController,
            () =>
            {
                if (enemy.combatOffensiveInstinct.myCombatPhase
                == CombatOffensiveInstinct.CombatPhase.SemiAlert)
                    return true;

                return false;
            },//PreCondition
            () =>
            {
                CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;

                if (combatPhase == CombatOffensiveInstinct.CombatPhase.SemiAlert)
                    return true;

                return false;
            } //Reset
            );

        moveCurve_and_Aim = new MoveCurve_and_Aim(enemyController,
            () => true, //PreCondition
            () => 
            {
                CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;
                if(combatPhase == CombatOffensiveInstinct.CombatPhase.FullAlert)
                    return true;

                if(combatPhase == CombatOffensiveInstinct.CombatPhase.Alert)
                    return true;

                if(combatPhase == CombatOffensiveInstinct.CombatPhase.SemiAlert)
                    return true;

                return false;
            } //Reset
            );

        startActionSelector.AddChildNode(idle_And_Shoot);
        startActionSelector.AddChildNode(idle_And_Aim);
        startActionSelector.AddChildNode(moveCurve_and_Aim);
    }
}
