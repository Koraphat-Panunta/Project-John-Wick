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
    public void Move(Vector2 MoveDir, float velocity, Quaternion rotation)
    {

    }
    public void Sprint(Vector2 SprintDir)
    {

    }
    public void Freez()
    {

    }
    public void Stand()
    {

    }
    public void Crouch()
    {

    }
    public void Dodge()
    {

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

        startSelecotr = new EnemyGoalSelector(_enemy,this,() => true);

        _encouterGoal = new EncouterGoal(_enemy, _enemyGOAP, _findingTarget);
        _holdingGoal = new HoldingGoal(_enemy, _enemyGOAP, _findingTarget);
        _takeCoverGoal = new TakeCoverGoal(_enemy, _enemyGOAP, _coverUseable);

    }
}
