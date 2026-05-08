using System;
using UnityEngine;

public class ParryNodeLeaf : PlayerStateNodeLeaf, IParryNode
{
    private readonly AnimationInteractScriptableObject parryScriptableObject;
    private readonly AnimationTriggerEventPlayer animationTriggerEventPlayer;

    private IMeleeAttackerAble parriedAttacker;
    private bool hasNotifiedParry;

    public string _stateName => this.parryScriptableObject != null ? this.parryScriptableObject.name : nameof(ParryNodeLeaf);
    public IDefendMeleeAttackAble _parrier => this.player;
    public IMeleeAttackerAble _parriedAttacker => this.parriedAttacker;

    public ParryNodeLeaf(Player player, Func<bool> preCondition, AnimationInteractScriptableObject parryScriptableObject)
        : base(player, preCondition)
    {
        this.parryScriptableObject = parryScriptableObject;
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(parryScriptableObject);

        this.animationTriggerEventPlayer.SubscribeEvent(MeleeAttackingPhase.Attacking.ToString(), this.OnAttackingPhase);
    }

    public override void Enter()
    {
        this.animationTriggerEventPlayer.Rewind();
        this.parriedAttacker = this.player.meleeAttackerAble;
        this.hasNotifiedParry = false;
        this.player._character._movementCompoent.CancleMomentum();
        base.Enter();
    }

    public override void UpdateNode()
    {
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
        if (this.animationTriggerEventPlayer.IsPlayFinish())
            this.isComplete = true;

        base.UpdateNode();
    }

    public override bool IsReset()
    {
        if (this.player.isDead)
            return true;
        return this.IsComplete();
    }

    public override bool IsComplete() => this.isComplete;

    private void OnAttackingPhase()
    {
        if (this.hasNotifiedParry)
            return;

        if (this.parriedAttacker == null)
            return;

        if (this.parriedAttacker._character is IGotParriedAble gotParriedAble)
        {
            gotParriedAble.OnParried(this._parrier);
            this.hasNotifiedParry = true;
        }
    }
}
