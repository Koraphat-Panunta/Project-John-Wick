using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour, IInitializedAble, IObserverEnemy
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private EnemyDataScriptableObject[] registeredEnemyTypes;
    [SerializeField] private int initialPoolSize = 6;
    [SerializeField] private int maxPoolSize = 30;

    private Dictionary<EnemyDataScriptableObject, ObjectPooling<Enemy>> _pools;
    private Dictionary<Enemy, EnemyDataScriptableObject> _enemyTypeMap;
    private Dictionary<Enemy, float> _clearEnemyList;

    private readonly int _corpseDisappearTime = 5;
    private readonly int _corpseDisappearDistance = 6;

    public void Initialized()
    {
        _pools = new Dictionary<EnemyDataScriptableObject, ObjectPooling<Enemy>>();
        _enemyTypeMap = new Dictionary<Enemy, EnemyDataScriptableObject>();
        _clearEnemyList = new Dictionary<Enemy, float>();

        foreach (EnemyDataScriptableObject data in registeredEnemyTypes)
            _pools[data] = new ObjectPooling<Enemy>(data.enemyPrefab, maxPoolSize, initialPoolSize, Vector3.zero);
    }

    public Enemy GetEnemy(EnemyDataScriptableObject data, Vector3 position, Quaternion rotation)
    {
        if (!_pools.TryGetValue(data, out ObjectPooling<Enemy> pool))
        {
            Debug.LogError("EnemyPoolManager: enemy type not registered — " + data.name);
            return null;
        }

        Enemy enemy = pool.Get(position, rotation);
        _enemyTypeMap[enemy] = data;
        enemy.AddObserver(this);
        return enemy;
    }

    public void OnNotify<T>(Enemy enemy, T node)
    {
        if (node is EnemyDeadStateNode)
        {
            _clearEnemyList[enemy] = 0;
            enemy.RemoveObserver(this);
        }
    }

    float _checkTimer = 0f;
    private readonly float _checkInterval = 0.25f;

    private void LateUpdate()
    {
        _checkTimer += Time.deltaTime;
        if (_checkTimer < _checkInterval) return;
        ClearCorpseEnemyUpdate();
        _checkTimer = 0f;
    }

    private void ClearCorpseEnemyUpdate()
    {
        if (_clearEnemyList.Count == 0) return;

        List<Enemy> enemies = _clearEnemyList.Keys.ToList();
        foreach (Enemy enemy in enemies)
        {
            if (!enemy.isDead) continue;

            if (_clearEnemyList[enemy] > _corpseDisappearTime)
            {
                if (IsObjectInCameraView(mainCamera, enemy.transform.position)) continue;
                if (Vector3.Distance(mainCamera.transform.position, enemy.transform.position) < _corpseDisappearDistance) continue;

                ReturnToPool(enemy);
            }
            else
            {
                _clearEnemyList[enemy] += _checkInterval;
            }
        }
    }

    private void ReturnToPool(Enemy enemy)
    {
        if (_enemyTypeMap.TryGetValue(enemy, out EnemyDataScriptableObject data) && _pools.TryGetValue(data, out ObjectPooling<Enemy> pool))
            pool.ReturnToPool(enemy);

        _clearEnemyList.Remove(enemy);
        _enemyTypeMap.Remove(enemy);
    }

    private bool IsObjectInCameraView(Camera cam, Vector3 objectPos)
    {
        Vector3 viewPos = cam.WorldToViewportPoint(objectPos);
        return viewPos.z > 0 && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1;
    }

    private void OnValidate()
    {
        if (mainCamera == null)
            mainCamera = FindAnyObjectByType<Camera>();
    }
}
