using System;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyAnimationManager : INodeManager
{
    #region EnemyBaselayerAnimation
    public INodeSelector startNodeSelector { get; set; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }
    protected INodeLeaf curNodeLeaf { get; set; }
    INodeLeaf INodeManager._curNodeLeaf { get => this.curNodeLeaf; set => this.curNodeLeaf = value; }
    public List<INodeManager> parallelNodeManahger { get; set; }
    public PlayAnimationNodeLeaf enemySpinKick { get; set; }
    public PlayAnimationNodeLeaf enemyDodgeNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf sprintBaseLayerNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf crouchBaseLayerNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf standMoveIdleBaseLayerNodeLeaf { get; set; }
    private RestNodeLeaf rest_BaseLayerAnimation_NodeLeaf { get; set; }
    #endregion

    #region UpperLayer
    public NodeManagerPortable upperlayerAnimationNodeManagerProtable;

    public NodeSelector upperLayerNodeSelector { get; set; }
    public RestNodeLeaf rest_UpperLayerAnimation_NodeLeaf { get; set; }

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
    #endregion

    #region EnemyAnimationNodeComponent
    public NodeComponentManager enemyAnimationNodeComponentManager { get; private set; }
    public NodeSelector layerUpperEnableDisableSelector { get; set; }
    public SetLayerAnimationNodeLeaf enableUpperLayer { get; set; }
    public SetLayerAnimationNodeLeaf disableUpperLayer { get; set; }
    public CrouchWeightSoftCoverNodeLeaf crouchWeightSoftCoverNodeLeaf { get; set; }


    #endregion

    public void InitailizedNode()
    {
        InitializedUpperLayer();
        InitializedBaseLayer();
        InitializedNodeComponent();

        parallelNodeManahger.Add(upperlayerAnimationNodeManagerProtable);
    }
    
    private void InitializedUpperLayer()
    {
        this.upperlayerAnimationNodeManagerProtable = new NodeManagerPortable();
        this.upperlayerAnimationNodeManagerProtable.InitialzedOuterNode(
            () => 
            {
                upperLayerNodeSelector = new NodeSelector(() => isEnableUpperLayer);
                rest_UpperLayerAnimation_NodeLeaf = new RestNodeLeaf(()=> true);

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

                this.upperlayerAnimationNodeManagerProtable.startNodeSelector.AddtoChildNode(this.upperLayerNodeSelector);
                this.upperlayerAnimationNodeManagerProtable.startNodeSelector.AddtoChildNode(this.rest_UpperLayerAnimation_NodeLeaf);

            });

    
    }
    private void InitializedBaseLayer()
    {
        startNodeSelector = new NodeSelector(() => true);

        enemySpinKick = new PlayAnimationNodeLeaf(
            () => enemyStateManager.TryGetCurNodeLeaf<EnemySpinKickGunFuNodeLeaf>()
            , animator, "EnemySpinKick", 0, .15f);
        enemyDodgeNodeLeaf = new PlayAnimationNodeLeaf(
            ()=> enemyStateManager.TryGetCurNodeLeaf<EnemyDodgeRollStateNodeLeaf>()
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
        rest_BaseLayerAnimation_NodeLeaf = new RestNodeLeaf(
            () => true);

        startNodeSelector.AddtoChildNode(enemySpinKick);
        startNodeSelector.AddtoChildNode(sprintBaseLayerNodeLeaf);
        startNodeSelector.AddtoChildNode(enemyDodgeNodeLeaf);
        startNodeSelector.AddtoChildNode(crouchBaseLayerNodeLeaf);
        startNodeSelector.AddtoChildNode(standMoveIdleBaseLayerNodeLeaf);
        startNodeSelector.AddtoChildNode(rest_BaseLayerAnimation_NodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }
    private void InitializedNodeComponent()
    {
        this.enemyAnimationNodeComponentManager = new NodeComponentManager();

        layerUpperEnableDisableSelector = new NodeSelector(
            () => true);
        enableUpperLayer = new SetLayerAnimationNodeLeaf(
            () => isEnableUpperLayer
            , animator, 1, 4f, 1);
        disableUpperLayer = new SetLayerAnimationNodeLeaf(
            () => true
            , animator, 1, 5f, 0);

        crouchWeightSoftCoverNodeLeaf = new CrouchWeightSoftCoverNodeLeaf(enemy, 0.725f, 6,
            () => enemyStateManager.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>()
            || enemyStateManager.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>());

        layerUpperEnableDisableSelector.AddtoChildNode(enableUpperLayer);
        layerUpperEnableDisableSelector.AddtoChildNode(disableUpperLayer);

        this.enemyAnimationNodeComponentManager.AddNode(layerUpperEnableDisableSelector);
        this.enemyAnimationNodeComponentManager.AddNode(crouchWeightSoftCoverNodeLeaf);

    }
    public void FixedUpdateNode()
    {
        nodeManagerBehavior.FixedUpdateNode(this);
        upperlayerAnimationNodeManagerProtable.UpdateNode();
        enemyAnimationNodeComponentManager.FixedUpdate();
    }


    public void UpdateNode()
    {
       nodeManagerBehavior.UpdateNode(this);
        upperlayerAnimationNodeManagerProtable.FixedUpdateNode();
        enemyAnimationNodeComponentManager.Update();
    }
    
}
