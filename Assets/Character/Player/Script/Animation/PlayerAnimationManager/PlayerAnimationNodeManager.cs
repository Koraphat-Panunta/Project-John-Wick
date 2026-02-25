
using UnityEngine;

public partial class PlayerAnimationManager 
{

    #region BaseLayer
    public NodeManagerPortable playerBaseLayerAnimationNodeManagerPortable;
    public NodeSelector basedLayerNodeSelector { get; set; }
    public PlayAnimationNodeLeaf deadNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf throwObjectNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf pokePickUpNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf getUpNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf boundOffNodeLeaf { get; set; }
    public NodeSelector parkourNodeSelector { get; set; }
    public PlayAnimationNodeLeaf vaultingNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf climbLowNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf climbHighNodeLeaf { get; set; }
    public NodeSelector gunFuBaseLayerNodeSelector { get; set; }

    public NodeSelector weaponDisarmSelector { get; set; }
    public PlayAnimationNodeLeaf weaponDisarmPrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf weaponDisarmSecondaryNodeLeaf { get; set; }
    public GunFuExecuteAnimationNodeLeaf executeAnimationNodeLeaf { get; set; }

    public NodeSelector restrictShieldSelector { get; set; }
    public PlayAnimationNodeLeaf restrictShieldEnterNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf restrictShieldExitNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf restrictShieldMoveNodeLeaf { get; set; }

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
    public PlayAnimationNodeLeaf sprintNodeLeaf { get; set; }

    public NodeSelector proneStateNodeSelector { get; set; }
    public PlayPoseAnimationNodeLeaf dolphinDiveAnimationNodeLeaf { get; set; }
    public PlayPoseAnimationNodeLeaf proneAnimationNodeLeaf { get; set; }

