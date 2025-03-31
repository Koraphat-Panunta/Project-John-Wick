using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

public class EnemyDirector : MonoBehaviour, IObserverEnemy
{

    [SerializeField] protected List<EnemyRoleBasedDecision> enemiesRole = new List<EnemyRoleBasedDecision>();
    private List<Enemy> enemies = new List<Enemy>();
    protected Dictionary<Enemy,EnemyRoleBasedDecision> enemysGetRole = new Dictionary<Enemy,EnemyRoleBasedDecision>();

    [SerializeField] private EnemyObjectPooling enemyObjectPooling;
    [SerializeField] private WeaponObjectPooling weaponObjectPooling;

    [SerializeField] public int MAX_ChaserCount;
    [SerializeField] private int chaserCount;
    [SerializeField] private int overwatchCount;
    public int allEnemiesAliveCount => enemiesRole.Count;

    [SerializeField] public float chaserChangeDelay;
    private float elapseTimeChaserChange;


    // Update is called once per frame
    void Update()
    {
        if(chaserCount < MAX_ChaserCount)
        {
            elapseTimeChaserChange -= Time.deltaTime;
            if(elapseTimeChaserChange <= 0)
            {
                elapseTimeChaserChange = chaserChangeDelay;
                AssignChaser();
            }
        }
    }

    private Task taskUpdatingEnemyRoleManager;
    private async Task UpdatingEnemyRoleManagerTask()
    {
        while (chaserCount < MAX_ChaserCount)
        {
            if (allEnemiesAliveCount <= 0)
                break;
            elapseTimeChaserChange -= Time.deltaTime;
            if (elapseTimeChaserChange <= 0)
            {
                elapseTimeChaserChange = chaserChangeDelay;
                AssignChaser();
            }
            await Task.Yield();
        }
    }

    [SerializeField, TextArea]
    private string enemyDirectorDebugLog;
    public void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
        if(enemyEvent == SubjectEnemy.EnemyEvent.GunFuGotHit
            || enemyEvent == SubjectEnemy.EnemyEvent.GunFuGotInteract)
        {
            AssignChaser(enemysGetRole[enemy]);
        }
        if(enemyEvent == SubjectEnemy.EnemyEvent.GotHit)
        {
            if (enemy._posture <= enemy._postureHeavy)
            {
                AssignChaser(enemysGetRole[enemy]);
            }
        }

        if(enemyEvent == SubjectEnemy.EnemyEvent.Dead) 
        {
            enemy.RemoveObserver(this);
            enemiesRole.Remove(enemysGetRole[enemy]);
            enemysGetRole.Remove(enemy);
            elapseTimeChaserChange = chaserChangeDelay;
            if (this.taskUpdatingEnemyRoleManager == null)
            {
                this.taskUpdatingEnemyRoleManager = UpdatingEnemyRoleManagerTask();
            }
            CalcuateRoleCount();
            OnEnemyBeenEliminate.Invoke(enemy);
            enemyDirectorDebugLog += enemy+" is dead";

        }

    }
    private void AssignChaser() //AutoChangeWhen Chaser < MaxChaser
    {

        EnemyRoleBasedDecision selectedEnemy = null;
        if(allEnemiesAliveCount <=0)
            return;
        for (int i = 0; i < allEnemiesAliveCount; i++)
        {
            if (enemiesRole[i].enemyActionNodeManager == enemiesRole[i].chaserRoleNodeManager)
                continue;

            if (selectedEnemy == null)
            {
                selectedEnemy = enemiesRole[i];
                continue;
            }

            if (Vector3.Distance(enemiesRole[i].enemy.targetKnewPos, enemiesRole[i].enemy.transform.position) <
                Vector3.Distance(selectedEnemy.enemy.targetKnewPos, selectedEnemy.enemy.transform.position)
                )
            {
                selectedEnemy = enemiesRole[i];
            }
        }

        if (selectedEnemy == null)
            return;

        selectedEnemy.ChangeRole(selectedEnemy.chaserRoleNodeManager);

        CalcuateRoleCount();

    }
    private void AssignChaser(EnemyRoleBasedDecision enemyRoleBased) //Manual Change Role
    {
        if(elapseTimeChaserChange > 0)
            return;

        if (chaserCount < MAX_ChaserCount)
            enemyRoleBased.ChangeRole(enemyRoleBased.chaserRoleNodeManager);
        else if (chaserCount >= MAX_ChaserCount)
        {
            foreach (EnemyRoleBasedDecision enemyRoleBasedDecision in enemiesRole)
            {
                if (enemyRoleBasedDecision.enemyActionNodeManager == enemyRoleBasedDecision.chaserRoleNodeManager)
                    enemyRoleBasedDecision.ChangeRole(enemyRoleBasedDecision.overwatchRoleNodeManager);
                break;
            }
            enemyRoleBased.ChangeRole(enemyRoleBased.chaserRoleNodeManager);
           
        }

        CalcuateRoleCount();

    }
    private void CalcuateRoleCount()
    {
        int chaserCount = 0;
        int overwatchCount = 0;

        if(enemiesRole.Count > 0)
        foreach (EnemyRoleBasedDecision enemyRole in enemiesRole)
        {
            if(enemyRole == null)
                continue;

            if (enemyRole.enemyActionNodeManager == enemyRole.chaserRoleNodeManager)
                chaserCount++;

            if (enemyRole.enemyActionNodeManager == enemyRole.overwatchRoleNodeManager)
                overwatchCount++;
        }

        this.chaserCount = chaserCount;
        this.overwatchCount = overwatchCount;
    }

    public void ClearEnemyAll()
    {
        if(enemies.Count <= 0)
            return;

        enemies.ForEach(enemy => { enemyObjectPooling.ReturnEnemy(enemy);});
        enemies.Clear();
    }
    public void ClearEnemy(Enemy enemy)
    {
        enemyObjectPooling.ReturnEnemy(enemy);
    }
    public void SpawnEnemy(EnemySpawner enemySpawner)
    {
        Enemy enemy = enemyObjectPooling.PullEnemy();
        enemy.transform.position = enemySpawner.transform.position;
        enemy.transform.rotation = enemySpawner.transform.rotation;
        enemysGetRole.Add(enemy,enemy.GetComponent<EnemyRoleBasedDecision>());
        enemiesRole.Add(enemysGetRole[enemy]);
        enemies.Add(enemy);
        enemy.AddObserver(this);
        Weapon weapon = weaponObjectPooling.Pull();
        weapon.AttatchWeaponTo(enemy);
        CalcuateRoleCount();
    }
     
    private void OnValidate()
    {
        enemyObjectPooling = FindAnyObjectByType<EnemyObjectPooling>();
        weaponObjectPooling = FindAnyObjectByType<WeaponObjectPooling>();
    }

    public Action<Enemy> OnEnemyBeenEliminate;
}
