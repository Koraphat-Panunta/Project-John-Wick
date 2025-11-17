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
            case HumanShieldExit_GunFu_NodeLeaf gotHumanShieldExitNodeLeaf:
                {
                    if(gotHumanShieldExitNodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Enter)
                        enemy._posture = 0;
                    break;
                }
        }
    }
}
