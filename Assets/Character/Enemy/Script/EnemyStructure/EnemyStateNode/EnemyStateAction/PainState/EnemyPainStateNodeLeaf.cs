using System;
using UnityEngine;

public class EnemyPainStateNodeLeaf : EnemyStateLeafNode
{

    protected Animator animator;
    public float painDuration { get; set; }
    public float time;

    public float miniPainStateDuration { get; protected set; }
    public float mediumPainStateDuration { get; protected set; }
    public float heavyPainStateDuration { get; protected set; }
    public EnemyPainStateNodeLeaf(
        Enemy enemy
        ,Func<bool> preCondition
        , Animator animator
        ,float miniPainStateDuration
        ,float mediumPainStateDuration
        ,float heavyPainStateDuration) : base(enemy,preCondition)
    {
        this.animator = animator;

        this.miniPainStateDuration = miniPainStateDuration;
        this.mediumPainStateDuration = mediumPainStateDuration;
        this.heavyPainStateDuration = heavyPainStateDuration;
    }
    public override void Enter()
    {

        time = 0;

        switch (enemy.getPosturePainPhase)
        {
            case Enemy.EnemyPosturePainStatePhase.MiniPainState:
                {
                    this.painDuration = this.miniPainStateDuration;
                    break;
                }
            case Enemy.EnemyPosturePainStatePhase.MediumPainState:
                {
                    this.painDuration = this.mediumPainStateDuration;
                    break;
                }
            case Enemy.EnemyPosturePainStatePhase.HeavyPainState: 
                {
                    this.painDuration = this.heavyPainStateDuration;
                    break;
                }
        }

        (enemy._movementCompoent as EnemyMovement).AddForcePush(enemy.forceSave, IMotionImplusePushAble.PushMode.InstanlyMaintainMomentum);
        animator.CrossFade("PainState", 0.1f, 0,0);

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
            isComplete = true;

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
        enemy._movementCompoent.UpdateMoveToDirWorld(Vector3.zero, enemy.painStateForceStop, enemy.painStateForceStop, MoveMode.MaintainMomentum);
        base.FixedUpdateNode();
    }

 

    
}
