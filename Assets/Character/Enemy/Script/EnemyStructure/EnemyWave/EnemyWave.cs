using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWave 
{
    public List<EnemyDetailSpawn> enemyListSpawn;
    [SerializeField] protected int enemyRemainStartSpawn;
    [SerializeField] protected float spawnDelay;
   
    public bool IsSpawnAble(int enemiesRemain)
    {
        if(enemiesRemain > this.enemyRemainStartSpawn)
            return false;

        if (this.spawnDelay <= 0)
            return true;
        this.spawnDelay -= Time.deltaTime;

        return false;
    }
    
}
