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

    public PlayAnimationNodeLeaf gotParriedNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf meleeAttackMoveAnimationNodeLeaf { get; set; } 
    public PlayAnimationNodeLeaf gotGunFuReloadNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf gotHitedDownNodeLeaf { get; set; }
    public PlayPoseAnimationNodeLeaf painStateAnimationNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf enemySpinKick { get; set; }
    public PlayAnimationNodeLeaf enemyDodgeNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf sprintBaseLayerNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf crouchBaseLayerNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf standMoveIdleBaseLayerNodeLeaf { get; set; }
    private RestNodeLeaf rest_BaseLayerAnimation_NodeLeaf { get; set; }

    private void InitializedBaseLayer()
    {
        this.startNodeSelector = new NodeSelector(() => true);

        this.gotParriedNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.enemyStateManager.TryGetCurNodeLeaf<GotParriedNodeLeaf>(out GotParriedNodeLeaf attackMoveMeleeWeaponNodeLeaf)
            , this.animator
            , "Got_Parried_I"
            , 0
            , .2f);
        this.meleeAttackMoveAnimationNodeLeaf = new PlayAnimationNodeLeaf(
            ()=> this.enemyStateManager.TryGetCurNodeLeaf<AttackMoveMeleeWeaponNodeLeaf>(out AttackMoveMeleeWeaponNodeLeaf attackMoveMeleeWeaponNodeLeaf)
            ,this.animator
            ,"MeleeAttack"
            ,0
            ,.2f);
        this.gotGunFuReloadNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.enemyStateManager.TryGetCurNodeLeaf<GotGunFuInteractingNodeLeaf>(out GotGunFuInteractingNodeLeaf interactingNodeLeaf)
            && interactingNodeLeaf == this.enemy.enemyStateManagerNode.gotGunFuReloadNodeLeaf
            , this.animator, "GotGunFuReload", 0, 0);
        this.gotHitedDownNodeLeaf = new PlayAnimationNodeLeaf(
            ()=> this.enemyStateManager.TryGetCurNodeLeaf<GotGunFuInteractingNodeLeaf>(out GotGunFuInteractingNodeLeaf interactingNodeLeaf)
            && interactingNodeLeaf == this.enemy.enemyStateManagerNode.gotHitDownNodeLeaf
            ,this.animator,"GotHitedDown", 0,0);
        this.painStateAnimationNodeLeaf = new PlayPoseAnimationNodeLeaf(
            () => (enemyStateManager.TryGetCurNodeLeaf<GotGunFuHitNodeLeaf>()
            || this.enemyStateManager.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>())
            , this.animator, "PainState", 0,this.basedAnimationPoseTimeNormalized,0f
            ,this.painStatePoseAnimationSCRP);

        this.enemySpinKick = new PlayAnimationNodeLeaf(
            () => enemyStateManager.TryGetCurNodeLeaf<EnemySpinKickGunFuNodeLeaf>()
            , animator, "EnemySpinKick", 0, .15f);

        this.sprintBaseLayerNodeLeaf = new PlayAnimationNodeLeaf(
            () => enemyStateManager.TryGetCurNodeLeaf<EnemySprintStateNodeLeaf>()
            , animator, "Sprint", 0, 0.25f);

        this.enemyDodgeNodeLeaf = new PlayAnimationNodeLeaf(
            () => enemyStateManager.TryGetCurNodeLeaf<EnemyDodgeRollStateNodeLeaf>()
            , animator, "Dodge", 0, 0.2f);

        this.crouchBaseLayerNodeLeaf = new PlayAnimationNodeLeaf(
            () => enemyStateManager.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>()
            || enemyStateManager.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>()
            , animator, "Crouch", 0, .2f);

        this.standMoveIdleBaseLayerNodeLeaf = new PlayAnimationNodeLeaf(
            () => enemyStateManager.TryGetCurNodeLeaf<EnemyStandIdleStateNodeLeaf>()
            || enemyStateManager.TryGetCurNodeLeaf<EnemyStandMoveStateNodeLeaf>()
            , animator, "Move/Idle", 0, .2f);

        this.rest_BaseLayerAnimation_NodeLeaf = new RestNodeLeaf(
            () => true);

        this.startNodeSelector.AddtoChildNode(this.gotParriedNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.meleeAttackMoveAnimationNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.gotGunFuReloadNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.gotHitedDownNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.painStateAnimationNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.enemySpinKick);
        this.startNodeSelector.AddtoChildNode(this.sprintBaseLayerNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.enemyDodgeNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.crouchBaseLayerNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.standMoveIdleBaseLayerNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.rest_BaseLayerAnimation_NodeLeaf);

        _nodeManagerBehavior.SearchingNewNode(this);
    }

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

    private void InitializedUpperLayer()
    {
        this.upperlayerAnimationNodeManagerProtable = new NodeManagerPortable();
        this.upperlayerAnimationNodeManagerProtable.InitialzedOuterNode(
            () =>
            {
                this.upperLayerNodeSelector = new NodeSelector(() => isEnableUpperLayer);
                rest_UpperLayerAnimation_NodeLeaf = new RestNodeLeaf(() => true);

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

    #endregion

    #region EnemyAnimationNodeComponent
    public NodeComponentManager enemyAnimationNodeComponentManager { get; private set; }
    public NodeSelector layerUpperEnableDisableSelector { get; set; }
    public SetLayerAnimationNodeLeaf enableUpperLayer { get; set; }
    public SetLayerAnimationNodeLeaf disableUpperLayer { get; set; }
    public CrouchWeightSoftCoverNodeLeaf crouchWeightSoftCoverNodeLeaf { get; set; }

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
    #endregion

    public void InitailizedNode()
    {
        InitializedBaseLayer();
        InitializedUpperLayer();
        InitializedNodeComponent();

        _parallelNodeManahger.Add(upperlayerAnimationNodeManagerProtable);
    }
    private void Start()
    {
        _nodeManagerBehavior.UpdateNodeAndCheckFindingNode(this);
    }
    public void FixedUpdateNode()
    {
        _nodeManagerBehavior.FixedUpdateNode(this);
        upperlayerAnimationNodeManagerProtable.FixedUpdateNode();
        enemyAnimationNodeComponentManager.FixedUpdate();
    }


    public void UpdateNode()
    {
        upperlayerAnimationNodeManagerProtable.UpdateNode();
        enemyAnimationNodeComponentManager.Update();
    }

    protected virtual void OnNotifyAnimationNode<T>(Enemy enemy,T var)
    {
        _nodeManagerBehavior.UpdateNodeAndCheckFindingNode(this);
    }
    
}
