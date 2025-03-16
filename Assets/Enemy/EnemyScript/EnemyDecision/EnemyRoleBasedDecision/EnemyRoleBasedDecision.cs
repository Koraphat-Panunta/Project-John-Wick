using UnityEngine;

public class EnemyRoleBasedDecision : EnemyDecision
{
    public override EnemyCommandAPI enemyCommand { get ; set ; }
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
        enemyCommand = GetComponent<EnemyCommandAPI>();
        curCombatPhase = CombatPhase.Chill;
        pressure = 0;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker)
    {
        if (curCombatPhase == CombatPhase.Alert)
            return;
        curCombatPhase = CombatPhase.Aware;

    }

    protected override void OnNotifySpottingTarget(GameObject target)
    {
        curCombatPhase = CombatPhase.Alert;
    }
}
