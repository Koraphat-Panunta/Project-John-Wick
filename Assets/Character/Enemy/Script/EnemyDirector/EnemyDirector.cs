using UnityEngine;
using System.Collections.Generic;
using static SubjectEnemy;

public class EnemyDirector : MonoBehaviour, IObserverEnemy,IObserverPlayer,IInitializedAble
{

    [SerializeField] protected List<EnemyRoleBasedDecision> enemiesRole = new List<EnemyRoleBasedDecision>();

    protected Dictionary<Enemy,EnemyRoleBasedDecision> enemysGetRole = new Dictionary<Enemy,EnemyRoleBasedDecision>();

    [SerializeField] public int MAX_ChaserCount;
    [SerializeField] private int chaserCount;
    [SerializeField] private int overwatchCount;
    public int allEnemiesAliveCount => enemiesRole.Count;

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
        enemiesRole.ForEach(eRole => 
        {
            this.AddEnemy(eRole);
        });
        assingTime = 0;
    }
    // Update is called once per frame
    void Update()
    {
        this.UpdateOverwatchShootPoint();
        this.UpdateRoleManager();    
    }
    
    public void AddEnemy(EnemyRoleBasedDecision enemyRoleBasedDecision)
    {
        enemyRoleBasedDecision.enemy.AddObserver(this);
        this.enemiesRole.Add(enemyRoleBasedDecision);
        this.enemysGetRole.Add(enemyRoleBasedDecision.enemy, enemyRoleBasedDecision);
        enemyRoleBasedDecision.enemyCommand.NormalFiringPattern = new NormalFiringPatternEnemyDirectorBased(enemyRoleBasedDecision.enemyCommand, this, enemyRoleBasedDecision);
    }
    public void RemoveEnemy(EnemyRoleBasedDecision enemyRoleBasedDecision)
    {
       this.RemoveEnemy(enemyRoleBasedDecision.enemy);
    }
    public void RemoveEnemy(Enemy enemy)
    {
        enemy.RemoveObserver(this);
        enemiesRole.Remove(enemysGetRole[enemy]);
        enemysGetRole.Remove(enemy);
    }
    public void Notify<T>(Enemy enemy,T node)
    {

        if (node is EnemyEvent enemyEvent 
            && enemyEvent == SubjectEnemy.EnemyEvent.GotBulletHit
            && enemy._posture <= enemy._postureHeavy)
            AssignChaser(enemysGetRole[enemy]);

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
                        AssignChaser(enemysGetRole[enemy]);
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
    public bool GetShooterPermission(EnemyRoleBasedDecision enemyRoleBasedDecision)
    {
       EnemyActionNodeManager roleAction = enemyRoleBasedDecision.enemyActionNodeManager;


        switch (roleAction)
        {
            case EnemyChaserRoleNodeManager enemyChaserRole:
                {
                    if (Vector3.Distance(enemyChaserRole.enemy.targetKnewPos, enemyChaserRole.enemy.transform.position) < 3.5f)
                        return true;

                    int isShootChaser = 0;
                    foreach(EnemyRoleBasedDecision enemyRoleBD in enemiesRole)
                    {
                        if(enemyRoleBD.enemyActionNodeManager == enemyRoleBD.chaserRoleNodeManager
                            &&enemyRoleBD.enemyCommand.NormalFiringPattern.isWillShoot)
                            isShootChaser++;

                        if (isShootChaser >= maxNumberChaserShooter)
                            return false;
                    }
                    return true;
                }
                break;
            case EnemyOverwatchRoleNodeManager enemyOverwatchRole: 
                {
                    if(Vector3.Distance(enemyOverwatchRole.enemy.targetKnewPos,enemyOverwatchRole.enemy.transform.position) < 3.5f)
                        return true;

                    if(overwatchShootPoint <=0)
                        return false;

                    int isShootOverwatch = 0;
                    foreach (EnemyRoleBasedDecision enemyRoleBD in enemiesRole)
                    {
                        if (enemyRoleBD.enemyActionNodeManager == enemyRoleBD.overwatchRoleNodeManager
                            && enemyRoleBD.enemyCommand.NormalFiringPattern.isWillShoot)
                            isShootOverwatch++;

                        if(isShootOverwatch >= maxNumberOverwatchShooter)
                            return false;
                    }
                    overwatchShootPoint--;
                    return true;
                  
                }
                break;
        }
        return false;
    }
    #endregion
    public List<Enemy> GetAllEnemyAlive()
    {
        List<Enemy> enemies = new List<Enemy>();
        foreach(EnemyRoleBasedDecision enemyRole in enemiesRole)
        {
            if (enemyRole.enemy.isDead == false)
                enemies.Add(enemyRole.enemy);
        }
        return enemies;
    }
    private void OnValidate()
    {
        if(player == null)
            player = FindAnyObjectByType<Player>();

        foreach(EnemyRoleBasedDecision enemyRoleBasedDecision in gameObject.GetComponentsInChildren<EnemyRoleBasedDecision>())
        {
            if(enemiesRole.Contains(enemyRoleBasedDecision) == false)
                enemiesRole.Add(enemyRoleBasedDecision);
        }
    }

   
    public void OnNotify<T>(Player player, T node)
    {
       
    }
    public List<EnemyRoleBasedDecision> GetAllEnemyRoleBasedDecision() => enemiesRole;

    
}
