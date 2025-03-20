using System;
using UnityEngine;

public class WeaponDisarmedGunFuGotInteractNodeLeaf : GunFu_GotInteract_NodeLeaf
{
    private float duration = 1f;
   
    public WeaponDisarmedGunFuGotInteractNodeLeaf(Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
    {
        
    }

    public override void Enter()
    {
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuGotInteract);
        enemy.animator.CrossFade("GotGunFuDisarmWeapon", 0.075f, 0);
        base.Enter();
    }

    public override void Exit()
    {
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
