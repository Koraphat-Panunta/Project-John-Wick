using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TakeCoverGoal : EnemyGoalLeaf
{
    private ICoverUseable coverUser;
    private float costDrainRate;

    private float coverPatternFlipFlop;
    private float flipFlopTiming;
    public TakeCoverGoal(EnemyCommandAPI enemyController, IEnemyGOAP enemyGOAP, ICoverUseable coverUseable) : base(enemyController, enemyGOAP)
    {
        this.coverUser = coverUseable;
        InitailizedActionNode();

    }
    public override List<EnemyGoal> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        costDrainRate = UnityEngine.Random.Range(1, 2);
        flipFlopTiming = UnityEngine.Random.Range(3, 7);
        coverPatternFlipFlop = flipFlopTiming;
        base.Enter();
    }

    public override void Exit()
    {
        enemyController.GetOffCover();
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
        if (100 / enemy.cost * enemy.intelligent > enemy.cost * enemy.strength)
            return true;

        if (combatPhase != CombatOffensiveInstinct.CombatPhase.FullAlert
            || combatPhase != CombatOffensiveInstinct.CombatPhase.Alert
            || combatPhase != CombatOffensiveInstinct.CombatPhase.SemiAlert)
            return true;

        Vector3 coverDir = coverUser.coverPoint.coverDir.normalized;
        Vector3 coverToTarget = (enemy.targetKnewPos - coverUser.coverPos).normalized;

        if (Vector3.Dot(coverDir, coverToTarget) < 0)
        {
            enemy.cost = math.clamp(enemy.cost + 45, 0, 100);
            return true;
        }

        return false;
    }

    public override bool PreCondition()
    {
        if (enemy.findingCover.FindCoverInRaduis(8, out CoverPoint coverPoint)==false)
            return false;

        CombatOffensiveInstinct.CombatPhase combatPhase = enemy.combatOffensiveInstinct.myCombatPhase;
        if (100 / enemy.cost * enemy.intelligent <= enemy.cost * enemy.strength)
            return false;

        if (combatPhase != CombatOffensiveInstinct.CombatPhase.FullAlert
            || combatPhase != CombatOffensiveInstinct.CombatPhase.Alert
            || combatPhase != CombatOffensiveInstinct.CombatPhase.SemiAlert)
            return false;

        return true;
    }

    private EnemyActionSelectorNode travelToCoverSelector { get; set; }
    private EnemyActionSelectorNode coverPatternSelector { get; set; }
    private MoveToCover moveToCover { get; set; }
    private SprintToCover sprintToCover { get; set; }
    private PeekCover peekCover { get; set; }
    private BackToCover backToCover { get; set; }   

    protected override EnemyActionLeafNode enemyActionLeaf { get; set; }
    protected override EnemyActionSelectorNode startActionSelector { get; set; }
    protected override void InitailizedActionNode()
    {
        startActionSelector = new EnemyActionSelectorNode(enemyController, () => true);

        travelToCoverSelector = new EnemyActionSelectorNode(enemyController,
            () => 
            {
                if(enemy.isInCover == false)
                    return true;

                return false;
            }); // PreCondition

        coverPatternSelector = new EnemyActionSelectorNode(enemyController,
            () => true); //PreCondition

        moveToCover = new MoveToCover(enemyController, coverUser,
            () => 
            {
                ICombatOffensiveInstinct combatOffensiveInstinct = enemy as ICombatOffensiveInstinct;
                if(combatOffensiveInstinct.combatOffensiveInstinct.offensiveIntensity > 60)
                    return true;
                else return false;
            },//PreCondition
            () => 
            {
                if(coverUser.isInCover)
                    return true;
                return false;
            }//Reset
            );

        sprintToCover = new SprintToCover(enemyController, 
            () => true,//PreCondition
            () => 
            {
                if(coverUser.isInCover)
                    return true;

                return false;
            }//Reset
            );

        peekCover = new PeekCover(enemyController, coverUser,
            () =>
            {
                if (coverPatternFlipFlop <= 0)
                    return true;

                return false;
            },//PreCondition
            () =>
            {
                coverPatternFlipFlop += Time.deltaTime;
                if (coverPatternFlipFlop > flipFlopTiming)
                {
                    flipFlopTiming = UnityEngine.Random.Range(3, 7);
                    coverPatternFlipFlop = flipFlopTiming;
                    return true;
                }

                return false;
            }//Reset
            );

        backToCover = new BackToCover(enemyController,
            () =>
            {
                return true;
            },//PreCondition
            () =>
            {
                coverPatternFlipFlop -= Time.deltaTime;
                if (coverPatternFlipFlop <= 0)
                    return true;

                return false;
            }//Reset
            );

        startActionSelector.AddChildNode(travelToCoverSelector);
        startActionSelector.AddChildNode(coverPatternSelector);

        travelToCoverSelector.AddChildNode(moveToCover);
        travelToCoverSelector.AddChildNode(sprintToCover);
        
        coverPatternSelector.AddChildNode(peekCover);
        coverPatternSelector.AddChildNode(backToCover);
    }
}
