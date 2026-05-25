using UnityEngine;
using System.Collections.Generic;
using static SubjectEnemy;

public class EnemyDirector :
    Actor,
    IObserverEnemy,
    IObserverPlayer,
    IInitializedAble
{
    protected Dictionary<Enemy, IEnemyDirectedAble> enemysDirectedAble = new Dictionary<Enemy, IEnemyDirectedAble>();

    [SerializeField] public int MAX_ChaserCount;
    [SerializeField] private int chaserCount;
    [SerializeField] private int overwatchCount;
    public int allEnemiesAliveCount => enemysDirectedAble.Count;

    [SerializeField] public float chaserChangeDelay;
    private float elapseTimeChaserChange;
    private int assignCycleCount;

    private const int MAX_MeleeChaserCount = 1;

    [SerializeField] private Player player;

    public Vector3 globalTargetKnowPos;

    public void Initialized()
    {
        player.AddObserver(this);
        this.globalTargetKnowPos = this.player.transform.position;
    }

    void Update()
    {
        UpdateOverwatchShootPoint();
        UpdateRoleManager();
    }

    public void AddEnemy(IEnemyDirectedAble enemyDirected)
    {
        enemyDirected._enemy.AddObserver(this);
        enemysDirectedAble.Add(enemyDirected._enemy, enemyDirected);
        enemyDirected._enemyCommandAPI.NormalFiringPattern = new NormalFiringPatternEnemyDirectorBased(enemyDirected._enemyCommandAPI, this, enemyDirected);
    }

    public void RemoveEnemy(IEnemyDirectedAble enemyRoleBasedDecision)
    {
        RemoveEnemy(enemyRoleBasedDecision._enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemy.RemoveObserver(this);
        enemysDirectedAble.Remove(enemy);
    }

    public void OnNotify<T>(Enemy enemy, T node)
    {
        if(node is FindiAndTrackingTargetNodeLeaf findiAndTrackingTargetNodeLeaf)
        {
            this.globalTargetKnowPos = findiAndTrackingTargetNodeLeaf.targetKnewPos;
        }

        if (node is EnemyEvent enemyEvent
            && enemyEvent == EnemyEvent.GotBulletHit
            && enemy.getPosturePainPhase == Enemy.EnemyPosturePainStatePhase.HeavyPainState)
            AssignChaserManual(enemysDirectedAble[enemy]);

        if (node is EnemyStateLeafNode enemyStateNode)
            switch (enemyStateNode)
            {
                case EnemyDeadStateNode deadNode when deadNode.curstate == NodePhase.Enter:
                    RemoveEnemy(enemy);
                    elapseTimeChaserChange = chaserChangeDelay;
                    RecalculateRoleCounts();
                    break;

                case IGotGunFuAttackNode:
                    AssignChaserManual(enemysDirectedAble[enemy]);
                    break;
            }
    }

    private void UpdateRoleManager()
    {
        if (chaserCount >= MAX_ChaserCount)
            return;

        elapseTimeChaserChange -= Time.deltaTime;
        if (elapseTimeChaserChange > 0)
            return;

        if (assignCycleCount >= MAX_ChaserCount)
        {
            elapseTimeChaserChange = chaserChangeDelay * 2;
            assignCycleCount = 0;
        }
        else
        {
            assignCycleCount++;
            elapseTimeChaserChange = chaserChangeDelay;
        }

        AssignChaserAuto();
    }

    // Promotes the nearest non-chaser enemy (Alert phase) to Ambush.
    // Prefers enemies within 5 units; otherwise picks the globally nearest candidate.
    // Melee enemies are skipped when the melee chaser slot is already filled.
    private void AssignChaserAuto()
    {
        if (allEnemiesAliveCount <= 0)
            return;

        int meleeChaserCount = CountChasersWithMelee();
        IEnemyDirectedAble nearestCandidate = null;

        foreach (IEnemyDirectedAble e in enemysDirectedAble.Values)
        {
            if (e._curCommandPerforme == EnemyRoleCommand.Ambush) continue;
            if (e._combatPhase != CombatPhase.Alert) continue;
            if (IsMeleeEnemy(e) && meleeChaserCount >= MAX_MeleeChaserCount) continue;

            float dist = Vector3.Distance(e._enemy.targetKnowPos, e._enemy.transform.position);

            if (dist <= 5f)
            {
                e.SetDirectorCommand(EnemyRoleCommand.Ambush);
                RecalculateRoleCounts();
                return;
            }

            if (nearestCandidate == null || dist <
                Vector3.Distance(nearestCandidate._enemy.targetKnowPos, nearestCandidate._enemy.transform.position))
            {
                nearestCandidate = e;
            }
        }

        if (nearestCandidate == null)
            return;

        nearestCandidate.SetDirectorCommand(EnemyRoleCommand.Ambush);
        RecalculateRoleCounts();
    }

    // Reactively promotes a specific enemy to Ambush (triggered by GunFu or heavy pain hit).
    // Rotates the first found Ambush enemy back to Support when at capacity.
    // Blocked for melee enemies when the melee chaser slot is already filled.
    private void AssignChaserManual(IEnemyDirectedAble enemyRoleBased)
    {
        if (elapseTimeChaserChange > 0)
            return;
        if (IsMeleeEnemy(enemyRoleBased) && CountChasersWithMelee() >= MAX_MeleeChaserCount)
            return;

        if (chaserCount >= MAX_ChaserCount)
        {
            foreach (IEnemyDirectedAble e in enemysDirectedAble.Values)
            {
                if (e._curCommandPerforme == EnemyRoleCommand.Ambush)
                {
                    e.SetDirectorCommand(EnemyRoleCommand.Support);
                    break;
                }
            }
        }

        enemyRoleBased.SetDirectorCommand(EnemyRoleCommand.Ambush);
        RecalculateRoleCounts();
    }

    private void RecalculateRoleCounts()
    {
        int chasers = 0;
        int overwatch = 0;

        foreach (IEnemyDirectedAble e in enemysDirectedAble.Values)
        {
            if (e._curCommandPerforme == EnemyRoleCommand.Ambush) chasers++;
            else if (e._curCommandPerforme == EnemyRoleCommand.Support) overwatch++;
        }

        chaserCount = chasers;
        overwatchCount = overwatch;
    }

    private static bool IsMeleeEnemy(IEnemyDirectedAble enemyDirected)
        => enemyDirected._enemy._curMeleeWeapon != null;

    private int CountChasersWithMelee()
    {
        int count = 0;
        foreach (IEnemyDirectedAble e in enemysDirectedAble.Values)
        {
            if (e._curCommandPerforme == EnemyRoleCommand.Ambush && IsMeleeEnemy(e))
                count++;
        }
        return count;
    }

    #region Shooter Permission

    [SerializeField] private int maxNumberChaserShooter;
    [SerializeField] private int maxNumberOverwatchShooter;
    [SerializeField] private int maxOverwatchShootPoint;
    [SerializeField] private int overwatchShootPoint;
    [SerializeField] private float shootPointCoolDown;
    [SerializeField] private float shootPointCoolDownTimer;

    private void UpdateOverwatchShootPoint()
    {
        if (overwatchShootPoint >= maxOverwatchShootPoint)
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
        if (Vector3.Distance(enemyDirected._enemy.targetKnowPos, enemyDirected._enemy.transform.position) < 3.5f)
            return true;

        switch (enemyDirected._curCommandPerforme)
        {
            case EnemyRoleCommand.Ambush:
                return CountActiveShooters(EnemyRoleCommand.Ambush) < maxNumberChaserShooter;

            case EnemyRoleCommand.Support:
                if (overwatchShootPoint <= 0) return false;
                if (CountActiveShooters(EnemyRoleCommand.Support) >= maxNumberOverwatchShooter) return false;
                overwatchShootPoint--;
                return true;
        }

        return false;
    }

    private int CountActiveShooters(EnemyRoleCommand role)
    {
        int count = 0;
        foreach (IEnemyDirectedAble e in enemysDirectedAble.Values)
        {
            if (e._curCommandPerforme == role && e._enemyCommandAPI.NormalFiringPattern.isWillShoot)
                count++;
        }
        return count;
    }

    #endregion

    public List<Enemy> GetAllEnemyAlive()
    {
        var enemies = new List<Enemy>();
        foreach (Enemy enemy in enemysDirectedAble.Keys)
        {
            if (!enemy.isDead)
                enemies.Add(enemy);
        }
        return enemies;
    }

    public void UpdateGlobalTargetKnowPos()
    {
        this.globalTargetKnowPos = this.player.transform.position;
    }

    private void OnValidate()
    {
        if (player == null)
            player = FindAnyObjectByType<Player>();
    }

    public void OnNotify<T>(Player player, T node) { }
}
