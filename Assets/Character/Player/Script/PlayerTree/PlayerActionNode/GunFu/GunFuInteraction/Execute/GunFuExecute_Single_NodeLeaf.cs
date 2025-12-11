using System;
using System.Threading.Tasks;
using UnityEngine;

public class GunFuExecute_Single_NodeLeaf : PlayerStateNodeLeaf, IGunFuExecuteNodeLeaf
{
    public IWeaponAdvanceUser weaponAdvanceUser;
    public IGunFuAble gunFuAble { get; set; }
    public IGotGunFuAttackedAble gotGunFuAttackedAble { get; set; }

    public AnimationInteractScriptableObject _gunFuExecuteInteractSCRP { get => this.gunFuExecuteInteractSCRP; }
    protected AnimationInteractScriptableObject gunFuExecuteInteractSCRP;

    IGunFuExecuteNodeLeaf.GunFuExecutePhase IGunFuExecuteNodeLeaf._curGunFuPhase { get => this.curGunFuPhase; set => this.curGunFuPhase = value; }
    private IGunFuExecuteNodeLeaf.GunFuExecutePhase curGunFuPhase;

    bool IGunFuExecuteNodeLeaf._isExecuteAldready { get => this.isExecuteAlready ; set => this.isExecuteAlready = value; }
    private bool isExecuteAlready;

    GunFuExecuteStateName IGunFuExecuteNodeLeaf._executeStateName { get => executeStateName; set => executeStateName = value; }
    private GunFuExecuteStateName executeStateName;

    string IGunFuNode._stateName => executeStateName.ToString();

    public SubjectAnimationInteract gunFuAble_SubjectInteract;
    public SubjectAnimationInteract got_GunFuAttacked_SubjectInteract;
    public AnimationTriggerEventPlayer animationTriggerEventPlayer;

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
        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(
            this.gunFuExecuteInteractSCRP.clip
            , this.gunFuExecuteInteractSCRP.enterNormalizedTime
            , this.gunFuExecuteInteractSCRP.endNormalizedTime
            , this.gunFuExecuteInteractSCRP.triggerEventDetail);

        this.gunFuAble_SubjectInteract.finishWarpEvent += this.Interact;
        this.animationTriggerEventPlayer.SubscribeEvent("Shoot", this.Shoot);
        this.animationTriggerEventPlayer.SubscribeEvent("Execute", this.Execute);

