using System;
using UnityEngine;

public partial class EnemyAnimationManager : INodeManager
{
    public INodeSelector startNodeSelector { get; set; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }
    protected INodeLeaf curNodeLeaf { get; set; }
    INodeLeaf INodeManager.curNodeLeaf { get => this.curNodeLeaf; set => this.curNodeLeaf = value; }

    public NodeCombine enemyAnimationCombineNode { get; set; }
    public NodeSelector layerUpperEnableDisableSelector { get; set; }
    public SetLayerAnimationNodeLeaf enableUpperLayer { get; set; }
    public SetLayerAnimationNodeLeaf disableUpperLayer { get; set; }
    public CrouchWeightSoftCoverNodeLeaf crouchWeightSoftCoverNodeLeaf { get; set; }

    public NodeSelector upperLayerNodeSelector { get; set; }
    public NodeSelector performReloadNodeSelector { get; set; }
    public PlayAnimationNodeLeaf reloadNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf tacticalReloadNodeLeaf { get; set; }

    public NodeSelector drawSwitchSelector { get; set; }
    public PlayAnimationNodeLeaf drawPrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf drawSecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf holsterPrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf holsterSecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf switchPrimaryToSecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf swtichSecondaryToPrimaryNodeLeaf { get; set; }

    public PlayAnimationNodeLeaf sprintManuverUpperNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf lowReady_ADS_WeaponManuverModeLeaf { get; set; }

    public NodeSelector baseLayerSelector { get; set; }
    public PlayAnimationNodeLeaf enemySpinKick { get; set; }
    public PlayAnimationNodeLeaf enemyDodgeNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf sprintBaseLayerNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf crouchBaseLayerNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf standMoveIdleBaseLayerNodeLeaf { get; set; }
    private RestNodeLeaf restBaseNodeLeaf { get; set; }
    public void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(()=>true);

        enemyAnimationCombineNode = new NodeCombine(()=> true);

        layerUpperEnableDisableSelector = new NodeSelector(()=> true);
        enableUpperLayer = new SetLayerAnimationNodeLeaf(()=> isEnableUpperLayer
        ,animator,1,4f,1);
        disableUpperLayer = new SetLayerAnimationNodeLeaf(() => true
        , animator, 1, 5f, 0);

        InitializedUpperLayer();
        InitializedBaseLayer();

        crouchWeightSoftCoverNodeLeaf = new CrouchWeightSoftCoverNodeLeaf(enemy,0.65f,5.5f,
            ()=> enemyStateManager.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>() || enemyStateManager.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>());

        restBaseNodeLeaf = new RestNodeLeaf(()=>true);

        startNodeSelector.AddtoChildNode(enemyAnimationCombineNode);

        enemyAnimationCombineNode.AddCombineNode(layerUpperEnableDisableSelector);
        enemyAnimationCombineNode.AddCombineNode(upperLayerNodeSelector);
        enemyAnimationCombineNode.AddCombineNode(baseLayerSelector);
        enemyAnimationCombineNode.AddCombineNode(crouchWeightSoftCoverNodeLeaf);

        layerUpperEnableDisableSelector.AddtoChildNode(enableUpperLayer);
        layerUpperEnableDisableSelector.AddtoChildNode(disableUpperLayer);

        upperLayerNodeSelector.AddtoChildNode(performReloadNodeSelector);
        upperLayerNodeSelector.AddtoChildNode(drawSwitchSelector);
        upperLayerNodeSelector.AddtoChildNode(sprintManuverUpperNodeLeaf);
        upperLayerNodeSelector.AddtoChildNode(lowReady_ADS_WeaponManuverModeLeaf);

        performReloadNodeSelector.AddtoChildNode(reloadNodeLeaf);
        performReloadNodeSelector.AddtoChildNode(tacticalReloadNodeLeaf);

        drawSwitchSelector.AddtoChildNode(drawPrimaryNodeLeaf);
        drawSwitchSelector.AddtoChildNode(drawSecondaryNodeLeaf);
        drawSwitchSelector.AddtoChildNode(holsterPrimaryNodeLeaf);
        drawSwitchSelector.AddtoChildNode(holsterSecondaryNodeLeaf);
        drawSwitchSelector.AddtoChildNode(switchPrimaryToSecondaryNodeLeaf);
        drawSwitchSelector.AddtoChildNode(swtichSecondaryToPrimaryNodeLeaf);

        baseLayerSelector.AddtoChildNode(enemySpinKick);
        baseLayerSelector.AddtoChildNode(sprintBaseLayerNodeLeaf);
        baseLayerSelector.AddtoChildNode(enemyDodgeNodeLeaf);
        baseLayerSelector.AddtoChildNode(crouchBaseLayerNodeLeaf);
        baseLayerSelector.AddtoChildNode(standMoveIdleBaseLayerNodeLeaf);
        baseLayerSelector.AddtoChildNode(restBaseNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }
    
