
using UnityEngine;

public partial class PlayerAnimationManager 
{

    #region BaseLayer
    public NodeManagerPortable playerBaseLayerAnimationNodeManagerPortable;
    public NodeSelector basedLayerNodeSelector { get; set; }
    public PlayAnimationNodeLeaf deadNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf throwObjectNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf parryPrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf parrySecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf quickshot_Sec_I_NodeLeaf { get; set; }
    public PlayAnimationNodeLeaf pokePickUpNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf getUpNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf boundOffNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf flinchNodeLeaf { get; set; }
    public NodeSelector parkourNodeSelector { get; set; }
    public PlayAnimationNodeLeaf vaultingNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf climbLowNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf climbHighNodeLeaf { get; set; }
    public NodeSelector gunFuBaseLayerNodeSelector { get; set; }


    public GunFuExecuteAnimationNodeLeaf executeAnimationNodeLeaf { get; set; }

    public NodeSelector restrictShieldSelector { get; set; }
    public PlayAnimationNodeLeaf restrictShieldEnterNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf restrictShieldExitNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf restrictShieldMoveNodeLeaf { get; set; }

    public PlayAnimationNodeLeaf knockDownNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf gunFuReloadNodeLeaf { get; set; }

    public NodeSelector humanShieldSelector { get; set; }
    public PlayAnimationNodeLeaf humanShieldEnterNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf humanShieldExitNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf humanShieldMoveNodeLeaf { get; set; }

    public PlayAnimationNodeLeaf hitDownNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf hit1NodeLeaf { get; set; }
    public PlayAnimationNodeLeaf hit2NodeLeaf { get; set; }
    public PlayAnimationNodeLeaf hit3NodeLeaf { get; set; }
    public PlayAnimationNodeLeaf spinKickNodeLeaf { get; set; }

    public PlayAnimationNodeLeaf fallingNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf landingRollNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf landingStandNodeLeaf { get; set; }

    public PlayAnimationNodeLeaf dodgeNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf slideNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf sprintNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf sprintChangeDirNodeLeaf { get; set; }

    public NodeSelector proneStateNodeSelector { get; set; }
    public PlayAnimationNodeLeaf obstacleJumpAnimationNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf wallJumpReversDolphinDiveAnimationNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf wallJumpForwardLeftDolphinDiveAnimationNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf wallJumpForwardRightDolphinDiveAnimationNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf dolphinDiveAnimationNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf diveStallAnimationNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf proneAnimationNodeLeaf { get; set; }

