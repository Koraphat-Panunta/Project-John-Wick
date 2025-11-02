using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class EnemySpawnPointRoom : EnemySpawnerPoint
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform exitPoint;
    [SerializeField] protected Dictionary<Enemy,EnemyCommandAPI> spawnedEnemy = new Dictionary<Enemy, EnemyCommandAPI> ();
    [SerializeField] Door door;

  
    protected virtual void Update()
    {
        if(spawnedEnemy.Count <= 0)
            return; 

        List<Enemy> enemies = this.spawnedEnemy.Keys.ToList<Enemy>();

        for(int i = 0; i < enemies.Count; i++)
        {
          
            if (spawnedEnemy[enemies[i]].MoveToPositionRotateToward(exitPoint.position, 1, 1)
                || enemies[i].isDead)
            {
                spawnedEnemy[enemies[i]].GetComponent<EnemyDecision>().enabled = true;
                spawnedEnemy.Remove(enemies[i]);
            }
        }


        if(spawnedEnemy.Count <=0)
            door.Close();
    }
    public override Vector3 spawnPosition { get => spawnPoint.position; protected set => spawnPoint.position = value; }
    public override Quaternion spawnRotiation { get => spawnPoint.rotation; protected set => spawnPoint.rotation = value; }

    public override Enemy SpawnEnemy(EnemyObjectManager enemyObjectManager)
    {
        Enemy enemy = base.SpawnEnemy(enemyObjectManager);
        spawnedEnemy.Add(enemy, enemy.GetComponent<EnemyCommandAPI>());
        enemy.GetComponent<EnemyDecision>().enabled = false;
        door.Open();

        return enemy;
    }
   

  
}