    private void InitializedUpperLayer()
    {
        upperLayerNodeSelector = new NodeSelector(() => isEnableUpperLayer);
        performReloadNodeSelector = new NodeSelector(() => isPerformReload);
        reloadNodeLeaf = new PlayAnimationNodeLeaf(() => enemyWeaponManuver.TryGetCurNodeLeaf<ReloadMagazineFullStageNodeLeaf>()
        , animator, "ReloadMagazineFullStage", 1, 0.1f);
        tacticalReloadNodeLeaf = new PlayAnimationNodeLeaf(() => enemyWeaponManuver.TryGetCurNodeLeaf<TacticalReloadMagazineFullStageNodeLeaf>()
        , animator, "TacticalReloadMagazineFullStage", 1, 0.1f);

        drawSwitchSelector = new NodeSelector(() => isDrawSwitchWeapon);
        drawPrimaryNodeLeaf = new PlayAnimationNodeLeaf(
            () => enemyWeaponManuver.TryGetCurNodeLeaf<DrawPrimaryWeaponManuverNodeLeaf>(),
            animator, "DrawPrimary", 1, .2f);
        drawSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
           () => enemyWeaponManuver.TryGetCurNodeLeaf<DrawSecondaryWeaponManuverNodeLeaf>(),
           animator, "DrawSecondary", 1, .2f);
        holsterPrimaryNodeLeaf = new PlayAnimationNodeLeaf(
           () => enemyWeaponManuver.TryGetCurNodeLeaf<HolsterPrimaryWeaponManuverNodeLeaf>(),
           animator, "HolsterPrimary", 1, .2f);
        holsterSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
         () => enemyWeaponManuver.TryGetCurNodeLeaf<HolsterSecondaryWeaponManuverNodeLeaf>(),
         animator, "HolsterSecondary", 1, .2f);
        switchPrimaryToSecondaryNodeLeaf = new PlayAnimationNodeLeaf(
         () => enemyWeaponManuver.TryGetCurNodeLeaf<PrimaryToSecondarySwitchWeaponManuverLeafNode>(),
         animator, "SwitchWeaponPrimary -> Secondary", 1, .2f);
        swtichSecondaryToPrimaryNodeLeaf = new PlayAnimationNodeLeaf(
         () => enemyWeaponManuver.TryGetCurNodeLeaf<SecondaryToPrimarySwitchWeaponManuverLeafNode>(),
         animator, "SwitchWeaponSecondary -> Primary", 1, .2f);

        sprintManuverUpperNodeLeaf = new PlayAnimationNodeLeaf(() => enemyStateManager.TryGetCurNodeLeaf<EnemySprintStateNodeLeaf>()
        , animator, "SprintWeaponSway", 0, .2f);
        lowReady_ADS_WeaponManuverModeLeaf = new PlayAnimationNodeLeaf(() => true
        , animator, "StandWeaponHand LowReady/ADS", 1, 0.2f);
    }
    private void InitializedBaseLayer()
    {
        baseLayerSelector = new NodeSelector(() => true);
        enemySpinKick = new PlayAnimationNodeLeaf(
            () => enemyStateManager.TryGetCurNodeLeaf<EnemySpinKickGunFuNodeLeaf>()
            , animator, "EnemySpinKick", 0, .15f);
        enemyDodgeNodeLeaf = new PlayAnimationNodeLeaf(()=> enemyStateManager.TryGetCurNodeLeaf<EnemyDodgeRollStateNodeLeaf>()
            , animator, "Dodge", 0, 0.2f);
        sprintBaseLayerNodeLeaf = new PlayAnimationNodeLeaf(
            () => enemyStateManager.TryGetCurNodeLeaf<EnemySprintStateNodeLeaf>()
            , animator, "Sprint", 0, 0.25f);
        crouchBaseLayerNodeLeaf = new PlayAnimationNodeLeaf(
            () => enemyStateManager.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>() 
            || enemyStateManager.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>()
            , animator, "Crouch", 0, .2f);
        standMoveIdleBaseLayerNodeLeaf = new PlayAnimationNodeLeaf(
            () => enemyStateManager.TryGetCurNodeLeaf<EnemyStandIdleStateNodeLeaf>() 
            || enemyStateManager.TryGetCurNodeLeaf<EnemyStandMoveStateNodeLeaf>()
            || enemyStateManager.TryGetCurNodeLeaf<EnemyStandTakeAimStateNodeLeaf>()
            , animator, "Move/Idle", 0, .2f);
    }
    public void FixedUpdateNode()
    {
        nodeManagerBehavior.FixedUpdateNode(this);
    }


    public void UpdateNode()
    {
       nodeManagerBehavior.UpdateNode(this);
    }
    private class RestNodeLeaf : AnimationNodeLeaf
    {
        public RestNodeLeaf(Func<bool> preCondition) : base(preCondition)
        {
        }
    }
}
