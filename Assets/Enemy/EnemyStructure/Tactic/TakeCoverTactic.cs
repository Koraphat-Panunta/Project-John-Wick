using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeCoverTactic : IEnemyTactic
{
    public Enemy enemy;
    public EnemyFindingCover enemyFindingCover;
    public TakeCoverTactic(Enemy enemy)
    {
        this.enemy = enemy;
        enemyFindingCover = new EnemyFindingCover();
    }
    public void Manufacturing()
    {
        //if (enemyFindingCover.FindingCover(enemy))
        //{

        //}
        //else
        //{

        //}
    }
}
