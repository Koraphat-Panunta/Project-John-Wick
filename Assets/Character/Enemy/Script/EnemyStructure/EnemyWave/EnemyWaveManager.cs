using UnityEngine;
using System.Collections.Generic;
public class EnemyWaveManager : MonoBehaviour,IObserverEnemy,IInitializedAble
{
    public Queue<EnemyWave> enemyWaves;
    public EnemyWave curWave;
    [SerializeField] public List<Enemy> enemies;
    public int numberOfEnemy => enemies.Count;
    [SerializeField] public Player player;

    [SerializeField] public EnemySpawnerPoint[] enemySpawnerPoints;
    [SerializeField] public EnemyDirector enemyDirector;

    public bool waveIsClear => enemyWaves.Count <= 0 && numberOfEnemy <= 0;
    public void Initialized()
    {
        enemyWaves = new Queue<EnemyWave>();
        enemies = new List<Enemy>();
    }
    
    public void AddEnemyWave(EnemyWave enemyWave)
    {
        this.enemyWaves.Enqueue(enemyWave);
    }
    public void EnemyWaveUpdate()
    {
        if(enemyWaves.Count <= 0)
            return;

        if (enemyWaves.Peek().IsSpawnAble(this.numberOfEnemy))
        {
            curWave = enemyWaves.Dequeue();
            EnemySpawnerPoint enemySpawnerPoint = GetSelectedEnemySpawnerPoint();
            //SpawnEnemyList
            while(curWave.enemyListSpawn.Count > 0)
            {
                EnemyDetailSpawn enemyListSpawn = curWave.enemyListSpawn[0];
                curWave.enemyListSpawn.RemoveAt(0);
                //SpawnEnemyNumber
                for (int j = 0; j < enemyListSpawn.numberSpawn; j++)
                {
                    Enemy spawnedEnemy = enemySpawnerPoint.SpawnEnemy(enemyListSpawn.enemyObjectManager, this.enemyDirector, enemyListSpawn.weaponObjectManager);
                    spawnedEnemy.AddObserver(this);
                    spawnedEnemy.targetKnewPos = player.transform.position;

                    EnemyCommunicator enemyCommunicator = new EnemyCommunicator();
                    enemyCommunicator.enemyCommunicateMassage = EnemyCommunicator.EnemyCommunicateMassage.SendTargetPosition;
                    spawnedEnemy.GetCommunicate<EnemyCommunicator,Vector3>(enemyCommunicator, player.transform.position);
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
                
                selectedSpawnPoint = enemySpawnerPoints[i];
                continue;
            }


            if (Vector3.Distance(player.transform.position, enemySpawnerPoints[i].transform.position)
                > Vector3.Distance(player.transform.position, selectedSpawnPoint.transform.position))
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
    private void OnValidate()
    {
        if (this.player == null)
            this.player = FindAnyObjectByType<Player>();
    }

}

