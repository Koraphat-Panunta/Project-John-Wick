using UnityEngine;
using System.Collections.Generic;
using static SubjectEnemy;
using System.Linq;

public class EnemyDirector :
    Actor, 
    IObserverEnemy
    ,IObserverPlayer
    ,IInitializedAble
{

    protected Dictionary<Enemy,IEnemyDirectedAble> enemysDirectedAble = new Dictionary<Enemy, IEnemyDirectedAble>();

    [SerializeField] public int MAX_ChaserCount;
    [SerializeField] private int chaserCount;
    [SerializeField] private int overwatchCount;
    public int allEnemiesAliveCount => enemysDirectedAble.Count;

    [SerializeField] public float chaserChangeDelay;
    private float elapseTimeChaserChange;

    [SerializeField] private int assingTime;

    [SerializeField] private Player player;
    public void Initialized()
    {
        player.AddObserver(this);
    }
   
    private void Start()
    {
        //enemiesRole.ForEach(eRole => 
        //{
        //    this.AddEnemy(eRole);
        //});
        assingTime = 0;
    }
    // Update is called once per frame
    void Update()
    {
        this.UpdateOverwatchShootPoint();
        this.UpdateRoleManager();    
    }
    
    public void AddEnemy(IEnemyDirectedAble enemyDirected)
    {
        enemyDirected._enemy.AddObserver(this);
        this.enemysDirectedAble.Add(enemyDirected._enemy, enemyDirected);
        enemyDirected._enemyCommandAPI.NormalFiringPattern = new NormalFiringPatternEnemyDirectorBased(enemyDirected._enemyCommandAPI, this, enemyDirected);
    }
    public void RemoveEnemy(IEnemyDirectedAble enemyRoleBasedDecision)
    {
       this.RemoveEnemy(enemyRoleBasedDecision._enemy);
    }
    public void RemoveEnemy(Enemy enemy)
    {
        enemy.RemoveObserver(this);
        enemysDirectedAble.Remove(enemy);
    }
    public void OnNotify<T>(Enemy enemy,T node)
    {

        if (node is EnemyEvent enemyEvent 
            && enemyEvent == SubjectEnemy.EnemyEvent.GotBulletHit
            && enemy.getPosturePainPhase == Enemy.EnemyPosturePainStatePhase.HeavyPainState)
            AssignChaser(enemysDirectedAble[enemy]);

        if (node is EnemyStateLeafNode enemyStateNodeLeaf)
            switch (enemyStateNodeLeaf)
            {
                case EnemyDeadStateNode deadStateNodeDead:
                    {
                        if (deadStateNodeDead.curstate == EnemyStateLeafNode.Curstate.Enter)
                        {
                            this.RemoveEnemy(enemy);
                            elapseTimeChaserChange = chaserChangeDelay;
                            CalcuateRoleCount();
                        }
                        break;
                    }
                case IGotGunFuAttackNode gotGunFuAttackAbleNode:
                    {
                        AssignChaser(enemysDirectedAble[enemy]);
                        break;
                    }
            }
    }
    private void UpdateRoleManager()
    {
        if (chaserCount < MAX_ChaserCount)
        {
            elapseTimeChaserChange -= Time.deltaTime;
            if (elapseTimeChaserChange <= 0)
            {
                if (assingTime >= MAX_ChaserCount)
                {
                    elapseTimeChaserChange = chaserChangeDelay * 2;
                    assingTime = 0;
                }
                else
                {
                    assingTime += 1;
                    elapseTimeChaserChange = chaserChangeDelay;
                }
                AssignChaser();
            }
        }
    }
    
    private void AssignChaser() //AutoChangeWhen Chaser < MaxChaser find near target
    {

        IEnemyDirectedAble selectedEnemy = null;
        if(allEnemiesAliveCount <=0)
            return;

        IEnemyDirectedAble[] enemies = this.enemysDirectedAble.Values.ToArray();

        for (int i = 0; i < allEnemiesAliveCount; i++)
        {
            if (enemies[i]._curCommandPerforme == EnemyRoleCommand.Ambush)
                continue;

            if (enemies[i]._combatPhase != CombatPhase.Alert)
                continue;

            if (Vector3.Distance(enemies[i]._enemy.targetKnowPos, enemies[i]._enemy.transform.position) <= 5) // Found the near target
            {
                enemies[i].SetDirectorCommand(EnemyRoleCommand.Ambush);
                return;
            }

            // Find the nearest as posible if distance > 5
            if (selectedEnemy == null)
            {
                selectedEnemy = enemies[i];
                continue;
            }

            if (Vector3.Distance(enemies[i]._enemy.targetKnowPos, enemies[i]._enemy.transform.position) <
                Vector3.Distance(selectedEnemy._enemy.targetKnowPos, selectedEnemy._enemy.transform.position)
                )
            {
                selectedEnemy = enemies[i];
            }
        }

        if (selectedEnemy == null)
            return;

        selectedEnemy.SetDirectorCommand(EnemyRoleCommand.Ambush);

        CalcuateRoleCount();

    }
    private void AssignChaser(IEnemyDirectedAble enemyRoleBased) //Manual Change Role
    {
        if(elapseTimeChaserChange > 0)
            return;

        if (chaserCount < MAX_ChaserCount)
            enemyRoleBased.SetDirectorCommand(EnemyRoleCommand.Ambush);
        else if (chaserCount >= MAX_ChaserCount)
        {
            foreach (Enemy enemy in this.enemysDirectedAble.Keys)
            {
                if (this.enemysDirectedAble[enemy]._curCommandPerforme == EnemyRoleCommand.Ambush)
                    this.enemysDirectedAble[enemy].SetDirectorCommand(EnemyRoleCommand.Support);
                break;
            }

            enemyRoleBased.SetDirectorCommand(EnemyRoleCommand.Ambush);

        }

        CalcuateRoleCount();

    }
    private void CalcuateRoleCount()
    {
        int chaserCount = 0;
        int overwatchCount = 0;

        if(this.enemysDirectedAble.Count > 0)
        foreach (IEnemyDirectedAble enemyDirected in this.enemysDirectedAble.Values)
        {

            if (enemyDirected._curCommandPerforme == EnemyRoleCommand.Ambush )
                chaserCount++;

            if (enemyDirected._curCommandPerforme == EnemyRoleCommand.Support)
                overwatchCount++;
        }

        this.chaserCount = chaserCount;
        this.overwatchCount = overwatchCount;
    }

    #region Manage number of shooter 


    [SerializeField] private int maxNumberChaserShooter;
    [SerializeField] private int maxNumberOverwatchShooter;

    [SerializeField] private int maxOverwatchShootPoint;
    [SerializeField] private int overwatchShootPoint;
    [SerializeField] private float shootPointCoolDown;
    [SerializeField] private float shootPointCoolDownTimer;
    private void UpdateOverwatchShootPoint()
    {
        if(overwatchShootPoint >= maxOverwatchShootPoint)
            return;

        if (shootPointCoolDownTimer >= shootPointCoolDown)
        {
            overwatchShootPoint++;
            shootPointCoolDownTimer = 0;
        }
        else
            shootPointCoolDownTimer += Time.deltaTime;
    }
    public bool GetShooterPermission(IEnemyDirectedAble enemyDirected)
    {
        EnemyRoleCommand directedCommand = enemyDirected._curCommandPerforme;

        // Free to shoot if near target
        if (Vector3.Distance(enemyDirected._enemy.targetKnowPos, enemyDirected._enemy.transform.position) < 3.5f)
            return true;

        switch (directedCommand)
        {
            case EnemyRoleCommand.Ambush:
                {
                    int isShootChaser = 0;

                    foreach(IEnemyDirectedAble enemyRoleBD in this.enemysDirectedAble.Values) // Count the all will shoot enemy
                    {
                        if(enemyRoleBD._curCommandPerforme == EnemyRoleCommand.Ambush
                            && enemyRoleBD._enemyCommandAPI.NormalFiringPattern.isWillShoot)
                            isShootChaser++;

                        if (isShootChaser >= maxNumberChaserShooter)
                            return false;
                    }
                    return true;
                }
        
            case EnemyRoleCommand.Support: 
                {
           
                    if(this.overwatchShootPoint <=0)
                        return false;

                    int isShootOverwatch = 0;
                    foreach (IEnemyDirectedAble enemyRoleBD in this.enemysDirectedAble.Values)
                    {
                        if (enemyRoleBD._curCommandPerforme == EnemyRoleCommand.Support
                            && enemyRoleBD._enemyCommandAPI.NormalFiringPattern.isWillShoot)
                            isShootOverwatch++;

                        if(isShootOverwatch >= maxNumberOverwatchShooter)
                            return false;
                    }
                    this.overwatchShootPoint--;
                    return true;
                  
                }
        
        }
        return false;
    }
    #endregion
    public List<Enemy> GetAllEnemyAlive()
    {
        List<Enemy> enemies = new List<Enemy>();
        foreach(Enemy enemy in this.enemysDirectedAble.Keys)
        {
            if (enemy.isDead == false)
                enemies.Add(enemy);
        }
        return enemies;
    }
    private void OnValidate()
    {
        if(player == null)
            player = FindAnyObjectByType<Player>();
    }

   
    public void OnNotify<T>(Player player, T node)
    {
       
    }
  
    
}
