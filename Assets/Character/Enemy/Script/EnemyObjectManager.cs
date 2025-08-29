using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyObjectManager:IObserverEnemy
{
    private Camera mainCamera;
    private Enemy enemyPrefab;
    protected ObjectPooling<Enemy> enemyObjPooling;
    public Dictionary<Enemy, float> clearEnemyList { get; protected set; }

    protected readonly int corpseDisapearTime = 5;
    protected readonly int corpseDisapearDistance = 6;

    public EnemyObjectManager(Enemy enemy, Camera mainCamera)
    {
        this.enemyPrefab = enemy;
        this.mainCamera = mainCamera;
        enemyObjPooling = new ObjectPooling<Enemy>(this.enemyPrefab, 20, 20, Vector3.zero);
        clearEnemyList = new Dictionary<Enemy, float>();
    }

    public Enemy SpawnEnemy(Vector3 position, Quaternion rotation, EnemyDirector enemyDirector)
    {
        Enemy enemy = this.SpawnEnemy(position, rotation);

        if (enemy.TryGetComponent<EnemyRoleBasedDecision>(out EnemyRoleBasedDecision enemyRoleBasedDecision))
        {
            enemyDirector.AddEnemy(enemyRoleBasedDecision);
        }


        return enemy;
    }
    public Enemy SpawnEnemy(Vector3 position, Quaternion rotation)
    {
        Enemy enemy = this.enemyObjPooling.Get();
        enemy.transform.position = position;
        enemy.transform.rotation = rotation;
        enemy.AddObserver(this);
        return enemy;
    }    
    private bool IsObjectInCameraView(Camera camera, Vector3 objectPos)
    {
        Vector3 viewPos = camera.WorldToViewportPoint(objectPos);
        bool volume = viewPos.z > 0 && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1;

        return volume;
    }

    float checkTimer = 0f;
    float checkInterval = 0.25f;
    public void ClearCorpseEnemyUpdate()
    {
        checkTimer += Time.deltaTime;

        if(checkTimer < checkInterval)
            return;


        if(clearEnemyList.Count > 0)
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
                    enemyObjPooling.ReturnToPool(enemy);
                    clearEnemyList.Remove(enemy);
                }
                else
                {
                    clearEnemyList[enemy] += checkInterval;
                }
            }
        }

        checkTimer = 0f;
    }

    public void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
       
    }

    public void Notify<T>(Enemy enemy, T node) where T : INode
    {
        if (node is EnemyDeadStateNode enemyDead)
        {
            clearEnemyList.Add(enemy, 0);
            enemy.RemoveObserver(this);
        }
    }
}


