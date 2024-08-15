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
    public EnemyLookForPlayer(Enemy enemy,LayerMask playerMask)
    {
        this.enemy = enemy;
        this._enemyFieldOfView = enemy.enemyFieldOfView;
        this.playerMask = playerMask;
    }
    public void Recived()
    {
        GameObject player;
        player = _enemyFieldOfView.FindSingleObjectInView(playerMask);
        if(player != null)
        {
            Debug.Log("IsSeeingPlayer");
            IsSeeingPlayer = true;
            Vector3 playerPos = player.transform.position;
            enemy.Target.transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z);
        }
        else
        {
            Debug.Log("Not SeeingPlayer");
            IsSeeingPlayer = false;
        }
    }

}
