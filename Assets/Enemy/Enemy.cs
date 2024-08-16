using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public FieldOfView enemyFieldOfView { get; private set; }
    public EnemyLookForPlayer enemyLookForPlayer;
    public EnemyGetShootDirection enemyGetShootDirection;
    // Start is called before the first frame update
    void Start()
    {
        Target = new GameObject();
        enemyStateManager.enemy = this;
        enemyWeaponCommand.enemy = this;
        enemyPath = new EnemyPath(agent);
        currentTactic = new Flanking(this);
        enemyFieldOfView = new FieldOfView(20, 137,this.gameObject);
        enemyLookForPlayer = new EnemyLookForPlayer(this,targetMask);
       enemyGetShootDirection = new EnemyGetShootDirection(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentTactic.Manufacturing();
    }
    private void FixedUpdate()
    {

    }

    public override void Aiming(Weapon weapon)
    {
        //Weapon Send Sensing
        animator.SetLayerWeight(1, weapon.weapon_StanceManager.AimingWeight);
    }

    public override void Firing(Weapon weapon)
    {
        
    }

    public override void LowReadying(Weapon weapon)
    {
        animator.SetLayerWeight(1, weapon.weapon_StanceManager.AimingWeight);
    }

    public override void Reloading(Weapon weapon, Reload.ReloadType reloadType)
    {
        
    }
}
