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
    public DisarmTargetWeaponEnemyActionNodeLeaf(Enemy enemy, EnemyCommandAPI enemyCommandAPI, Func<bool> preCondition, EnemyActionNodeManager enemyActionNodeManager) : base(enemy, enemyCommandAPI, preCondition, enemyActionNodeManager)
    {
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
        switch (curDisarmWeapon)
        {
            case DisarmTargetWeaponPhase.SprintToKick:
                {
                    if (enemyCommandAPI.MoveToPositionRotateToward(targetPosition, 1,1, 3.5f) 
                       /* && (Vector3.Dot(enemy.transform.forward,(enemy.targetKnewPos-enemy.transform.position).normalized)>0.75f)*/)
                    {
                        enemyCommandAPI.SpinKick();
                        curDisarmWeapon = DisarmTargetWeaponPhase.FindingWeapon;
                    }
                }
                break;    
            case DisarmTargetWeaponPhase.FindingWeapon: 
                {
                    findingWeaponElapseTime += Time.deltaTime;
                    if (enemy.findingWeaponBehavior.FindingWeapon(enemy.transform.position, 3))
                    {
                        findingWeaponElapseTime = 0;
                        curDisarmWeapon = DisarmTargetWeaponPhase.PickingUpWeapon;
                    }
                    else if(findingWeaponElapseTime >= findingWeaponTime)
                    {
                        findingWeaponElapseTime = 0;
                        curDisarmWeapon= DisarmTargetWeaponPhase.SprintToKick;
                    }
                }
                break;
            case DisarmTargetWeaponPhase.PickingUpWeapon:
                {
                    if (enemy.findingWeaponBehavior.weaponFindingSelecting == null)
                        curDisarmWeapon = DisarmTargetWeaponPhase.SprintToKick;

                    if (enemyCommandAPI.MoveToPositionRotateToward(enemy.findingWeaponBehavior.weaponFindingSelecting.transform.position, 1, 1, enemy.findingWeaponBehavior.findingWeaponRaduisDefault))
                    {
                        enemyCommandAPI.PickUpWeapon();
                    }
                }
                break;
        }
       
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
        base.UpdateNode();
    }
}
