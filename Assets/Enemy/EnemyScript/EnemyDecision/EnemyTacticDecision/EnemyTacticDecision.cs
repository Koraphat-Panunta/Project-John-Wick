using NUnit.Framework;
using UnityEngine;

public class EnemyTacticDecision : EnemyDecision
{
    public override EnemyCommandAPI enemyCommand { get ; set ; }
    public TacticDecision curTacticDecision { get; set ; }
    public SearchingTacticDecision searchingTacticDecision { get; private set; }
    public EncouterTacticDecision encouterTacticDecision { get; private set; }
    public HoldingTacticDecision holdingTacticDecision { get; private set; }
    public TakeCoverTacticDecision takeCoverTacticDecision { get; private set; }

    public enum CombatPhase
    {
        Chill,
        Aware,
        Alert
    }
    public CombatPhase curCombatPhase;
    public float pressure;

    protected override void Awake()
    {
        base.Awake();
        enemy.NotifyCommunicate += OnNotifyGetCommunicate;

        curCombatPhase = CombatPhase.Chill;

        searchingTacticDecision = new SearchingTacticDecision(enemy, this);
        encouterTacticDecision = new EncouterTacticDecision(enemy, this);
        holdingTacticDecision = new HoldingTacticDecision(enemy, this);
        takeCoverTacticDecision = new TakeCoverTacticDecision(enemy, this);

        curTacticDecision = searchingTacticDecision;
        curTacticDecision.Enter();

    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (curTacticDecision != null)
            curTacticDecision.Update();
        base .Update();
    }

    protected override void FixedUpdate()
    {
        if (curTacticDecision != null)
            curTacticDecision.FixedUpdate();
        base.FixedUpdate();
    }

    public void ChangeTactic(TacticDecision nexttacticDecision)
    {
        if (nexttacticDecision == curTacticDecision)
            return;

        curTacticDecision.Exit();
        curTacticDecision = nexttacticDecision;
        curTacticDecision.Enter();
    }

    private void OnDrawGizmos()
    {
       
    }

    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker)
    {
        if(curCombatPhase == CombatPhase.Alert)
            return;
        curCombatPhase = CombatPhase.Aware;

    }

    protected override void OnNotifySpottingTarget(GameObject target)
    {
        curCombatPhase = CombatPhase.Alert;
        enemyCommand.NotifyFriendly(15, EnemyCommunicator.EnemyCommunicateMassage.SendTargetPosition);
    }

    private void OnNotifyGetCommunicate(Communicator communicator)
    {
        if (communicator is EnemyCommunicator enemyCommunicator == false)
            return;
        switch (enemyCommunicator.enemyCommunicateMassage)
        {
            case EnemyCommunicator.EnemyCommunicateMassage.SendTargetPosition:
                {
                    if (curCombatPhase == CombatPhase.Alert)
                        return;
                    curCombatPhase = CombatPhase.Aware;
                }
                break;
        }
    }
}
