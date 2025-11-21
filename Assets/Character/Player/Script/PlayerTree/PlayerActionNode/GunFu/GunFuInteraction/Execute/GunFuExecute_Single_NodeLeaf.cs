using System;
using UnityEngine;

public class GunFuExecute_Single_NodeLeaf : PlayerStateNodeLeaf, IGunFuExecuteNodeLeaf
{
    public IWeaponAdvanceUser weaponAdvanceUser;
    public IGunFuAble gunFuAble { get; set; }
    public IGotGunFuAttackedAble gotGunFuAttackedAble { get; set; }

    public AnimationInteractScriptableObject _gunFuExecuteInteractSCRP { get; }
    protected AnimationInteractScriptableObject gunFuExecuteInteractSCRP;

    IGunFuExecuteNodeLeaf.GunFuExecutePhase IGunFuExecuteNodeLeaf._curGunFuPhase { get => this.curGunFuPhase; set => this.curGunFuPhase = value; }
    private IGunFuExecuteNodeLeaf.GunFuExecutePhase curGunFuPhase;

    bool IGunFuExecuteNodeLeaf._isExecuteAldready { get => this.isExecuteAlready ; set => this.isExecuteAlready = value; }
    private bool isExecuteAlready;

    GunFuExecuteStateName IGunFuExecuteNodeLeaf._executeStateName { get => executeStateName; set => executeStateName = value; }
    private GunFuExecuteStateName executeStateName;

    string IGunFuNode._stateName => executeStateName.ToString();

    protected SubjectAnimationInteract gunFuAble_SubjectInteract;
    protected SubjectAnimationInteract got_GunFuAttacked_SubjectInteract;

    public GunFuExecute_Single_NodeLeaf(
        Player player
        , Func<bool> preCondition
        , AnimationInteractScriptableObject gunFuExecuteInteractSCRP
        ,GunFuExecuteStateName stateName
        ) : base(player, preCondition)
    {
        this.gunFuAble = player;
        this.gunFuExecuteInteractSCRP = gunFuExecuteInteractSCRP;
        this.weaponAdvanceUser = player;
        this.executeStateName = stateName;

        this.gunFuAble_SubjectInteract = new SubjectAnimationInteract(this.gunFuExecuteInteractSCRP, this.gunFuExecuteInteractSCRP.animationInteractCharacterDetail[0]);
        this.got_GunFuAttacked_SubjectInteract = new SubjectAnimationInteract(this.gunFuExecuteInteractSCRP, this.gunFuExecuteInteractSCRP.animationInteractCharacterDetail[1]);

        this.gunFuAble_SubjectInteract.finishWarpEvent += this.Interact;
        this.gunFuAble_SubjectInteract.animationTriggerEventPlayer.SubscribeEvent("Shoot",this.Shoot);
        this.gunFuAble_SubjectInteract.animationTriggerEventPlayer.SubscribeEvent("Execute",this.Execute);
    }

    public override void Enter()
    {
        gotGunFuAttackedAble = gunFuAble.executedAbleGunFu;
        curGunFuPhase = IGunFuExecuteNodeLeaf.GunFuExecutePhase.Warping;
        gunFuAble._character._movementCompoent.CancleMomentum();
        gotGunFuAttackedAble._character._movementCompoent.CancleMomentum();

        Vector3 executeDir = gotGunFuAttackedAble._character.transform.position - gunFuAble._character.transform.position;
        executeDir = new Vector3(executeDir.x,0,executeDir.z).normalized;

        this.gunFuAble_SubjectInteract.RestartSubject(
            gunFuAble._character
            ,gotGunFuAttackedAble._character.transform.position
            ,executeDir);
        this.got_GunFuAttacked_SubjectInteract.RestartSubject(
            gotGunFuAttackedAble._character
            ,gotGunFuAttackedAble._character.transform.position
            ,executeDir);

        base.Enter();
    }

    public override void Exit()
    {
        isExecuteAlready = false;
        gunFuAble._character.enableRootMotion = false;

        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }
    public override bool IsComplete()
    {
       if(gunFuAble_SubjectInteract.animationTriggerEventPlayer.IsPlayFinish())
            return true;
        return false;
    }

    public override bool IsReset()
    {
        if(this.IsComplete())
            return true;
        return false;
    }

    public override void UpdateNode()
    {
        this.gunFuAble_SubjectInteract.UpdateInteract(Time.deltaTime);
        this.got_GunFuAttacked_SubjectInteract.UpdateInteract(Time.deltaTime);

        base.UpdateNode();
    }
    
    private void Interact(Character character)
    {
        _ = SubjectAnimationInteract.DelayRootMotionEnable(gunFuAble._character);
        curGunFuPhase = IGunFuExecuteNodeLeaf.GunFuExecutePhase.Interacting;
        this.gotGunFuAttackedAble.TakeGunFuAttacked(this, gunFuAble);
    }
    private void Shoot()
    {
        WeaponShootBlank.ShootBlank(weaponAdvanceUser._currentWeapon);
    }

    private void Execute()
    {
        BulletExecute bulletExecute = new BulletExecute(weaponAdvanceUser._currentWeapon);
        gotGunFuAttackedAble._damageAble.TakeDamage(bulletExecute);
    }
   
}
