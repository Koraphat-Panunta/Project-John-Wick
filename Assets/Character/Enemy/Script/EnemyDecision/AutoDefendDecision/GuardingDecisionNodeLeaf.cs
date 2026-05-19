using UnityEngine;

public class GuardingDecisionNodeLeaf : NodeLeaf 
    , IObserverEnemy
{
    private readonly Enemy _enemy;
    private readonly EnemyCommandAPI _api;

    private readonly float _minGuardRate;
    private readonly float _maxGuardRate;
    private readonly float _minCounterRate;
    private readonly float _maxCounterRate;

    private float _gotHitReactionGuardRate;
    private float _counterReactionRate;

    public float gotHitReactionGuardRate => _gotHitReactionGuardRate;
    public float counterReactionRate     => _counterReactionRate;

    private TaskingExecuteQueue counterAttackTask;

    private ITaskingExecute evadeTask;
    private ITaskingExecute couterTask;

    public GuardingDecisionNodeLeaf(
        Enemy enemy
        , EnemyCommandAPI api
        ,
        float minGuardRate, float maxGuardRate,
        float minCounterRate, float maxCounterRate)
        : base(() => enemy.isDead == false)
    {
        this._enemy = enemy;
        this._api = api;
        this._minGuardRate = minGuardRate;
        this._maxGuardRate = maxGuardRate;
        this._minCounterRate = minCounterRate;
        this._maxCounterRate = maxCounterRate;

        this.counterAttackTask = new TaskingExecuteQueue();
        this.evadeTask = new TaskingExecute
            (
            ()=> this._api.TriggerEvade(this._enemy.transform.forward * -1)
            ,()=> this._enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyDodgeStateNodeLeaf>()
            );
        this.couterTask = new TaskingExecute
            (
            ()=> this._api.SpinKick()
            ,()=> this._enemy.stateManagerNode.TryGetCurNodeLeaf<EnemySpinKickGunFuNodeLeaf>()
            );

        this._enemy.AddObserver(this);
    }

    public override void Enter()
    {
        this._gotHitReactionGuardRate = Random.Range(_minGuardRate, _maxGuardRate);
        this._counterReactionRate = Random.Range(_minCounterRate, _maxCounterRate);
        base.Enter();
    }

    public override void UpdateNode()
    {
        this.counterAttackTask.Update();

        if (this._gotHitReactionGuardRate <= 0)
            _api.Guard();

        base.UpdateNode();
    }

    public override bool IsReset() => _enemy.isDead;
    public override bool IsComplete() => false;

   
    private void CounterAttack()
    {
        this.counterAttackTask.Enqueue(this.evadeTask);
        this.counterAttackTask.Enqueue(this.couterTask);
    }

    public void OnNotify<T>(Enemy enemy, T node)
    {


        if(node is GotGunFuHitNodeLeaf gunFuHitNodeLeaf 
            && gunFuHitNodeLeaf.curGotHitPhase == GotGunFuHitNodeLeaf.GotHitPhase.Enter)
        {
            this._gotHitReactionGuardRate = Mathf.Clamp
                (
                this._gotHitReactionGuardRate - 1
                , 0
                , this._gotHitReactionGuardRate
                );
        }
        
        if(node is BlockStateNodeLeaf blockStateNodeLeaf 
            && blockStateNodeLeaf.curstate == EnemyStateLeafNode.Curstate.Enter)
        {
            this._counterReactionRate = Mathf.Clamp
               (
               this._counterReactionRate - 1
               , 0
               , this._counterReactionRate
               );

            if (_counterReactionRate <= 0)
            {
                this.CounterAttack();
                this._counterReactionRate = Random.Range(_minCounterRate, _maxCounterRate);
            }
        }
    }
}
