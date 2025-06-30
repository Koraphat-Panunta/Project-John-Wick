using NUnit.Framework.Constraints;
using UnityEngine;

public partial class PlayerAnimationManager : INodeManager
{
    public INodeSelector startNodeSelector { get; set; }
    INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager.curNodeLeaf { get => curNodeLeaf; set => curNodeLeaf = value ; }
    public NodeManagerBehavior nodeManagerBehavior { get ; set; }
    public NodeCombine upper_based_LayerCombineNode { get; set; }
    public SetLayerAnimationNodeLeaf enableLayerAnimationNodeLeaf { get; set; }
    public SetLayerAnimationNodeLeaf disableLayerAnimationNodeLeaf { get; set; }

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
    public PlayAnimationNodeLeaf quickSwithcHolsterNodeLeaf { get; set; }

    public PlayAnimationNodeLeaf drawPrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf drawSecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf holsterPrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf holsterSecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf switchPrimaryToSecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf swtichSecondaryToPrimaryNodeLeaf { get; set; }

    public PlayAnimationNodeLeaf sprintUpperNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf moveIdleUpperNodeLeaf { get; set; }

    public NodeSelector basedLayerNodeSelector { get; set; }
    public NodeSelector parkourNodeSelector { get; set; }
    public PlayAnimationNodeLeaf climbLowNodeLeaf { get; set; }
    public NodeSelector gunFuBaseLayerNodeSelector { get; set; }
    public NodeSelector weaponDisarmSelector { get; set; }
    public PlayAnimationNodeLeaf weaponDisarmPrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf weaponDisarmSecondaryNodeLeaf { get; set; }
    public NodeSelector executeNodeSelector { get; set; }
    public PlayAnimationNodeLeaf executePrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf executeSecondaryNodeLeaf { get; set; }
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
    public NodeSelector upperLayerEnableDisableSelector { get; set; }

   
    public void FixedUpdateNode()
    {
        nodeManagerBehavior.FixedUpdateNode(this);
    }

    public void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(()=> true);

        upper_based_LayerCombineNode = new NodeCombine(()=> true);

        this.InitializedUpperLayer();
        this.InitializedBasedLayer();

        upperLayerEnableDisableSelector = new NodeSelector(()=> true);
        enableLayerAnimationNodeLeaf = new SetLayerAnimationNodeLeaf(()=> isEnableUpperLayer
        ,animator,1,3f,1);
        disableLayerAnimationNodeLeaf = new SetLayerAnimationNodeLeaf(() => true
        , animator, 1, 3f, 0);


        startNodeSelector.AddtoChildNode(upper_based_LayerCombineNode);

        upper_based_LayerCombineNode.AddCombineNode(upperLayerEnableDisableSelector);
        upper_based_LayerCombineNode.AddCombineNode(upperLayerNodeSelector);
        upper_based_LayerCombineNode.AddCombineNode(basedLayerNodeSelector);

        upperLayerEnableDisableSelector.AddtoChildNode(enableLayerAnimationNodeLeaf);
        upperLayerEnableDisableSelector.AddtoChildNode(disableLayerAnimationNodeLeaf);

        upperLayerNodeSelector.AddtoChildNode(performReloadNodeSelector);
        upperLayerNodeSelector.AddtoChildNode(performGunFuUpperLayerNodeSelector);
        upperLayerNodeSelector.AddtoChildNode(drawSwitchSelector);
        upperLayerNodeSelector.AddtoChildNode(sprintUpperNodeLeaf);
        upperLayerNodeSelector.AddtoChildNode(moveIdleUpperNodeLeaf);

        performReloadNodeSelector.AddtoChildNode(reloadNodeLeaf);
        performReloadNodeSelector.AddtoChildNode(tacticalReloadNodeLeaf);

        performGunFuUpperLayerNodeSelector.AddtoChildNode(humanShieldPrimaryStayNodeLeaf);
        performGunFuUpperLayerNodeSelector.AddtoChildNode(humanShieldSecondaryStayNodeLeaf);
        performGunFuUpperLayerNodeSelector.AddtoChildNode(restrictShieldPrimaryStayNodeLeaf);
        performGunFuUpperLayerNodeSelector.AddtoChildNode(restrictShieldSecondaryStayNodeLeaf);

        quickSwitchSelector.AddtoChildNode(quickSwitchDrawNodeLeaf);
        quickSwitchSelector.AddtoChildNode(quickSwithcHolsterNodeLeaf);

