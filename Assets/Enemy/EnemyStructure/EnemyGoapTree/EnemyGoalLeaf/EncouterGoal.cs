using System;
using System.Collections.Generic;
using UnityEngine;
using static CombatOffensiveInstinct;

public class EncouterGoal : EnemyGoalLeaf
{

    public override List<EnemyGoal> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public EncouterGoal(EnemyControllerAPI enemyController, IEnemyGOAP enemyGOAP,IFindingTarget findingTarget) : base(enemyController, enemyGOAP)
    {
        InitailizedActionNode();
    }
   
   
    public override void Enter()
    {
       

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
        if (enemyActionLeaf != null)
            enemyActionLeaf.FixedUpdate();

        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;

        if(combatPhase == CombatOffensiveInstinct.CombatPhase.Chill
            || combatPhase == CombatOffensiveInstinct.CombatPhase.Suspect)
            return true;

        if (enemy.strength * enemy.cost < enemy.intelligent * (100/enemy.cost))
            return true;

        else return false;
    }

    public override bool PreCondition()
    {
        CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;

        if (combatPhase == CombatOffensiveInstinct.CombatPhase.FullAlert 
            || combatPhase == CombatOffensiveInstinct.CombatPhase.Alert
            || combatPhase == CombatOffensiveInstinct.CombatPhase.SemiAlert)
        return true;
        
        else return false;
    } 

    #region InitailiedActionNode
   

    private MoveCurve_and_Aim moveCurve_And_Aim;
    private MoveCurve_and_Shoot moveCurve_And_Shoot;
    private Idle_and_Shoot idle_And_Shoot;
    private Idle_and_Aim idle_And_Aim;


    private EnemyActionSelectorNode moveSelector;
    private EnemyActionSelectorNode idleSelector;

    protected override EnemyActionLeafNode enemyActionLeaf { get; set; }
    protected override EnemyActionSelectorNode startActionSelector { get ; set ; }

    protected override void InitailizedActionNode()
    {
        startActionSelector = new EnemyActionSelectorNode(enemyController,()=>true);

        moveSelector = new EnemyActionSelectorNode(enemyController, 
            () =>
        {
            float distance = (enemy.targetKnewPos - enemy.transform.position).magnitude;
            if (distance > 5.5f * (enemy.intelligent / enemy.cost))
                return true;
            else
                return false;
        } );//PreCondition
        idleSelector = new EnemyActionSelectorNode(enemyController,
            () => 
            {
                return true;
            });//PreCondition

        this.moveCurve_And_Aim = new MoveCurve_and_Aim(enemyController
            , () =>
            {
                return true;
            }  // PreCondition
            , () =>
            {
                float distance = (enemy.targetKnewPos - enemy.transform.position).magnitude;
                CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;

                if (distance > 5.5f * (enemy.intelligent / enemy.cost))
                    return false;

                if (combatPhase == CombatOffensiveInstinct.CombatPhase.FullAlert
                || combatPhase == CombatOffensiveInstinct.CombatPhase.Alert)
                    return true;

                return true;

            }//Reset
            );
        this.moveCurve_And_Shoot = new MoveCurve_and_Shoot(enemyController,
            () =>
            {
                CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;

                if(combatPhase == CombatOffensiveInstinct.CombatPhase.FullAlert
                ||combatPhase == CombatOffensiveInstinct.CombatPhase.Alert)
                    return true;
                else 
                    return false;
            }, //PreCondition
            () =>
            {
                CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;

                float distance = (enemy.targetKnewPos - enemy.transform.position).magnitude;

                if(distance > 5.5f *( enemy.intelligent/enemy.cost))
                    return false;

                if (combatPhase == CombatOffensiveInstinct.CombatPhase.FullAlert)
                    return false;

                if(combatPhase == CombatOffensiveInstinct.CombatPhase.Alert)
                    return false;
               
                return true;
            } //Reset
            );
        this.idle_And_Shoot = new Idle_and_Shoot(enemyController,
            () =>
            {
                CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;

                if (combatPhase == CombatPhase.FullAlert)
                    return true;

                if (combatPhase == CombatPhase.Alert)
                    return true;

                return false;
            },//PreCondition
            () => 
            {
                CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;
                float distance = (enemy.targetKnewPos-enemy.transform.position).magnitude;

                if(combatPhase == CombatPhase.FullAlert)
                    return false;

                if(combatPhase == CombatPhase.Alert)
                    return false;

                if(distance > 5.5f * enemy.intelligent / enemy.cost)
                    return true;

                return true;
            });//Reset
        this.idle_And_Aim = new Idle_and_Aim(enemyController,
            () =>
            {
                return true;
            }, //PreCondition
            () =>
            {
                CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;
                float distance = (enemy.targetKnewPos - enemy.transform.position).magnitude;

                if (combatPhase == CombatPhase.FullAlert)
                    return true;

                if (combatPhase == CombatPhase.Alert)
                    return true;

                if (distance > 5.5f * enemy.intelligent / enemy.cost)
                    return true;

                return false;
            }); //Reset

        startActionSelector.AddChildNode(moveSelector);
        startActionSelector.AddChildNode(idleSelector);

        moveSelector.AddChildNode(moveCurve_And_Shoot);
        moveSelector.AddChildNode(moveCurve_And_Aim);

        idleSelector.AddChildNode(idle_And_Shoot);
        idleSelector.AddChildNode(idle_And_Aim);

        startActionSelector.Transition(out EnemyActionLeafNode enemyActionLeaf);
        this.enemyActionLeaf = enemyActionLeaf;
    }

    #endregion
}
