using System;
using System.Collections.Generic;
using UnityEngine;

public class EncouterGoal : EnemyGoalLeaf
{

    public override List<EnemyGoal> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public EncouterGoal(EnemyControllerAPI enemyController, IEnemyGOAP enemyGOAP,IFindingTarget findingTarget) : base(enemyController, enemyGOAP)
    {
        
    }
   
   
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
        if(enemy.isInCombat == false)
            return true;

        if (enemy.strength * enemy.cost
            < enemy.intelligent * (enemy.maxCost/enemy.cost))
            return true;

        else return false;
    }

    public override bool PreCondition()
    {
        if(enemy.isInCombat == true)
        return true;
        
        else return false;
    }
    public override float GetCost()
    {
        return enemy.strength * enemy.cost;
    }
    public override void Update()
    {
        base.Update();
    }

    #region InitailiedActionNode
    public override void ActionUpdate()
    {
        
    }

    public override void ActionFixedUpdate()
    {
       
    }

    private MoveCurve_and_Aim moveCurve_And_Aim;
    private MoveCurve_and_Shoot moveCurve_And_Shoot;
    private Idle_and_Aim idle_And_Aim;
    private Idle_and_LowReady idle_And_LowReady;

    protected override EnemyActionLeafNode enemyActionLeaf { get; set; }
    protected override EnemyActionSelectorNode startActionSelector { get ; set ; }

    protected override void InitailizedActionNode()
    {
        startActionSelector = new EnemyActionSelectorNode(enemyController,()=>true);

        this.moveCurve_And_Aim = new MoveCurve_and_Aim(enemyController
            ,() => enemy.isInCombat // PreCondition
            ,() => 
            {
                float distance = (enemy.targetKnewPos - enemy.transform.position).magnitude;

                if(distance < 2.5f)
                    return true;

                if(enemy.isSpotingtarget )
                    return true;

                else return false;
            }//Reset
            );
            
    }

    #endregion
}
