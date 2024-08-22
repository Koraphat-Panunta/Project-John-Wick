using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IEnemyHitReaction
{
    public bool isEnd { get; protected set; }
    public abstract void Performed(EnemyStateManager enemyState);
    public abstract void Enter(EnemyStateManager enemyState);
    public abstract void End(EnemyStateManager enemyState);
}
