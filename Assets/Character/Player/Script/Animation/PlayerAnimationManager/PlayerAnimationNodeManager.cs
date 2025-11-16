using NUnit.Framework.Constraints;
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
    public PlayAnimationNodeLeaf restrictShieldPrimaryEnterNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf restrictShieldSecondaryEnterNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf restrictShieldExitNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf restrictShieldMoveNodeLeaf { get; set; }

    public PlayAnimationNodeLeaf humanThrowNodeLeaf { get; set; }
    public NodeSelector humanShieldSelector { get; set; }
    public PlayAnimationNodeLeaf humanShieldPrimaryEnterNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf humanShieldSecondaryEnterNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf humanShieldMoveNodeLeaf { get; set; }

    public PlayAnimationNodeLeaf hit1NodeLeaf { get; set; }
    public PlayAnimationNodeLeaf hit2NodeLeaf { get; set; }
    public PlayAnimationNodeLeaf hit3NodeLeaf { get; set; }
    public PlayAnimationNodeLeaf spinKickNodeLeaf { get; set; }

    public PlayAnimationNodeLeaf dodgeNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf sprintNodeLeaf { get; set; }
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
        getUpNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is PlayerGetUpStateNodeLeaf
            , animator, "PlayerSpringGetUp", 0, .2f);
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

        dodgeNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is PlayerDodgeRollStateNodeLeaf,
            animator, "DodgeRoll", 0, .2f, 0.1f);
        sprintNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is PlayerSprintNode,
            animator, "Sprint", 0, .5f);
        moveCrouchNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is PlayerCrouch_Idle_NodeLeaf || playerStateNodeMnager.GetCurNodeLeaf() is PlayerCrouch_Move_NodeLeaf,
            animator, "Crouch", 0, .4f);
        moveStandNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is PlayerStandIdleNodeLeaf || playerStateNodeMnager.GetCurNodeLeaf() is PlayerStandMoveNodeLeaf,
            animator, "Move/Idle", 0, .6f);
    }
    private void InitializedGunFuBasedLayer()
    {
        gunFuBaseLayerNodeSelector = new NodeSelector(() => isPerformGunFu);

        weaponDisarmSelector = new NodeSelector(() => playerStateNodeMnager.GetCurNodeLeaf() is WeaponDisarm_GunFuInteraction_NodeLeaf weaponDisarmNodeLeaf);
        weaponDisarmPrimaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is WeaponDisarm_GunFuInteraction_NodeLeaf weaponDisarmNodeLeaf
            && weaponDisarmNodeLeaf.disarmedWeapon is PrimaryWeapon
            , animator, "GunFuPrimaryDisarm", 0, AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration
            ,player.primaryWeaponDisarmGunFuScriptableObject.enterNormalizedTime);
        weaponDisarmSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is WeaponDisarm_GunFuInteraction_NodeLeaf weaponDisarmNodeLeaf
            && weaponDisarmNodeLeaf.disarmedWeapon is SecondaryWeapon
            , animator, "GunFuSecondaryDisarm", 0, AnimationInteractScriptableObject.transitionRootDrivenAnimationDuration
            ,player.secondaryWeaponDisarmGunFuScriptableObject.enterNormalizedTime);

        executeAnimationNodeLeaf = new GunFuExecuteAnimationNodeLeaf(() => executeAnimationNodeLeaf.gunFuExecuteNodeLeaf != null, player, animator);

        restrictShieldSelector = new NodeSelector(() => playerStateNodeMnager.GetCurNodeLeaf() is RestrictGunFuStateNodeLeaf);
        restrictShieldPrimaryEnterNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is RestrictGunFuStateNodeLeaf restrictNodeLeaf
        && (restrictNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Enter)
        && player._currentWeapon is PrimaryWeapon
        , animator, "Restrict_Enter", 0, 0.35f);
        restrictShieldSecondaryEnterNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is RestrictGunFuStateNodeLeaf restrictNodeLeaf
        && (restrictNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Enter)
        && player._currentWeapon is SecondaryWeapon
        , animator, "Restrict_Enter", 0, .35f);
        restrictShieldExitNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is RestrictGunFuStateNodeLeaf restrictNodeLeaf
        && (restrictNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Exit
        || restrictNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.ExitAttack)
        , animator, "Restrict_Exit", 0, .35f);
        restrictShieldMoveNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is RestrictGunFuStateNodeLeaf restrictNodeLeaf
        && (restrictNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Stay)
        , animator, "Move/Idle", 0, .35f);

        humanThrowNodeLeaf = new PlayAnimationNodeLeaf(
           () =>
           {
               if (playerStateNodeMnager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf humanShield_GunFuInteraction_NodeLeaf
               && (humanShield_GunFuInteraction_NodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Exit
               || humanShield_GunFuInteraction_NodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.ExitAttacked))
                   return true;

               return false;
           }
           , animator, "HS_Exit", 0, .35f);

        humanShieldSelector = new NodeSelector(() => playerStateNodeMnager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf);
        humanShieldPrimaryEnterNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf humanShieldNodeLeaf
        && (humanShieldNodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Enter)
        && player._currentWeapon is PrimaryWeapon
        , animator, "HS_P_Enter", 0, .35f);
        humanShieldSecondaryEnterNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf humanShieldNodeLeaf
        && (humanShieldNodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Enter)
        && player._currentWeapon is SecondaryWeapon
        , animator, "HS_P_Enter", 0, .35f);
        humanShieldMoveNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf humanShieldNodeLeaf
       && (humanShieldNodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Stay || humanShieldNodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
       , animator, "Move/Idle", 0, .35f);

        hit1NodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is GunFuHitNodeLeaf gunFuHitNodeLeaf
            && gunFuHitNodeLeaf._stateName == "Hit1",
            animator, "Hit1", 0, .1f, player.hit1.animationGunFuHitOffset);
        hit2NodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is GunFuHitNodeLeaf gunFuHitNodeLeaf
            && gunFuHitNodeLeaf._stateName == "Hit2",
            animator, "Hit2", 0, .1f, player.hit2.animationGunFuHitOffset);
        hit3NodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is GunFuHitNodeLeaf gunFuHitNodeLeaf
            && gunFuHitNodeLeaf._stateName == "Hit3",
            animator, "Hit3", 0, .1f, player.hit3.animationGunFuHitOffset);
        spinKickNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.GetCurNodeLeaf() is GunFuHitNodeLeaf gunFuHitNodeLeaf
            && gunFuHitNodeLeaf._stateName == "DodgeSpinKick",
            animator, "DodgeSpinKick", 0, .1f, player.dodgeSpinKick.animationGunFuHitOffset);
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
                basedLayerNodeSelector.AddtoChildNode(getUpNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(boundOffNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(parkourNodeSelector);
                basedLayerNodeSelector.AddtoChildNode(gunFuBaseLayerNodeSelector);
                basedLayerNodeSelector.AddtoChildNode(dodgeNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(sprintNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(moveCrouchNodeLeaf);
                basedLayerNodeSelector.AddtoChildNode(moveStandNodeLeaf);

                gunFuBaseLayerNodeSelector.AddtoChildNode(weaponDisarmSelector);
                gunFuBaseLayerNodeSelector.AddtoChildNode(executeAnimationNodeLeaf);
                gunFuBaseLayerNodeSelector.AddtoChildNode(restrictShieldSelector);
                gunFuBaseLayerNodeSelector.AddtoChildNode(humanThrowNodeLeaf);
                gunFuBaseLayerNodeSelector.AddtoChildNode(humanShieldSelector);
                gunFuBaseLayerNodeSelector.AddtoChildNode(hit1NodeLeaf);
                gunFuBaseLayerNodeSelector.AddtoChildNode(hit2NodeLeaf);
                gunFuBaseLayerNodeSelector.AddtoChildNode(hit3NodeLeaf);
                gunFuBaseLayerNodeSelector.AddtoChildNode(spinKickNodeLeaf);

                restrictShieldSelector.AddtoChildNode(restrictShieldPrimaryEnterNodeLeaf);
                restrictShieldSelector.AddtoChildNode(restrictShieldSecondaryEnterNodeLeaf);
                restrictShieldSelector.AddtoChildNode(restrictShieldExitNodeLeaf);
                restrictShieldSelector.AddtoChildNode(restrictShieldMoveNodeLeaf);

                humanShieldSelector.AddtoChildNode(humanShieldPrimaryEnterNodeLeaf);
                humanShieldSelector.AddtoChildNode(humanShieldSecondaryEnterNodeLeaf);
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
    public PlayAnimationNodeLeaf reloadNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf tacticalReloadNodeLeaf { get; set; }

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
    public PlayAnimationBaseStateOffsetNodeLeaf moveIdleUpperNodeLeaf { get; set; }

    public NodeManagerPortable playerUpperLayerNodeManagerPortable { get; private set; }
    private void InitializedUpperLayerNodeManager()
    {
        playerUpperLayerNodeManagerPortable = new NodeManagerPortable();
        playerUpperLayerNodeManagerPortable.InitialzedOuterNode(
            () =>
            {
                this.InitializedUpperLayer();

                upperLayerNodeSelector.AddtoChildNode(performReloadNodeSelector);
                upperLayerNodeSelector.AddtoChildNode(performGunFuUpperLayerNodeSelector);
                upperLayerNodeSelector.AddtoChildNode(drawSwitchSelector);
                upperLayerNodeSelector.AddtoChildNode(sprintUpperNodeLeaf);
                upperLayerNodeSelector.AddtoChildNode(quickSwitchWeaponManuverNodeLeaf);
                upperLayerNodeSelector.AddtoChildNode(moveIdleUpperNodeLeaf);

                performReloadNodeSelector.AddtoChildNode(reloadNodeLeaf);
                performReloadNodeSelector.AddtoChildNode(tacticalReloadNodeLeaf);

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
        reloadNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<ReloadMagazineFullStageNodeLeaf>()
            , animator, "ReloadMagazineFullStage", 1, .3f);
        tacticalReloadNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<TacticalReloadMagazineFullStageNodeLeaf>()
            , animator, "TacticalReloadMagazineFullStage", 1, .3f);

        performGunFuUpperLayerNodeSelector = new NodeSelector(() => isPerformGunFu);
        humanShieldPrimaryStayNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<HumanShield_GunFu_NodeLeaf>() && player._currentWeapon is PrimaryWeapon,
            animator, "HS_Stay_Primary", 1, .25f, .3f);
        humanShieldSecondaryStayNodeLeaf = new PlayAnimationNodeLeaf(
           () => playerStateNodeMnager.TryGetCurNodeLeaf<HumanShield_GunFu_NodeLeaf>() && player._currentWeapon is SecondaryWeapon,
           animator, "HS_Stay_Secondary", 1, .25f, .3f);
        restrictShieldPrimaryStayNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<RestrictGunFuStateNodeLeaf>() && player._currentWeapon is PrimaryWeapon,
            animator, "Restrict_Stay_Primary", 1, .25f, .3f);
        restrictShieldSecondaryStayNodeLeaf = new PlayAnimationNodeLeaf(
           () => playerStateNodeMnager.TryGetCurNodeLeaf<RestrictGunFuStateNodeLeaf>() && player._currentWeapon is SecondaryWeapon,
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

        moveIdleUpperNodeLeaf = new PlayAnimationBaseStateOffsetNodeLeaf(
        () => true,
        animator, "StandWeaponHand LowReady/ADS", 1, 0, .2f);

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