    public PlayAnimationNodeLeaf moveCrouchNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf moveStandNodeLeaf { get; set; }
    private void InitializedBasedLayer()
    {
        basedLayerNodeSelector = new NodeSelector(() => true);
        deadNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is PlayerDeadNodeLeaf, animator, "Dead", 0, 0.14f);
        throwObjectNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<PlayerThrowWeaponNodeLeaf>(), animator, "Throwing", 0, .05f,player.throwObjectAnimationTriggerEventSCRP.enterNormalizedTime);
        parryPrimaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<ParryNodeLeaf>()
                  && player._currentWeapon is PrimaryWeapon,
            animator, "OCM_Parry_Pri_I", 0, .025f);
        parrySecondaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<ParryNodeLeaf>()
                  && player._currentWeapon is SecondaryWeapon,
            animator, "OCM_Parry_Sec_I", 0, .025f);
        quickshot_Sec_I_NodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<QuickShootRangeWeaponNodeLeaf>()
            , animator
            , "Quickshot_Sec_I"
            , 0
            , .025f);
        pokePickUpNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<PlayerPokePickUpWeaponNodeLeaf>()
            , animator
            , "PokePickUp"
            , 0, .07f
            , (playerStateNodeMnager as PlayerStateNodeManager).playerPokePickUpWeaponNodeLeaf.animationTriggerEventSCRP.enterNormalizedTime);
 
        boundOffNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is PlayerBrounceOffNodeLeaf
            , animator, "PlayerBounceOff", 0, .05f);
        flinchNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<PlayerAnimationBaseState>(out PlayerAnimationBaseState n)
                  && n == (playerStateNodeMnager as PlayerStateNodeManager).playerFlinchNodeLeaf
            , animator, "Flinch_State", 0, .15f);
        parkourNodeSelector = new NodeSelector(() => playerStateNodeMnager.GetCurNodeLeaf() is IParkourNodeLeaf);
        vaultingNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is VaultingNodeLeaf vaultingNodeLeaf
            && vaultingNodeLeaf.nameState == "Vaulting"
            , animator, "Vaulting", 0, .2f);
        climbHighNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is ClimbParkourNodeLeaf climbHighNodeLeaf
            && climbHighNodeLeaf.nameState == "ClimbHigh"
            , animator, "ClimbHigh", 0, .2f);
        climbLowNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is ClimbParkourNodeLeaf climbLowNodeLeaf
            && climbLowNodeLeaf.nameState == "ClimbLow"
            , animator, "ClimbLow", 0, .2f);

        InitializedGunFuBasedLayer();

        this.fallingNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.playerStateNodeMnager.TryGetCurNodeLeaf<PlayerFallingStateNodeLeaf>()
            ,this.animator
            , "Falling"
            ,0
            ,.2f
            );
        this.landingRollNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.playerStateNodeMnager.TryGetCurNodeLeaf<PlayerLandingRollStateNodeLeaf>()
            , this.animator
            , "LandingRoll"
            , 0
            , .1f
            );
        this.landingStandNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.playerStateNodeMnager.TryGetCurNodeLeaf<PlayerLandingStandStateNodeLeaf>()
            , this.animator
            , "LandingStand"
            , 0
            , .1f
            );

        dodgeNodeLeaf = new PlayAnimationNodeLeaf(
            () => 
            {
                if (playerStateNodeMnager.GetCurNodeLeaf() is PlayerDodgeRollStateNodeLeaf)
                    return true;
                return false;
            }
            ,
            animator, "DodgeRoll", 0, .2f, 0.1f);
        sprintNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is PlayerSprintNode,
            animator, "Sprint", 0, .4f);

        this.slideNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<PlayerSlideNodeLeaf>(),
            animator, "Slide", 0, .35f);

        sprintChangeDirNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is PlayerSprintChangeDirectionNode,
            animator, "SprintChangeDir", 0, .1f);

        this.proneStateNodeSelector = new NodeSelector(
            ()=> this.player.stance == Stance.prone);

        this.obstacleJumpAnimationNodeLeaf = new PlayPoseAnimationNodeLeaf
            (() => this.playerStateNodeMnager.TryGetCurNodeLeaf<ObstacleJumpDolphinDiveNodeLeaf>
            (out ObstacleJumpDolphinDiveNodeLeaf playerDolphinDiveStateNodeLeaf) && playerDolphinDiveStateNodeLeaf.timer
            < this.player.playerStateNodeManager.obstacleJumpDolphinDiveNodeLeaf.anticipateTime + .2f
            , this.animator, "ObstacleJump", 0, .2f, 
            this.basedAnimationPoseTimeNormalzied,1,false
            );

        this.wallJumpReversDolphinDiveAnimationNodeLeaf = new PlayPoseAnimationNodeLeaf
            (() => this.playerStateNodeMnager.TryGetCurNodeLeaf<WallJumpReversDolphinDiveNodeLeaf>
            (out WallJumpReversDolphinDiveNodeLeaf playerDolphinDiveStateNodeLeaf) && playerDolphinDiveStateNodeLeaf.timer 
            < this.player.playerStateNodeManager.wallJumpReversDolphinDiveNodeLeaf.anticipateTime + .2f
            , this.animator, "WallJumpRevers", 0, .25f,this.basedAnimationPoseTimeNormalzied, 
            this.player.playerStateNodeManager.wallJumpReversDolphinDiveNodeLeaf.anticipateTime + .2f
            ,false);

        this.wallJumpForwardLeftDolphinDiveAnimationNodeLeaf = new PlayPoseAnimationNodeLeaf
            (() => this.playerStateNodeMnager.TryGetCurNodeLeaf<WallJumpForwardDolphinDiveNodeLeaf>
            (out WallJumpForwardDolphinDiveNodeLeaf playerDolphinDiveStateNodeLeaf) && playerDolphinDiveStateNodeLeaf.timer
            < this.player.playerStateNodeManager.wallJumpForwardDolphinDiveNodeLeaf.jumpOutTime && playerDolphinDiveStateNodeLeaf.isJumpLeft
            , this.animator, "WallForwardLeft", 0, .25f, this.basedAnimationPoseTimeNormalzied,
            this.player.playerStateNodeManager.wallJumpForwardDolphinDiveNodeLeaf.jumpOutTime
            , false);

        this.wallJumpForwardRightDolphinDiveAnimationNodeLeaf = new PlayPoseAnimationNodeLeaf
            (() => this.playerStateNodeMnager.TryGetCurNodeLeaf<WallJumpForwardDolphinDiveNodeLeaf>
            (out WallJumpForwardDolphinDiveNodeLeaf playerDolphinDiveStateNodeLeaf) && playerDolphinDiveStateNodeLeaf.timer
            < this.player.playerStateNodeManager.wallJumpForwardDolphinDiveNodeLeaf.jumpOutTime
            , this.animator, "WallForwardRight", 0, .25f, this.basedAnimationPoseTimeNormalzied,
            this.player.playerStateNodeManager.wallJumpForwardDolphinDiveNodeLeaf.jumpOutTime
            , false);

        this.dolphinDiveAnimationNodeLeaf = new PlayAnimationNodeLeaf
            (() => this.playerStateNodeMnager.TryGetCurNodeLeaf<PlayerDolphinDiveStateNodeLeaf>
            (out PlayerDolphinDiveStateNodeLeaf playerDolphinDiveStateNodeLeaf) && playerDolphinDiveStateNodeLeaf.isPassingJump == false
            , this.animator, "Dolphin Dive", 0, .1f);
        this.diveStallAnimationNodeLeaf = new PlayAnimationNodeLeaf(() => this.playerStateNodeMnager.TryGetCurNodeLeaf<PlayerDolphinDiveStateNodeLeaf>
            (out PlayerDolphinDiveStateNodeLeaf playerDolphinDiveStateNodeLeaf) && playerDolphinDiveStateNodeLeaf.isPassingJump 
            , this.animator, "DiveStall", 0, .2f);
        this.proneAnimationNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.playerStateNodeMnager.TryGetCurNodeLeaf<PlayerProneStateNodeLeaf>()
            , this.animator, "Prone", 0, .25f );

        this.getUpNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is PlayerGetUpStateNodeLeaf
            , this.animator, "KickUp", 0,.1f);

        moveCrouchNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is PlayerCrouch_Idle_NodeLeaf || playerStateNodeMnager.GetCurNodeLeaf() is PlayerCrouch_Move_NodeLeaf,
            animator, "Crouch", 0, .2f);
        moveStandNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is PlayerStandIdleNodeLeaf || playerStateNodeMnager.GetCurNodeLeaf() is PlayerStandMoveNodeLeaf,
            animator, "Move/Idle", 0, .2f);
    }
    private void InitializedGunFuBasedLayer()
    {
        gunFuBaseLayerNodeSelector = new NodeSelector(
            () => 
            {
                if (this.isPerformGunFu)
                {
                    return true;
                }
                return false;
            });

      

        executeAnimationNodeLeaf = new GunFuExecuteAnimationNodeLeaf(() => executeAnimationNodeLeaf.gunFuExecuteNodeLeaf != null, player, animator);

        restrictShieldSelector = new NodeSelector(() => playerStateNodeMnager.TryGetCurNodeLeaf<RestrainGunFuStateNodeLeaf>());
        restrictShieldEnterNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf restrictNodeLeaf
        && (restrictNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Enter)
        , animator,OCM_ManaverStateName.Restrain.ToString(), 0, 0.35f);
        restrictShieldExitNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf restrictNodeLeaf
        && (restrictNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Exit
        )
        , animator, OCM_ManaverStateName.RestrainExit.ToString(), 0, .35f);
        restrictShieldMoveNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf restrictNodeLeaf
        && (restrictNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Stay)
        , animator, "Move/Idle", 0, .35f);

        this.knockDownNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.playerStateNodeMnager.TryGetCurNodeLeaf<OCM_KnockDown_NodeLeaf>()
            , this.animator
            , "KnockDown"
            , 0
            , 0
            );

        this.gunFuReloadNodeLeaf = new PlayAnimationNodeLeaf
            (
            ()=> this.playerStateNodeMnager.TryGetCurNodeLeaf<OCMReloadNodeLeaf>()
            ,this.animator
            ,"GunFuReload"
            ,0
            ,0
            );

        humanShieldSelector = new NodeSelector(
            () => 
            {
                if(playerStateNodeMnager.TryGetCurNodeLeaf<HumanShield_GunFu_NodeLeaf>()
                || playerStateNodeMnager.TryGetCurNodeLeaf<HumanShieldExit_GunFu_NodeLeaf>())
                    return true;
            return false;
            });
        humanShieldEnterNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf humanShieldNodeLeaf
        && (humanShieldNodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Enter)
        , animator
        , OCM_ManaverStateName.HumanShield.ToString()
        , 0
        ,AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration
        , player.humanShieldSCRP.animationInteractCharacterDetail[0].enterAnimationOffsetNormalizedTime);
        humanShieldExitNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is HumanShieldExit_GunFu_NodeLeaf humanShieldExitNodeLeaf
        , animator
        , OCM_ManaverStateName.HumanShieldExit.ToString()
        , 0
        , AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration
        , player.humanShieldSCRP.animationInteractCharacterDetail[0].enterAnimationOffsetNormalizedTime);
        humanShieldMoveNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf humanShieldNodeLeaf
        , animator, "Move/Idle", 0, .35f);

        this.hitDownNodeLeaf = new PlayAnimationNodeLeaf(
            ()=> this.playerStateNodeMnager.TryGetCurNodeLeaf<OCM_HitDownNodeLeaf>()
            ,this.animator, "HitDown", 0
            , AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration
            );
        hit1NodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is OCM_Hit_NodeLeaf gunFuHitNodeLeaf
            && gunFuHitNodeLeaf._stateName == "Hit1",
            animator, "Hit1", 0, .1f, this.player.hit1.enterNormalizedTime);
        hit2NodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is OCM_Hit_NodeLeaf gunFuHitNodeLeaf
            && gunFuHitNodeLeaf._stateName == "Hit2",
            animator, "Hit2", 0, .1f, player.hit2.enterNormalizedTime);
        hit3NodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is OCM_Hit_NodeLeaf gunFuHitNodeLeaf
            && gunFuHitNodeLeaf._stateName == "Hit3",
            animator, "Hit3", 0, .1f, player.hit3.enterNormalizedTime);
        spinKickNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is OCM_Hit_NodeLeaf gunFuHitNodeLeaf
            && gunFuHitNodeLeaf._stateName == "DodgeSpinKick",
            animator, "DodgeSpinKick", 0, .25f, player.dodgeSpinKick.enterNormalizedTime
            );
    }
    private void InitializedBasedLayerNodeManager()
    {
        playerBaseLayerAnimationNodeManagerPortable = new NodeManagerPortable();
        playerBaseLayerAnimationNodeManagerPortable.InitialzedOuterNode(
            () =>
            {
                this.InitializedBasedLayer();

                basedLayerNodeSelector.AddtoChildNode(deadNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(throwObjectNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(parryPrimaryNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(parrySecondaryNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(quickshot_Sec_I_NodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(pokePickUpNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(boundOffNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(flinchNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(parkourNodeSelector);
                basedLayerNodeSelector.AddtoChildNode(gunFuBaseLayerNodeSelector);
                basedLayerNodeSelector.AddtoChildNode(this.fallingNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(this.landingRollNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(this.landingStandNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(dodgeNodeLeaf);
                this.basedLayerNodeSelector.AddtoChildNode(this.getUpNodeLeaf);
                this.basedLayerNodeSelector.AddtoChildNode(this.proneStateNodeSelector);
                basedLayerNodeSelector.AddtoChildNode(slideNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(sprintChangeDirNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(sprintNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(moveCrouchNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(moveStandNodeLeaf);

                this.proneStateNodeSelector.AddtoChildNode(this.obstacleJumpAnimationNodeLeaf);
                this.proneStateNodeSelector.AddtoChildNode(this.wallJumpReversDolphinDiveAnimationNodeLeaf);
                this.proneStateNodeSelector.AddtoChildNode(this.wallJumpForwardLeftDolphinDiveAnimationNodeLeaf);
                this.proneStateNodeSelector.AddtoChildNode(this.wallJumpForwardRightDolphinDiveAnimationNodeLeaf);
                this.proneStateNodeSelector.AddtoChildNode(this.dolphinDiveAnimationNodeLeaf);
                this.proneStateNodeSelector.AddtoChildNode(this.diveStallAnimationNodeLeaf);
                this.proneStateNodeSelector.AddtoChildNode(this.proneAnimationNodeLeaf);


                gunFuBaseLayerNodeSelector.AddtoChildNode(executeAnimationNodeLeaf);
                gunFuBaseLayerNodeSelector.AddtoChildNode(restrictShieldSelector);
                this.gunFuBaseLayerNodeSelector.AddtoChildNode(this.knockDownNodeLeaf);
                this.gunFuBaseLayerNodeSelector.AddtoChildNode(this.gunFuReloadNodeLeaf);
                gunFuBaseLayerNodeSelector.AddtoChildNode(humanShieldSelector);
                this.gunFuBaseLayerNodeSelector.AddtoChildNode(this.hitDownNodeLeaf);
                gunFuBaseLayerNodeSelector.AddtoChildNode(hit1NodeLeaf);
                gunFuBaseLayerNodeSelector.AddtoChildNode(hit2NodeLeaf);
                gunFuBaseLayerNodeSelector.AddtoChildNode(hit3NodeLeaf);
                gunFuBaseLayerNodeSelector.AddtoChildNode(spinKickNodeLeaf);

                restrictShieldSelector.AddtoChildNode(restrictShieldEnterNodeLeaf);
                restrictShieldSelector.AddtoChildNode(restrictShieldExitNodeLeaf);
                restrictShieldSelector.AddtoChildNode(restrictShieldMoveNodeLeaf);

                humanShieldSelector.AddtoChildNode(humanShieldEnterNodeLeaf);
                humanShieldSelector.AddtoChildNode(humanShieldExitNodeLeaf);
                humanShieldSelector.AddtoChildNode(humanShieldMoveNodeLeaf);

                parkourNodeSelector.AddtoChildNode(vaultingNodeLeaf);
                parkourNodeSelector.AddtoChildNode(climbHighNodeLeaf);
                parkourNodeSelector.AddtoChildNode(climbLowNodeLeaf);

                this.playerBaseLayerAnimationNodeManagerPortable.startNodeSelector.AddtoChildNode(basedLayerNodeSelector);
            });
    }
    #endregion

    #region UpperBodyLayer
    public NodeManagerPortable playerUpperBodyLayerNodeManagerPortable { get; private set; }
    public NodeSelector upperBodyLayerNodeSelector { get; set; }

    public PlayAnimationNodeLeaf humanShieldNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf restrianShieldNodeLeaf { get; set; }
    public RestNodeLeaf upperBodyLayerRestNodeLeaf { get; set; }

    public NodeSelector performReloadNodeSelector { get; set; }
    public NodeSelector shotgunReloadNodeSelector { get; set; }
    public PlayAnimationNodeLeaf chamberloadShotgunNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf preLoadShotgunNodeLeaf { get; set; }
    public PlayPoseAnimationNodeLeaf quadloadShotgunNodeLeaf { get; set; }
    public PlayPoseAnimationNodeLeaf rifleReloadNodeLeaf { get; set; }
    public PlayPoseAnimationNodeLeaf rifleTacticalReloadNodeLeaf { get; set; }
    public PlayPoseAnimationNodeLeaf pistolReloadNodeLeaf { get; set; }
    public PlayPoseAnimationNodeLeaf pistolTacticalReloadNodeLeaf { get; set; }

    public NodeSelector drawSwitchSelector { get; set; }
    public NodeSelector quickSwitchSelector { get; set; }
    public PlayAnimationNodeLeaf quickSwitchDrawNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf quickSwitchHolsterSecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf quickSwitchHoslterPrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf drawPrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf drawSecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf holsterPrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf holsterSecondaryNodeLeaf { get; set; }

    private void InitializedUpperBodyLayerNodeManager()
    {
        playerUpperBodyLayerNodeManagerPortable = new NodeManagerPortable();
        playerUpperBodyLayerNodeManagerPortable.InitialzedOuterNode(
            () =>
            {
                this.InitializedUpperBodyLayer();
                this.playerUpperBodyLayerNodeManagerPortable.startNodeSelector.AddtoChildNode(upperBodyLayerNodeSelector);
            });
    }
    private void InitializedUpperBodyLayer()
    {
        this.upperBodyLayerNodeSelector = new NodeSelector(() => this.isEnableUpperBodyLayer);

        this.humanShieldNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.playerStateNodeMnager.TryGetCurNodeLeaf<HumanShield_GunFu_NodeLeaf>(out HumanShield_GunFu_NodeLeaf humanShield_GunFu_NodeLeaf)
            && humanShield_GunFu_NodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Stay,
            this.animator, "OCM_HumanShield", 1, .25f, 1);

        this.restrianShieldNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.playerStateNodeMnager.TryGetCurNodeLeaf<RestrainGunFuStateNodeLeaf>(out RestrainGunFuStateNodeLeaf restrainGunFuStateNodeLeaf)
            && restrainGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Stay,
            this.animator, "OCM_Restrain", 1, .25f, 1);

        this.performReloadNodeSelector = new NodeSelector(() => this.isPerformReload);
        this.shotgunReloadNodeSelector = new NodeSelector(
            () => this.playerWeaponManuverNodeManager.TryGetCurNodeLeaf<IShotgunReloadNode>());
        this.chamberloadShotgunNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.playerWeaponManuverNodeManager.TryGetCurNodeLeaf<ChamberLoadShotgunNodeLeaf>()
               && this.player._currentWeapon is AutomaticShotgunModel,
            this.animator, "ChamberLoadShotgun", 1, .1f);
        this.preLoadShotgunNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.playerWeaponManuverNodeManager.TryGetCurNodeLeaf<PreloadNodeLeaf>()
               && this.player._currentWeapon is AutomaticShotgunModel,
            this.animator, "PreLoadShotgun", 1, .1f);
        this.quadloadShotgunNodeLeaf = new PlayPoseAnimationNodeLeaf(
            () => this.playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuardLoadNodeLeaf>()
               && this.player._currentWeapon is AutomaticShotgunModel,
            this.animator, "QuadLoadShotgun", 1, .1f, this.upperBodyAnimationPoseTimeNormalized, 1, true);
        this.rifleReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<ReloadMagazineFullStageNodeLeaf>()
               && this.player._currentWeapon is AssultRifle_AR15Model,
            this.animator, "ReloadMagazine_AR15", 1, .05f, this.upperBodyAnimationPoseTimeNormalized, 1, false);
        this.rifleTacticalReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<TacticalReloadMagazineFullStageNodeLeaf>()
               && this.player._currentWeapon is AssultRifle_AR15Model,
            this.animator, "TacticalReloadMagazine_AR15", 1, .05f, this.upperBodyAnimationPoseTimeNormalized, 1, false);
        this.pistolReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<ReloadMagazineFullStageNodeLeaf>()
               && this.player._currentWeapon is Glock17_9mm,
            this.animator, "ReloadMagazine_Glock17", 1, .05f, this.upperBodyAnimationPoseTimeNormalized, 1, false);
        this.pistolTacticalReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<TacticalReloadMagazineFullStageNodeLeaf>()
               && this.player._currentWeapon is Glock17_9mm,
            this.animator, "TacticalReloadMagazine_Glock17", 1, .05f, this.upperBodyAnimationPoseTimeNormalized, 1, false);

        this.drawSwitchSelector = new NodeSelector(() => isDrawSwitchWeapon);
        this.quickSwitchSelector = new NodeSelector(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<IQuickSwitchNode>());
        this.quickSwitchDrawNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuickSwitch_Draw_NodeLeaf>(),
            animator, "QuickSwitchDraw", 1, 0.2f);
        this.quickSwitchHolsterSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuickSwitch_HolsterSecondaryWeapon_NodeLeaf>(),
            animator, "QuickSwitchHolsterSecondary", 1, 0.25f);
        this.quickSwitchHoslterPrimaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuickSwitch_HolsterPrimaryWeapon_NodeLeaf>(),
            animator, "QuickSwitchHolsterPrimary", 1, 0.25f);
        this.drawPrimaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<DrawPrimaryWeaponManuverNodeLeaf>(),
            animator, "DrawPrimary", 1, .2f);
        this.drawSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<DrawSecondaryWeaponManuverNodeLeaf>(),
            animator, "DrawSecondary", 1, .2f);
        this.holsterPrimaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<HolsterPrimaryWeaponManuverNodeLeaf>(),
            animator, "HolsterPrimary", 1, .2f);
        this.holsterSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<HolsterSecondaryWeaponManuverNodeLeaf>(),
            animator, "HolsterSecondary", 1, .2f);

        this.upperBodyLayerRestNodeLeaf = new RestNodeLeaf(() => true);

        this.upperBodyLayerNodeSelector.AddtoChildNode(this.humanShieldNodeLeaf);
        this.upperBodyLayerNodeSelector.AddtoChildNode(this.restrianShieldNodeLeaf);
        this.upperBodyLayerNodeSelector.AddtoChildNode(this.performReloadNodeSelector);
        this.upperBodyLayerNodeSelector.AddtoChildNode(this.drawSwitchSelector);
        this.upperBodyLayerNodeSelector.AddtoChildNode(this.upperBodyLayerRestNodeLeaf);

        this.performReloadNodeSelector.AddtoChildNode(this.shotgunReloadNodeSelector);
        this.performReloadNodeSelector.AddtoChildNode(this.rifleReloadNodeLeaf);
        this.performReloadNodeSelector.AddtoChildNode(this.rifleTacticalReloadNodeLeaf);
        this.performReloadNodeSelector.AddtoChildNode(this.pistolReloadNodeLeaf);
        this.performReloadNodeSelector.AddtoChildNode(this.pistolTacticalReloadNodeLeaf);

        this.shotgunReloadNodeSelector.AddtoChildNode(this.chamberloadShotgunNodeLeaf);
        this.shotgunReloadNodeSelector.AddtoChildNode(this.preLoadShotgunNodeLeaf);
        this.shotgunReloadNodeSelector.AddtoChildNode(this.quadloadShotgunNodeLeaf);

        this.drawSwitchSelector.AddtoChildNode(this.quickSwitchSelector);
        this.drawSwitchSelector.AddtoChildNode(this.drawPrimaryNodeLeaf);
        this.drawSwitchSelector.AddtoChildNode(this.drawSecondaryNodeLeaf);
        this.drawSwitchSelector.AddtoChildNode(this.holsterPrimaryNodeLeaf);
        this.drawSwitchSelector.AddtoChildNode(this.holsterSecondaryNodeLeaf);

        this.quickSwitchSelector.AddtoChildNode(this.quickSwitchDrawNodeLeaf);
        this.quickSwitchSelector.AddtoChildNode(this.quickSwitchHolsterSecondaryNodeLeaf);
        this.quickSwitchSelector.AddtoChildNode(this.quickSwitchHoslterPrimaryNodeLeaf);
    }
    #endregion

    #region UpperArmLayer
    public NodeSelector upperArmLayerNodeSelector { get; set; }

    public PlayAnimationMotionTimeMatchBaseLayerNodeLeaf sprintUpperNodeLeaf { get; set; }
    public PlayAnimationMotionTimeMatchBaseLayerNodeLeaf sprintChangeDirUpperNodeLeaf { get; set; }
    public NodeSelector weaponHandSelector { get; set; }
    public PlayAnimationBaseStateOffsetNodeLeaf primaryWeaponHandUpperNodeLeaf { get; set; }
    public PlayAnimationBaseStateOffsetNodeLeaf secondaryWeaponHandUpperNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf restUpperArmNodeLeaf { get; set; }
    public NodeManagerPortable playerUpperArmLayerNodeManagerPortable { get; private set; }
    private void InitializedUpperArmLayerNodeManager()
    {
        playerUpperArmLayerNodeManagerPortable = new NodeManagerPortable();
        playerUpperArmLayerNodeManagerPortable.InitialzedOuterNode(
            () =>
            {
                this.InitializedUpperArmLayer();

                upperArmLayerNodeSelector.AddtoChildNode(this.sprintChangeDirUpperNodeLeaf);
                upperArmLayerNodeSelector.AddtoChildNode(this.sprintUpperNodeLeaf);
                upperArmLayerNodeSelector.AddtoChildNode(this.weaponHandSelector);
                upperArmLayerNodeSelector.AddtoChildNode(this.restUpperArmNodeLeaf);

                this.weaponHandSelector.AddtoChildNode(this.primaryWeaponHandUpperNodeLeaf);
                this.weaponHandSelector.AddtoChildNode(this.secondaryWeaponHandUpperNodeLeaf);

                this.playerUpperArmLayerNodeManagerPortable.startNodeSelector.AddtoChildNode(upperArmLayerNodeSelector);
            });
    }
    private void InitializedUpperArmLayer()
    {
        upperArmLayerNodeSelector = new NodeSelector(() => isEnableUpperArmLayer);

        sprintUpperNodeLeaf = new PlayAnimationMotionTimeMatchBaseLayerNodeLeaf(
         () => playerStateNodeMnager.TryGetCurNodeLeaf<PlayerSprintNode>() 
         ,
         animator, "SprintWeaponSway", 2, 0, this.upperAnimationPoseTimeNormalized, .35f);

        sprintChangeDirUpperNodeLeaf = new PlayAnimationMotionTimeMatchBaseLayerNodeLeaf(
         () => playerStateNodeMnager.TryGetCurNodeLeaf<PlayerSprintChangeDirectionNode>(),
         animator, "SprintWeaponSway", 2, 0, this.upperAnimationPoseTimeNormalized, 1f);

        this.weaponHandSelector = new NodeSelector(()=> this.player._currentWeapon != null);

        this.primaryWeaponHandUpperNodeLeaf = new PlayAnimationBaseStateOffsetNodeLeaf(
            () => this.player._currentWeapon != null
            && this.player._currentWeapon is PrimaryWeapon,
            animator
            , "PrimaryWeaponHand", 2, 0, .2f);

        this.secondaryWeaponHandUpperNodeLeaf = new PlayAnimationBaseStateOffsetNodeLeaf(
            () => true
            , this.animator
            , "SecondaryWeaponHand", 2, 0, .2f);

        this.restUpperArmNodeLeaf = new PlayAnimationNodeLeaf(
            () => true
            , this.animator
            , "Rest"
            , 2
            , .1f);

    }
    #endregion

    #region playerAnimationNodeComponentManager
    public CrouchWeightSoftCoverNodeLeaf crouchWeightSoftCoverNodeLeaf { get; set; }

    public SetLayerAnimationNodeLeaf enableUpperBodyLayerAnimationNodeLeaf { get; set; }
    public SetLayerAnimationNodeLeaf disableUpperBodyLayerAnimationNodeLeaf { get; set; }
    public NodeSelector upperBodyLayerEnableDisableSelector { get; set; }

    public SetLayerAnimationNodeLeaf enableLayerAnimationNodeLeaf { get; set; }
    public SetLayerAnimationNodeLeaf disableLayerAnimationNodeLeaf { get; set; }
    public NodeSelector upperArmLayerEnableDisableSelector { get; set; }

    private NodeComponentManager playerAnimationNodeComponentManager;
    private void InitializedAnimationNodeComponent()
    {
        playerAnimationNodeComponentManager = new NodeComponentManager();

        crouchWeightSoftCoverNodeLeaf = new CrouchWeightSoftCoverNodeLeaf(player, 0.65f, 2.5f,
            () => playerStateNodeMnager.GetCurNodeLeaf() is PlayerCrouch_Idle_NodeLeaf
            || playerStateNodeMnager.GetCurNodeLeaf() is PlayerCrouch_Move_NodeLeaf
            );

        upperBodyLayerEnableDisableSelector = new NodeSelector(() => true);
        enableUpperBodyLayerAnimationNodeLeaf = new SetLayerAnimationNodeLeaf(() => isEnableUpperBodyLayer, animator, 1, 8f, 1);
        disableUpperBodyLayerAnimationNodeLeaf = new SetLayerAnimationNodeLeaf(() => true, animator, 1, 8f, 0);
        upperBodyLayerEnableDisableSelector.AddtoChildNode(enableUpperBodyLayerAnimationNodeLeaf);
        upperBodyLayerEnableDisableSelector.AddtoChildNode(disableUpperBodyLayerAnimationNodeLeaf);

        upperArmLayerEnableDisableSelector = new NodeSelector(() => true);
        enableLayerAnimationNodeLeaf = new SetLayerAnimationNodeLeaf(() => isEnableUpperArmLayer
        , animator, 2, 8f, 1);
        disableLayerAnimationNodeLeaf = new SetLayerAnimationNodeLeaf(() => true
        , animator, 2, 8f, 0);

        upperArmLayerEnableDisableSelector.AddtoChildNode(enableLayerAnimationNodeLeaf);
        upperArmLayerEnableDisableSelector.AddtoChildNode(disableLayerAnimationNodeLeaf);

        this.playerAnimationNodeComponentManager.AddNode(this.crouchWeightSoftCoverNodeLeaf);
        this.playerAnimationNodeComponentManager.AddNode(this.upperBodyLayerEnableDisableSelector);
        this.playerAnimationNodeComponentManager.AddNode(this.upperArmLayerEnableDisableSelector);

    }
    #endregion


    public void InitailizedNode()
    {
        this.upperBodyAnimationPoseTimeNormalized = new AnimationPoseTimeNormalized();
        this.upperAnimationPoseTimeNormalized = new AnimationPoseTimeNormalized();
        this.basedAnimationPoseTimeNormalzied = new AnimationPoseTimeNormalized();

        this.InitializedBasedLayerNodeManager();
        this.InitializedUpperBodyLayerNodeManager();
        this.InitializedUpperArmLayerNodeManager();
        this.InitializedAnimationNodeComponent();

    }

    private void UpdateNode()
    {
        this.playerBaseLayerAnimationNodeManagerPortable.UpdateNode();
        this.playerUpperBodyLayerNodeManagerPortable.UpdateNode();
        this.playerUpperArmLayerNodeManagerPortable.UpdateNode();
        this.playerAnimationNodeComponentManager.Update();
    }
    private void FixedUpdateNode()
    {
        this.playerBaseLayerAnimationNodeManagerPortable.FixedUpdateNode();
        this.playerUpperBodyLayerNodeManagerPortable.FixedUpdateNode();
        this.playerUpperArmLayerNodeManagerPortable.FixedUpdateNode();
        this.playerAnimationNodeComponentManager.FixedUpdate();

    }
}
