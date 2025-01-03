using System;
using System.Collections.Generic;
using UnityEngine;

public class PeekCover : EnemyActionLeafNode
{
    private ICoverUseable coverUser;
    private NormalFiringPattern firingPattern;
    public PeekCover(EnemyControllerAPI enemyController) : base(enemyController)
    {
    }

    public PeekCover(EnemyControllerAPI enemyController,ICoverUseable coverUser, Func<bool> preCondition, Func<bool> isReset) : base(enemyController, preCondition, isReset)
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

        if(enemy.currentWeapon.aimingWeight >= 1)
        switch (enemy.combatOffensiveInstinct.myCombatPhase)
        {
            case CombatOffensiveInstinct.CombatPhase.FullAlert:
                {
                    enemyController.AimDownSight();
                    Quaternion rotate = new RotateObjectToward().RotateToward(
                        enemy.targetKnewPos - enemy.transform.position, enemy.transform, 7);
                    enemyController.Rotate(rotate);
                    firingPattern.Performing();
                };
                break;
            case CombatOffensiveInstinct.CombatPhase.Alert:
                {
                    enemyController.AimDownSight();
                    Quaternion rotate = new RotateObjectToward().RotateToward(
                        enemy.targetKnewPos - enemy.transform.position, enemy.transform, 7);
                    enemyController.Rotate(rotate);
                    firingPattern.Performing();
                };
                break;
            case CombatOffensiveInstinct.CombatPhase.SemiAlert:
                {
                    enemyController.AimDownSight();
                    Quaternion rotate = new RotateObjectToward().RotateToward(
                        enemy.targetKnewPos - enemy.transform.position, enemy.transform, 7);
                    enemyController.Rotate(rotate);
                };
                break;
        }
        base.Update();
    }
}
