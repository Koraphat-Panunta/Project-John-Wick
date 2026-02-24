using UnityEngine;

public partial class Enemy : IObserverEnemy
{

    public void OnNotify<T>(Enemy enemy, T node)
    {
        if (enemy._isPainTrigger
            || enemy._triggerHitedGunFu)
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
                    if (gotHumanShieldExitNodeLeaf.curstate == EnemyStateLeafNode.Curstate.Enter)
                        enemy._posture = 0;
                    break;
                }
            case GetUpStateNodeLeaf getUpStateNodeLeaf:
                {
                    if (getUpStateNodeLeaf.isStandingComplete)
                        enemy._posture = enemy._maxPosture;
                    break;
                }
        }
    }
}
