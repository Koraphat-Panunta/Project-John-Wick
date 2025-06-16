using System;
using UnityEngine;

public class WeaponGotDisarmedGunFuGotInteractNodeLeaf : GunFu_GotInteract_NodeLeaf
{
    private float duration => weaponGotDisarmedScriptableObject.animationClip.length * weaponGotDisarmedScriptableObject.exitNormalized;
    private WeaponGotDisarmedScriptableObject weaponGotDisarmedScriptableObject;
    public WeaponGotDisarmedGunFuGotInteractNodeLeaf(WeaponGotDisarmedScriptableObject weaponGotDisarmedScriptableObject,Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
    {
        this.weaponGotDisarmedScriptableObject = weaponGotDisarmedScriptableObject;
    }

    public override void Enter()
    {
        enemy.NotifyObserver(enemy, this);
        enemy.animator.CrossFade(weaponGotDisarmedScriptableObject.StateName, 0.25f, 0,weaponGotDisarmedScriptableObject.enterNormalized);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.animationDrivenMotionState);
        base.Enter();
        _timer = weaponGotDisarmedScriptableObject.animationClip.length * weaponGotDisarmedScriptableObject.enterNormalized;
    }

    public override void Exit()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return base.IsComplete();
    }

    public override bool IsReset()
    {

        if (_timer > duration || (enemy._triggerHitedGunFu || enemy._isPainTrigger))
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        if(_timer >= duration)
            isComplete = true;
        enemy.enemyMovement.MoveToDirWorld(Vector3.zero, enemy.breakMaxSpeed, enemy.breakMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
        base.UpdateNode();
    }
}
