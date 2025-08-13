using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine.Rendering;

public class EnemyDirector : MonoBehaviour, IObserverEnemy,IObserverPlayer
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

    //[SerializeField, TextArea] private string debugEnemyDirector;

    [SerializeField] private Player player;
    private void Awake()
    {
        player.AddObserver(this);
    }
    private void Start()
    {
        enemiesRole.ForEach(eRole => 
        { 
            eRole.enemy.AddObserver(this);
            this.enemysGetRole.Add(eRole.enemy, eRole);
            eRole.enemyCommand.NormalFiringPattern = new NormalFiringPatternEnemyDirectorBased(eRole.enemyCommand, this, eRole);
        });
        assingTime = 0;
        chaserShooterPoint = maxNumberChaserShooter;
        overwatchShooterPoint = maxNumberOverwatchShooter;
    }
    // Update is called once per frame
    void Update()
    {
       this.UpdateRoleManager();    
    }
    public void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
        if(enemyEvent == SubjectEnemy.EnemyEvent.GotBulletHit
            && enemy._posture <= enemy._postureHeavy)
            AssignChaser(enemysGetRole[enemy]);
    }
    public void Notify<T>(Enemy enemy,T node)where T : INode
    {
        if(node is EnemyStateLeafNode enemyStateNodeLeaf)
            switch (enemyStateNodeLeaf)
            {
                case EnemyDeadStateNode deadStateNodeDead:
                    {
                        enemy.RemoveObserver(this);
                        enemiesRole.Remove(enemysGetRole[enemy]);
                        enemysGetRole.Remove(enemy);
                        elapseTimeChaserChange = chaserChangeDelay;
                        CalcuateRoleCount();
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

    [SerializeField] private float delayChaserShooterPoint;
    [SerializeField] private float delayOverwatchShooterPoint;

    [SerializeField] private int chaserShooterPoint;
    [SerializeField] private int overwatchShooterPoint;

    private Task taskUpdateChaserShooterPoint;
    private Task taskUpdateOverwatchShooterPoint;
    private Task taskUpdateYieldAllShooterOnPlayerAim;

    [SerializeField] private float yieldShooterOnPlayerAim;
    [SerializeField] private float delayYieldShooterOnPlayerAim;
    [SerializeField] private bool isYieldAllShooter;
    private async Task UpdatingChaserShooterPoint()
    {
        while(chaserShooterPoint < maxNumberChaserShooter)
        {
            float delay = delayChaserShooterPoint * 1000;
            await Task.Delay((int)delay);
            chaserShooterPoint++;
        }

        taskUpdateChaserShooterPoint = null;
    }
    private async Task UpdatingOverwatchShooterPoint()
    {
        while(overwatchShooterPoint < maxNumberOverwatchShooter)
        {
            float delay = delayOverwatchShooterPoint * 1000;
            await Task.Delay((int)delay);
            overwatchShooterPoint++;
        }

        taskUpdateOverwatchShooterPoint = null;
    }
    private async Task UpdatingYieldAllShooterOnPlayerAim()
    {
       
        isYieldAllShooter = true;
        float delay = yieldShooterOnPlayerAim * 1000;
        await Task.Delay((int)delay);
        isYieldAllShooter = false;
        await Task.Delay((int)delayYieldShooterOnPlayerAim * 1000);

        taskUpdateYieldAllShooterOnPlayerAim = null;
    }
    public bool GetShooterPermission(EnemyRoleBasedDecision enemyRoleBasedDecision)
    {
       EnemyActionNodeManager roleAction = enemyRoleBasedDecision.enemyActionNodeManager;
        if (isYieldAllShooter)
            return false;

        switch (roleAction)
        {
            case EnemyChaserRoleNodeManager enemyChaserRole:
                {
                    if (chaserShooterPoint > 0)
                    {
                        chaserShooterPoint -= 1;
                        if (taskUpdateChaserShooterPoint == null)
                            taskUpdateChaserShooterPoint = UpdatingChaserShooterPoint();
                        return true;
                    }
                }
                break;
            case EnemyOverwatchRoleNodeManager enemyOverwatchRole: 
                {
                    

                    if (overwatchShooterPoint > 0)
                    {
                        overwatchShooterPoint -= 1;
                        if (taskUpdateOverwatchShooterPoint == null)
                            taskUpdateOverwatchShooterPoint = UpdatingOverwatchShooterPoint();
                        return true;
                    }
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
        if(node is WeaponManuverLeafNode weaponManuverLeafNode)
            switch (weaponManuverLeafNode)
            {
                case AimDownSightWeaponManuverNodeLeaf aimDownSightWeaponManuverNodeLeaf :
                    taskUpdateYieldAllShooterOnPlayerAim = UpdatingYieldAllShooterOnPlayerAim();
                    break;
            }
    }
    public List<EnemyRoleBasedDecision> GetAllEnemyRoleBasedDecision() => enemiesRole;
}
