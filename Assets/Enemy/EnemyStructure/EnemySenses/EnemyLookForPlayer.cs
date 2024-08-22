using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookForPlayer : IEnemySensing
{
    private FieldOfView _enemyFieldOfView;
    private Enemy enemy;
    private LayerMask playerMask;
    public bool IsSeeingPlayer= false;
    public Vector3 _lastSeenPosition = Vector3.zero;
    public float lostSightTiming { get; private set; }
    public EnemyLookForPlayer(Enemy enemy,LayerMask playerMask)
    {
        this.enemy = enemy;
        this._enemyFieldOfView = enemy.enemyFieldOfView;
        this.playerMask = playerMask;
        lostSightTiming = 0;
    }
    public void Recived()
    {
        GameObject player;
        player = _enemyFieldOfView.FindSingleObjectInView(playerMask,new Vector3(0,1.2f,0));
        if(player != null)
        {
            IsSeeingPlayer = true;
            lostSightTiming = 0;
            Vector3 playerPos = player.transform.position;
            enemy.Target.transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z);
        }
        else
        {
            lostSightTiming += Time.deltaTime;
            IsSeeingPlayer = false;
        }
    }

}
