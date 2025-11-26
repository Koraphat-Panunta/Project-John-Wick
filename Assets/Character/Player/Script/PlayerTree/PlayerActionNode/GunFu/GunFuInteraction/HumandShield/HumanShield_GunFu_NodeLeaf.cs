using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class HumanShield_GunFu_NodeLeaf : PlayerStateNodeLeaf,IGunFuNode,INodeLeafTransitionAble
{
    IWeaponAdvanceUser weaponAdvanceUser => player.weaponAdvanceUser;

    public string _stateName => GunFuManaverStateName.HumanShield.ToString() ;

    public IGunFuAble gunFuAble { get => this.player; set { } }
    public IGotGunFuAttackedAble gotGunFuAttackedAble { get; set; }

    public enum HumanShieldInteractionPhase
    {
        Enter,
        Stay,
        Exit,
    }
    public HumanShieldInteractionPhase curIntphase;
    public AnimationInteractScriptableObject animationInteractScriptableObject { get; protected set; }
    public TransformOffsetSCRP transformOffsetSCRP { get; protected set; }
    public SubjectAnimationInteract subject_GunFuAble { get; protected set; }
    public SubjectAnimationInteract subject_GotGunFuAble { get; protected set; }



    public INodeManager nodeManager { get => this.player.playerStateNodeManager; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    private Vector3 gotHumanShieldPosition => player.transform.position 
        + (player.transform.forward * transformOffsetSCRP.postitionOffset.z)
        + (player.transform.right * transformOffsetSCRP.postitionOffset.x)
        + (player.transform.up * transformOffsetSCRP.postitionOffset.y);

    private Quaternion gotHumanShieldRotation => player.transform.rotation * Quaternion.Euler(transformOffsetSCRP.rotationEulerOffset);

    private float pullWeight;

    public float humanShield_Stay_Timer { get; protected set; }
    public float humanShield_Stay_Duration { get; protected set; }
    
    public HumanShield_GunFu_NodeLeaf(Player player, Func<bool> preCondition,AnimationInteractScriptableObject animationInteractScriptableObject,TransformOffsetSCRP transformOffsetSCRP) : base(player, preCondition)
    {
        this.humanShield_Stay_Duration = 5;
        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
        this.animationInteractScriptableObject = animationInteractScriptableObject;
        this.transformOffsetSCRP = transformOffsetSCRP;
        subject_GunFuAble = new SubjectAnimationInteract(animationInteractScriptableObject, animationInteractScriptableObject.animationInteractCharacterDetail[0]);
        subject_GotGunFuAble = new SubjectAnimationInteract(animationInteractScriptableObject, animationInteractScriptableObject.animationInteractCharacterDetail[1]);

        this.subject_GunFuAble.finishWarpEvent += Interact;
    }

    public override void Enter()
    {
        this.humanShield_Stay_Timer = 0;
        this.nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        curIntphase = HumanShieldInteractionPhase.Enter;
        this.gotGunFuAttackedAble = player.attackedAbleGunFu;

        this.subject_GunFuAble.RestartSubject(
            player
            , this.gotGunFuAttackedAble._character.transform.position
            , this.gotGunFuAttackedAble._character.transform.forward);
        this.subject_GotGunFuAble.RestartSubject(
            this.gotGunFuAttackedAble._character
            , this.gotGunFuAttackedAble._character.transform.position
            , this.gotGunFuAttackedAble._character.transform.forward);

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
        if(IsComplete())
            return true;

        if(player.isDead)
            return true;

        if(player._triggerHitedGunFu)
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        switch (curIntphase)
        {
            case HumanShieldInteractionPhase.Enter:
                {
                    this.subject_GunFuAble.UpdateInteract(Time.deltaTime);
                    this.subject_GotGunFuAble.UpdateInteract(Time.deltaTime);

                    if (this.subject_GunFuAble.animationTriggerEventPlayer.IsPlayFinish())
                    {
                        player.enableRootMotion = false;
                        curIntphase = HumanShieldInteractionPhase.Stay;
                        player.NotifyObserver(this.player,this);
                    }
                }
                break;

            case HumanShieldInteractionPhase.Stay:
                {
                    pullWeight = 1;

                    this.gotGunFuAttackedAble._character.transform.position = Vector3.Lerp(
                        this.gotGunFuAttackedAble._character.transform.position
                        , this.gotHumanShieldPosition
                        , this.pullWeight);

                    gotGunFuAttackedAble._character.transform.rotation = Quaternion.Lerp(
                        this.gotGunFuAttackedAble._character.transform.rotation
                        , this.gotHumanShieldRotation
                        , this.pullWeight);

                    this.humanShield_Stay_Timer += Time.deltaTime;

                    player._movementCompoent.MoveToDirLocal(
                        player.inputMoveDir_Local
                        , player.StandMoveAccelerate
                        , player.StandMoveMaxSpeed
                        , MoveMode.MaintainMomentum
                        );

                    if (weaponAdvanceUser._isAimingCommand == false
                        ||(this.humanShield_Stay_Timer >= this.humanShield_Stay_Duration))
                    {
                        curIntphase = HumanShieldInteractionPhase.Exit;
                        player.NotifyObserver(this.player, this);
                    }
                       
                }
                break;
            case HumanShieldInteractionPhase.Exit: 
                {
                    nodeLeafTransitionBehavior.TransitionAbleAll(this);
                    nodeLeafTransitionBehavior.TransitioningCheck(this);
                }
                break;
            
        }
        base.UpdateNode();
    }
    private void Interact(Character character)
    {
        this.gunFuAble._character.enableRootMotion = true;
        this.gotGunFuAttackedAble.TakeGunFuAttacked(this, this.gunFuAble);
    }
    public bool TransitioningCheck() => nodeLeafTransitionBehavior.TransitioningCheck(this);
   
    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);
    
}
