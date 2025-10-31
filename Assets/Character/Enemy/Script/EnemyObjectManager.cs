using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyObjectManager: MonoBehaviour ,IInitializedAble,IObserverEnemy
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Enemy enemyPrefab;
    protected ObjectPooling<Enemy> enemyObjPooling;
    public Dictionary<Enemy, float> clearEnemyList { get; protected set; }

    [SerializeField] protected readonly int corpseDisapearTime = 5;
    [SerializeField] protected readonly int corpseDisapearDistance = 6;

    [SerializeField] private int initializedSNumber = 6;
    [SerializeField] private int maxPoolNumber = 30;
   
    public void Initialized()
    {
      
        enemyObjPooling = new ObjectPooling<Enemy>(this.enemyPrefab,this.maxPoolNumber ,this.initializedSNumber , Vector3.zero);
        clearEnemyList = new Dictionary<Enemy, float>();
    }

    public Enemy GetEnemy(Vector3 position, Quaternion rotation)
    {
        Enemy enemy = this.enemyObjPooling.Get(position,rotation);
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
    private void LateUpdate()
    {
        checkTimer += Time.deltaTime;

        if (checkTimer < checkInterval)
            return;
        checkTimer = 0f;

        this.ClearCorpseEnemyUpdate();
    }
    private void ClearCorpseEnemyUpdate()
    {


        if (clearEnemyList.Count > 0)
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


    }

    public void Notify<T>(Enemy enemy, T node) 
    {
        if (node is EnemyDeadStateNode enemyDead)
        {
            clearEnemyList.Add(enemy, 0);
            enemy.RemoveObserver(this);
        }
    }

    private void OnValidate()
    {
        if (this.enemyPrefab == null)
        {
            throw new System.Exception("Please Set EnemyPrefab Initialized " + this);
        }
        if(this.mainCamera == null)
        {
            this.mainCamera = FindAnyObjectByType<Camera>();
        }
    }
}


