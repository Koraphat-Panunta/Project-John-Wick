using UnityEngine;

[RequireComponent(typeof(EnemyCommandAPI))]
public class EnemyAIGoapController : MonoBehaviour, IEnemyGOAP/*, IEncounterGoal, IHoldingGoal, ITakeCoverGoal*/, IPatrolingGoal
{

    void Start()
    {
        InitailizedGOAP();
    }

    // UpdateNode is called once per frame
    void Update()
    {
        GOAP_Update();
    }
    private void FixedUpdate()
    {
        GOAP_FixedUpdate();
    }

    public EnemyGoalLeaf curGoal { get; set; }
    public EnemyGoalSelector startSelecotr { get; set; }
    public Enemy _enemy { get; set; }
    public EnemyCommandAPI _enemyController { get; set; }

    #region InitailizedEncouter

    public IEnemyGOAP _enemyGOAP { get => this; }
    public IFindingTarget _findingTarget { get => _enemy; }
    public EncouterGoal _encouterGoal { get; set; }

    #endregion

    #region InitailizedHolding
    public HoldingGoal _holdingGoal { get; set; }

    #endregion

    #region InitailizedTakeCover

    public ICoverUseable _coverUseable => _enemy;
    public TakeCoverGoal _takeCoverGoal { get; set; }

    #endregion

    #region InitializedSearchGoal

    private SearchingGoal _searchingGoal { get; set; }

    #endregion

    #region InitailizedPatrolingGoal
    public PatrolingGoal _patrolingGoal { get; set; }

    #endregion

    public void GOAP_Update()
    {
        if (curGoal.IsReset())
        {
            curGoal.Exit();
            startSelecotr.Transition(out EnemyGoalLeaf enemyGoalLeaf);
            curGoal = enemyGoalLeaf;
            Debug.Log("Goal =" + curGoal);
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
        _enemy = this._enemy;
        _enemyController = GetComponent<EnemyCommandAPI>();

        startSelecotr = new EnemyGoalSelector(_enemyController, this, () => true);

        _searchingGoal = new SearchingGoal(_enemyController, this, _findingTarget);
        _encouterGoal = new EncouterGoal(_enemyController, _enemyGOAP, _findingTarget);
        _holdingGoal = new HoldingGoal(_enemyController, _enemyGOAP, _findingTarget);
        _takeCoverGoal = new TakeCoverGoal(_enemyController, _enemyGOAP, _coverUseable);
        _patrolingGoal = new PatrolingGoal(_enemyController, this, _enemy);

        startSelecotr.AddChildNode(_takeCoverGoal);
        startSelecotr.AddChildNode(_encouterGoal);
        startSelecotr.AddChildNode(_holdingGoal);
        startSelecotr.AddChildNode(_searchingGoal);
        startSelecotr.AddChildNode(_patrolingGoal);

        startSelecotr.Transition(out EnemyGoalLeaf enemyGoalLeaf);
        curGoal = enemyGoalLeaf;
        Debug.Log("Goal =" + curGoal);
        curGoal.Enter();
    }
}
