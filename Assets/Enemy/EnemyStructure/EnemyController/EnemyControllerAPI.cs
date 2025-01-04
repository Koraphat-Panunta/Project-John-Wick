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
        enemy.moveInputVelocity_World = MoveDirWorld.normalized*velocity;
    }
    public void RotateToPos(Vector3 pos,float rotSpeed)
    {
        Quaternion rotation = new RotateObjectToward().RotateToward(pos - enemy.transform.position, enemy.transform, rotSpeed);

        Rotate(rotation);
    }
    public void Rotate(Quaternion rotate)
    {
        enemy.rotating = rotate;
    }
    //public void FreezRotate()
    //{
    //    enemy.rotating = Quaternion.identity;
    //}
    public void Sprint()
    {
        enemy.isSprint = true;
    }
    public void Freez()
    {
        enemy.isSprint = false;
        enemy.moveInputVelocity_World = Vector3.zero;

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
    public void TakeCover()
    {
        enemy.isInCover = true;
    }
    public void GetOffCover()
    {
        enemy.isInCover = false;
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

    #region InitializedSearchGoal

    private SearchingGoal _searchingGoal { get; set; }   

    #endregion

    public void GOAP_Update()
    {
        if (curGoal.IsReset())
        {
            curGoal.Enter();
            startSelecotr.Transition(out EnemyGoalLeaf enemyGoalLeaf);
            curGoal = enemyGoalLeaf;
            curGoal.Enter();
        }

        if (curGoal != null) 
        curGoal.Update();
    }

    public void GOAP_FixedUpdate()
    {
        if (curGoal != null) 
        curGoal.FixedUpdate();
    }

    public void InitailizedGOAP()
    {
        _enemy = this.enemy;
        _enemyController = this;

        startSelecotr = new EnemyGoalSelector(this,this,() => true);

        _searchingGoal = new SearchingGoal(_enemyController,this, _findingTarget);
        _encouterGoal = new EncouterGoal(this, _enemyGOAP, _findingTarget);
        _holdingGoal = new HoldingGoal(this, _enemyGOAP, _findingTarget);
        _takeCoverGoal = new TakeCoverGoal(this, _enemyGOAP, _coverUseable);

        startSelecotr.AddChildNode(_takeCoverGoal);
        startSelecotr.AddChildNode(_encouterGoal);
        startSelecotr.AddChildNode(_holdingGoal);
        startSelecotr.AddChildNode(_searchingGoal);


    }
}
