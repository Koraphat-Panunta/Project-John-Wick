using UnityEngine;

public class DodgingDecisionNodeLeaf : NodeLeaf 
    , IObserverEnemy
{
    private readonly Enemy _enemy;
    private readonly EnemyCommandAPI _api;

    private readonly float _minCoolDown;
    private readonly float _maxCoolDown;


    public float dodgeCoolDownTimer { get; private set; }
    private IRangeWeaponAdvanceUser _targetFireArmed;

    public DodgingDecisionNodeLeaf(
        Enemy enemy
        , EnemyCommandAPI api,
        float minCoolDown
        , float maxCoolDown
        , float minHitRate
        , float maxHitRate
        )
        : base(() => enemy.isDead == false)
    {
        _enemy = enemy;
        _api = api;
        _minCoolDown = minCoolDown;
        _maxCoolDown = maxCoolDown;
      
    }

    public override void Enter()
    {
        dodgeCoolDownTimer = Random.Range(_minCoolDown, _maxCoolDown);
       
        base.Enter();
    }

    public override void UpdateNode()
    {
        if (dodgeCoolDownTimer > 0)
            dodgeCoolDownTimer -= Time.deltaTime;

        if (dodgeCoolDownTimer <= 0 && IsBeenAimedAt())
            _api.Dodge(Quaternion.AngleAxis(Random.Range(-30, 30), Vector3.up)
                * (_enemy.transform.right * (Random.value > 0.5f ? 1 : -1)));
       

        base.UpdateNode();
    }

    public override bool IsReset() => _enemy.isDead;
    public override bool IsComplete() => false;

    private bool IsBeenAimedAt()
    {
        if (_enemy.target == null)
            return false;

        if (_enemy.target.TryGetComponent<I_EnemyAITargeted>(out I_EnemyAITargeted enemyAITargeted)
            && enemyAITargeted.selfEnemyAIBeenTargeted is IRangeWeaponAdvanceUser weapon)
            _targetFireArmed = weapon;
        else
            _targetFireArmed = null;

        if (_targetFireArmed == null)
            return false;

        return EnemyBewareAnalysis.IsTargetAimingTo(_targetFireArmed, _enemy.transform.position, 1.7f, 12f);
    }

    public void OnNotify<T>(Enemy enemy, T node)
    {
        switch (node)
        {
            case Enemy.EnemyEvent.GotBulletHit:
                {
                    this.dodgeCoolDownTimer -= 1;
                }break;
            case GotGunFuHitNodeLeaf gotGunFuHitNodeLeaf:
                {
                    if(gotGunFuHitNodeLeaf.curGotHitPhase == GotGunFuHitNodeLeaf.GotHitPhase.Enter)
                        this.dodgeCoolDownTimer -= 1;
                }break;
            case EnemyDodgeStateNodeLeaf enemyDodgeStateNodeLeaf:
                {
                    if(enemyDodgeStateNodeLeaf == this._enemy.enemyStateManagerNode.enemyDodgeRollStateNodeLeaf 
                        && enemyDodgeStateNodeLeaf.curstate == EnemyStateLeafNode.Curstate.Exit)
                        this.dodgeCoolDownTimer = Random.Range(_minCoolDown, _maxCoolDown);
                }break;
        }
    }
}
