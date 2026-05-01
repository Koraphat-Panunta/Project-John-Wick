using System;
using UnityEngine;

public class GotExecuteOnGround_NodeLeaf : EnemyStateLeafNode,IGotGunFuExecuteNodeLeaf
{


    private Animator _animator => enemy.animator;


    public IGotGunFuAttackedAble _gotExecutedGunFu => enemy;

    public IGunFuAble _executerGunFu => _gotExecutedGunFu.gunFuAbleAttacker;

    GotExecutedStateName IGotGunFuExecuteNodeLeaf._gotExecutedStateName => this.gotExecutedStateName;
    private GotExecutedStateName gotExecutedStateName;
    private IGunFuExecuteNodeLeaf gunFuExecuteNodeLeaf;
    public string gotExecuteStateName { get => this.gotExecutedStateName.ToString(); }

    public GotExecuteOnGround_NodeLeaf(
        Enemy enemy,AnimationTriggerEventSCRP gunFuExecuteSInteractSCRP, Transform root,Transform hipsBone, Transform[] bones,GotExecutedStateName gotExecuteStateName, Func<bool> preCondition) : base(enemy, preCondition)
    {   
        this.gotExecutedStateName = gotExecuteStateName;

    }
  
    public override bool IsComplete()
    {
        if(this.isComplete)
            return true;

        return false;
    }

    public override bool IsReset()
    {
        if(IsComplete())
            return true;

        if (this._executerGunFu == null)
            return true;

        return false;
    }

    public override void Enter()
    {
        this.gunFuExecuteNodeLeaf = this._gotExecutedGunFu.curAttackerGunFuNode as IGunFuExecuteNodeLeaf;
        _gotExecutedGunFu._character._movementCompoent.CancleMomentum();

        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);
        _animator.CrossFade(gotExecuteStateName, 0, 0, this.gunFuExecuteNodeLeaf._gunFuExecuteInteractSCRP.animationInteractCharacterDetail[1].enterAnimationOffsetNormalizedTime);

        _ = SubjectAnimationInteract.DelayRootMotion(this._gotExecutedGunFu._character);

        enemy.NotifyObserver(enemy, this); 

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

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
    public void Releses()
    {
        this.isComplete = true;
    }
}
 
