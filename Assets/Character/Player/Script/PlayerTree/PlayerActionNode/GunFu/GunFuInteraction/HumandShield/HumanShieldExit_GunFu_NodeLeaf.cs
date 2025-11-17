using System;
using UnityEngine;

public class HumanShieldExit_GunFu_NodeLeaf : PlayerStateNodeLeaf,IGunFuNode
{
    public string _stateName => "HumanShieldExit";

    public IGunFuAble gunFuAble { get => player; set { } }
    public IGotGunFuAttackedAble gotGunFuAttackedAble { get; set; }
    protected AnimationInteractScriptableObject animationInteractScriptableObject { get; set; }
    public SubjectAnimationInteract subject_GunFuAble { get; protected set; }
    public SubjectAnimationInteract subject_GotGunFuAble { get; protected set; }
    

    public HumanShieldExit_GunFu_NodeLeaf(Player player,AnimationInteractScriptableObject animationInteractScriptableObject, Func<bool> preCondition) : base(player, preCondition)
    {
        this.animationInteractScriptableObject = animationInteractScriptableObject;
        this.subject_GunFuAble = new SubjectAnimationInteract(animationInteractScriptableObject, animationInteractScriptableObject.animationInteractCharacterDetail[0]);
        this.subject_GotGunFuAble = new SubjectAnimationInteract(animationInteractScriptableObject, animationInteractScriptableObject.animationInteractCharacterDetail[1]);
    }
    public override void Enter()
    {
        gotGunFuAttackedAble = gunFuAble.attackedAbleGunFu;

        this.subject_GunFuAble.RestartSubject(
            this.gunFuAble._character
            , this.gunFuAble._character.transform.position
            , this.gunFuAble._character.transform.forward);

        this.subject_GotGunFuAble.RestartSubject(
            this.gotGunFuAttackedAble._character
            , this.gunFuAble._character.transform.position
            , this.gunFuAble._character.transform.forward);

        gunFuAble._character.enableRootMotion = true;
        gotGunFuAttackedAble._character.enableRootMotion = true;

        base.Enter();
    }
    public override void UpdateNode()
    {
        this.subject_GunFuAble.UpdateInteract(Time.deltaTime);
        this.subject_GotGunFuAble.UpdateInteract(Time.deltaTime);
        base.UpdateNode();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
