using System;
using UnityEngine;

public class WeaponGotDisarmedGunFuGotInteractNodeLeaf : GunFu_GotInteract_NodeLeaf
{
    private string stateName;

    private AnimationTriggerEventSCRP weaponGotDisarmedScriptableObject;
    private AnimationTriggerEventPlayer animationTriggerEventPlayer;
    public WeaponGotDisarmedGunFuGotInteractNodeLeaf(AnimationTriggerEventSCRP weaponGotDisarmedScriptableObject,string StateName,Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
    {
        this.weaponGotDisarmedScriptableObject = weaponGotDisarmedScriptableObject;
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(weaponGotDisarmedScriptableObject);
        this.stateName = StateName;
    }

    public override void Enter()
    {
        animationTriggerEventPlayer.Rewind();
        enemy.animator.CrossFade(stateName, 0.25f, 0,weaponGotDisarmedScriptableObject.enterNormalizedTime);
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.animationDrivenMotionState);
        enemy._character.enableRootMotion = true;
        enemy.NotifyObserver(enemy, this);
        base.Enter();

    }

    public override void Exit()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);
        enemy._character.enableRootMotion = false;
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return this.animationTriggerEventPlayer.IsPlayFinish();
    }

    public override bool IsReset()
    {

        if ((enemy._triggerHitedGunFu || enemy._isPainTrigger))
            return true;

        if(IsComplete())
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
        enemy._movementCompoent.MoveToDirWorld(Vector3.zero, enemy.breakMaxSpeed, enemy.breakMaxSpeed, MoveMode.MaintainMomentum);
        base.UpdateNode();
    }
}
