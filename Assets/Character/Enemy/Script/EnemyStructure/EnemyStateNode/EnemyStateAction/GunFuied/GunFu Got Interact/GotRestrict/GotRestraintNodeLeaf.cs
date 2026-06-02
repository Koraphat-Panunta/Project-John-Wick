using System;
using UnityEngine;

public class GotRestraintNodeLeaf : EnemyStateLeafNode, IGotGunFuAttackNode
{
   
    public override bool isComplete { get; protected set; }

    private I_OCM_Attack_Able attacker => enemy.gunFuAbleAttacker;

    public RestrainGunFuStateNodeLeaf.RestrictGunFuPhase curRestrainPhase;

    private GotRestrictScriptableObject gotRestrictScriptableObject;
    private RestrainGunFuStateNodeLeaf attackerRestrict;

    private AnimationTriggerEventPlayer exitAnimationTriggerEventPlayer;

    public GotRestraintNodeLeaf(GotRestrictScriptableObject gotRestrictScriptableObject, Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
    {
        this.gotRestrictScriptableObject = gotRestrictScriptableObject;
        this.exitAnimationTriggerEventPlayer = new AnimationTriggerEventPlayer(gotRestrictScriptableObject.animationTriggerEventSCRP);
    }

    public override void Enter()
    {
        this.enemy._movementCompoent.CancleMomentum();
        this.exitAnimationTriggerEventPlayer.Rewind();
        this.attackerRestrict = enemy.gunFuAbleAttacker.curGunFuNode as RestrainGunFuStateNodeLeaf;
        this.curRestrainPhase = RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Enter;
        this.enemy.enableRootMotion = true;

        enemy.friendlyFirePreventingBehavior.DisableFriendlyFirePreventing();
        enemy.NotifyObserver(enemy, this);
        base.Enter();
    }

    public override void Exit()
    {
        this.enemy.enableRootMotion = false;
        this.curRestrainPhase = RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.None;
        enemy.friendlyFirePreventingBehavior.EnableFriendlyFirePreventing();
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return isComplete;
    }

    public override bool IsReset()
    {
        if (IsComplete())
            return true;
        if (enemy._triggerEnterGotAttacked_OCM)
            return true;
        if (enemy.isDead)
            return true;
        if (curRestrainPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Exit
            && enemy._isPainTrigger)
            return true;
        return false;
    }

    public void Hold()
    {
        this.curRestrainPhase = RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Stay;
        this.enemy.enableRootMotion = false;
        this.enemy.NotifyObserver(enemy, this);
    }

    public void Relese()
    {
        this.curRestrainPhase = RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Exit;
        this.enemy.enableRootMotion = true;
        this.enemy.NotifyObserver(enemy, this);
    }

    public override void UpdateNode()
    {
       if(curRestrainPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Exit)
        {
            this.exitAnimationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
            if(this.exitAnimationTriggerEventPlayer.IsPlayFinish())
                this.isComplete = true;
        }
        base.UpdateNode();
    }
}
