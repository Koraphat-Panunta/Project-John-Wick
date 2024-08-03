using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyTactic 
{
    [SerializeField]
    public EnemyTactic()
    {

    }

    public virtual void TacticEnter(TacticManager tacticManager)
    {

    }
    public virtual void TacticExit(TacticManager tacticManager)
    {

    }
    public virtual void TacticUpdate(EnemyBrain enemyBrain)
    {
    }
    public virtual void TacticFixedUpdate(EnemyBrain enemyBrain)
    {

    }
}
