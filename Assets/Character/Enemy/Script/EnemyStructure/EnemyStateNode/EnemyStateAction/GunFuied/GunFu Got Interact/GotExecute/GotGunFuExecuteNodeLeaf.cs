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

    private IGunFuExecuteNodeLeaf gunFuExecuteNodeLeaf;

    public GotGunFuExecuteNodeLeaf(Enemy enemy, Func<bool> preCondition, AnimationTriggerEventSCRP animationTriggerEventSCRP, GotExecutedStateName gotExecuteStateName) : base(enemy, preCondition)
    {
        this.gotExecutedStateName = gotExecuteStateName;
    }
    
    public override void Enter()
    {
        this.gunFuExecuteNodeLeaf = this._gotExecutedGunFu.curAttackerGunFuNode as IGunFuExecuteNodeLeaf;
        this._gotExecutedGunFu._character._movementCompoent.CancleMomentum();
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);
        this._gotExecutedGunFu._character.animator.CrossFade
            (gotExecuteStateName
            , 0 
            , 0
            , gunFuExecuteNodeLeaf._gunFuExecuteInteractSCRP.animationInteractCharacterDetail[1].enterAnimationOffsetNormalizedTime);


        _ = SubjectAnimationInteract.DelayRootMotion(this._gotExecutedGunFu._character);
        base.Enter();
  
    }
    public override void Exit()
    {
        this._gotExecutedGunFu._character.enableRootMotion = false;

        base.Exit();
    }

    public override void UpdateNode()
    {
        
        //Debug.Log("this._gotExecutedGunFu._character._movementCompoent.V_World = "+this._gotExecutedGunFu._character._movementCompoent.curMoveVelocity_World);
        this._gotExecutedGunFu._character._movementCompoent.CancleMomentum();
        base.UpdateNode();
    }
    public override bool IsReset()
    {
        if(this.IsComplete())
            return true;

        if(this._executerGunFu == null)
            return true;

        return false;
    }

    public void Releses()
    {
        this.isComplete = true;
    }
}
