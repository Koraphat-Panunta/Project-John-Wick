using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class HumanShield_GunFu_NodeLeaf : PlayerStateNodeLeaf
    ,I_OCM_Node
    ,INodeLeafTransitionAble
{
    IRangeWeaponAdvanceUser weaponAdvanceUser => player;

    public string _stateName => OCM_ManaverStateName.HumanShield.ToString() ;

    public I_OCM_Attack_Able gunFuAble { get => this.player; set { } }
    public I_Got_OCM_Attacked_Able gotGunFuAttackedAble { get; set; }

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

    private Vector3 gotHumanShieldPosition => this.player._movementCompoent.curPosition
        + (this.player.transform.forward * transformOffsetSCRP.postitionOffset.z)
        + (this.player.transform.right * transformOffsetSCRP.postitionOffset.x)
        + (this.player.transform.up * transformOffsetSCRP.postitionOffset.y);

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

        //NONE-SOLID-Implement//
        this._cameraYawRotator = new CameraYawRotator(player.cinemachineCamera, 90f * 1.5f);
        //NONE-SOLID-Implement//
    }

    public override void Enter()
    {

        this.humanShield_Stay_Timer = 0;
        this.nodeLeafTransitionBehavior.DisableTransitionAbleAll(this);
        curIntphase = HumanShieldInteractionPhase.Enter;
        this.gotGunFuAttackedAble = player.attackedAbleGunFu;

        Vector3 anchorDir = (this.gotGunFuAttackedAble._character.transform.position - this.gunFuAble._character.transform.position);
        anchorDir = new Vector3(anchorDir.x, 0, anchorDir.z).normalized;

        this.subject_GunFuAble.RestartSubject(
            player
            , this.gotGunFuAttackedAble._character.transform.position
            , anchorDir);
        this.subject_GotGunFuAble.RestartSubject(
            this.gotGunFuAttackedAble._character
            , this.gotGunFuAttackedAble._character.transform.position
            , anchorDir);

        this.pullWeight = 0;

        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        player._movementCompoent.UpdateMovement();
        base.FixedUpdateNode();
    }

    public override bool IsReset()
    {
        if(IsComplete())
            return true;

        if(player.isDead)
            return true;

        if(player._triggerEnterGotAttacked_OCM)
            return true;

        return false;
    }

    public override void UpdateNode()
    {
        switch (curIntphase)
        {
            case HumanShieldInteractionPhase.Enter:
                {
                    //NONE-SOLID-Implement//
                    this._cameraYawRotator.RotateTowards(gotGunFuAttackedAble._character.transform.forward * -1);
                    //NONE-SOLID-Implement//
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
                    pullWeight = Mathf.Clamp01(this.pullWeight + Time.deltaTime * 2);

                    this.gotGunFuAttackedAble._character._movementCompoent.SetPosition(Vector3.Lerp
                        (
                        this.gotGunFuAttackedAble._character.transform.position
                        , this.gotHumanShieldPosition
                        , this.pullWeight)
                        );

                    gotGunFuAttackedAble._character._movementCompoent.SetRotation(Quaternion.Lerp
                        (
                        this.gotGunFuAttackedAble._character.transform.rotation
                        , this.gotHumanShieldRotation
                        , this.pullWeight
                        )
                        );

                    this.humanShield_Stay_Timer += Time.deltaTime;



                    player._movementCompoent.UpdateMoveToDirLocal(
                        player.inputMoveDir_Local * player.StandMoveMaxSpeed
                        , player.StandMoveAccelerate
                        , MoveMode.MaintainMomentumDirection
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
        this.gotGunFuAttackedAble.TakeGunFuAttacked(this, this.gunFuAble, true);
    }
    public bool TransitioningCheck() => nodeLeafTransitionBehavior.TransitioningCheck(this);
   
    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);

    public void OnNotifyFeedBackVisitor(IDamageAble damageAble)
    {
        this.player.OnNotifyFeedBackVisitor(damageAble);
    }

    //NONE-SOLID-Implement//
    private CameraYawRotator _cameraYawRotator;
    //NONE-SOLID-Implement//
}
