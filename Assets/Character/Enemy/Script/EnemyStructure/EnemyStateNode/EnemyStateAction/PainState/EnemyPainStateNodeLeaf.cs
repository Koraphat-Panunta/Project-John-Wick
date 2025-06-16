using System;
using UnityEngine;

public abstract class EnemyPainStateNodeLeaf : EnemyStateLeafNode
{
    protected Animator animator;
    protected abstract string stateName { get; }
    protected EnemyPainStateNodeLeaf(Enemy enemy,Func<bool> preCondition, Animator animator) : base(enemy,preCondition)
    {
        this.animator = animator;
    }
    public override void Enter()
    {

        time = 0;
        (enemy.enemyMovement as EnemyMovement).AddForcePush(enemy.forceSave, IMotionImplusePushAble.PushMode.InstanlyMaintainMomentum);
        animator.CrossFade(stateName, 0.1f, 0,0);

        enemy.NotifyObserver(enemy, this);

        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void UpdateNode()
    {
        time += Time.deltaTime;
        if(time >= painDuration) 
        {
            isComplete = true;
            enemy._painPart = IPainStateAble.PainPart.None;
        }

    }
    public override bool IsComplete()
    {
        return base.IsComplete();
    }
    public override bool IsReset()
    {
        if(IsComplete())
            return true; 

        if(enemy._isPainTrigger)
            return true;

        if(enemy._triggerHitedGunFu)
            return true;

        if(enemy.isDead)
            return true;

        else return false;
    }

    public override void FixedUpdateNode()
    {
        enemy.enemyMovement.MoveToDirWorld(Vector3.zero, enemy.painStateForceStop, enemy.painStateForceStop, IMovementCompoent.MoveMode.MaintainMomentum);
        base.FixedUpdateNode();
    }
    public abstract float painDuration { get; set; }
    public float time;
    public abstract IPainStateAble.PainPart painPart { get; set; }
 

    
}
