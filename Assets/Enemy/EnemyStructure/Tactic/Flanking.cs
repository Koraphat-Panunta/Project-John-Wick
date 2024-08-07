using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flanking : EnemyTactic
{
    private EnemyAction EnemyAction;
    public Flanking()
    {

    }
    public override void TacticEnter(TacticManager tacticManager)
    {
        if (EnemyAction == null)
        {
            EnemyAction = tacticManager._enemyBrain._enemyAction;
        }
        EnemyAction._enemyStateManager.ChangeState(EnemyAction._enemyStateManager._move);
    }
    public override void TacticExit(TacticManager tacticManager)
    {
       
    }

    public override void TacticFixedUpdate(EnemyBrain enemyBrain)
    {
        
    }

    public override void TacticUpdate(EnemyBrain enemyBrain)
    {
        Debug.Log("TacticUpdate");
        if(Vector3.Distance(enemyBrain.Target.transform.position,enemyBrain.enemy.gameObject.transform.position)<2.5f)
        {
            EnemyAction._enemyStateManager.ChangeState(EnemyAction._enemyStateManager._idle);
        }
        else
        {
            EnemyAction._enemyStateManager.ChangeState(EnemyAction._enemyStateManager._move);
        }

    }
}
