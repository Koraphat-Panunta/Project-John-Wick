using System;
using System.Collections.Generic;
using UnityEngine;

public class PeekCover : EnemyActionLeafNode
{
    private ICoverUseable coverUser;
    private NormalFiringPattern firingPattern;
    public PeekCover(EnemyCommandAPI enemyController) : base(enemyController)
    {
    }

    public PeekCover(EnemyCommandAPI enemyController,ICoverUseable coverUser, Func<bool> preCondition, Func<bool> isReset) : base(enemyController, preCondition, isReset)
    {
        this.coverUser = coverUser;
        firingPattern = new NormalFiringPattern(enemyController);
    }

    public override List<EnemyActionNode> childNode { get => base.childNode; set => base.childNode = value; }
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
        return base.IsReset();
    }

    public override bool PreCondition()
    {
        return base.PreCondition();
    }

    public override void Update()
    {
        enemyController.AimDownSight();

        if(enemy.weaponManuverManager.aimingWeight >= 1)
        switch (enemy.combatOffensiveInstinct.myCombatPhase)
        {
            case CombatOffensiveInstinct.CombatPhase.FullAlert:
                {
                    enemyController.AimDownSight();
                    enemyController.RotateToPosition(enemy.targetKnewPos, 7);
                    firingPattern.Performing();
                };
                break;
            case CombatOffensiveInstinct.CombatPhase.Alert:
                {
                        enemyController.AimDownSight();
                        enemyController.RotateToPosition(enemy.targetKnewPos, 7);
                        firingPattern.Performing();
                    };
                break;
            case CombatOffensiveInstinct.CombatPhase.SemiAlert:
                {
                    enemyController.AimDownSight();
                    enemyController.RotateToPosition(enemy.targetKnewPos, 7);
                    
                };
                break;
        }
        base.Update();
    }
}
