using System;
using UnityEngine;

public class HoldSightActionNodeLeaf : EnemyActionNodeLeaf
{
    public HoldSightActionNodeLeaf(
        Enemy enemy
        , EnemyCommandAPI enemyCommandAPI
        , Func<bool> preCondition
        , EnemyDecision enemyDecision
        ) 
        : base(
            enemy
            , enemyCommandAPI
            , preCondition
            , enemyDecision
            )
    {
    }
    public override void Enter()
    {
        this.UpdateHoldPosition();
        base.Enter();
    }

    public override void UpdateNode()
    {
        this.enemyCommandAPI.AimDownSight(this.holdPosition);
        this.enemyCommandAPI.FreezPosition();
        base.UpdateNode();
    }
  

    private Vector3 holdPosition;
    private void UpdateHoldPosition()
    {
        BlindSpotFinder.TryFindCoverEntryAlongPath_Debug(
            this.enemy.humanoidBone._top_head_bone.position
            ,this.enemy._movementCompoent.curPosition
            ,this.enemy.targetKnowPos
            ,LayerMask.GetMask("Default")
            ,out this.holdPosition
            ,out Vector3 firstBlockedPoint);
    }
}
