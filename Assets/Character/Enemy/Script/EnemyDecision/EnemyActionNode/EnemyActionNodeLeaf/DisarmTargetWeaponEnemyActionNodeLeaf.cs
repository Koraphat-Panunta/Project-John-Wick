using System;
using UnityEngine;

public class DisarmTargetWeaponEnemyActionNodeLeaf : EnemyActionNodeLeaf
{
    private Vector3 targetPosition => enemy.targetKnewPos;
    private enum DisarmTargetWeaponPhase
    {
        SprintToKick,
        FindingWeapon,
        PickingUpWeapon
    }
    private DisarmTargetWeaponPhase curDisarmWeapon;
    private readonly float findingWeaponTime = 3;
    private float findingWeaponElapseTime;

    protected IEnemyActionNodeManagerImplementDecision enemyActionNodeManagerImplementDecision;
    public DisarmTargetWeaponEnemyActionNodeLeaf(Enemy enemy
        , EnemyCommandAPI enemyCommandAPI
        , Func<bool> preCondition
        ,EnemyDecision enemyDecision
        , IEnemyActionNodeManagerImplementDecision enemyActionNodeManagerImplementDecision) : base(enemy, enemyCommandAPI, preCondition, enemyDecision)
    {
        this.enemyActionNodeManagerImplementDecision = enemyActionNodeManagerImplementDecision;
    }

    public override void Enter()
    {
        curDisarmWeapon = DisarmTargetWeaponPhase.SprintToKick;
        findingWeaponElapseTime = 0;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        
       
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        if(enemy._currentWeapon != null)
            return true;
        return base.IsComplete();
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override void UpdateNode()
    {
        switch (curDisarmWeapon)
        {
            case DisarmTargetWeaponPhase.SprintToKick:
                {

                    if (enemyCommandAPI.SprintToPosition(targetPosition, 1,3.25f)
                       /* && (Vector3.Dot(enemy._transform.forward,(enemy.targetKnewPos-enemy._transform.position).normalized)>0.75f)*/)
                    {
                        enemyCommandAPI.SpinKick();
                        if (enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemySpinKickGunFuNodeLeaf>())
                            curDisarmWeapon = DisarmTargetWeaponPhase.FindingWeapon;
                    }
                    else
                        enemyCommandAPI.enemyAutoDefendCommand.UpdateAutoDefend();

                }
                break;
            case DisarmTargetWeaponPhase.FindingWeapon:
                {
                    findingWeaponElapseTime += Time.deltaTime;
                    if (enemy._findingWeaponBehavior.FindingWeapon(enemy.transform.position, 3))
                    {
                        findingWeaponElapseTime = 0;
                        curDisarmWeapon = DisarmTargetWeaponPhase.PickingUpWeapon;
                    }
                    else if (findingWeaponElapseTime >= findingWeaponTime)
                    {
                        findingWeaponElapseTime = 0;
                        curDisarmWeapon = DisarmTargetWeaponPhase.SprintToKick;
                    }
                }
                break;
            case DisarmTargetWeaponPhase.PickingUpWeapon:
                {
                    if (enemy._findingWeaponBehavior.weaponFindingSelecting == null)
                        curDisarmWeapon = DisarmTargetWeaponPhase.SprintToKick;

                    if (enemyCommandAPI.MoveToPositionRotateToward(enemy._findingWeaponBehavior.weaponFindingSelecting.transform.position, 1, 1, enemy._findingWeaponBehavior.findingWeaponRaduisDefault))
                    {
                        enemyCommandAPI.PickUpWeapon();
                    }
                }
                break;
        }

        base.UpdateNode();
    }
}
