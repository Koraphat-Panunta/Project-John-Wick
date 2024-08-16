using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetShootDirection 
{
    private Enemy enemy;
    private Weapon weapon;
    private Vector3 TargetPos;
    public EnemyGetShootDirection(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public Vector3 GetDir()
    {
        Vector3 dir = enemy.transform.forward;
        return new Vector3(dir.x+Random.Range(-1,1), dir.y+ Random.Range(-1, 1), dir.z);
        
    }

}
