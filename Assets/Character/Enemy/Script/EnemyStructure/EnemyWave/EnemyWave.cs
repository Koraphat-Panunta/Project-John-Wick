using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave 
{
    public Queue<EnemyListSpawn> enemyListSpawn;
    public struct EnemyListSpawn
    {
        public EnemyObjectManager enemyObjectManager;
        public WeaponObjectManager weaponObjectManager;
        public int numberSpawn;

        public EnemyListSpawn(EnemyObjectManager enemyObjectManager,WeaponObjectManager weaponObjectManager,int numberSpawn)
        {
            this.enemyObjectManager = enemyObjectManager;
            this.weaponObjectManager = weaponObjectManager;
            this.numberSpawn = numberSpawn;
        }
    }
    protected Func<bool> spawnCondition;
    protected float spawnDelay;
    public EnemyWave(Func<bool> spawnCondition, EnemyListSpawn[] enemyListSpawns) : this(spawnCondition,enemyListSpawns, 0)
    {

    }
    public EnemyWave(Func<bool> spawnCondition, EnemyListSpawn[] enemyListSpawns, float spawnDelay)
    {
        this.spawnCondition = spawnCondition;
        this.spawnDelay = spawnDelay;
        enemyListSpawn = new Queue<EnemyListSpawn>();
        for (int i = 0; i < enemyListSpawns.Length; i++) 
        {
            enemyListSpawn.Enqueue(enemyListSpawns[i]);
        }
    }
    public bool IsSpawnAble()
    {
        if (spawnCondition.Invoke())
        {
            if(this.spawnDelay <= 0)
                return true;
            this.spawnDelay -= Time.deltaTime;
        }

        return false;
    }
    
}
