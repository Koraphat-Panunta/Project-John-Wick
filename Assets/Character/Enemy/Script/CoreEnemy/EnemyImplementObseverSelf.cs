using UnityEngine;

public partial class Enemy : IObserverEnemy
{

    public void OnNotify<T>(Enemy enemy, T node)
    {
        if (enemy._isPainTrigger
            || enemy._triggerEnterGotAttacked_OCM)
        {
          

            switch (enemy.getPosturePainPhase)
            {
                case EnemyPosturePainStatePhase.MiniPainState:
                    {
                        enemy.enemyStateManagerNode.painStateNodeLeaf.SetPainStateDuration(enemy.miniPainStateDuration);
                        break;
                    }
                case EnemyPosturePainStatePhase.MediumPainState:
                    {
                        enemy.enemyStateManagerNode.painStateNodeLeaf.SetPainStateDuration(enemy.mediumPainStateDuration);
                        break;
                    }
                case EnemyPosturePainStatePhase.HeavyPainState:
                    {
                        enemy.enemyStateManagerNode.painStateNodeLeaf.SetPainStateDuration(enemy.heavyPainStateDuration);
                        break;
                    }
            }
        }
        switch (node)
        {
            case HumanShield_Exit_GotInteract_NodeLeaf gotHumanShieldExitNodeLeaf:
                {
                    if (gotHumanShieldExitNodeLeaf.curstate == NodePhase.Enter)
                        enemy._posture = 0;
                    break;
                }
            case GetUpStateNodeLeaf getUpStateNodeLeaf:
                {
                    if (getUpStateNodeLeaf.isStandingComplete)
                    {
                        this.stanceCommand = Stance.stand;
                        enemy._posture = enemy._maxPosture;
                        Debug.Log("enemy._posture = "+ enemy._posture);
                    }
                    break;
                }
            case EnemySprintStateNodeLeaf getSprintStateNodeLeaf:
                {
                    this.stanceCommand = Stance.stand;
                    break;
                }
        }

        if (this._isInPain)
            this.reactionTime.SetGauge(0);
    }
}
