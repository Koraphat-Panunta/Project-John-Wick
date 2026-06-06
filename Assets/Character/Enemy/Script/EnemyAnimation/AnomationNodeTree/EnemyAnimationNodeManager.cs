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

    public PlayAnimationNodeLeaf blockAnimationNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf gotParriedNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf meleeAttackMoveAnimationNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf gotKnockDownNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf gotGunFuReloadNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf gotHitedDownNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf gotRestrictEnterNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf gotRestrictExitNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf gotMeleeExecuteAnimationNodeLeaf { get; set; }
    public PlayPoseAnimationNodeLeaf painStateAnimationNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf enemySpinKick { get; set; }
    public PlayAnimationNodeLeaf enemyNormalHit { get; set; }
    public PlayAnimationNodeLeaf enemyEvadeNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf enemyDodgeNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf sprintBaseLayerNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf crouchBaseLayerNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf standMoveIdleBaseLayerNodeLeaf { get; set; }
    private RestNodeLeaf rest_BaseLayerAnimation_NodeLeaf { get; set; }

    private void InitializedBaseLayer()
    {
        this.startNodeSelector = new NodeSelector(() => true);

        this.blockAnimationNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.enemyStateManager.TryGetCurNodeLeaf<BlockStateNodeLeaf>()
            , this.animator
            , "Block"
            , 0
            , .2f);
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
        this.gotKnockDownNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.enemyStateManager.TryGetCurNodeLeaf<GotGunFuInteractingNodeLeaf>(out GotGunFuInteractingNodeLeaf interactingNodeLeaf)
            && interactingNodeLeaf == this.enemy.enemyStateManagerNode.gotKnockDown_OCM_NodeLeaf
            , this.animator, "GotKnockDown", 0, 0);
        this.gotGunFuReloadNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.enemyStateManager.TryGetCurNodeLeaf<GotGunFuInteractingNodeLeaf>(out GotGunFuInteractingNodeLeaf interactingNodeLeaf)
            && interactingNodeLeaf == this.enemy.enemyStateManagerNode.gotGunFuReloadNodeLeaf
            , this.animator, "GotGunFuReload", 0, 0);
        this.gotHitedDownNodeLeaf = new PlayAnimationNodeLeaf(
            ()=> this.enemyStateManager.TryGetCurNodeLeaf<GotGunFuInteractingNodeLeaf>(out GotGunFuInteractingNodeLeaf interactingNodeLeaf)
            && interactingNodeLeaf == this.enemy.enemyStateManagerNode.gotHitDownNodeLeaf
            ,this.animator,"GotHitedDown", 0,0);

        this.gotRestrictEnterNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.enemyStateManager.TryGetCurNodeLeaf<GotRestraintNodeLeaf>(out GotRestraintNodeLeaf n)
                  && (n.curRestrainPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Enter || n.curRestrainPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Stay),
            this.animator,
            "GotRestrain_Enter",
            0, 0.075f,
            0);

        

        this.gotRestrictExitNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.enemyStateManager.TryGetCurNodeLeaf<GotRestraintNodeLeaf>(out GotRestraintNodeLeaf n)
                  && (n.curRestrainPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Exit ),
            this.animator,
            "GotRestrain_Exit",
            0, 0.075f,
            0);

        this.gotMeleeExecuteAnimationNodeLeaf = new PlayAnimationNodeLeaf(
            () => this.enemyStateManager.TryGetCurNodeLeaf<GotGunFuInteractingNodeLeaf>(out GotGunFuInteractingNodeLeaf interactingNodeLeaf)
            && interactingNodeLeaf == this.enemy.enemyStateManagerNode.gotMeleeExecuteNodeLeaf
            , this.animator, "Got_MeleeExecute_I", 0, 0);

        this.painStateAnimationNodeLeaf = new PlayPoseAnimationNodeLeaf(
            () => (enemyStateManager.TryGetCurNodeLeaf<GotGunFuHitNodeLeaf>()
            || this.enemyStateManager.TryGetCurNodeLeaf<EnemyPainStateNodeLeaf>())
            , this.animator, "PainState", 0,this.basedAnimationPoseTimeNormalized,.1f
            ,this.painStatePoseAnimationSCRP);

        this.enemySpinKick = new PlayAnimationNodeLeaf(
            () => enemyStateManager.TryGetCurNodeLeaf<Enemy_OCM_Hit_NodeLeaf>(out Enemy_OCM_Hit_NodeLeaf n)
                  && n == this.enemy.enemyStateManagerNode.enemy_OCM_Heavy_hit_NodeLeaf
            , animator, "OCM_SpinKick", 0, .15f);

        this.enemyNormalHit = new PlayAnimationNodeLeaf(
            () => enemyStateManager.TryGetCurNodeLeaf<Enemy_OCM_Hit_NodeLeaf>(out Enemy_OCM_Hit_NodeLeaf n)
                  && n == this.enemy.enemyStateManagerNode.enemy_OCM_Normal_hit_NodeLeaf
            , animator, "OCM_Normal_Attack", 0, .15f);

        this.sprintBaseLayerNodeLeaf = new PlayAnimationNodeLeaf(
            () => enemyStateManager.TryGetCurNodeLeaf<EnemySprintStateNodeLeaf>()
            , animator, "Sprint", 0, 0.25f);

        this.enemyEvadeNodeLeaf = new PlayAnimationNodeLeaf(
            () => enemyStateManager.TryGetCurNodeLeaf<EnemyDodgeStateNodeLeaf>(out EnemyDodgeStateNodeLeaf enemyEvadeStateNode)
            && enemyEvadeStateNode == this.enemy.enemyStateManagerNode.evadeStateNodeLeaf
            , animator, "Evade", 0, 0.2f);

        this.enemyDodgeNodeLeaf = new PlayAnimationNodeLeaf(
            () => enemyStateManager.TryGetCurNodeLeaf<EnemyDodgeStateNodeLeaf>()
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

        this.startNodeSelector.AddtoChildNode(this.blockAnimationNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.gotParriedNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.meleeAttackMoveAnimationNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.gotMeleeExecuteAnimationNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.gotKnockDownNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.gotGunFuReloadNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.gotHitedDownNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.gotRestrictEnterNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.gotRestrictExitNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.painStateAnimationNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.enemySpinKick);
        this.startNodeSelector.AddtoChildNode(this.enemyNormalHit);
        this.startNodeSelector.AddtoChildNode(this.sprintBaseLayerNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.enemyDodgeNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.crouchBaseLayerNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.standMoveIdleBaseLayerNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.rest_BaseLayerAnimation_NodeLeaf);

        _nodeManagerBehavior.SearchingNewNode(this);
    }

    #endregion

    #region UpperBodyLayer
    public NodeManagerPortable upperBodyLayerNodeManagerPortable { get; private set; }

    public NodeSelector upperLayerNodeSelector { get; set; }
    public RestNodeLeaf rest_UpperLayerAnimation_NodeLeaf { get; set; }

    public NodeSelector shotgunReloadNodeSelector { get; set; }
    public PlayAnimationNodeLeaf chamberloadShotgunNodeLeaf { get; set; }
    public PlayAnimationNodeLeaf preLoadShotgunNodeLeaf { get; set; }
    public PlayPoseAnimationNodeLeaf quadloadShotgunNodeLeaf { get; set; }
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

    private void InitializedUpperBodyLayer()
    {
        this.upperBodyLayerNodeManagerPortable = new NodeManagerPortable();
        this.upperBodyLayerNodeManagerPortable.InitialzedOuterNode(
            () =>
            {
                this.upperLayerNodeSelector = new NodeSelector(() => isEnableUpperBodyLayer);

                this.performReloadNodeSelector = new NodeSelector(() => isPerformReload);

                this.shotgunReloadNodeSelector = new NodeSelector(
                    () => this.enemyWeaponManuver.TryGetCurNodeLeaf<IShotgunReloadNode>());

                this.chamberloadShotgunNodeLeaf = new PlayAnimationNodeLeaf(
                    () => this.enemyWeaponManuver.TryGetCurNodeLeaf<ChamberLoadShotgunNodeLeaf>()
                    && this.enemy._currentWeapon is AutomaticShotgunModel,
                    this.animator, "ChamberLoadShotgun", 1, .1f);

                this.preLoadShotgunNodeLeaf = new PlayAnimationNodeLeaf(
                    () => this.enemyWeaponManuver.TryGetCurNodeLeaf<PreloadNodeLeaf>()
                    && this.enemy._currentWeapon is AutomaticShotgunModel,
                    this.animator, "PreLoadShotgun", 1, .1f);

                this.quadloadShotgunNodeLeaf = new PlayPoseAnimationNodeLeaf(
                    () => this.enemyWeaponManuver.TryGetCurNodeLeaf<QuardLoadNodeLeaf>()
                    && this.enemy._currentWeapon is AutomaticShotgunModel,
                    this.animator, "QuadLoadShotgun", 1, .1f, this.upperBodyAnimationPoseTimeNormalized, 1, true);

                this.rifleReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
                    () => this.enemyWeaponManuver.TryGetCurNodeLeaf<ReloadMagazineFullStageNodeLeaf>()
                    && this.enemy._currentWeapon is AssultRifle_AR15Model,
                    this.animator, "ReloadMagazine_AR15", 1, .3f, this.upperBodyAnimationPoseTimeNormalized, 1, false);

                this.rifleTacticalReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
                    () => this.enemyWeaponManuver.TryGetCurNodeLeaf<TacticalReloadMagazineFullStageNodeLeaf>()
                    && this.enemy._currentWeapon is AssultRifle_AR15Model,
                    this.animator, "TacticalReloadMagazine_AR15", 1, .3f, this.upperBodyAnimationPoseTimeNormalized, 1, false);

                this.pistolReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
                    () => this.enemyWeaponManuver.TryGetCurNodeLeaf<ReloadMagazineFullStageNodeLeaf>()
                    && this.enemy._currentWeapon is Glock17_9mm,
                    this.animator, "ReloadMagazine_Glock17", 1, .3f, this.upperBodyAnimationPoseTimeNormalized, 1, false);

                this.pistolTacticalReloadNodeLeaf = new PlayPoseAnimationNodeLeaf(
                    () => this.enemyWeaponManuver.TryGetCurNodeLeaf<TacticalReloadMagazineFullStageNodeLeaf>()
                    && this.enemy._currentWeapon is Glock17_9mm,
                    this.animator, "TacticalReloadMagazine_Glock17", 1, .3f, this.upperBodyAnimationPoseTimeNormalized, 1, false);

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

                upperLayerNodeSelector.AddtoChildNode(this.performReloadNodeSelector);
                upperLayerNodeSelector.AddtoChildNode(drawSwitchSelector);

                this.performReloadNodeSelector.AddtoChildNode(this.shotgunReloadNodeSelector);
                this.performReloadNodeSelector.AddtoChildNode(this.rifleReloadNodeLeaf);
                this.performReloadNodeSelector.AddtoChildNode(this.rifleTacticalReloadNodeLeaf);
                this.performReloadNodeSelector.AddtoChildNode(this.pistolReloadNodeLeaf);
                this.performReloadNodeSelector.AddtoChildNode(this.pistolTacticalReloadNodeLeaf);

                this.shotgunReloadNodeSelector.AddtoChildNode(this.chamberloadShotgunNodeLeaf);
                this.shotgunReloadNodeSelector.AddtoChildNode(this.preLoadShotgunNodeLeaf);
                this.shotgunReloadNodeSelector.AddtoChildNode(this.quadloadShotgunNodeLeaf);

                drawSwitchSelector.AddtoChildNode(drawPrimaryNodeLeaf);
                drawSwitchSelector.AddtoChildNode(drawSecondaryNodeLeaf);
                drawSwitchSelector.AddtoChildNode(holsterPrimaryNodeLeaf);
                drawSwitchSelector.AddtoChildNode(holsterSecondaryNodeLeaf);

                this.upperBodyLayerNodeManagerPortable.startNodeSelector.AddtoChildNode(this.upperLayerNodeSelector);
            });
    }

    #endregion

    #region UpperArmLayer
    public NodeManagerPortable upperArmLayerNodeManagerPortable { get; private set; }
    public NodeSelector upperArmLayerNodeSelector { get; set; }
    public NodeSelector weaponHandSelector { get; set; }
    public PlayAnimationBaseStateOffsetNodeLeaf primaryWeaponHandUpperNodeLeaf { get; set; }
    public PlayAnimationBaseStateOffsetNodeLeaf secondaryWeaponHandUpperNodeLeaf { get; set; }
    public PlayAnimationBaseStateOffsetNodeLeaf holdMeleeWeaponNodeLeaf { get; set; }

    private void InitializedUpperArmLayer()
    {
        upperArmLayerNodeManagerPortable = new NodeManagerPortable();
        upperArmLayerNodeManagerPortable.InitialzedOuterNode(
            () =>
            {
                upperArmLayerNodeSelector = new NodeSelector(() => this.isEnableUpperArmLayer);

                primaryWeaponHandUpperNodeLeaf = new PlayAnimationBaseStateOffsetNodeLeaf(
                    () => this.enemy._currentWeapon != null && this.enemy._currentWeapon is PrimaryWeapon,
                    animator, "PrimaryWeaponHand", 2, 0, .2f);
                holdMeleeWeaponNodeLeaf = new PlayAnimationBaseStateOffsetNodeLeaf(
                    () => this.enemy._curMeleeWeapon != null,
                    animator, "Hold_Melee_Weapon", 2, 0, .2f);
                secondaryWeaponHandUpperNodeLeaf = new PlayAnimationBaseStateOffsetNodeLeaf(
                    () => true,
                    this.animator, "SecondaryWeaponHand", 2, 0, .2f);

                weaponHandSelector = new NodeSelector(() => true);
                weaponHandSelector.AddtoChildNode(primaryWeaponHandUpperNodeLeaf);
                weaponHandSelector.AddtoChildNode(holdMeleeWeaponNodeLeaf);
                weaponHandSelector.AddtoChildNode(secondaryWeaponHandUpperNodeLeaf);

                upperArmLayerNodeSelector.AddtoChildNode(weaponHandSelector);
                upperArmLayerNodeManagerPortable.startNodeSelector.AddtoChildNode(upperArmLayerNodeSelector);
            });
    }

    #endregion

    #region EnemyAnimationNodeComponent
    public NodeComponentManager enemyAnimationNodeComponentManager { get; private set; }
    public NodeSelector layerUpperEnableDisableSelector { get; set; }
    public SetLayerAnimationNodeLeaf enableUpperLayer { get; set; }
    public SetLayerAnimationNodeLeaf disableUpperLayer { get; set; }

    public NodeSelector layerUpperArmEnableDisableSelector { get; set; }
    public SetLayerAnimationNodeLeaf enableUpperArmLayer { get; set; }
    public SetLayerAnimationNodeLeaf disableUpperArmLayer { get; set; }

    public CrouchWeightSoftCoverNodeLeaf crouchWeightSoftCoverNodeLeaf { get; set; }

    private void InitializedNodeComponent()
    {
        this.enemyAnimationNodeComponentManager = new NodeComponentManager();

        layerUpperEnableDisableSelector = new NodeSelector(() => true);
        enableUpperLayer = new SetLayerAnimationNodeLeaf(
            () => isEnableUpperBodyLayer, animator, 1, 4f, 1);
        disableUpperLayer = new SetLayerAnimationNodeLeaf(
            () => true, animator, 1, 5f, 0);
        layerUpperEnableDisableSelector.AddtoChildNode(enableUpperLayer);
        layerUpperEnableDisableSelector.AddtoChildNode(disableUpperLayer);

        layerUpperArmEnableDisableSelector = new NodeSelector(() => true);
        enableUpperArmLayer = new SetLayerAnimationNodeLeaf(
            () => isEnableUpperArmLayer, animator, 2, 4f, 1);
        disableUpperArmLayer = new SetLayerAnimationNodeLeaf(
            () => true, animator, 2, 5f, 0);
        layerUpperArmEnableDisableSelector.AddtoChildNode(enableUpperArmLayer);
        layerUpperArmEnableDisableSelector.AddtoChildNode(disableUpperArmLayer);

        crouchWeightSoftCoverNodeLeaf = new CrouchWeightSoftCoverNodeLeaf(enemy, 0.725f, 6,
            () => enemyStateManager.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>()
            || enemyStateManager.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>());

        this.enemyAnimationNodeComponentManager.AddNode(layerUpperEnableDisableSelector);
        this.enemyAnimationNodeComponentManager.AddNode(layerUpperArmEnableDisableSelector);
        this.enemyAnimationNodeComponentManager.AddNode(crouchWeightSoftCoverNodeLeaf);
    }
    #endregion

    public void InitailizedNode()
    {
        InitializedBaseLayer();
        InitializedUpperBodyLayer();
        InitializedUpperArmLayer();
        InitializedNodeComponent();

        _parallelNodeManahger.Add(upperBodyLayerNodeManagerPortable);
        _parallelNodeManahger.Add(upperArmLayerNodeManagerPortable);
    }
    private void Start()
    {
        _nodeManagerBehavior.UpdateNodeAndCheckFindingNode(this);
    }
    public void FixedUpdateNode()
    {
        _nodeManagerBehavior.FixedUpdateNode(this);
        upperBodyLayerNodeManagerPortable.FixedUpdateNode();
        upperArmLayerNodeManagerPortable.FixedUpdateNode();
        enemyAnimationNodeComponentManager.FixedUpdate();
    }


    public void UpdateNode()
    {
        upperBodyLayerNodeManagerPortable.UpdateNode();
        upperArmLayerNodeManagerPortable.UpdateNode();
        enemyAnimationNodeComponentManager.Update();
    }

    protected virtual void OnNotifyAnimationNode<T>(Enemy enemy,T var)
    {
        _nodeManagerBehavior.UpdateNodeAndCheckFindingNode(this);
    }
    
}