        this.got_GunFuAttacked_SubjectInteract.finishWarpEvent += this.Interact;
    }

    public override void Enter()
    {
        isExecuteAlready = false;
        gotGunFuAttackedAble = gunFuAble.executedAbleGunFu;
        gotGunFuAttackedAble._character._movementCompoent.isOnUpdateEnable = false;
        curGunFuPhase = IGunFuExecuteNodeLeaf.GunFuExecutePhase.Warping;
        Vector3 executeDir = gunFuAble._character.transform.position - gotGunFuAttackedAble._character.transform.position ;
        executeDir = new Vector3(executeDir.x,0,executeDir.z).normalized;
        Vector3 executePos = gotGunFuAttackedAble._character.transform.position;

        this.gunFuAble_SubjectInteract.RestartSubject(
            gunFuAble._character
            , executePos
            , executeDir);
        this.got_GunFuAttacked_SubjectInteract.RestartSubject(
            gotGunFuAttackedAble._character
            , executePos
            , executeDir);
        this.animationTriggerEventPlayer.Rewind();

        this.gunFuAble._character._movementCompoent.CancleMomentum();
        this.gotGunFuAttackedAble._character._movementCompoent.CancleMomentum();

        base.Enter();
    }

    public override void Exit()
    {
        isExecuteAlready = false;
        gunFuAble._character.enableRootMotion = false;
        gotGunFuAttackedAble._character._movementCompoent.isOnUpdateEnable = true;

        

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
        //Debug.Log("this.gunFuAble._character._movementCompoent.V_World = " + this.gunFuAble._character._movementCompoent.curMoveVelocity_World);
        this.UpdateSubject();
        this.animationTriggerEventPlayer.UpdatePlay(Time.deltaTime);

        base.UpdateNode();
    }
    protected virtual void UpdateSubject()
    {
        this.gunFuAble_SubjectInteract.UpdateInteract(Time.deltaTime);
        this.got_GunFuAttacked_SubjectInteract.UpdateInteract(Time.deltaTime);
    }
    //private void BeginWarp(Character character)
    //{
    //    if(character == gunFuAble._character)
    //    {
    //        Debug.DrawRay(this.gunFuAble_SubjectInteract.exitPosition,this.gun)
    //    }
    //    if(character == gotGunFuAttackedAble._character)
    //    {

    //    }
    //}
    private void Interact(Character character)
    {
        if(character == gunFuAble._character)
        {
            _ = SubjectAnimationInteract.DelayRootMotion(character);
            curGunFuPhase = IGunFuExecuteNodeLeaf.GunFuExecutePhase.Interacting;

        }
         if(character == gotGunFuAttackedAble._character)
        {
            Debug.Log("gotGunFuAttackedAble " + gotGunFuAttackedAble + " TakeGunFuAttacked ");
            this.gotGunFuAttackedAble.TakeGunFuAttacked(this, gunFuAble);
            //Debug.Log("Player anchor Distance pos = "
            //    + Vector3.Distance(
            //        gunFuAble._character.transform.position
            //        , gunFuAble_SubjectInteract.anhorPosition)
            //    );
            //Debug.Log("Player anchor Distance rot = "
            //    + Quaternion.Angle(
            //        gunFuAble._character.transform.rotation
            //        , Quaternion.LookRotation(gunFuAble_SubjectInteract.anhorDir))
            //    );

            //Debug.Log("Enemy anchor Distance pos = "
            //    + Vector3.Distance(
            //        gotGunFuAttackedAble._character.transform.position
            //        , got_GunFuAttacked_SubjectInteract.anhorPosition)
            //    );
            //Debug.Log("Enemy anchor Distance rot = "
            //    + Quaternion.Angle(
            //        gotGunFuAttackedAble._character.transform.rotation
            //        , Quaternion.LookRotation(got_GunFuAttacked_SubjectInteract.anhorDir))
            //    );
        }
    }
    private void Shoot()
    {
        WeaponShootBlank.ShootBlank(weaponAdvanceUser._currentWeapon);
    }

    private void Execute()
    {
        BulletExecute bulletExecute = new BulletExecute(weaponAdvanceUser._currentWeapon);
        gotGunFuAttackedAble._damageAble.TakeDamage(bulletExecute);

        //Debug.Log("Character : " + gunFuAble_SubjectInteract.character + " execute anchor Distance pos = "
        //+ Vector3.Distance(
        //           gunFuAble_SubjectInteract.character.transform.position
        //          , gunFuAble_SubjectInteract.anhorPosition)
        //      );
        //Debug.Log("Character : " + gunFuAble_SubjectInteract.character + " execute anchor Distance rot = "
        //+ Quaternion.Angle(
        //     gunFuAble_SubjectInteract.character.transform.rotation
        //    , Quaternion.LookRotation(gunFuAble_SubjectInteract.anhorDir))
        //);

        //Debug.Log("Character : " + got_GunFuAttacked_SubjectInteract.character + " execute anchor Distance pos = "
        //+ Vector3.Distance(
        //           got_GunFuAttacked_SubjectInteract.character.transform.position
        //          , got_GunFuAttacked_SubjectInteract.anhorPosition)
        //      );
        //Debug.Log("Character : " + got_GunFuAttacked_SubjectInteract.character + " execute anchor Distance rot = "
        //+ Quaternion.Angle(
        //     got_GunFuAttacked_SubjectInteract.character.transform.rotation
        //    , Quaternion.LookRotation(got_GunFuAttacked_SubjectInteract.anhorDir))
        //);

        isExecuteAlready = true;

        player.NotifyObserver(player, this);
    }

    
}
