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
    [SerializeField] private WeaponPoolManager weaponPoolManager;

    public bool waveIsClear => enemyWaves.Count <= 0 && numberOfEnemy <= 0;

    [SerializeField] private bool isStartWave = false;

    private Camera _camera;
    private int _spawnRoundRobinIndex;
    private readonly List<EnemySpawnerPoint> _validSpawnPoints = new List<EnemySpawnerPoint>();

    private void Start()
    {
        _camera = Camera.main;
    }

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

            BuildValidSpawnPoints();
            while(curWave.enemyListSpawn.Count > 0)
            {
                EnemySpawnerData spawnData = curWave.enemyListSpawn[0];
                curWave.enemyListSpawn.RemoveAt(0);
                for (int j = 0; j < spawnData.numberSpawn; j++)
                {
                    Enemy spawnedEnemy = GetNextSpawnPoint().SpawnEnemy(enemyPoolManager, weaponPoolManager, spawnData, this.enemyDirector);
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

    private void BuildValidSpawnPoints()
    {
        _validSpawnPoints.Clear();
        _spawnRoundRobinIndex = 0;
        for (int i = 0; i < enemySpawnerPoints.Length; i++)
            if (IsOffCamera(enemySpawnerPoints[i].spawnPosition))
                _validSpawnPoints.Add(enemySpawnerPoints[i]);

        if (_validSpawnPoints.Count == 0)
            for (int i = 0; i < enemySpawnerPoints.Length; i++)
                _validSpawnPoints.Add(enemySpawnerPoints[i]);
    }

    private EnemySpawnerPoint GetNextSpawnPoint()
    {
        EnemySpawnerPoint point = _validSpawnPoints[_spawnRoundRobinIndex % _validSpawnPoints.Count];
        _spawnRoundRobinIndex++;
        return point;
    }

    private bool IsOffCamera(Vector3 worldPos)
    {
        if (_camera == null) return true;
        Vector3 vp = _camera.WorldToViewportPoint(worldPos);
        return !(vp.z > 0 && vp.x > 0 && vp.x < 1 && vp.y > 0 && vp.y < 1);
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
                if (this.enemySpawnerPoints[i] != null)
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

