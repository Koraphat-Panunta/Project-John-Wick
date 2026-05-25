using System;
using UnityEngine;

public class MeleeExecute_NodeLeaf : 
    PlayerStateNodeLeaf
    , IGunFuExecuteNodeLeaf
    , IDamageVisitor
{
    public I_OCM_Attack_Able gunFuAble { get; set; }
    public I_Got_OCM_Attacked_Able gotGunFuAttackedAble { get; set; }
    public ExecuteMethod executeMethod;

    public AnimationInteractScriptableObject _gunFuExecuteInteractSCRP => meleeExecuteInteractSCRP;
    private AnimationInteractScriptableObject meleeExecuteInteractSCRP;

    IGunFuExecuteNodeLeaf.GunFuExecutePhase IGunFuExecuteNodeLeaf._curGunFuPhase
    { get => curGunFuPhase; set => curGunFuPhase = value; }
    private IGunFuExecuteNodeLeaf.GunFuExecutePhase curGunFuPhase;

    bool IGunFuExecuteNodeLeaf._isExecuteAldready { get => isExecuteAlready; set => isExecuteAlready = value; }
    private bool isExecuteAlready;

    GunFuExecuteStateName IGunFuExecuteNodeLeaf._executeStateName { get => executeStateName; set => executeStateName = value; }
    private GunFuExecuteStateName executeStateName;

    string I_OCM_Node._stateName => executeStateName.ToString();



    public SubjectAnimationInteract gunFuAble_SubjectInteract;
    public SubjectAnimationInteract got_GunFuAttacked_SubjectInteract;
    public AnimationTriggerEventPlayer animationTriggerEventPlayer;
    public AnimationTriggerAudioEventPlayer audioTriggerEventPlayer;

    public MeleeExecute_NodeLeaf(
        Player player,
        Func<bool> preCondition,
        AnimationInteractScriptableObject interactSCRP,
        GunFuExecuteStateName stateName) : base(player, preCondition)
    {
        this.gunFuAble = player;
        this.meleeExecuteInteractSCRP = interactSCRP;
        this.executeStateName = stateName;
        this.executeMethod = new ExecuteMethod(this);

        this.gunFuAble_SubjectInteract = new SubjectAnimationInteract(
            interactSCRP, interactSCRP.animationInteractCharacterDetail[0]);
        this.got_GunFuAttacked_SubjectInteract = new SubjectAnimationInteract(
            interactSCRP, interactSCRP.animationInteractCharacterDetail[1]);

        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(
            interactSCRP.clip,
            interactSCRP.enterNormalizedTime,
            interactSCRP.endNormalizedTime,
            interactSCRP.triggerEventDetail);

        this.audioTriggerEventPlayer = new AnimationTriggerAudioEventPlayer(
            interactSCRP.clip,
            interactSCRP.enterNormalizedTime,
            interactSCRP.endNormalizedTime,
            interactSCRP.audioAnimationInteractTriggerEvents);

        this.gunFuAble_SubjectInteract.finishWarpEvent += this.Interact;
        this.got_GunFuAttacked_SubjectInteract.finishWarpEvent += this.Interact;

        this.animationTriggerEventPlayer.SubscribeEvent("Execute", this.Execute);
    }

    public override void Enter()
    {
        isExecuteAlready = false;
        gotGunFuAttackedAble = gunFuAble.attackedAbleGunFu;
        gotGunFuAttackedAble._character._movementCompoent.isOnUpdateEnable = false;
        curGunFuPhase = IGunFuExecuteNodeLeaf.GunFuExecutePhase.Warping;

        Vector3 executeDir = gotGunFuAttackedAble._character.transform.position - gunFuAble._character.transform.position;
        executeDir = new Vector3(executeDir.x, 0, executeDir.z).normalized;
        Vector3 executePos = gotGunFuAttackedAble._character.transform.position;

        this.gunFuAble_SubjectInteract.RestartSubject(gunFuAble._character, executePos, executeDir);
        this.got_GunFuAttacked_SubjectInteract.RestartSubject(gotGunFuAttackedAble._character, executePos, executeDir);
        this.animationTriggerEventPlayer.Rewind();
        this.audioTriggerEventPlayer.Rewind();

        this.gunFuAble._character._movementCompoent.CancleMomentum();
        this.gotGunFuAttackedAble._character._movementCompoent.CancleMomentum();

        base.Enter();
    }

    public override void Exit()
    {
        isExecuteAlready = false;
        this.gunFuAble._character.enableRootMotion = false;
        this.gotGunFuAttackedAble._character._movementCompoent.isOnUpdateEnable = true;
        base.Exit();
    }

    public override void UpdateNode()
    {
        this.gunFuAble_SubjectInteract.UpdateInteract(Time.deltaTime);
        this.got_GunFuAttacked_SubjectInteract.UpdateInteract(Time.deltaTime);
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);
        this.audioTriggerEventPlayer.Update(Time.deltaTime, this.gunFuAble._character.transform.position);
        base.UpdateNode();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return gunFuAble_SubjectInteract.animationTriggerEventPlayer.IsPlayFinish();
    }

    public override bool IsReset()
    {
        return IsComplete();
    }

    private void Interact(Character character)
    {
        if (character == gunFuAble._character)
        {
            _ = SubjectAnimationInteract.DelayRootMotion(character);
            curGunFuPhase = IGunFuExecuteNodeLeaf.GunFuExecutePhase.Interacting;
        }
        if (character == gotGunFuAttackedAble._character)
        {
            this.gotGunFuAttackedAble.TakeGunFuAttacked(this, gunFuAble, true);
        }
    }

    private void Execute()
    {
        Debug.Log("Execute");

        gotGunFuAttackedAble._damageAble.TakeDamage(this.executeMethod);
        isExecuteAlready = true;
        player.NotifyObserver(player, this);
    }

    public void OnNotifyFeedBackVisitor(IDamageAble damageAble)
    {
        this.player.OnNotifyFeedBackVisitor(damageAble);
    }
}
