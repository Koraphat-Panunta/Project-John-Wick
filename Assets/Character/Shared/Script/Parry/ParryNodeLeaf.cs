using System;
using UnityEngine;

public class ParryNodeLeaf : PlayerStateNodeLeaf, IParryNode
{
    private readonly AnimationInteractScriptableObject parryScriptableObject;
    private readonly AnimationTriggerEventPlayer animationTriggerEventPlayer;

    private IMeleeAttackerAble parriedAttacker;
    private bool hasNotifiedParry;

    //NONE-SOLID-Implement//
    private CameraYawRotator _cameraYawRotator;
    //NONE-SOLID-Implement//

    public string _stateName => this.parryScriptableObject != null ? this.parryScriptableObject.name : nameof(ParryNodeLeaf);
    public IDefendMeleeAttackAble _parrier => this.player;
    public IMeleeAttackerAble _parriedAttacker => this.parriedAttacker;

    protected SubjectAnimationInteract sbjectAnimationInteract_1;
    protected SubjectAnimationInteract sbjectAnimationInteract_2;

    public ParryNodeLeaf(Player player, Func<bool> preCondition, AnimationInteractScriptableObject parryScriptableObject)
        : base(player, preCondition)
    {
        this.parryScriptableObject = parryScriptableObject;
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(parryScriptableObject);

        this.animationTriggerEventPlayer.SubscribeEvent(MeleeAttackingPhase.Attacking.ToString(), this.OnAttackingPhase);

        this.sbjectAnimationInteract_1 = new SubjectAnimationInteract(parryScriptableObject, parryScriptableObject.animationInteractCharacterDetail[0]);
        this.sbjectAnimationInteract_2 = new SubjectAnimationInteract(parryScriptableObject, parryScriptableObject.animationInteractCharacterDetail[1]);

        //NONE-SOLID-Implement//
        this._cameraYawRotator = new CameraYawRotator(player.cinemachineCamera, 90f * 4);
        //NONE-SOLID-Implement//
    }

    public override void Enter()
    {

        this.animationTriggerEventPlayer.Rewind();
        this.parriedAttacker = this.player.meleeAttackerAble;
        this.hasNotifiedParry = false;
    
        Vector3 dir = this.parriedAttacker._character._movementCompoent.curPosition - this.player._movementCompoent.curPosition;
        dir = new Vector3(dir.x, 0, dir.z);
        dir.Normalize();
        this.sbjectAnimationInteract_1.RestartSubject(this.player, this.parriedAttacker._character._movementCompoent.curPosition, dir);
        this.sbjectAnimationInteract_2.RestartSubject(this.parriedAttacker._character, this.parriedAttacker._character._movementCompoent.curPosition, dir);
        this.player._character._movementCompoent.CancleMomentum();
        base.Enter();
    }

    public override void UpdateNode()
    {
        if (this.hasNotifiedParry)
        {
            //NONE-SOLID-Implement//
            Vector3 toAttacker = (this.parriedAttacker._character._movementCompoent.curPosition - this.player._movementCompoent.curPosition).normalized * -1;
            this._cameraYawRotator.RotateTowards(toAttacker);
            //NONE-SOLID-Implement//
        }
        this.sbjectAnimationInteract_1.UpdateInteract(Time.deltaTime);
        this.sbjectAnimationInteract_2.UpdateInteract(Time.deltaTime);
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
        if (this.animationTriggerEventPlayer.IsPlayFinish())
        {
            Debug.Log("ParryComplete");
            this.isComplete = true;
        }

        base.UpdateNode();
    }
    public override void Exit()
    {
        this.player.enableRootMotion = false;
        Debug.Log("ParryExit()");
        base.Exit();
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
        this.player.enableRootMotion = true;

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