        drawSwitchSelector.AddtoChildNode(quickSwitchSelector);
        drawSwitchSelector.AddtoChildNode(drawPrimaryNodeLeaf);
        drawSwitchSelector.AddtoChildNode(drawSecondaryNodeLeaf);
        drawSwitchSelector.AddtoChildNode(holsterPrimaryNodeLeaf);
        drawSwitchSelector.AddtoChildNode(holsterSecondaryNodeLeaf);
        drawSwitchSelector.AddtoChildNode(switchPrimaryToSecondaryNodeLeaf);
        drawSwitchSelector.AddtoChildNode(swtichSecondaryToPrimaryNodeLeaf);

        basedLayerNodeSelector.AddtoChildNode(parkourNodeSelector);
        basedLayerNodeSelector.AddtoChildNode(gunFuBaseLayerNodeSelector);
        basedLayerNodeSelector.AddtoChildNode(dodgeNodeLeaf);
        basedLayerNodeSelector.AddtoChildNode(sprintNodeLeaf);
        basedLayerNodeSelector.AddtoChildNode(moveCrouchNodeLeaf);
        basedLayerNodeSelector.AddtoChildNode(moveStandNodeLeaf);

        gunFuBaseLayerNodeSelector.AddtoChildNode(weaponDisarmSelector);
        gunFuBaseLayerNodeSelector.AddtoChildNode(executeNodeSelector);
        gunFuBaseLayerNodeSelector.AddtoChildNode(restrictShieldSelector);
        gunFuBaseLayerNodeSelector.AddtoChildNode(humanThrowNodeLeaf);
        gunFuBaseLayerNodeSelector.AddtoChildNode(humanShieldSelector);
        gunFuBaseLayerNodeSelector.AddtoChildNode(hit1NodeLeaf);
        gunFuBaseLayerNodeSelector.AddtoChildNode(hit2NodeLeaf);
        gunFuBaseLayerNodeSelector.AddtoChildNode(hit3NodeLeaf);
        gunFuBaseLayerNodeSelector.AddtoChildNode(spinKickNodeLeaf);

        executeNodeSelector.AddtoChildNode(executePrimaryNodeLeaf);
        executeNodeSelector.AddtoChildNode(executeSecondaryNodeLeaf);

        restrictShieldSelector.AddtoChildNode(restrictShieldPrimaryEnterNodeLeaf);
        restrictShieldSelector.AddtoChildNode(restrictShieldSecondaryEnterNodeLeaf);
        restrictShieldSelector.AddtoChildNode(restrictShieldExitNodeLeaf);
        restrictShieldSelector.AddtoChildNode(restrictShieldMoveNodeLeaf);

        humanShieldSelector.AddtoChildNode(humanShieldPrimaryEnterNodeLeaf);
        humanShieldSelector.AddtoChildNode(humanShieldSecondaryEnterNodeLeaf);
        humanShieldSelector.AddtoChildNode(humanShieldMoveNodeLeaf);

        weaponDisarmSelector.AddtoChildNode(weaponDisarmPrimaryNodeLeaf);
        weaponDisarmSelector.AddtoChildNode(weaponDisarmSecondaryNodeLeaf);

