using UnityEngine;

public class GotKnockDown_GunFuGotHitNodeLeaf : GunFu_GotHit_NodeLeaf
{
    public GotKnockDown_GunFuGotHitNodeLeaf(Enemy enemy, GunFu_GotHit_ScriptableObject gunFu_GotHit_ScriptableObject) : base(enemy, gunFu_GotHit_ScriptableObject)
    {
    }

    public override void Enter()
    {
        animator.CrossFade(stateName, 0.005f, 0);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.animationDrivenMotionState);
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuGotHit);
        base.Enter();
    }

    public override void Exit()
    {
        enemy.posture = 0;
        enemy._isPainTrigger = true;
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        return _isExit;
    }

    public override void Update()
    {
        base.Update();
    }
}
