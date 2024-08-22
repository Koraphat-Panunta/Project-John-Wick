using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Reload;

public class Enemy : Character
{
    [SerializeField] public Animator animator;
    [SerializeField] public WeaponSocket weaponSocket;
    [SerializeField] public NavMeshAgent agent;
    public GameObject Target;
    [SerializeField] public EnemyStateManager enemyStateManager;
    [SerializeField] public EnemyWeaponCommand enemyWeaponCommand;
    [SerializeField] public EnemyPath enemyPath;

    public LayerMask targetMask;
    public IEnemyTactic currentTactic;
    public FieldOfView enemyFieldOfView;
    public EnemyLookForPlayer enemyLookForPlayer;
    public EnemyGetShootDirection enemyGetShootDirection;

    public IEnemyHitReaction enemyHitReaction;

    [SerializeField] private bool isImortal;
    // Start is called before the first frame update
    void Start()
    {
        Target = new GameObject();
        enemyStateManager.enemy = this;
        enemyWeaponCommand.enemy = this;
        enemyPath = new EnemyPath(agent);
        currentTactic = new SerchingTactic(this);
        enemyFieldOfView = new FieldOfView(120, 137,this.gameObject.transform);
        enemyLookForPlayer = new EnemyLookForPlayer(this,targetMask);
       enemyGetShootDirection = new EnemyGetShootDirection(this);
        base.HP = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {

    }

    public override void TakeDamage(float Damage)
    {
        base.TakeDamage(Damage);
        if(base.HP <= 0 && isImortal == false)
        {
            enemyStateManager.ChangeState(enemyStateManager.enemyDead);
        }
    }

    public override void Aiming(Weapon weapon)
    {
        //Weapon Send Sensing
        animator.SetLayerWeight(1, weapon.weapon_StanceManager.AimingWeight);
        base.Aiming(weapon);
    }

    public override void Firing(Weapon weapon)
    {
        animator.SetTrigger("Firing");
        animator.SetLayerWeight(3, 1);
        StartCoroutine(RecoveryFiringLayerWeight());
        base.Firing(weapon);
    }

    public override void LowReadying(Weapon weapon)
    {
        animator.SetLayerWeight(1, weapon.weapon_StanceManager.AimingWeight);
        base.LowReadying(weapon);
    }

    public override void Reloading(Weapon weapon, Reload.ReloadType reloadType)
    {
        if (reloadType == ReloadType.TacticalReload)
        {
            animator.SetTrigger("TacticalReload");
            animator.SetLayerWeight(2, 1);
        }
        else if (reloadType == ReloadType.ReloadMagOut)
        {
            animator.SetTrigger("Reloading");
            animator.SetLayerWeight(2, 1);
        }
        else if (reloadType == ReloadType.ReloadFinished)
        {
            StartCoroutine(RecoveryReloadLayerWeight());
        }
        base.Reloading(weapon, reloadType);
    }
    IEnumerator RecoveryReloadLayerWeight()
    {
        float RecoveryWeight = 10;
        while (animator.GetLayerWeight(2) > 0)
        {
            animator.SetLayerWeight(2, animator.GetLayerWeight(2) - (RecoveryWeight * Time.deltaTime));
            yield return null;
        }
    }
    IEnumerator RecoveryFiringLayerWeight()
    {
        float RecoveryWeight = 10;
        while (animator.GetLayerWeight(3) > 0)
        {
            animator.SetLayerWeight(3, animator.GetLayerWeight(3) - (RecoveryWeight * Time.deltaTime));
            yield return null;
        }
    }
}
