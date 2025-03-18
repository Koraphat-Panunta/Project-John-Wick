using UnityEngine;
using System.Collections.Generic;

public class EnemyRoleManager : MonoBehaviour, IObserverEnemy,IGameLevelMasterObserver
{

    [SerializeField] protected List<EnemyRoleBasedDecision> enemies = new List<EnemyRoleBasedDecision>();
    protected Dictionary<Enemy,EnemyRoleBasedDecision> enemysRole = new Dictionary<Enemy,EnemyRoleBasedDecision>();


    [SerializeField] private int MAX_ChaserCount;
    [SerializeField] private int chaserCount;
    [SerializeField] private int overwatchCount;
    public int allEnemiesAliveCount => enemies.Count;

    [SerializeField] private float chaserChangeDelay;
    private float elapseTimeChaserChange;

    [SerializeField] InGameLevelGameMaster InGameLevelGameMaster;
    private void Awake()
    {
        InGameLevelGameMaster.AddObserver(this);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isGameStart == false)
            { return; }

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

    public void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
        if(enemyEvent == SubjectEnemy.EnemyEvent.GunFuGotHit
            || enemyEvent == SubjectEnemy.EnemyEvent.GunFuGotInteract)
        {
            AssignChaser(enemysRole[enemy]);
        }
        if(enemyEvent == SubjectEnemy.EnemyEvent.GotHit)
        {
            if (enemy._posture <= enemy._postureHeavy)
            {
                AssignChaser(enemysRole[enemy]);
            }
        }

        if(enemyEvent == SubjectEnemy.EnemyEvent.Dead) 
        {
            enemy.RemoveObserver(this);
            enemies.Remove(enemysRole[enemy]);
            enemysRole.Remove(enemy);

            elapseTimeChaserChange = chaserChangeDelay;
            CalcuateRoleCount();
        }

    }
    private void AssignChaser()
    {

        EnemyRoleBasedDecision selectedEnemy = null;

        for (int i = 0; i < allEnemiesAliveCount; i++)
        {
            if (enemies[i].enemyActionNodeManager == enemies[i].chaserRoleNodeManager)
                continue;

            if (selectedEnemy == null)
            {
                selectedEnemy = enemies[i];
                continue;
            }

            if (Vector3.Distance(enemies[i].enemy.targetKnewPos, enemies[i].enemy.transform.position) <
                Vector3.Distance(selectedEnemy.enemy.targetKnewPos, selectedEnemy.enemy.transform.position)
                )
            {
                selectedEnemy = enemies[i];
            }
        }

        if (selectedEnemy == null)
            return;

        selectedEnemy.ChangeRole(selectedEnemy.chaserRoleNodeManager);

        CalcuateRoleCount();

    }
    private void AssignChaser(EnemyRoleBasedDecision enemyRoleBased)
    {
        if(elapseTimeChaserChange > 0)
            return;

        if (chaserCount < MAX_ChaserCount)
            enemyRoleBased.ChangeRole(enemyRoleBased.chaserRoleNodeManager);
        else if (chaserCount >= MAX_ChaserCount)
        {
            foreach (EnemyRoleBasedDecision enemyRoleBasedDecision in enemies)
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

        foreach (EnemyRoleBasedDecision enemyRole in enemies)
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
    bool isGameStart = false;
    public void OnNotify(InGameLevelGameMaster inGameLevelGameMaster)
    {
        if(inGameLevelGameMaster.curNodeLeaf == inGameLevelGameMaster.levelHotelGamplayGameMasterNodeLeaf
            &&isGameStart == false)
        {
            foreach (EnemyRoleBasedDecision enemyRoleBasedDecision in enemies)
            {
                enemysRole.Add(enemyRoleBasedDecision.enemy, enemyRoleBasedDecision);
                enemyRoleBasedDecision.enemy.AddObserver(this);
            }
            CalcuateRoleCount();
            isGameStart = true;
        }
    }
}
