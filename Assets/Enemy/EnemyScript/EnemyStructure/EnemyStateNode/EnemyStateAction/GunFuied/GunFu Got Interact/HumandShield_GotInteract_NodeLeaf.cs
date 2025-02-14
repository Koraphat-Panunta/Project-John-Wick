using System;
using UnityEngine;
using static HumanShield_GunFuInteraction_NodeLeaf;

public class HumandShield_GotInteract_NodeLeaf : GunFu_GotInteract_NodeLeaf
{
    public Animator animator;
    public string stateNameEnter = "HumandShielded Enter";
    public string stateNameStay = "HumandShielded Stay";
    public string stateNameExit = "HumandShielded Throw";

    float got_threwDown_time;

    HumanShield_GunFuInteraction_NodeLeaf.InteractionPhase interactionPhase;
    public HumandShield_GotInteract_NodeLeaf(Enemy enemy,Func<bool> preCondition,Animator animator) : base(enemy, preCondition)
    {
        this.animator = animator;
    }

    public override bool _isExit { get; set; }

    public override void Enter()
    {
        got_threwDown_time = 0;
        _isExit = false;
        StateEnter();

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

    public override bool IsReset()
    {
        if(_isExit)
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        if(interactionPhase == InteractionPhase.Exit)
        {
            got_threwDown_time += Time.deltaTime;
            if(got_threwDown_time >= 2.5f)
            {
                //enemy.ChangeStateNode(enemy.fallDown_EnemyState_NodeLeaf);
                _isExit = true;
            }
            else if(got_threwDown_time >= 0.7f)
            {
                enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.ragdollMotionState);
            }

        }
        base.UpdateNode();
    }

    public void StateEnter()
    {
        animator.CrossFade(stateNameEnter, 0.075f, 0);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        interactionPhase = InteractionPhase.Enter;

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuGotInteract);
    }
    public void StateStay()
    {
        animator.CrossFade(stateNameStay, 0.075f, 0);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        interactionPhase = InteractionPhase.Stay;

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuGotInteract);
    }
    public void StateExit()
    {
        animator.CrossFade(stateNameExit, 0.075f, 0);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        interactionPhase = InteractionPhase.Exit;

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuGotInteract);
        enemy._posture = 0;
    }
}
