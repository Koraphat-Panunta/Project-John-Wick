using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using static Reload;

public class Enemy : SubjectEnemy, IWeaponAdvanceUser
{
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public MultiRotationConstraint rotationConstraint;
    public GameObject Target;
    public EnemyStateManager enemyStateManager;
    public EnemyWeaponCommand enemyWeaponCommand;
    [SerializeField] public EnemyPath enemyPath;

    public LayerMask targetMask;
    public IEnemyTactic currentTactic;
    public FieldOfView enemyFieldOfView;
    public EnemyLookForPlayer enemyLookForPlayer;
    public EnemyHearingSensing enemyHearingSensing;
    public EnemyGetShootDirection enemyGetShootDirection;
    public EnemyComunicate enemyComunicate;
    public float cost;
    public float pressure;

    public IEnemyHitReaction enemyHitReaction;
    public EnemyAgentMovementOverride enemyAgentMovementOverride;

    public EnemyMiniFlinch enemyMiniFlinch;

    [SerializeField] private bool isImortal;
    public Transform rayCastPos;
    public bool isIncombat;

   

    void Start()
    {
        Target = new GameObject();
        enemyStateManager = new EnemyStateManager(this);    
        //enemyWeaponCommand = new EnemyWeaponCommand(this);
        enemyPath = new EnemyPath(agent);
        
        enemyFieldOfView = new FieldOfView(120, 225,this.gameObject.transform);
        enemyLookForPlayer = new EnemyLookForPlayer(this,targetMask);
       enemyGetShootDirection = new EnemyGetShootDirection(this);
        enemyHearingSensing = new EnemyHearingSensing(this);
        enemyComunicate = new EnemyComunicate(this);
        enemyAgentMovementOverride = new EnemyAgentMovementOverride(agent);
        enemyMiniFlinch = new EnemyMiniFlinch(this);

        enemyStateManager._currentState = enemyStateManager._idle;
        enemyStateManager._currentState.StateEnter(enemyStateManager);

        currentTactic = new SerchingTactic(this);
        Initialized_IWeaponAdvanceUser();
        new WeaponFactorySTI9mm().CreateWeapon(this);
        cost = Random.Range(36, 40);
        pressure = 100;
        this.isIncombat = false;
        //base.isDead = false;

        base.HP = 100;
    }

    void Update()
    {
        enemyStateManager.Update();
    }
    private void FixedUpdate()
    {
        enemyStateManager.FixedUpdate();
    }

    public override void TakeDamage(float Damage)
    {
        base.TakeDamage(Damage);
        if(base.HP <= 0 && isImortal == false)
        {
            NotifyObserver(this, EnemyEvent.Dead);
            enemyStateManager.ChangeState(enemyStateManager.enemyDead);
        }
    }

    //public override void Aiming(Weapon weapon)
    //{
    //    //Weapon Send Sensing
    //    animator.SetLayerWeight(1, weapon.weapon_StanceManager.AimingWeight);
    //    base.Aiming(weapon);
    //}

    //public override void Firing(Weapon weapon)
    //{
    //    animator.SetTrigger("Firing");
    //    animator.SetLayerWeight(3, 1);
    //    StartCoroutine(RecoveryFiringLayerWeight());
    //    base.Firing(weapon);
    //}

    //public override void LowReadying(Weapon weapon)
    //{
    //    animator.SetLayerWeight(1, weapon.weapon_StanceManager.AimingWeight);
    //    base.LowReadying(weapon);
    //}

    //public override void Reloading(Weapon weapon, Reload.ReloadType reloadType)
    //{
    //    if (reloadType == ReloadType.TacticalReload)
    //    {
    //        animator.SetTrigger("TacticalReload");
    //        animator.SetLayerWeight(2, 1);
    //    }
    //    else if (reloadType == ReloadType.ReloadMagOut)
    //    {
    //        animator.SetTrigger("Reloading");
    //        animator.SetLayerWeight(2, 1);
    //    }
    //    else if (reloadType == ReloadType.ReloadFinished)
    //    {
    //        StartCoroutine(RecoveryReloadLayerWeight(weapon));
    //    }
    //    base.Reloading(weapon, reloadType);
    //}
    public IEnumerator RecoveryReloadLayerWeight(Weapon weapon)
    {
        float RecoveryWeight = 10;
        while (animator.GetLayerWeight(2) > 0)
        {
            //enemyWeaponCommand.ammoProuch.prochReload.Performed(weapon);
            new AmmoProchReload(weaponBelt.ammoProuch).Performed(weapon);
            animator.SetLayerWeight(2, animator.GetLayerWeight(2) - (RecoveryWeight * Time.deltaTime));
            yield return null;
        }
    }
    public IEnumerator RecoveryFiringLayerWeight()
    {
        float RecoveryWeight = 10;
        while (animator.GetLayerWeight(3) > 0)
        {
            animator.SetLayerWeight(3, animator.GetLayerWeight(3) - (RecoveryWeight * Time.deltaTime));
            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        if (Target != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(Target.transform.position, 0.5f);
        }
    }

    [SerializeField]private Transform weaponMainSocket;
    [SerializeField] private Transform primaryWeaponHoster;
    [SerializeField] private Transform secondaryWeaponHoster;
    public Animator weaponUserAnimator { get; set; }
    public Weapon currentWeapon { get; set; }
    public Transform currentWeaponSocket { get; set; }
    public Transform leftHandSocket { get; set; }
    public Vector3 pointingPos { 
        get { return enemyGetShootDirection.GetDir(); }
        set { } 
    }
    public WeaponBelt weaponBelt { get; set; }
    public WeaponAfterAction weaponAfterAction { get; set; }
    public WeaponCommand weaponCommand { get; set; }
    public void Initialized_IWeaponAdvanceUser()
    {
        weaponUserAnimator = animator;
        currentWeaponSocket = weaponMainSocket;
        weaponBelt = new WeaponBelt(primaryWeaponHoster, secondaryWeaponHoster, new AmmoProuch(90, 90, 360, 360));
        weaponAfterAction = new WeaponAfterActionEnemy(this);
        weaponCommand = new WeaponCommand(this);
    }
}
