using UnityEngine;

public class EnemyBlockingTest : EnemyDecision
{
    public override void Initialized()
    {
        
        base.Initialized();
    }
    protected override void Update()
    {
        this.enemyCommand.enemyAutoDefendCommand.UpdateDefendActionBlackBoard();

        //if (
        //     Vector3.Distance(this.enemy._movementCompoent.curPosition, this.enemy.targetKnowPos) < 3.5f
        //     && this.isSpotingTarget)
        //    this.enemyCommand.SpinKick();
        
        base.Update();
    }
    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker)
    {
        
    }

    private bool isSpotingTarget;

    protected override void OnNotifySpottingTarget(GameObject target)
    {
       this.isSpotingTarget = true;
    }
}
