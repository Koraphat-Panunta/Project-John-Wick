using UnityEngine;

public class EnemyTestAimingDecision : EnemyDecision
{
    [SerializeField] Transform aimingPoint;
    protected override void Update()
    {
        enemyCommand.AimDownSight(this.aimingPoint.position);
        base.Update();
    }
    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker)
    {
        
    }

    protected override void OnNotifySpottingTarget(GameObject target)
    {
        
    }
}
