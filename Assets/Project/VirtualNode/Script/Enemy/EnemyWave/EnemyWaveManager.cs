using UnityEngine;
using System.Collections.Generic;
public class EnemyWaveManager : Actor,IObserverEnemy
{
    [SerializeField] protected List<EnemyWave> enemyWaves;
    public EnemyWave curWave;
    [SerializeField] public List<Enemy> enemies = new List<Enemy>();
    public int numberOfEnemy => enemies.Count;
    [SerializeField] public Player player;

    [SerializeField] public EnemySpawnerPoint[] enemySpawnerPoints;
    [SerializeField] public EnemyDirector enemyDirector;
    [SerializeField] private EnemyPoolManager enemyPoolManager;

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
                EnemySpawnerData spawnData = curWave.enemyListSpawn[0];
                curWave.enemyListSpawn.RemoveAt(0);
                //SpawnEnemyNumber
                for (int j = 0; j < spawnData.numberSpawn; j++)
                {
                    Enemy spawnedEnemy = enemySpawnerPoint.SpawnEnemy(enemyPoolManager, spawnData, this.enemyDirector);
                    spawnedEnemy.AddObserver(this);
                    spawnedEnemy.enemyStateManagerNode.findAndTrackTargetNodeLeaf.SetTargetKnowPos(this.player.transform.position);

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
    public void OnNotify<T>(Enemy enemy, T node) 
    {
        if (node is EnemyDeadStateNode deadStateNode && deadStateNode.curstate == EnemyStateLeafNode.Curstate.Enter)
        {
            enemies.Remove(enemy);
            enemy.RemoveObserver(this);
            if (waveIsClear)
            {
                base.NotifyObserver<EnemyWaveEvent>(EnemyWaveEvent.OnWaveEnd);
            }
        }
    }

    public enum EnemyWaveEvent
    {
        OnWaveEnd,
    }

    protected override void OnDrawGizmos()
    {
        if (isEnableGizmos
            && enemySpawnerPoints != null
            && enemySpawnerPoints.Length > 0)
        {
            for (int i = 0; i < enemySpawnerPoints.Length; i++)
            {
                Gizmos.color = Color.white * .5f;
                Gizmos.DrawLine(transform.position, enemySpawnerPoints[i].spawnPosition);
            }
        }
        base.OnDrawGizmos();
    }
    private void OnValidate()
    {
        if (this.player == null)
            this.player = FindAnyObjectByType<Player>();
    }

}

