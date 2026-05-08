using UnityEngine;

public class EnemyMeleeAttackTest : EnemyDecision
{

    protected override void Update()
    {
        if (this.isSpoting)
        {
            float distance = Vector3.Distance(this.transform.position, this.enemy.targetKnowPos);
            if (distance > this.enemy._meleeAttackMoveScriptableObject_I._beginAttackDistance)
            {
                this.enemyCommand.SprintToPosition(this.enemy.targetKnowPos, 1);
            }
            else
            {
                this.enemyCommand.MeleeAttack();
            }
        }
        base.Update();
    }
    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker)
    {
        
    }
    public bool isSpoting;
    protected override void OnNotifySpottingTarget(GameObject target)
    {
       this.isSpoting = true;
    }
}
