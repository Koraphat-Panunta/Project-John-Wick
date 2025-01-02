using UnityEngine;
[RequireComponent(typeof(Enemy))]
public class EnemyControllerAPI : MonoBehaviour,IEnemyGOAP,IEncounterGoal,IHoldingGoal,ITakeCoverGoal
{
    public Enemy enemy;


    void Start()
    {
        enemy=GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {

            
    }
    private void FixedUpdate()
    {
        
    }
    public void Move(Vector2 MoveDirWorld, float velocity)
    {
        enemy.moveVelocity_World = MoveDirWorld.normalized*velocity;
    }
    public void Rotate(Quaternion rotate)
    {
        enemy.rotating = rotate;
    }
    public void FreezRotate()
    {
        enemy.rotating = Quaternion.identity;
    }
    public void Sprint(Vector2 SprintDir)
    {
        enemy.isSprint = true;
    }
    public void Freez()
    {
        enemy.isSprint = false;

    }
    public void Stand()
    {
        enemy.curStance = IMovementCompoent.Stance.Stand;
    }
    public void Crouch()
    {
        enemy.curStance = IMovementCompoent.Stance.Crouch;
        enemy.isSprint = false;
    }
    public void Dodge()
    {
        
    }

    public void LowReady()
    {
        IWeaponAdvanceUser weaponAdvanceUser = enemy as IWeaponAdvanceUser;

        weaponAdvanceUser.isAiming = false;
        weaponAdvanceUser.isPullTrigger = false;
    }
    public void AimDownSight()
    {
        IWeaponAdvanceUser weaponAdvanceUser = enemy as IWeaponAdvanceUser;

        weaponAdvanceUser.isAiming = true;
    }
    public void PullTrigger()
    {
        IWeaponAdvanceUser weaponAdvanceUser = enemy as IWeaponAdvanceUser;

        weaponAdvanceUser.isPullTrigger = true;
    }
    public void CancleTrigger()
    {
        IWeaponAdvanceUser weaponAdvanceUser = enemy as IWeaponAdvanceUser;

        weaponAdvanceUser.isPullTrigger = false;
    }
    public void Reload()
    {
        IWeaponAdvanceUser weaponAdvanceUser = enemy as IWeaponAdvanceUser;

        weaponAdvanceUser.isReload = true;
    }


    public EnemyGoalLeaf curGoal { get ; set ; }
    public EnemyGoalSelector startSelecotr { get ; set ; }
    public Enemy _enemy { get ; set ; }
    public EnemyControllerAPI _enemyController { get ; set ; }

    #region InitailizedEncouter

    public IEnemyGOAP _enemyGOAP { get => this ; }
    public IFindingTarget _findingTarget { get => _enemy; }
    public EncouterGoal _encouterGoal { get; set; }

    #endregion

    #region InitailizedHolding
    public HoldingGoal _holdingGoal { get; set; }

    #endregion

    #region InitailizedTakeCover

    public ICoverUseable _coverUseable => _enemy ;
    public TakeCoverGoal _takeCoverGoal { get; set ; }

    #endregion

    public void GOAP_Update()
    {
        
    }

    public void GOAP_FixedUpdate()
    {
        
    }

    public void InitailizedGOAP()
    {
        _enemy = this.enemy;
        _enemyController = this;

        startSelecotr = new EnemyGoalSelector(this,this,() => true,()=> 100);

        _encouterGoal = new EncouterGoal(this, _enemyGOAP, _findingTarget);
        _holdingGoal = new HoldingGoal(this, _enemyGOAP, _findingTarget);
        _takeCoverGoal = new TakeCoverGoal(this, _enemyGOAP, _coverUseable);

    }
}
