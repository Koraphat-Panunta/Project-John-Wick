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
    public Dictionary<Enemy, float> clearEnemyList { get; protected set; }

    protected readonly int corpseDisapearTime = 10;
    protected readonly int corpseDisapearDistance = 20;

    public EnemyObjectManager(Enemy enemy, Camera mainCamera)
    {
        this.enemyPrefab = enemy;
        this.mainCamera = mainCamera;
        enemyObjPooling = new ObjectPooling<Enemy>(this.enemyPrefab, 10, 4, Vector3.zero);
        clearEnemyList = new Dictionary<Enemy, float>();
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
        AssignEnemyCleaner(enemy);
        return enemy;
    }
    public void ReturnEnemy(Enemy enemy)
    {
        enemyObjPooling.ReturnToPool(enemy);
    }
    public void AssignEnemyCleaner(Enemy enemyCorpse)
    {
        clearEnemyList.Add(enemyCorpse, 0);
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
        while (clearEnemyList.Count > 0)
        {
            List<Enemy> enemies = clearEnemyList.Keys.ToList();
            foreach (Enemy enemy in enemies)
            {

                //Debug.Log("enemy "+ enemy + " check");
                if (enemy.isDead == false)
                    continue;
                //Debug.Log("enemy " + enemy + " isdead");

                if (clearEnemyList[enemy] > this.corpseDisapearTime)
                {
                    //Debug.Log("enemy " + enemy + " isDisapearTime");
                    if (this.IsObjectInCameraView(mainCamera, enemy.transform.position))
                        continue;
                    //Debug.Log("enemy " + enemy + " outsideCamera View");
                    if (Vector3.Distance(mainCamera.transform.position, enemy.transform.position) < corpseDisapearDistance)
                        continue;
                    //Debug.Log("enemy " + enemy + " outsideCamera Distance");
                    this.ReturnEnemy(enemy);
                    clearEnemyList.Remove(enemy);
                }
                else
                    clearEnemyList[enemy] += Time.deltaTime;
            }
            await Task.Yield();
        }
        clearCorpse = null;

    }
}


