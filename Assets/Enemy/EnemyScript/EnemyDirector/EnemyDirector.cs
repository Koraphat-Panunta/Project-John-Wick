using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

public class EnemyDirector : MonoBehaviour, IObserverEnemy
{

    [SerializeField] protected List<EnemyRoleBasedDecision> enemiesRole = new List<EnemyRoleBasedDecision>();
    //private List<Enemy> enemies = new List<Enemy>();
    protected Dictionary<Enemy,EnemyRoleBasedDecision> enemysGetRole = new Dictionary<Enemy,EnemyRoleBasedDecision>();

    //[SerializeField] private EnemyObjectPooling enemyObjectPooling;
    //[SerializeField] private WeaponObjectPooling weaponObjectPooling;

    [SerializeField] public int MAX_ChaserCount;
    [SerializeField] private int chaserCount;
    [SerializeField] private int overwatchCount;
    public int allEnemiesAliveCount => enemiesRole.Count;

    [SerializeField] public float chaserChangeDelay;
    private float elapseTimeChaserChange;

    [SerializeField] private int assingTime;

    [SerializeField, TextArea] private string debugEnemyDirector;
    private void Start()
    {
        enemiesRole.ForEach(eRole => 
        { 
            eRole.enemy.AddObserver(this);
            debugEnemyDirector += "Add obsever " + eRole.enemy +"\n";
            this.enemysGetRole.Add(eRole.enemy, eRole);
        });
        assingTime = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (chaserCount < MAX_ChaserCount)
        {
            elapseTimeChaserChange -= Time.deltaTime;
            if (elapseTimeChaserChange <= 0)
            {
                if (assingTime >= MAX_ChaserCount){
                    elapseTimeChaserChange = chaserChangeDelay * 2;
                    assingTime = 0;
                }
                else{
                    assingTime += 1;
                    elapseTimeChaserChange = chaserChangeDelay;
                }
                AssignChaser();
            }
        }
    }
    public void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
        Debug.Log("Enemy = " + enemy + "EnemyEvent = " + enemyEvent + "\n");
        debugEnemyDirector += "Enemy = " + enemy + "EnemyEvent = " + enemyEvent + "\n";
        if (enemyEvent == SubjectEnemy.EnemyEvent.Dead)
        {
            debugEnemyDirector += "Enter Dead" + "\n";
            Debug.Log("Enter Dead" + "\n");
            enemy.RemoveObserver(this);
            enemiesRole.Remove(enemysGetRole[enemy]);
            enemysGetRole.Remove(enemy);
            elapseTimeChaserChange = chaserChangeDelay;
            CalcuateRoleCount();
            Debug.Log("Enter Dead" + "\n");
            debugEnemyDirector += "Exit Dead" + "\n";
            return;
        }

        if (enemyEvent == SubjectEnemy.EnemyEvent.GunFuGotHit
            || enemyEvent == SubjectEnemy.EnemyEvent.GunFuGotInteract)
        {
            AssignChaser(enemysGetRole[enemy]);
        }
        else if(enemyEvent == SubjectEnemy.EnemyEvent.GotHit)
        {
            debugEnemyDirector += "Enter Hit" + "\n";
            Debug.Log("Enter Hit" + "\n");
            if (enemy._posture <= enemy._postureHeavy)
            {
                AssignChaser(enemysGetRole[enemy]);
            }
            Debug.Log("Exit Hit" + "\n");
            debugEnemyDirector += "Exit Hit" + "\n";
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

            if (enemiesRole[i]._curCombatPhase != IEnemyActionNodeManagerImplementDecision.CombatPhase.Alert)
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

    //public void ClearEnemyAll()
    //{
    //    if(enemies.Count <= 0)
    //        return;

    //    enemies.ForEach(enemy => { enemyObjectPooling.ReturnEnemy(enemy);});
    //    enemies.Clear();
    //}
    //public void ClearEnemy(Enemy enemy)
    //{
    //    enemyObjectPooling.ReturnEnemy(enemy);
    //}
    //public void SpawnEnemy(EnemySpawner enemySpawner)
    //{
    //    Enemy enemy = enemyObjectPooling.PullEnemy();
    //    enemy.transform.position = enemySpawner.transform.position;
    //    enemy.transform.rotation = enemySpawner.transform.rotation;
    //    enemysGetRole.Add(enemy,enemy.GetComponent<EnemyRoleBasedDecision>());
    //    enemiesRole.Add(enemysGetRole[enemy]);
    //    enemies.Add(enemy);
    //    enemy.AddObserver(this);
    //    Weapon weapon = weaponObjectPooling.Pull();
    //    weapon.AttatchWeaponTo(enemy);
    //    CalcuateRoleCount();
    //}
     
    private void OnValidate()
    {
        //enemyObjectPooling = FindAnyObjectByType<EnemyObjectPooling>();
        //weaponObjectPooling = FindAnyObjectByType<WeaponObjectPooling>();
    }

    //public Action<Enemy> OnEnemyBeenEliminate;
}
