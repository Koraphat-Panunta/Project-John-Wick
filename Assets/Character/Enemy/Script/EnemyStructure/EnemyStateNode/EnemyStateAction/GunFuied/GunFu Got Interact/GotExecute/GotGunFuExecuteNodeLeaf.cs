using System;
using UnityEngine;
using System.Threading.Tasks;

public class GotGunFuExecuteNodeLeaf : EnemyStateLeafNode, IGotGunFuExecuteNodeLeaf
{

    public IGotGunFuAttackedAble _gotExecutedGunFu => enemy;
    public IGunFuAble _executerGunFu => this._gotExecutedGunFu.gunFuAbleAttacker;
    public string gotExecuteStateName { get => this._gotExecutedStateName.ToString(); }

    public GotExecutedStateName _gotExecutedStateName => gotExecutedStateName;
    private GotExecutedStateName gotExecutedStateName;

    protected AnimationTriggerEventPlayer animationTriggerEventPlayer;

    public GotGunFuExecuteNodeLeaf(Enemy enemy, Func<bool> preCondition, AnimationTriggerEventSCRP animationTriggerEventSCRP, GotExecutedStateName gotExecuteStateName) : base(enemy, preCondition)
    {
        this.gotExecutedStateName = gotExecuteStateName;
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(animationTriggerEventSCRP);
    }
    
    public override void Enter()
    {
        this.animationTriggerEventPlayer.Rewind();
        this._gotExecutedGunFu._character.animator.CrossFade
            (gotExecuteStateName
            , AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration , 0, this.animationTriggerEventPlayer.enterNormalizedTime);
        this._gotExecutedGunFu._character._movementCompoent.CancleMomentum();
        _= SubjectAnimationInteract.DelayRootMotionEnable(_gotExecutedGunFu._character);

        base.Enter();
    }
    public override void Exit()
    {
        this._gotExecutedGunFu._character.enableRootMotion = false;
        base.Exit();
    }
    public override void UpdateNode()
    {
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);

        base.UpdateNode();
    }
    public override bool IsReset()
    {
        if(this.animationTriggerEventPlayer.IsPlayFinish())
            return true;

        if(this.enemy.isDead)
            return true;

        return false;
    }
   
}
