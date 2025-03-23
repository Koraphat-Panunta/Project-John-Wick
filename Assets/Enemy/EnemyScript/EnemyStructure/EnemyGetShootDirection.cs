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
    public Vector3 GetShootingPos()
    {
        float accuracy = Random.Range(0, 0.05f);
        Vector3 dirTarget = (enemy.targetKnewPos - (enemy.rayCastPos.position+ new Vector3(0,0.2f,0))).normalized;
        Vector3 dir = new Vector3(enemy.transform.forward.x,
           dirTarget.y, enemy.transform.forward.z);
         dir = new Vector3(dir.x + Random.Range(-accuracy, accuracy), dir.y + Random.Range(-accuracy, accuracy), dir.z);
        Ray ray = new Ray(enemy.currentWeapon.bulletSpawnerPos.position, dir);
        return ray.GetPoint(100);
        //return new Vector3(dir.x + Random.Range(-accuracy, accuracy), dir.y + Random.Range(-accuracy, accuracy), dir.z);
        
    }
    public Vector3 GetPointingPos()
    { 
        Vector3 dirTarget = (enemy.targetKnewPos - (enemy.rayCastPos.position + new Vector3(0, 0.2f, 0))).normalized;

        Ray ray = new Ray(enemy.currentWeapon.bulletSpawnerPos.position, dirTarget);
        return ray.GetPoint(100);
    }

}
