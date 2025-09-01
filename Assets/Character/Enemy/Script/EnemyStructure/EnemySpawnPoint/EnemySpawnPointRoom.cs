using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class EnemySpawnPointRoom : EnemySpawnerPoint
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform exitPoint;
    [SerializeField] protected Dictionary<Enemy,EnemyCommandAPI> spawnedEnemy;
    [SerializeField] Door door;
    private readonly float exitMaxTime =3 ;
    private float exitTimer;
    protected override  void Awake()
    {
        spawnedEnemy = new Dictionary<Enemy, EnemyCommandAPI>();
        base.Awake();
    }
    protected virtual void Update()
    {
        if(spawnedEnemy.Count <= 0)
            return;

        exitTimer -= Time.deltaTime;    

        List<Enemy> enemies = this.spawnedEnemy.Keys.ToList<Enemy>();

        for(int i = 0; i < enemies.Count; i++)
        {
            if(exitTimer <= 0)
            {
                enemies[i]._movementCompoent.SetPosition(exitPoint.position);
                spawnedEnemy.Remove(enemies[i]);
                continue;
            }
            if (spawnedEnemy[enemies[i]].MoveToPositionRotateToward(exitPoint.position, 1, 1))
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

    public override bool SpawnEnemy(EnemyObjectManager enemyObjectManager, EnemyDirector enemyDirector, WeaponObjectManager weaponObjectManager, bool isForceSpawn,out Enemy enemy)
    {
        if(base.SpawnEnemy(enemyObjectManager, enemyDirector, weaponObjectManager, isForceSpawn, out enemy))
        {
            exitTimer = exitMaxTime;
            spawnedEnemy.Add(enemy,enemy.GetComponent<EnemyCommandAPI>());
            enemy.GetComponent<EnemyDecision>().enabled = false;
            door.Open();
            return true;
        }
        return false;
    }

  
}
