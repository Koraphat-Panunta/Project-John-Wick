using UnityEngine;

public partial class PlayerAnimationManager : INodeManager
{
    public INodeSelector startNodeSelector { get; set; }
    private INodeLeaf curNodeLeaf;
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
    public NodeSelector gunFuBaseLayerNodeSelector { get; set; }
    public NodeSelector executeNodeSelector { get; set; }
    public PlayAnimationNodeLeaf executePrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf executeSecondaryNodeLeaf { get; set; }

    public NodeSelector restrictShieldSelector { get; set; }
    public PlayAnimationNodeLeaf restrictShieldPrimaryEnterNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf restrictShieldSecondaryEnterNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf restrictShieldExitNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf restrictShieldMoveNodeLeaf { get; set; }

    public NodeSelector humanShieldSelector { get; set; }
    public PlayAnimationNodeLeaf humanShieldPrimaryEnterNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf humanShieldSecondaryEnterNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf humanShieldExitNodeLeaf { get; set; }
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

    public void Awake()
    {
        nodeManagerBehavior = new NodeManagerBehavior();
    }
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
        quickSwitchDrawNodeLeaf = new PlayAnimationNodeLeaf(
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
           () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<DrawSecondaryWeaponManuverNodeLeaf>(),
           animator, "HolsterPrimary", 1, .2f);
        holsterSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
         () => playerWeaponManuverNodeManager.TryGetCurNodeLeaf<DrawSecondaryWeaponManuverNodeLeaf>(),
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
        InitializedGunFuBasedLayer();

    }
    private void InitializedGunFuBasedLayer() 
    {
        gunFuBaseLayerNodeSelector = new NodeSelector(() => isPerformGunFu);

        executeNodeSelector = new NodeSelector(
            ()=> playerStateNodeMnager.TryGetCurNodeLeaf<GunFuExecuteNodeLeaf>());
        executePrimaryNodeLeaf = new PlayAnimationNodeLeaf(
            ()=> player._currentWeapon is PrimaryWeapon,animator, "GunFu_EX_stepOn_Rifle",0,.35f);
        executeSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
            ()=> player._currentWeapon is SecondaryWeapon,animator, "GunFu_EX_Knee",0,0.35f);


    }

    public void UpdateNode()
    {
       nodeManagerBehavior.UpdateNode(this);
    }
}