        parkourNodeSelector.AddtoChildNode(climbLowNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);

    }
    private void InitializedUpperLayer()
    {
        upperLayerNodeSelector = new NodeSelector(() => isEnableUpperLayer);

        performReloadNodeSelector = new NodeSelector(()=> isPerformReload);
        reloadNodeLeaf = new PlayAnimationNodeLeaf(
            ()=> playerWeaponManuverNodeManager.TryGetCurNodeLeaf<ReloadMagazineFullStageNodeLeaf>()
            ,animator, "ReloadMagazineFullStage",1,.3f);
        tacticalReloadNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<TacticalReloadMagazineFullStageNodeLeaf>()
            , animator, "TacticalReloadMagazineFullStage", 1, .3f);

        performGunFuUpperLayerNodeSelector = new NodeSelector(()=> isPerformGunFu);
        humanShieldPrimaryStayNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<HumanShield_GunFuInteraction_NodeLeaf>() && player._currentWeapon is PrimaryWeapon,
            animator, "HS_Stay_Primary",1,.25f,.3f);
        humanShieldSecondaryStayNodeLeaf = new PlayAnimationNodeLeaf(
           () => playerStateNodeMnager.TryGetCurNodeLeaf<HumanShield_GunFuInteraction_NodeLeaf>() && player._currentWeapon is SecondaryWeapon,
           animator, "HS_Stay_Secondary", 1, .25f, .3f);
        restrictShieldPrimaryStayNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<RestrictGunFuStateNodeLeaf>() && player._currentWeapon is PrimaryWeapon,
            animator, "Restrict_Stay_Primary", 1, .25f, .3f);
        restrictShieldSecondaryStayNodeLeaf = new PlayAnimationNodeLeaf(
           () => playerStateNodeMnager.TryGetCurNodeLeaf<RestrictGunFuStateNodeLeaf>() && player._currentWeapon is SecondaryWeapon,
           animator, "Restrict_Stay_Secondary", 1, .25f, .3f);

        drawSwitchSelector = new NodeSelector(()=> isDrawSwitchWeapon);

        quickSwitchSelector = new NodeSelector(()=> playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuickDrawWeaponManuverLeafNodeLeaf>());
        quickSwitchDrawNodeLeaf = new PlayAnimationNodeLeaf(
            ()=> playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuickDrawWeaponManuverLeafNodeLeaf>(out QuickDrawWeaponManuverLeafNodeLeaf quickSwitchNodeLeaf)
            && quickSwitchNodeLeaf.quickDrawPhase == QuickDrawWeaponManuverLeafNodeLeaf.QuickDrawPhase.Draw
            ,animator, "QuickDraw",1,0.1f);
        quickSwithcHolsterNodeLeaf = new PlayAnimationNodeLeaf(
          () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuickDrawWeaponManuverLeafNodeLeaf>(out QuickDrawWeaponManuverLeafNodeLeaf quickSwitchNodeLeaf)
          && quickSwitchNodeLeaf.quickDrawPhase == QuickDrawWeaponManuverLeafNodeLeaf.QuickDrawPhase.HolsterSecondary
          , animator, "QuickHolster", 1, 0.2f);

        drawPrimaryNodeLeaf = new PlayAnimationNodeLeaf(
            ()=> playerWeaponManuverNodeManager.TryGetCurNodeLeaf<DrawPrimaryWeaponManuverNodeLeaf>(),
            animator, "DrawPrimary",1,.2f);
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

        sprintUpperNodeLeaf = new PlayAnimationNodeLeaf(
         () => playerStateNodeMnager.TryGetCurNodeLeaf<PlayerSprintNode>(),
         animator, "SprintWeaponSway", 1, .2f);

        moveIdleUpperNodeLeaf = new PlayAnimationNodeLeaf(
        () =>true,
        animator, "StandWeaponHand LowReady/ADS", 1, .2f);

    } 
    private void InitializedBasedLayer() 
    {
        basedLayerNodeSelector = new NodeSelector(() => true);
        parkourNodeSelector = new NodeSelector(() => playerStateNodeMnager.TryGetCurNodeLeaf<IParkourNodeLeaf>());
        climbLowNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<ClimbParkourNodeLeaf>(out ClimbParkourNodeLeaf climbLowNodeLeaf)
            , animator, "ClimbLow", 0, .23f);

        InitializedGunFuBasedLayer();

        dodgeNodeLeaf = new PlayAnimationNodeLeaf(()=> playerStateNodeMnager.TryGetCurNodeLeaf<PlayerDodgeRollStateNodeLeaf>(),
            animator, "DodgeRoll",0,.2f);
        sprintNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.TryGetCurNodeLeaf<PlayerSprintNode>(),
            animator, "Sprint", 0, .5f);
        moveCrouchNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<PlayerCrouch_Idle_NodeLeaf>() || playerStateNodeMnager.TryGetCurNodeLeaf<PlayerCrouch_Move_NodeLeaf>(),
            animator, "Crouch", 0, .4f);
        moveStandNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<PlayerStandIdleNodeLeaf>() || playerStateNodeMnager.TryGetCurNodeLeaf<PlayerStandMoveNodeLeaf>(),
            animator, "Move/Idle", 0, .3f);
    }
    private void InitializedGunFuBasedLayer() 
    {
        gunFuBaseLayerNodeSelector = new NodeSelector(() => isPerformGunFu);

        weaponDisarmSelector = new NodeSelector(()=>playerStateNodeMnager.TryGetCurNodeLeaf<WeaponDisarm_GunFuInteraction_NodeLeaf>(out WeaponDisarm_GunFuInteraction_NodeLeaf weaponDisarmNodeLeaf));
        weaponDisarmPrimaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<WeaponDisarm_GunFuInteraction_NodeLeaf>(out WeaponDisarm_GunFuInteraction_NodeLeaf weaponDisarmNodeLeaf) 
            && weaponDisarmNodeLeaf.disarmedWeapon is PrimaryWeapon
            , animator, "GunFuPrimaryDisarm", 0, 0.2f);
        weaponDisarmSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<WeaponDisarm_GunFuInteraction_NodeLeaf>(out WeaponDisarm_GunFuInteraction_NodeLeaf weaponDisarmNodeLeaf)
            && weaponDisarmNodeLeaf.disarmedWeapon is SecondaryWeapon
            , animator, "GunFuSecondaryDisarm", 0, 0.2f);

        executeNodeSelector = new NodeSelector(
            ()=> playerStateNodeMnager.TryGetCurNodeLeaf<GunFuExecuteNodeLeaf>());
        executePrimaryNodeLeaf = new PlayAnimationNodeLeaf(
            ()=> player._currentWeapon is PrimaryWeapon,animator, "GunFu_EX_stepOn_Rifle",0,.35f);
        executeSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
            ()=> player._currentWeapon is SecondaryWeapon,animator, "GunFu_EX_Knee",0,.35f);

        restrictShieldSelector = new NodeSelector(() => playerStateNodeMnager.TryGetCurNodeLeaf<RestrictGunFuStateNodeLeaf>());
        restrictShieldPrimaryEnterNodeLeaf = new PlayAnimationNodeLeaf(()=> playerStateNodeMnager.TryGetCurNodeLeaf<RestrictGunFuStateNodeLeaf>(out RestrictGunFuStateNodeLeaf restrictNodeLeaf)
        && (restrictNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Enter)
        && player._currentWeapon is PrimaryWeapon
        ,animator, "Restrict_Enter",0,0.35f);
        restrictShieldSecondaryEnterNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.TryGetCurNodeLeaf<RestrictGunFuStateNodeLeaf>(out RestrictGunFuStateNodeLeaf restrictNodeLeaf)
        && (restrictNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Enter)
        && player._currentWeapon is SecondaryWeapon
        ,animator, "Restrict_Enter", 0,.35f);
        restrictShieldExitNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.TryGetCurNodeLeaf<RestrictGunFuStateNodeLeaf>(out RestrictGunFuStateNodeLeaf restrictNodeLeaf)
        && (restrictNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Exit
        ||restrictNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.ExitAttack)
        , animator, "Restrict_Exit", 0, .35f);
        restrictShieldMoveNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.TryGetCurNodeLeaf<RestrictGunFuStateNodeLeaf>(out RestrictGunFuStateNodeLeaf restrictNodeLeaf)
        && (restrictNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Stay)
        , animator, "Move/Idle", 0, .35f);

        humanThrowNodeLeaf = new PlayAnimationNodeLeaf(
           () => 
           {
               Debug.Log("playerStateNodeMnager.TryGetCurNodeLeaf<HumanThrowGunFuInteractionNodeLeaf>() = " + playerStateNodeMnager.TryGetCurNodeLeaf<HumanThrowGunFuInteractionNodeLeaf>());
               Debug.Log("player curState = " + playerStateNodeMnager.GetCurNodeLeaf());
               if(playerStateNodeMnager.TryGetCurNodeLeaf<HumanThrowGunFuInteractionNodeLeaf>())
                   return true;

               return false;
           }
           , animator, "HS_Exit", 0, .35f);

        humanShieldSelector = new NodeSelector(()=> playerStateNodeMnager.TryGetCurNodeLeaf<HumanShield_GunFuInteraction_NodeLeaf>());
        humanShieldPrimaryEnterNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.TryGetCurNodeLeaf<HumanShield_GunFuInteraction_NodeLeaf>(out HumanShield_GunFuInteraction_NodeLeaf humanShieldNodeLeaf)
        && (humanShieldNodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Enter) 
        && player._currentWeapon is PrimaryWeapon
        , animator, "HS_P_Enter", 0, .35f);
        humanShieldSecondaryEnterNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.TryGetCurNodeLeaf<HumanShield_GunFuInteraction_NodeLeaf>(out HumanShield_GunFuInteraction_NodeLeaf humanShieldNodeLeaf)
        && (humanShieldNodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Enter)
        && player._currentWeapon is SecondaryWeapon
        , animator, "HS_P_Enter", 0, .35f);
        humanShieldMoveNodeLeaf = new PlayAnimationNodeLeaf(() => playerStateNodeMnager.TryGetCurNodeLeaf<HumanShield_GunFuInteraction_NodeLeaf>(out HumanShield_GunFuInteraction_NodeLeaf humanShieldNodeLeaf)
       && (humanShieldNodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Stay || humanShieldNodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
       , animator, "Move/Idle", 0, .35f);

        hit1NodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<Hit1GunFuNode>(),
            animator, "Hit", 0, .1f);
        hit2NodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<Hit2GunFuNode>(),
            animator, "Hit2", 0, .1f);
        hit3NodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<KnockDown_GunFuNode>(),
            animator, "KnockDown", 0, .1f);
        spinKickNodeLeaf = new PlayAnimationNodeLeaf(
            () => playerStateNodeMnager.TryGetCurNodeLeaf<DodgeSpinKicklGunFuNodeLeaf>(),
            animator, "DodgeSpinKick", 0, .1f);
    }

    public void UpdateNode()
    {
       nodeManagerBehavior.UpdateNode(this);
    }
}
