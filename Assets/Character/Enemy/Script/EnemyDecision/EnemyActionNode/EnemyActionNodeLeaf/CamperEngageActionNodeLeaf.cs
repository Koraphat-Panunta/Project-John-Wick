using System;
using System.Collections;
using UnityEngine;

public class CamperEngageActionNodeLeaf : EnemyActionNodeLeaf
{
    float maxDelayTime = .5f;
    float minDelayTime = .05f;

    private bool isShootAble;
    private bool isApprouch;

    private EnemyMoveCurvePath curvePath;
    public CamperEngageActionNodeLeaf(
        Enemy enemy
        , EnemyCommandAPI enemyCommandAPI
        , Func<bool> preCondition
        , EnemyDecision enemyDecision
        ) : base(enemy, enemyCommandAPI, preCondition, enemyDecision)
    {
        this.curvePath = new EnemyMoveCurvePath(.25f,2);
    }

    public override void Enter()
    {
        this.curvePath.GenaratePath(this.enemy._movementCompoent.curPosition,this.enemy.targetKnowPos);
        this.isShootAble = false;
        this.isApprouch = RandomUtil.Chance();
        this.enemy.StartCoroutine(this.DelayShootAble());
        base.Enter();
    }
    
    public override void UpdateNode()
    {
        EnemyOffendCommandWeaponBased.Hold(this.enemy,this.enemyCommandAPI,this.enemy.targetKnowPos);
        if (this.isShootAble) 
        {
            EnemyOffendCommandWeaponBased.Engage(this.enemy, this.enemyCommandAPI, this.enemy.targetKnowPos);
        }
        if (this.isApprouch)
        {
            EnemyOffendCommandWeaponBased.Ambush(this.enemy, this.enemyCommandAPI, this.enemy.targetKnowPos);
            if (this.curvePath.TryGetCurvePoint(out Vector3 _curvePoint))
                this.enemyCommandAPI.MoveToPosition(_curvePoint, 1);
            else
                this.enemyCommandAPI.MoveToPosition(this.enemy.targetKnowPos, 1);
        }


        base.UpdateNode();
    }

    IEnumerator DelayShootAble()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(this.minDelayTime,this.maxDelayTime));
        this.isShootAble = true;
    }
}
