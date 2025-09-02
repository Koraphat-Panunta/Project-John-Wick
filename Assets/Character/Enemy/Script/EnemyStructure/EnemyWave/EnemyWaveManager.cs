using UnityEngine;
using System.Collections.Generic;
public class EnemyWaveManager : IObserverEnemy
{
    public Queue<EnemyWave> enemyWaves;
    public EnemyWave curWave;
    public List<Enemy> enemies;
    public int numberOfEnemy => enemies.Count;
    public Transform fartestRefSpawnPos;

    public EnemySpawnerPoint[] enemySpawnerPoints;
    public EnemyDirector enemyDirector;

    public bool waveIsClear => enemyWaves.Count <= 0;
    public EnemyWaveManager(Transform fartestRefSpawnPos, EnemySpawnerPoint[] enemySpawnerPoints,EnemyDirector enemyDirector)
    {
        enemyWaves = new Queue<EnemyWave>();
        enemies = new List<Enemy>();
        this.enemySpawnerPoints = enemySpawnerPoints;
        this.fartestRefSpawnPos = fartestRefSpawnPos;
        this.enemyDirector = enemyDirector;
    }
    public void AddEnemyWave(EnemyWave enemyWave)
    {
        this.enemyWaves.Enqueue(enemyWave);
    }
    public void EnemyWaveUpdate()
    {
        if(enemyWaves.Count <= 0)
            return;

        if (enemyWaves.Peek().IsSpawnAble())
        {
            curWave = enemyWaves.Dequeue();
            EnemySpawnerPoint enemySpawnerPoint = GetSelectedEnemySpawnerPoint();
            //SpawnEnemyList
            while(curWave.enemyListSpawn.Count > 0)
            {
                EnemyWave.EnemyListSpawn enemyListSpawn = curWave.enemyListSpawn.Dequeue();
                //SpawnEnemyNumber
                for (int j = 0; j < enemyListSpawn.numberSpawn; j++)
                {
                    enemySpawnerPoint.SpawnEnemy(enemyListSpawn.enemyObjectManager, this.enemyDirector, enemyListSpawn.weaponObjectManager, out Enemy spawnedEnemy);
                    spawnedEnemy.AddObserver(this);
                    enemies.Add(spawnedEnemy);
                }

            }
           
        }
    }


    private EnemySpawnerPoint GetSelectedEnemySpawnerPoint()
    {
        EnemySpawnerPoint selectedSpawnPoint = null;
        for (int i = 0; i < enemySpawnerPoints.Length; i++)
        {
            if(selectedSpawnPoint == null)
            {
                if (i >= enemySpawnerPoints.Length - 1)
                {
                    selectedSpawnPoint = enemySpawnerPoints[i];
                    break;
                }

                if (enemySpawnerPoints[i].preCondition.Invoke())
                    selectedSpawnPoint = enemySpawnerPoints[i];
                continue;
            }


            if (enemySpawnerPoints[i].preCondition.Invoke()
                && Vector3.Distance(fartestRefSpawnPos.position, enemySpawnerPoints[i].transform.position)
                > Vector3.Distance(fartestRefSpawnPos.position, selectedSpawnPoint.transform.position))
                selectedSpawnPoint = enemySpawnerPoints[i];
        }
        return selectedSpawnPoint;
    }
   
 

    public void Notify<T>(Enemy enemy, T node) 
    {
        if (node is EnemyDeadStateNode deadStateNode && deadStateNode.curstate == EnemyStateLeafNode.Curstate.Enter)
        {
            enemies.Remove(enemy);
            enemy.RemoveObserver(this);
        }
    }
}
