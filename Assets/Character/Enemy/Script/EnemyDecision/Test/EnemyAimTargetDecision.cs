using UnityEngine;

public class EnemyAimTargetDecision : EnemyDecision
{
    [SerializeField] Transform target;
    [SerializeField] bool pullTrigger;

    protected override void Update()
    {
        if (target == null) return;

        enemyCommand.AimDownSight(target.position);

        if (pullTrigger)
            enemyCommand.PullTrigger();

        base.Update();
    }

    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker) { }
    protected override void OnNotifySpottingTarget(GameObject spottedTarget) { }
}
