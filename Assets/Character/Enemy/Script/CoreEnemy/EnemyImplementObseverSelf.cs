using UnityEngine;

public partial class Enemy : IObserverEnemy
{

    public void Notify<T>(Enemy enemy, T node)
    {
        switch (node)
        {
            case GotGunFuHitNodeLeaf gotGunFuHitNodeLeaf:
                {
                    if(gotGunFuHitNodeLeaf == (enemyStateManagerNode as EnemyStateManagerNode).gotHit3_GunFuNodeLeaf 
                        && gotGunFuHitNodeLeaf.curstate == EnemyStateLeafNode.Curstate.Enter)
                    {
                        enemy._posture = 0;
                    }
                    break;
                }
            case HumanShield_Exit_GotInteract_NodeLeaf gotHumanShieldExitNodeLeaf:
                {
                    if(gotHumanShieldExitNodeLeaf.curstate == EnemyStateLeafNode.Curstate.Enter)
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
