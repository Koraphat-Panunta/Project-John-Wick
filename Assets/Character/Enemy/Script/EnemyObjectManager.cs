using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyObjectManager
{
    private Camera mainCamera;
    private Enemy enemyPrefab;
    protected ObjectPooling<Enemy> enemyObjPooling;
    protected Dictionary<Enemy, float> corpseEnemy;

    protected readonly int corpseDisapearTime = 10;
    protected readonly int corpseDisapearDistance = 20;

    public EnemyObjectManager(Enemy enemy, Camera mainCamera)
    {
        this.enemyPrefab = enemy;
        this.mainCamera = mainCamera;
        enemyObjPooling = new ObjectPooling<Enemy>(this.enemyPrefab, 10, 5, Vector3.zero);
    }
    private void Awake()
    {
        enemyObjPooling = new ObjectPooling<Enemy>(enemyPrefab, 10, 7, Vector3.zero);
        corpseEnemy = new Dictionary<Enemy, float>();
    }

    public Enemy SpawnEnemy(Vector3 position, Quaternion rotation, EnemyDirector enemyDirector)
    {
        Enemy enemy = this.SpawnEnemy(position, rotation);

        if (enemy.TryGetComponent<EnemyRoleBasedDecision>(out EnemyRoleBasedDecision enemyRoleBasedDecision))
            enemyDirector.AddEnemy(enemyRoleBasedDecision);

        return enemy;
    }
    public Enemy SpawnEnemy(Vector3 position, Quaternion rotation)
    {
        Enemy enemy = this.enemyObjPooling.Get();
        enemy.transform.position = position;
        enemy.transform.rotation = rotation;

        return enemy;
    }
    public void ReturnEnemy(Enemy enemy)
    {
        enemyObjPooling.ReturnToPool(enemy);
    }
    public void AssignEnemyCorpseCleaner(Enemy enemyCorpse)
    {
        corpseEnemy.Add(enemyCorpse, 0);
        if (clearCorpse == null)
        {
            clearCorpse = ClearCorpseEnemyUpdate();
        }
    }
    private bool IsObjectInCameraView(Camera camera, Vector3 objectPos)
    {
        Vector3 viewPos = camera.WorldToViewportPoint(objectPos);
        bool isInView = viewPos.z > 0 && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1;

        return isInView;
    }
    private Task clearCorpse;
    private async Task ClearCorpseEnemyUpdate()
    {
        while (corpseEnemy.Count > 0)
        {
            List<Enemy> enemies = corpseEnemy.Keys.ToList();
            foreach (Enemy enemy in enemies)
            {
                if (corpseEnemy[enemy] > this.corpseDisapearTime)
                {
                    if (this.IsObjectInCameraView(mainCamera, enemy.transform.position))
                        continue;

                    if (Vector3.Distance(mainCamera.transform.position, enemy.transform.position) < corpseDisapearDistance)
                        continue;

                    enemyObjPooling.ReturnToPool(enemy);
                    corpseEnemy.Remove(enemy);
                }
                else
                    corpseEnemy[enemy] += Time.deltaTime;
            }
            await Task.Yield();
        }
        clearCorpse = null;

    }
}


