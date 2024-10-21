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
        float accuracy = Random.Range(0, 0.05f);
        Vector3 dir = enemy.transform.forward;
        return new Vector3(dir.x + Random.Range(-accuracy, accuracy), dir.y + Random.Range(-accuracy, accuracy), dir.z);
        
    }

}
