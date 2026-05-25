using System;
using UnityEngine;

public class GotParriedNodeLeaf : EnemyStateLeafNode, IGotParriedNode
{
    private readonly AnimationTriggerEventPlayer animationTriggerEventPlayer;

    public GotParriedNodeLeaf(Enemy enemy, AnimationTriggerEventSCRP staggerScriptableObject, Func<bool> preCondition)
        : base(enemy, preCondition)
    {
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(staggerScriptableObject);
    }

    public override void Enter()
    {
        Debug.Log("Got Parried Enter");

        if (enemy.motionControlManager.curMotionState != enemy.motionControlManager.codeDrivenMotionState)
            enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        this.isComplete = false;
        this.animationTriggerEventPlayer.Rewind();
        _ = SubjectAnimationInteract.DelayRootMotion(this.enemy);

        base.Enter();
    }

    public override void Exit()
    {
        this.enemy._triggerGotParried = false;
        this.enemy._parrier = null;
        this.enemy.enableRootMotion = false;
        base.Exit();
    }

    public override void UpdateNode()
    {
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
        if (this.animationTriggerEventPlayer.IsPlayFinish())
            this.isComplete = true;

        base.UpdateNode();
    }

    public override bool IsComplete() => this.isComplete;
    public override bool IsReset()
    {
        if (this.enemy.isDead)
            return true;

        if (this.enemy._isPainTrigger
            || this.enemy._triggerEnterGotAttacked_OCM)
            return true;

        return this.IsComplete();
    }
}
