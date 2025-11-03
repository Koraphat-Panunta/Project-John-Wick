using UnityEngine;
using System.Collections.Generic;
public class EnemyWaveManager : MonoBehaviour,IObserverEnemy
{
    [SerializeField] protected List<EnemyWave> enemyWaves;
    public EnemyWave curWave;
    [SerializeField] public List<Enemy> enemies = new List<Enemy>();
    public int numberOfEnemy => enemies.Count;
    [SerializeField] public Player player;

    [SerializeField] public EnemySpawnerPoint[] enemySpawnerPoints;
    [SerializeField] public EnemyDirector enemyDirector;

    public bool waveIsClear => enemyWaves.Count <= 0 && numberOfEnemy <= 0;

    [SerializeField] private bool isStartWave = false;

   
    private void Update()
    {
        this.EnemyWaveUpdate();
    }
    private void EnemyWaveUpdate()
    {
        if(isStartWave == false)
            return;

        if(enemyWaves.Count <= 0)
            return;

        if (enemyWaves[0].IsSpawnAble(this.numberOfEnemy))
        {
            curWave = enemyWaves[0];
            enemyWaves.RemoveAt(0);

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
   
    public void StartWave()
    {
        this.isStartWave = true;
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

