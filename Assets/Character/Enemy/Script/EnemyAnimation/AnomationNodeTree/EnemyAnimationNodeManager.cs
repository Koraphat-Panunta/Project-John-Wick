using System;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyAnimationManager : INodeManager
{
    #region EnemyBaselayerAnimation
    public INodeSelector startNodeSelector { get; set; }
    public NodeManagerBehavior _nodeManagerBehavior { get; set; }
    protected INodeLeaf curNodeLeaf { get; set; }
    INodeLeaf INodeManager._curNodeLeaf { get => this.curNodeLeaf; set => this.curNodeLeaf = value; }
    public List<INodeManager> _parallelNodeManahger { get; set; }

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
    public PlayPoseAnimationNodeLeaf rifleReloadNodeLeaf { get; set; }
    public PlayPoseAnimationNodeLeaf rifleTacticalReloadNodeLeaf { get; set; }
    public PlayPoseAnimationNodeLeaf pistolReloadNodeLeaf { get; set; }
    public PlayPoseAnimationNodeLeaf pistolTacticalReloadNodeLeaf { get; set; }

    public NodeSelector drawSwitchSelector { get; set; }
    public PlayAnimationNodeLeaf drawPrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf drawSecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf holsterPrimaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf holsterSecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf switchPrimaryToSecondaryNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf swtichSecondaryToPrimaryNodeLeaf { get; set; }

    public PlayAnimationNodeLeaf sprintManuverUpperNodeLeaf { get; set; }

    public NodeSelector weaponHandSelector { get; set; }
    public PlayAnimationBaseStateOffsetNodeLeaf primaryWeaponHandUpperNodeLeaf { get; set; }
    public PlayAnimationBaseStateOffsetNodeLeaf secondaryWeaponHandUpperNodeLeaf { get; set; }

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

        _parallelNodeManahger.Add(upperlayerAnimationNodeManagerProtable);
    }
    
    private void InitializedUpperLayer()
    {
        this.upperlayerAnimationNodeManagerProtable = new NodeManagerPortable();
        this.upperlayerAnimationNodeManagerProtable.InitialzedOuterNode(
            () => 
            {
                this.upperLayerNodeSelector = new NodeSelector(() => isEnableUpperLayer);
                rest_UpperLayerAnimation_NodeLeaf = new RestNodeLeaf(()=> true);

                this.performReloadNodeSelector = new NodeSelector(() => isPerformReload);

                this.rifleReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
                     () => this.enemyWeaponManuver.TryGetCurNodeLeaf<ReloadMagazineFullStageNodeLeaf>()
                     && this.enemy._currentWeapon is AssultRifle_AR15Model
                     , this.animator           
                     , "ReloadMagazine_AR15"           
                     , 1          
                     , .3f          
                     , this.upperAnimationPoseTimeNormalized          
                     , 1         
                     , false);

                this.rifleTacticalReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
                    () => this.enemyWeaponManuver.TryGetCurNodeLeaf<TacticalReloadMagazineFullStageNodeLeaf>()
                    && this.enemy._currentWeapon is AssultRifle_AR15Model
                    , this.animator
                    , "TacticalReloadMagazine_AR15"
                    , 1
                    , .3f
                    , this.upperAnimationPoseTimeNormalized
                    , 1
                    , false);
                this.pistolReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
                    () => this.enemyWeaponManuver.TryGetCurNodeLeaf<ReloadMagazineFullStageNodeLeaf>()
                    && this.enemy._currentWeapon is Glock17_9mm
                    , this.animator
                    , "ReloadMagazine_Glock17"
                    , 1
                    , .3f
                    , this.upperAnimationPoseTimeNormalized
                    , 1
                    , false);
                this.pistolTacticalReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
                    () => this.enemyWeaponManuver.TryGetCurNodeLeaf<TacticalReloadMagazineFullStageNodeLeaf>()
                    && this.enemy._currentWeapon is Glock17_9mm
                    , this.animator
                    , "TacticalReloadMagazine_Glock17"
                    , 1
                    , .3f
                    , this.upperAnimationPoseTimeNormalized
                    , 1
                    , false);


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
                , animator, "SprintWeaponSway", 1, .2f);

                this.weaponHandSelector = new NodeSelector(() => true);

                this.primaryWeaponHandUpperNodeLeaf = new PlayAnimationBaseStateOffsetNodeLeaf(
                    () => this.enemy._currentWeapon != null
                    && this.enemy._currentWeapon is PrimaryWeapon,
                    animator
                    , "PrimaryWeaponHand", 1, 0, .2f);

                this.secondaryWeaponHandUpperNodeLeaf = new PlayAnimationBaseStateOffsetNodeLeaf(
                    () => true
                    , this.animator
                    , "SecondaryWeaponHand", 1, 0, .2f);

                upperLayerNodeSelector.AddtoChildNode(this.performReloadNodeSelector);
                upperLayerNodeSelector.AddtoChildNode(drawSwitchSelector);
                upperLayerNodeSelector.AddtoChildNode(sprintManuverUpperNodeLeaf);
                upperLayerNodeSelector.AddtoChildNode(this.weaponHandSelector);

                this.performReloadNodeSelector.AddtoChildNode(this.rifleReloadNodeLeaf);
                this.performReloadNodeSelector.AddtoChildNode(this.rifleTacticalReloadNodeLeaf);
                this.performReloadNodeSelector.AddtoChildNode(this.pistolReloadNodeLeaf);
                this.performReloadNodeSelector.AddtoChildNode(this.pistolTacticalReloadNodeLeaf);

                this.weaponHandSelector.AddtoChildNode(this.primaryWeaponHandUpperNodeLeaf);
                this.weaponHandSelector.AddtoChildNode(this.secondaryWeaponHandUpperNodeLeaf);

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

        _nodeManagerBehavior.SearchingNewNode(this);
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
        _nodeManagerBehavior.FixedUpdateNode(this);
        upperlayerAnimationNodeManagerProtable.UpdateNode();
        enemyAnimationNodeComponentManager.FixedUpdate();
    }


    public void UpdateNode()
    {
       _nodeManagerBehavior.UpdateNode(this);
        upperlayerAnimationNodeManagerProtable.FixedUpdateNode();
        enemyAnimationNodeComponentManager.Update();
    }
    
}