    public PlayAnimationNodeLeaf moveCrouchNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf moveStandNodeLeaf { get; set; }
    private void InitializedBasedLayer()
    {
        basedLayerNodeSelector = new NodeSelector(() => true);
        deadNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is PlayerDeadNodeLeaf, animator, "Dead", 0, 0.14f);
        throwObjectNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<PlayerThrowWeaponNodeLeaf>(), animator, "Throwing", 0, .05f,player.throwObjectAnimationTriggerEventSCRP.enterNormalizedTime);
        pokePickUpNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<PlayerPokePickUpWeaponNodeLeaf>()
            , animator
            , "PokePickUp"
            , 0, .07f
            , (playerStateNodeMnager as PlayerStateNodeManager).playerPokePickUpWeaponNodeLeaf.animationTriggerEventSCRP.enterNormalizedTime);
 
        boundOffNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is PlayerBrounceOffGotAttackGunFuNodeLeaf
            , animator, "PlayerBounceOff", 0, .05f);
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
            animator, "Sprint", 0, .5f);

        this.proneStateNodeSelector = new NodeSelector(
            ()=> this.player.stance == Stance.prone);
        this.dolphinDiveAnimationNodeLeaf = new PlayPoseAnimationNodeLeaf
            (() => this.playerStateNodeMnager.TryGetCurNodeLeaf<PlayerDolphinDiveStateNodeLeaf>()
            , this.animator, "Dolphin Dive", 0, .1f, this.basedAnimationPoseTimeNormalzied, .5f, false);
        this.proneAnimationNodeLeaf = new PlayPoseAnimationNodeLeaf(
            () => this.playerStateNodeMnager.TryGetCurNodeLeaf<PlayerProneStateNodeLeaf>()
            , this.animator, "Prone", 0, 0, this.basedAnimationPoseTimeNormalzied, 1, false);
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

        weaponDisarmSelector = new NodeSelector(() => playerStateNodeMnager.GetCurNodeLeaf() is WeaponDisarm_GunFuInteraction_NodeLeaf weaponDisarmNodeLeaf);
        weaponDisarmPrimaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is WeaponDisarm_GunFuInteraction_NodeLeaf weaponDisarmNodeLeaf
            && weaponDisarmNodeLeaf.disarmedWeapon is PrimaryWeapon
            , animator, GunFuManaverStateName.WeaponDisarmPrimary.ToString(), 0, AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration
            ,player.primaryWeaponDisarmGunFuScriptableObject.enterNormalizedTime);
        weaponDisarmSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is WeaponDisarm_GunFuInteraction_NodeLeaf weaponDisarmNodeLeaf
            && weaponDisarmNodeLeaf.disarmedWeapon is SecondaryWeapon
            , animator, GunFuManaverStateName.WeaponDisarmSecondary.ToString(), 0, AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration
            ,player.secondaryWeaponDisarmGunFuScriptableObject.enterNormalizedTime);

        executeAnimationNodeLeaf = new GunFuExecuteAnimationNodeLeaf(() => executeAnimationNodeLeaf.gunFuExecuteNodeLeaf != null, player, animator);

        restrictShieldSelector = new NodeSelector(() => playerStateNodeMnager.TryGetCurNodeLeaf<RestrainGunFuStateNodeLeaf>());
        restrictShieldEnterNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf restrictNodeLeaf
        && (restrictNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Enter)
        , animator,GunFuManaverStateName.Restrain.ToString(), 0, 0.35f);
        restrictShieldExitNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf restrictNodeLeaf
        && (restrictNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Exit
        || restrictNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.ExitAttack)
        , animator,GunFuManaverStateName.RestrainExit.ToString(), 0, .35f);
        restrictShieldMoveNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf restrictNodeLeaf
        && (restrictNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Stay)
        , animator, "Move/Idle", 0, .35f);

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
        , GunFuManaverStateName.HumanShield.ToString()
        , 0
        ,AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration
        , player.humanShieldSCRP.animationInteractCharacterDetail[0].enterAnimationOffsetNormalizedTime);
        humanShieldExitNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is HumanShieldExit_GunFu_NodeLeaf humanShieldExitNodeLeaf
        , animator
        , GunFuManaverStateName.HumanShieldExit.ToString()
        , 0
        , AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration
        , player.humanShieldSCRP.animationInteractCharacterDetail[0].enterAnimationOffsetNormalizedTime);
        humanShieldMoveNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf humanShieldNodeLeaf
        , animator, "Move/Idle", 0, .35f);

        this.hitDownNodeLeaf = new PlayAnimationNodeLeaf(
            ()=> this.playerStateNodeMnager.TryGetCurNodeLeaf<GunFuHitDownNodeLeaf>()
            ,this.animator, "HitDown", 0,0);
        hit1NodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is GunFuHitNodeLeaf gunFuHitNodeLeaf
            && gunFuHitNodeLeaf._stateName == "Hit1",
            animator, "Hit1", 0, .1f, this.player.hit1.enterNormalizedTime);
        hit2NodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is GunFuHitNodeLeaf gunFuHitNodeLeaf
            && gunFuHitNodeLeaf._stateName == "Hit2",
            animator, "Hit2", 0, .1f, player.hit2.enterNormalizedTime);
        hit3NodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is GunFuHitNodeLeaf gunFuHitNodeLeaf
            && gunFuHitNodeLeaf._stateName == "Hit3",
            animator, "Hit3", 0, .1f, player.hit3.enterNormalizedTime);
        spinKickNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is GunFuHitNodeLeaf gunFuHitNodeLeaf
            && gunFuHitNodeLeaf._stateName == "DodgeSpinKick",
            animator, "DodgeSpinKick", 0, .1f, player.dodgeSpinKick.enterNormalizedTime
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
                basedLayerNodeSelector.AddtoChildNode(pokePickUpNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(boundOffNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(parkourNodeSelector);
                basedLayerNodeSelector.AddtoChildNode(gunFuBaseLayerNodeSelector);
                basedLayerNodeSelector.AddtoChildNode(this.fallingNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(this.landingRollNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(this.landingStandNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(dodgeNodeLeaf);
                this.basedLayerNodeSelector.AddtoChildNode(this.getUpNodeLeaf);
                this.basedLayerNodeSelector.AddtoChildNode(this.proneStateNodeSelector);
                basedLayerNodeSelector.AddtoChildNode(sprintNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(moveCrouchNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(moveStandNodeLeaf);

                this.proneStateNodeSelector.AddtoChildNode(this.dolphinDiveAnimationNodeLeaf);
                this.proneStateNodeSelector.AddtoChildNode(this.proneAnimationNodeLeaf);

                gunFuBaseLayerNodeSelector.AddtoChildNode(weaponDisarmSelector);
                gunFuBaseLayerNodeSelector.AddtoChildNode(executeAnimationNodeLeaf);
                gunFuBaseLayerNodeSelector.AddtoChildNode(restrictShieldSelector);
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

                weaponDisarmSelector.AddtoChildNode(weaponDisarmPrimaryNodeLeaf);
                weaponDisarmSelector.AddtoChildNode(weaponDisarmSecondaryNodeLeaf);

                parkourNodeSelector.AddtoChildNode(vaultingNodeLeaf);
                parkourNodeSelector.AddtoChildNode(climbHighNodeLeaf);
                parkourNodeSelector.AddtoChildNode(climbLowNodeLeaf);

                this.playerBaseLayerAnimationNodeManagerPortable.startNodeSelector.AddtoChildNode(basedLayerNodeSelector);
            });
    }
    #endregion

    #region UpperLayer
    public NodeSelector upperLayerNodeSelector { get; set; }
    public NodeSelector performReloadNodeSelector { get; set; }

    public PlayPoseAnimationNodeLeaf rifleReloadNodeLeaf { get; set; }
    public PlayPoseAnimationNodeLeaf rifleTacticalReloadNodeLeaf { get; set; }
    public PlayPoseAnimationNodeLeaf pistolReloadNodeLeaf { get; set; }
    public PlayPoseAnimationNodeLeaf pistolTacticalReloadNodeLeaf { get; set; }

    public NodeSelector performGunFuUpperLayerNodeSelector { get; set; }
    public PlayAnimationNodeLeaf humanShieldPrimaryStayNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf humanShieldSecondaryStayNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf restrictShieldPrimaryStayNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf restrictShieldSecondaryStayNodeLeaf { get; set; }

    public NodeSelector drawSwitchSelector { get; set; }
    public NodeSelector quickSwitchSelector { get; set; }
    public PlayAnimationNodeLeaf quickSwitchDrawNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf quickSwitchHolsterSecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf quickSwitchHoslterPrimaryNodeLeaf { get; set; }


    public PlayAnimationNodeLeaf drawPrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf drawSecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf holsterPrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf holsterSecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf switchPrimaryToSecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf swtichSecondaryToPrimaryNodeLeaf { get; set; }

    public PlayAnimationBaseStateOffsetNodeLeaf sprintUpperNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf quickSwitchWeaponManuverNodeLeaf { get; set; }

    public NodeSelector weaponHandSelector { get; set; }
    public PlayAnimationBaseStateOffsetNodeLeaf primaryWeaponHandUpperNodeLeaf { get; set; }
    public PlayAnimationBaseStateOffsetNodeLeaf secondaryWeaponHandUpperNodeLeaf { get; set; }

    public NodeManagerPortable playerUpperLayerNodeManagerPortable { get; private set; }
    private void InitializedUpperLayerNodeManager()
    {
        playerUpperLayerNodeManagerPortable = new NodeManagerPortable();
        playerUpperLayerNodeManagerPortable.InitialzedOuterNode(
            () =>
            {
                this.InitializedUpperLayer();

                upperLayerNodeSelector.AddtoChildNode(this.performReloadNodeSelector);
                upperLayerNodeSelector.AddtoChildNode(performGunFuUpperLayerNodeSelector);
                upperLayerNodeSelector.AddtoChildNode(drawSwitchSelector);
                upperLayerNodeSelector.AddtoChildNode(sprintUpperNodeLeaf);
                upperLayerNodeSelector.AddtoChildNode(quickSwitchWeaponManuverNodeLeaf);
                upperLayerNodeSelector.AddtoChildNode(this.weaponHandSelector);

                this.weaponHandSelector.AddtoChildNode(this.primaryWeaponHandUpperNodeLeaf);
                this.weaponHandSelector.AddtoChildNode(this.secondaryWeaponHandUpperNodeLeaf);

                this.performReloadNodeSelector.AddtoChildNode(this.rifleReloadNodeLeaf);
                this.performReloadNodeSelector.AddtoChildNode(this.rifleTacticalReloadNodeLeaf);
                this.performReloadNodeSelector.AddtoChildNode(this.pistolReloadNodeLeaf);
                this.performReloadNodeSelector.AddtoChildNode(this.pistolTacticalReloadNodeLeaf);

                performGunFuUpperLayerNodeSelector.AddtoChildNode(humanShieldPrimaryStayNodeLeaf);
                performGunFuUpperLayerNodeSelector.AddtoChildNode(humanShieldSecondaryStayNodeLeaf);
                performGunFuUpperLayerNodeSelector.AddtoChildNode(restrictShieldPrimaryStayNodeLeaf);
                performGunFuUpperLayerNodeSelector.AddtoChildNode(restrictShieldSecondaryStayNodeLeaf);

                drawSwitchSelector.AddtoChildNode(quickSwitchSelector);
                drawSwitchSelector.AddtoChildNode(drawPrimaryNodeLeaf);
                drawSwitchSelector.AddtoChildNode(drawSecondaryNodeLeaf);
                drawSwitchSelector.AddtoChildNode(holsterPrimaryNodeLeaf);
                drawSwitchSelector.AddtoChildNode(holsterSecondaryNodeLeaf);
                drawSwitchSelector.AddtoChildNode(switchPrimaryToSecondaryNodeLeaf);
                drawSwitchSelector.AddtoChildNode(swtichSecondaryToPrimaryNodeLeaf);

                quickSwitchSelector.AddtoChildNode(quickSwitchDrawNodeLeaf);
                quickSwitchSelector.AddtoChildNode(quickSwitchHolsterSecondaryNodeLeaf);
                quickSwitchSelector.AddtoChildNode(quickSwitchHoslterPrimaryNodeLeaf);

                this.playerUpperLayerNodeManagerPortable.startNodeSelector.AddtoChildNode(upperLayerNodeSelector);
            });
    }
    private void InitializedUpperLayer()
    {
        upperLayerNodeSelector = new NodeSelector(() => isEnableUpperLayer);

        performReloadNodeSelector = new NodeSelector(() => isPerformReload);

        this.rifleReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<ReloadMagazineFullStageNodeLeaf>() 
            && this.player._currentWeapon is AssultRifle_AR15Model 
            , this.animator
            , "ReloadMagazine_AR15"
            , 1
            ,.05f
            ,this.upperAnimationPoseTimeNormalized
            ,1
            ,false);
        this.rifleTacticalReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<TacticalReloadMagazineFullStageNodeLeaf>()
            && this.player._currentWeapon is AssultRifle_AR15Model
            , this.animator
            , "TacticalReloadMagazine_AR15"
            , 1
            ,.05f
            ,this.upperAnimationPoseTimeNormalized
            ,1
            ,false);
        this.pistolReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<ReloadMagazineFullStageNodeLeaf>()
            && this.player._currentWeapon is Glock17_9mm
            , this.animator
            , "ReloadMagazine_Glock17"
            , 1
            , .05f
            , this.upperAnimationPoseTimeNormalized
            , 1
            , false);
        this.pistolTacticalReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<TacticalReloadMagazineFullStageNodeLeaf>()
            && this.player._currentWeapon is Glock17_9mm
            , this.animator
            , "TacticalReloadMagazine_Glock17"
            , 1
            , .05f
            , this.upperAnimationPoseTimeNormalized
            , 1
            , false);

        performGunFuUpperLayerNodeSelector = new NodeSelector(() => isPerformGunFu);
        humanShieldPrimaryStayNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<HumanShield_GunFu_NodeLeaf>() && player._currentWeapon is PrimaryWeapon,
            animator, "HS_Stay_Primary", 1, .25f, .3f);
        humanShieldSecondaryStayNodeLeaf = new PlayAnimationNodeLeaf(
           () => playerStateNodeMnager.TryGetCurNodeLeaf<HumanShield_GunFu_NodeLeaf>() && player._currentWeapon is SecondaryWeapon,
           animator, "HS_Stay_Secondary", 1, .25f, .3f);
        restrictShieldPrimaryStayNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<RestrainGunFuStateNodeLeaf>() && player._currentWeapon is PrimaryWeapon,
            animator, "Restrict_Stay_Primary", 1, .25f, .3f);
        restrictShieldSecondaryStayNodeLeaf = new PlayAnimationNodeLeaf(
           () => playerStateNodeMnager.TryGetCurNodeLeaf<RestrainGunFuStateNodeLeaf>() && player._currentWeapon is SecondaryWeapon,
           animator, "Restrict_Stay_Secondary", 1, .25f, .3f);

        drawSwitchSelector = new NodeSelector(() => isDrawSwitchWeapon);

        quickSwitchSelector = new NodeSelector(() => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<IQuickSwitchNode>());
        quickSwitchDrawNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuickSwitch_Draw_NodeLeaf>()
            , animator
            , "QuickSwitchDraw"
            , 1
            , 0.2f);
        quickSwitchHolsterSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuickSwitch_HolsterSecondaryWeapon_NodeLeaf>()
            , animator
            , "QuickSwitchHolsterSecondary"
            , 1
            , 0.25f);
        quickSwitchHoslterPrimaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuickSwitch_HolsterPrimaryWeapon_NodeLeaf>()
            , animator
            , "QuickSwitchHolsterPrimary"
            , 1
            , 0.25f);


        drawPrimaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<DrawPrimaryWeaponManuverNodeLeaf>(),
            animator, "DrawPrimary", 1, .2f);
        drawSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
           () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<DrawSecondaryWeaponManuverNodeLeaf>(),
           animator, "DrawSecondary", 1, .2f);
        holsterPrimaryNodeLeaf = new PlayAnimationNodeLeaf(
           () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<HolsterPrimaryWeaponManuverNodeLeaf>(),
           animator, "HolsterPrimary", 1, .2f);
        holsterSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
         () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<HolsterSecondaryWeaponManuverNodeLeaf>(),
         animator, "HolsterSecondary", 1, .2f);
        switchPrimaryToSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
         () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<PrimaryToSecondarySwitchWeaponManuverLeafNode>(),
         animator, "SwitchWeaponPrimary -> Secondary", 1, .2f);
        swtichSecondaryToPrimaryNodeLeaf = new PlayAnimationNodeLeaf(
         () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<SecondaryToPrimarySwitchWeaponManuverLeafNode>(),
         animator, "SwitchWeaponSecondary -> Primary", 1, .2f);

        sprintUpperNodeLeaf = new PlayAnimationBaseStateOffsetNodeLeaf(
         () => playerStateNodeMnager.TryGetCurNodeLeaf<PlayerSprintNode>(),
         animator, "SprintWeaponSway", 1, 0, .2f);

        quickSwitchWeaponManuverNodeLeaf = new PlayAnimationNodeLeaf(
            () =>
            playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuickSwitch_AimDownSight_NodeLeaf>()
            || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuickSwitch_LowReady_NodeLeaf>()
            , animator, "QuickSwitchWeaponManuver", 1, .25f);

        this.weaponHandSelector = new NodeSelector(()=> true);

        this.primaryWeaponHandUpperNodeLeaf = new PlayAnimationBaseStateOffsetNodeLeaf(
            () => this.player._currentWeapon != null
            && this.player._currentWeapon is PrimaryWeapon,
            animator
            , "PrimaryWeaponHand", 1, 0, .2f);

        this.secondaryWeaponHandUpperNodeLeaf = new PlayAnimationBaseStateOffsetNodeLeaf(
            () => true
            , this.animator
            , "SecondaryWeaponHand", 1, 0, .2f);

    }
    #endregion

    #region playerAnimationNodeComponentManager
    public CrouchWeightSoftCoverNodeLeaf crouchWeightSoftCoverNodeLeaf { get; set; }

    public SetLayerAnimationNodeLeaf enableLayerAnimationNodeLeaf { get; set; }
    public SetLayerAnimationNodeLeaf disableLayerAnimationNodeLeaf { get; set; }
    public NodeSelector upperLayerEnableDisableSelector { get; set; }

    private NodeComponentManager playerAnimationNodeComponentManager;
    private void InitializedAnimationNodeComponent()
    {
        playerAnimationNodeComponentManager = new NodeComponentManager();

        crouchWeightSoftCoverNodeLeaf = new CrouchWeightSoftCoverNodeLeaf(player, 0.65f, 2.5f,
            () => playerStateNodeMnager.GetCurNodeLeaf() is PlayerCrouch_Idle_NodeLeaf
            || playerStateNodeMnager.GetCurNodeLeaf() is PlayerCrouch_Move_NodeLeaf
            );

        upperLayerEnableDisableSelector = new NodeSelector(() => true);
        enableLayerAnimationNodeLeaf = new SetLayerAnimationNodeLeaf(() => isEnableUpperLayer
        , animator, 1, 3f, 1);
        disableLayerAnimationNodeLeaf = new SetLayerAnimationNodeLeaf(() => true
        , animator, 1, 3f, 0);

        upperLayerEnableDisableSelector.AddtoChildNode(enableLayerAnimationNodeLeaf);
        upperLayerEnableDisableSelector.AddtoChildNode(disableLayerAnimationNodeLeaf);

        this.playerAnimationNodeComponentManager.AddNode(this.crouchWeightSoftCoverNodeLeaf);
        this.playerAnimationNodeComponentManager.AddNode(this.upperLayerEnableDisableSelector);

    }
    #endregion



    public void InitailizedNode()
    {
        this.upperAnimationPoseTimeNormalized = new AnimationPoseTimeNormalized();
        this.basedAnimationPoseTimeNormalzied = new AnimationPoseTimeNormalized();

        this.InitializedBasedLayerNodeManager();
        this.InitializedUpperLayerNodeManager();
        this.InitializedAnimationNodeComponent();

    }

    private void UpdateNode()
    {
        this.playerBaseLayerAnimationNodeManagerPortable.UpdateNode();
        this.playerUpperLayerNodeManagerPortable.UpdateNode();
        this.playerAnimationNodeComponentManager.Update();
    }
    private void FixedUpdateNode()
    {
        this.playerBaseLayerAnimationNodeManagerPortable.FixedUpdateNode();
        this.playerUpperLayerNodeManagerPortable.FixedUpdateNode();
        this.playerAnimationNodeComponentManager.FixedUpdate();

    }
}
