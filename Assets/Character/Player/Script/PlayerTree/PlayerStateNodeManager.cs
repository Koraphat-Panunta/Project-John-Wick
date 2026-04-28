using System.Collections.Generic;
using UnityEngine;
using static Player;

public class PlayerStateNodeManager : 
    INodeManager
    ,INodeNotifyBackAble
{
    private INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager._curNodeLeaf { get; set; }
    public INodeSelector startNodeSelector { get; set; }
    public NodeManagerBehavior _nodeManagerBehavior { get; set; }
    public NodeComponentManager _nodeComponentManager { get; set; }
    public List<INodeManager> _parallelNodeManahger { get; set; }

    public Player player;
    public PlayerStateNodeManager(Player player) 
    { 
        this.player = player;
        this._nodeManagerBehavior = new NodeManagerBehavior();
        this._nodeComponentManager = new NodeComponentManager();
        this._parallelNodeManahger = new List<INodeManager>();
        this.InitailizedNode();
        this.InitializedNodeComponent();
    }
    public void FixedUpdateNode() 
    {
        this._nodeManagerBehavior.FixedUpdateNode(this);
        this._nodeComponentManager.FixedUpdate();
    }

    public void UpdateNode() 
    {
        this._nodeManagerBehavior.UpdateNodeAndCheckFindingNode(this);
        this._nodeComponentManager.Update();
    } 

    #region InitializedNodeState

    public PlayerSelectorStateNode stanceSelectorNode { get; private set; }
    public PlayerDeadNodeLeaf deadNodeLeaf { get; private set; }

    public PlayerSelectorStateNode standSelectorNode { get; private set; }
    public PlayerDodgeRollStateNodeLeaf playerDodgeRollStateNodeLeaf { get; private set; }
    public VaultingNodeLeaf vaultingNodeLeaf { get; private set; }
    public ClimbParkourNodeLeaf climbLowNodeLeaf { get; private set; }
    public ClimbParkourNodeLeaf climbHighNodeLeaf { get; private set; }
    public PlayerFallingStateNodeLeaf fallingStateNodeLeaf { get; private set; }
    public PlayerLandingRollStateNodeLeaf landingRollStateNodeLeaf { get; private set; }
    public PlayerLandingStandStateNodeLeaf landingStandStateNodeLeaf { get; private set; }
    public PlayerSprintNode playerSprintNode { get; private set; }
    public NodeSelector dolphinDiveSelector { get; private set; }
    public ObstacleJumpDolphinDiveNodeLeaf obstacleJumpDolphinDiveNodeLeaf { get; private set; }
    public WallJumpReversDolphinDiveNodeLeaf wallJumpReversDolphinDiveNodeLeaf { get; private set; }
    public WallJumpForwardDolphinDiveNodeLeaf wallJumpForwardDolphinDiveNodeLeaf { get; private set; }
    public PlayerDolphinDiveStateNodeLeaf playerDolphinDiveStateNodeLeaf { get; private set; }
    public PlayerSelectorStateNode standIncoverSelector { get; private set; }
    public PlayerStandIdleNodeLeaf playerStandIdleNode { get; private set; }
    public PlayerStandMoveNodeLeaf playerStandMoveNode { get; private set; }

    public PlayerSelectorStateNode crouchSelectorNode { get; private set; }
    public PlayerCrouch_Move_NodeLeaf playerCrouch_Move_NodeLeaf { get; private set; }
    public PlayerCrouch_Idle_NodeLeaf playerCrouch_Idle_NodeLeaf { get; private set; }
    public PlayerInCoverStandMoveNodeLeaf playerInCoverStandMoveNode { get; private set; }
    public PlayerInCoverStandIdleNodeLeaf playerInCoverStandIdleNode { get; private set; }

    public PlayerSelectorStateNode proneStanceSelector { get; private set; }
    public PlayerProneStateNodeLeaf proneStateNodeLeaf { get; private set; }
    public PlayerGetUpStateNodeLeaf playerGetUpStateNodeLeaf { get; private set; }

    public PlayerPokePickUpWeaponNodeLeaf playerPokePickUpWeaponNodeLeaf { get; private set; }

    public PlayerThrowWeaponNodeLeaf playerThrowWeaponNodeLeaf { get; private set; }

    public PlayerSelectorStateNode gotGunFuAttackSelectorNodeLeaf { get; private set; }
    public PlayerBrounceOffGotAttackGunFuNodeLeaf playerBrounceOffGotAttackGunFuNodeLeaf { get; private set; }

    public NodeSelector executeGunFuSelector { get; set; }

    public NodeSelector gunFuExecute_Single_Secondary_Selector;
    public GunFuExecute_Single_NodeLeaf gunFuExecute_Single_Secondary_NodeLeaf_I { get; set; }

    public NodeSelector gunFuExecute_Single_Primary_Selector;
    public GunFuExecute_Single_NodeLeaf gunFuExecute_Single_Primary_NodeLeaf_I { get; set; }
    public GunFuExecute_Single_NodeLeaf gunFuExecute_Single_Primary_Dodge_NodeLeaf_I { get; set; }
    public GunFuExecute_Single_NodeLeaf gunFuExecute_Single_Secondary_Dodge_NodeLeaf_I { get; set; }
    public NodeSelector executeGunFuOnGroundSelector { get; set; }
    public GunFuExecute_Single_NodeLeaf gunFuExecute_OnGround { get; protected set; }

    public NodeSelector triggerHitGunFuSelector { get; private set; }
    public GunFuHitDownNodeLeaf hitDownNodeLeaf { get; private set; }
    public GunFuHitNodeLeaf hit1gunFuNodeLeaf { get; private set; }
    public GunFuReloadNodeLeaf gunFuReloadNodeLeaf { get; private set; }
    public HumanShield_GunFu_NodeLeaf humanShield_GunFuInteraction_NodeLeaf { get; private set; }
    public HumanShieldExit_GunFu_NodeLeaf humanShieldExit_GunFu_NodeLeaf { get; private set; }
    public RestrainGunFuStateNodeLeaf restrictGunFuStateNodeLeaf { get; private set; }
    public GunFuHitNodeLeaf Hit2GunFuNodeLeaf { get; private set; }
    public GunFuHitNodeLeaf Hit3GunFuNodeLeaf { get; private set; }
    public GunFuHitNodeLeaf dodgeSpinKicklGunFuNodeLeaf { get; private set; }

    public void InitailizedNode()
    {
        startNodeSelector = new PlayerSelectorStateNode(this.player, () => true);

        deadNodeLeaf = new PlayerDeadNodeLeaf(this.player, () => player.isDead);

        stanceSelectorNode = new PlayerSelectorStateNode(this.player,
            () => { return true; });
        playerDodgeRollStateNodeLeaf = new PlayerDodgeRollStateNodeLeaf(player,
            () =>
            this.player.triggerDodgeRoll
            && this.player.inputMoveDir_World.magnitude > 0
            && this.player.staminaGauge.CompareValue_Greater_Equal_ThanGauge(this.player.playerStatsScriptableObject.dodgeStaminaDrain)
            );
        vaultingNodeLeaf = new VaultingNodeLeaf(player,
            () => player._isParkourCommand && player.isSprint, player._movementCompoent, player.vaultingScrp);
        climbLowNodeLeaf = new ClimbParkourNodeLeaf(player,
            ()=>player._isParkourCommand,player._movementCompoent,player.climbLowScrp);
        climbHighNodeLeaf = new ClimbParkourNodeLeaf(player,
            () => player._isParkourCommand, player._movementCompoent, player.climbHighScrp);

        this.fallingStateNodeLeaf = new PlayerFallingStateNodeLeaf
            (this.player,this,this.player._movementCompoent as PlayerMovement
            ,()=> this.player.playerMovement.isProximityInAir  );
        this.landingRollStateNodeLeaf = new PlayerLandingRollStateNodeLeaf
            (this.player, this.player.playerMovement, .75f, this.player.StandMoveMaxSpeed
            , () => this.fallingStateNodeLeaf.isComplete && this.fallingStateNodeLeaf.fallingVelocity >= 10);
        this.landingStandStateNodeLeaf = new PlayerLandingStandStateNodeLeaf
            (this.player, this.player.playerMovement, .34f, .66f
            , () => this.fallingStateNodeLeaf.isComplete && true);

        standSelectorNode = new PlayerSelectorStateNode(this.player,
            () => { return this.player.stanceCommand == Stance.stand || player.isSprint; });
        this.playerSprintNode = new PlayerSprintNode(this.player,this, () => this.player.isSprint && player.inputMoveDir_World.magnitude > 0 );

        this.dolphinDiveSelector = new NodeSelector(
            () => this.player.triggerDodgeRoll);

        this.obstacleJumpDolphinDiveNodeLeaf = new ObstacleJumpDolphinDiveNodeLeaf(this.player
            , () => true
            && this.player.staminaGauge.CompareValue_Greater_Equal_ThanGauge(this.player.playerStatsScriptableObject.dolphinDiveStaminaDrain));

        this.wallJumpReversDolphinDiveNodeLeaf = new WallJumpReversDolphinDiveNodeLeaf(this.player
            ,()=> true
            && this.player.staminaGauge.CompareValue_Greater_Equal_ThanGauge(this.player.playerStatsScriptableObject.dolphinDiveStaminaDrain));

        this.wallJumpForwardDolphinDiveNodeLeaf = new WallJumpForwardDolphinDiveNodeLeaf(this.player
            , () => true
            && this.player.staminaGauge.CompareValue_Greater_Equal_ThanGauge(this.player.playerStatsScriptableObject.dolphinDiveStaminaDrain));

        this.playerDolphinDiveStateNodeLeaf = new PlayerDolphinDiveStateNodeLeaf(this.player
            , () => true
            && this.player.staminaGauge.CompareValue_Greater_Equal_ThanGauge(this.player.playerStatsScriptableObject.dolphinDiveStaminaDrain));

        standIncoverSelector = new PlayerSelectorStateNode(this.player,
            () => { return this.player.isInCover; });

        playerStandMoveNode = new PlayerStandMoveNodeLeaf(this.player,
            () => { return this.player.inputMoveDir_Local.magnitude > 0; });

        playerStandIdleNode = new PlayerStandIdleNodeLeaf(this.player,
            () => true);


        playerInCoverStandMoveNode = new PlayerInCoverStandMoveNodeLeaf(this.player,
            () => { return this.player.inputMoveDir_Local.magnitude > 0; });

        playerInCoverStandIdleNode = new PlayerInCoverStandIdleNodeLeaf(this.player,
            () => true);


        crouchSelectorNode = new PlayerSelectorStateNode(this.player,
            () => this.player.stanceCommand == Stance.crouch);

        playerCrouch_Move_NodeLeaf = new PlayerCrouch_Move_NodeLeaf(this.player,
           () => this.player.inputMoveDir_Local.magnitude > 0);

        playerCrouch_Idle_NodeLeaf = new PlayerCrouch_Idle_NodeLeaf(this.player,
            () => this.player.inputMoveDir_Local.magnitude <= 0 || true);


        this.proneStanceSelector = new PlayerSelectorStateNode(this.player, 
            () => this.player.stanceCommand == Stance.prone);
        this.proneStateNodeLeaf = new PlayerProneStateNodeLeaf(this.player,this
            ,()=> true);
        this.playerGetUpStateNodeLeaf = new PlayerGetUpStateNodeLeaf( this.player, 
            () => (this.player.inputMoveDir_World.magnitude > 0 && this.player.isSprint) || (this.player.triggerDodgeRoll));

        playerPokePickUpWeaponNodeLeaf = new PlayerPokePickUpWeaponNodeLeaf(
            this.player, this.player.pokePickUpAnimationSCRP, this.player.humanoidBone._rightFootBone,
            () => (player._isInteractCommand == true || player.commandBufferManager.TryGetCommand(nameof(player._isInteractCommand)))
            && this.player.currentInteractable is Weapon 
            && player._weaponManuverManager.isPickingUpWeaponManuverAble
            );

        playerThrowWeaponNodeLeaf = new PlayerThrowWeaponNodeLeaf(this.player
            , this.player.throwObjectAnimationTriggerEventSCRP,
            () => player._isTriggerThrowCommand 
            && player._currentWeapon != null
            );

        gotGunFuAttackSelectorNodeLeaf = new PlayerSelectorStateNode(this.player, 
            () => player._triggerHitedGunFu);
        playerBrounceOffGotAttackGunFuNodeLeaf = new PlayerBrounceOffGotAttackGunFuNodeLeaf( this.player,
            () => player.curAttackerGunFuNode is EnemySpinKickGunFuNodeLeaf);

        executeGunFuSelector = new NodeSelector(
            ()=> player._triggerExecuteGunFu
            && player.executeGauge._gauge >= this.player.executeGauge.maxGauge
            && player.executedAbleGunFu != null
            && player._currentWeapon != null
            && player._currentWeapon.chamber.isReadyShoot );
        executeGunFuOnGroundSelector = new NodeSelector(
            () => player.executedAbleGunFu._character is IRagdollAble downGetUpAble 
            && downGetUpAble._isFallDown);

        gunFuExecute_Single_Primary_Dodge_NodeLeaf_I = new GunFuExecute_Single_NodeLeaf(player,
            () => (player._triggerExecuteGunFu
            && player.executeGauge._gauge >= this.player.executeGauge.maxGauge
            && player.executedAbleGunFu != null
            && player._currentWeapon != null
            && player._currentWeapon.chamber.isReadyShoot 
            && player._currentWeapon is PrimaryWeapon
            && (player.executedAbleGunFu._character as IRagdollAble)._isFallDown == false)
            , player.gunFuExecute_Single_Primary_Dodge_ScriptableObject_I
            ,GunFuExecuteStateName.GunFu_Execute_Dodge_Primary
            );
        gunFuExecute_Single_Secondary_Dodge_NodeLeaf_I = new GunFuExecute_Single_NodeLeaf(player,
            ()=> (player._triggerExecuteGunFu
            && player.executeGauge._gauge >= this.player.executeGauge.maxGauge
            && player.executedAbleGunFu != null
            && player._currentWeapon != null
            && player._currentWeapon.chamber.isReadyShoot
            && player._currentWeapon is SecondaryWeapon
            && (player.executedAbleGunFu._character as IRagdollAble)._isFallDown == false)
            ,player.gunFuExecute_Single_Secondary_Dodge_ScriptableObject_I
            ,GunFuExecuteStateName.GunFu_Execute_Dodge_Secondary
            );

        gunFuExecute_Single_Primary_Selector = new NodeSelector(
            () => player._currentWeapon is PrimaryWeapon);

        gunFuExecute_Single_Primary_NodeLeaf_I = new GunFuExecute_Single_NodeLeaf(
            player,
            () => true
            , player.gunFuExecute_Single_Primary_ScriptableObject_I
            ,GunFuExecuteStateName.GunFu_Execute_Single_Primary_I
            );

        gunFuExecute_Single_Secondary_Selector = new NodeSelector(
            ()=> player._currentWeapon is SecondaryWeapon);

        gunFuExecute_Single_Secondary_NodeLeaf_I = new GunFuExecute_Single_NodeLeaf(
            player,
            () => true
            , player.gunFuExecute_Single_Secondary_ScriptableObject_I
            ,GunFuExecuteStateName.GunFu_Execute_Single_Secondary_I
            );
       

        gunFuExecute_OnGround = new GunFuExecute_Single_NodeLeaf(player,
            () => 
            {
                
                if (
                player.executedAbleGunFu._character is IRagdollAble downGetUpAble
                && downGetUpAble._isFallDown 
                )
                    return true;
                return false;
            }
            ,this.player.gunFu_Single_Execute_OnGround
            ,GunFuExecuteStateName.GunFu_Single_Execute_OnGround
            );
       
       

        this.triggerHitGunFuSelector = new NodeSelector(
            () =>this.player.attackedAbleGunFu != null
            && this.player.attackedAbleGunFu._character.isDead == false
            && (this.player._triggerGunFu || player.commandBufferManager.TryGetCommand(nameof(player._triggerGunFu)))
            && this.player.staminaGauge.CompareValue_Greater_Equal_ThanGauge(this.player.playerStatsScriptableObject.dodgeStaminaDrain));
        this.hitDownNodeLeaf = new GunFuHitDownNodeLeaf(this.player,this.player.gunFuHitDownScriptableObject,this.player.hit1
            ,() =>  this.player.attackedAbleGunFu != null
            && this.player._triggerGunFu
            && this.player.attackedAbleGunFu._character.stance == Stance.prone
            && this.player.attackedAbleGunFu._character.isDead == false);

        this.hit1gunFuNodeLeaf = new GunFuHitNodeLeaf(this.player,
            () => true 
            && this.player.staminaGauge.CompareValue_Greater_Equal_ThanGauge(this.player.playerStatsScriptableObject.dodgeStaminaDrain)
            //&& this.player.attackedAbleGunFu != null
            //&& this.player.attackedAbleGunFu._character.isDead == false
            , this.player.hit1);

        this.gunFuReloadNodeLeaf = new GunFuReloadNodeLeaf(this.player,
            () => this.player.attackedAbleGunFu != null
            && (this.player._isReloadCommand || this.player.commandBufferManager.TryGetCommand(nameof(this.player._isReloadCommand)))
            && this.player.attackedAbleGunFu._character.isDead == false
            && this.player.staminaGauge._gauge > 0
            ,this.player.gunFuReloadScripatableObject
            );

        this.restrictGunFuStateNodeLeaf = new RestrainGunFuStateNodeLeaf(player.restrictScriptableObject, player,
            () =>
            {
                if (player._isAimingCommand
                && this.player.attackedAbleGunFu != null
                && this.player.attackedAbleGunFu._character.stance != Stance.prone)
                {
                    if (player._currentWeapon != null
                    && this.player.staminaGauge.CompareValue_Greater_Equal_ThanGauge(this.player.playerStatsScriptableObject.restrainHumanShieldStaminaDrain))
                        return true;
                }
                return false;
            });

        humanShield_GunFuInteraction_NodeLeaf = new HumanShield_GunFu_NodeLeaf(this.player,
            () => this.player._isAimingCommand
            && this.player.attackedAbleGunFu != null
            && this.player.attackedAbleGunFu._character.stance != Stance.prone
            && this.player.attackedAbleGunFu._character.isDead == false
            && this.player.staminaGauge.CompareValue_Greater_Equal_ThanGauge(this.player.playerStatsScriptableObject.restrainHumanShieldStaminaDrain)
            , this.player.humanShieldSCRP
            ,this.player.humanShieldTargetAdjustTransform);

        this.humanShieldExit_GunFu_NodeLeaf = new HumanShieldExit_GunFu_NodeLeaf(this.player
            ,this.player.humanShield_Exit_SCRP
            ,() => true);
        
        Hit2GunFuNodeLeaf = new GunFuHitNodeLeaf(this.player, 
            () => (this.player._triggerGunFu || this.player.commandBufferManager.TryGetCommand(nameof(player._triggerGunFu)))
            && this.player.attackedAbleGunFu != null
            && this.player.attackedAbleGunFu._character.stance != Stance.prone
            && this.player.staminaGauge.CompareValue_Greater_Equal_ThanGauge(this.player.playerStatsScriptableObject.HitStaminaDrain)
            , this.player.hit2);
        Hit3GunFuNodeLeaf = new GunFuHitNodeLeaf(this.player, 
            () => 
            {

                if((this.player._triggerGunFu 
                || this.player.commandBufferManager.TryGetCommand(nameof(player._triggerGunFu)))
                && this.player.attackedAbleGunFu != null
                && this.player.attackedAbleGunFu._character.stance != Stance.prone
                && this.player.staminaGauge.CompareValue_Greater_Equal_ThanGauge(this.player.playerStatsScriptableObject.HitStaminaDrain))
                    return true;

                else return false;
            } 
        , this.player.hit3);
        dodgeSpinKicklGunFuNodeLeaf = new GunFuHitNodeLeaf(this.player, 
            () => (this.player._triggerGunFu || this.player.commandBufferManager.TryGetCommand(nameof(player._triggerGunFu)))
            && this.player.staminaGauge.CompareValue_Greater_Equal_ThanGauge(this.player.playerStatsScriptableObject.HitStaminaDrain)
       , player.dodgeSpinKick);


        startNodeSelector.AddtoChildNode(deadNodeLeaf);
        startNodeSelector.AddtoChildNode(stanceSelectorNode);

        stanceSelectorNode.AddtoChildNode(gotGunFuAttackSelectorNodeLeaf);
        stanceSelectorNode.AddtoChildNode(vaultingNodeLeaf);
        stanceSelectorNode.AddtoChildNode(climbHighNodeLeaf);
        stanceSelectorNode.AddtoChildNode(climbLowNodeLeaf);
        stanceSelectorNode.AddtoChildNode(this.fallingStateNodeLeaf);
        stanceSelectorNode.AddtoChildNode(playerDodgeRollStateNodeLeaf);
        stanceSelectorNode.AddtoChildNode(executeGunFuSelector);
        stanceSelectorNode.AddtoChildNode(this.triggerHitGunFuSelector);
        stanceSelectorNode.AddtoChildNode(playerThrowWeaponNodeLeaf);
        stanceSelectorNode.AddtoChildNode(playerPokePickUpWeaponNodeLeaf);
        stanceSelectorNode.AddtoChildNode(this.proneStanceSelector);
        stanceSelectorNode.AddtoChildNode(standSelectorNode);
        stanceSelectorNode.AddtoChildNode(crouchSelectorNode);


        this.fallingStateNodeLeaf.AddTransitionNode(this.landingRollStateNodeLeaf);
        this.fallingStateNodeLeaf.AddTransitionNode(this.landingStandStateNodeLeaf);
// 
        standSelectorNode.AddtoChildNode(playerSprintNode);
        standSelectorNode.AddtoChildNode(playerStandMoveNode);
        standSelectorNode.AddtoChildNode(playerStandIdleNode);

        this.playerSprintNode.AddTransitionNode(this.dolphinDiveSelector);

        this.dolphinDiveSelector.AddtoChildNode(this.obstacleJumpDolphinDiveNodeLeaf);
        this.dolphinDiveSelector.AddtoChildNode(this.wallJumpReversDolphinDiveNodeLeaf);
        this.dolphinDiveSelector.AddtoChildNode(this.wallJumpForwardDolphinDiveNodeLeaf);
        this.dolphinDiveSelector.AddtoChildNode(this.playerDolphinDiveStateNodeLeaf);

        playerDodgeRollStateNodeLeaf.AddTransitionNode(this.hitDownNodeLeaf);
        playerDodgeRollStateNodeLeaf.AddTransitionNode(dodgeSpinKicklGunFuNodeLeaf);
        playerDodgeRollStateNodeLeaf.AddTransitionNode(gunFuExecute_Single_Primary_Dodge_NodeLeaf_I);
        playerDodgeRollStateNodeLeaf.AddTransitionNode(gunFuExecute_Single_Secondary_Dodge_NodeLeaf_I);

        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(this.gunFuReloadNodeLeaf);
        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(executeGunFuSelector);
        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(restrictGunFuStateNodeLeaf);
        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(Hit2GunFuNodeLeaf);

        gotGunFuAttackSelectorNodeLeaf.AddtoChildNode(playerBrounceOffGotAttackGunFuNodeLeaf);

        this.triggerHitGunFuSelector.AddtoChildNode(this.hitDownNodeLeaf);
        this.triggerHitGunFuSelector.AddtoChildNode(this.hit1gunFuNodeLeaf);

        this.hitDownNodeLeaf.AddTransitionNode(this.gunFuReloadNodeLeaf);
        this.hitDownNodeLeaf.AddTransitionNode(this.restrictGunFuStateNodeLeaf);

        this.hit1gunFuNodeLeaf.AddTransitionNode(this.executeGunFuSelector);
        this.hit1gunFuNodeLeaf.AddTransitionNode(this.gunFuReloadNodeLeaf);
        this.hit1gunFuNodeLeaf.AddTransitionNode(this.Hit2GunFuNodeLeaf);
        this.hit1gunFuNodeLeaf.AddTransitionNode(this.restrictGunFuStateNodeLeaf);

        this.Hit2GunFuNodeLeaf.AddTransitionNode(this.executeGunFuSelector);
        this.Hit2GunFuNodeLeaf.AddTransitionNode(this.gunFuReloadNodeLeaf);
        this.Hit2GunFuNodeLeaf.AddTransitionNode(this.Hit3GunFuNodeLeaf);
        this.Hit2GunFuNodeLeaf.AddTransitionNode(this.humanShield_GunFuInteraction_NodeLeaf);

        this.humanShield_GunFuInteraction_NodeLeaf.AddTransitionNode(this.gunFuReloadNodeLeaf);
        this.humanShield_GunFuInteraction_NodeLeaf.AddTransitionNode(this.humanShieldExit_GunFu_NodeLeaf);

        this.restrictGunFuStateNodeLeaf.AddTransitionNode(this.gunFuReloadNodeLeaf);
        this.restrictGunFuStateNodeLeaf.AddTransitionNode(this.Hit3GunFuNodeLeaf);

        crouchSelectorNode.AddtoChildNode(playerCrouch_Move_NodeLeaf);
        crouchSelectorNode.AddtoChildNode(playerCrouch_Idle_NodeLeaf);

        this.proneStanceSelector.AddtoChildNode(this.proneStateNodeLeaf);
        this.proneStateNodeLeaf.AddTransitionNode(this.playerGetUpStateNodeLeaf);

        executeGunFuSelector.AddtoChildNode(executeGunFuOnGroundSelector);
        executeGunFuSelector.AddtoChildNode(gunFuExecute_Single_Secondary_Selector);
        executeGunFuSelector.AddtoChildNode(gunFuExecute_Single_Primary_Selector);

        gunFuExecute_Single_Primary_Selector.AddtoChildNode(gunFuExecute_Single_Primary_NodeLeaf_I);

        gunFuExecute_Single_Secondary_Selector.AddtoChildNode(gunFuExecute_Single_Secondary_NodeLeaf_I);

        executeGunFuOnGroundSelector.AddtoChildNode(gunFuExecute_OnGround);

        _nodeManagerBehavior.SearchingNewNode(this);
    }
    #endregion

    #region NodeComponent
    public RegenarateGaugeNodeLeaf regenarateHPNodeLeaf;
    public RegenarateGaugeNodeLeaf regenarateStaminaNodeLeaf;
    public RegenarateGaugeNodeLeaf regenarateExecuteGaugeNodeLeaf;
    private void InitializedNodeComponent()
    {
        this.regenarateHPNodeLeaf = new RegenarateGaugeNodeLeaf
            (
            ()=> this.player.GetHP() < (this.player.GetMaxHp() * 0.5f) 
            && this.player.isDead == false
            ,this.player._hpGauge
            , this.player.GetMaxHp() * 0.5f
            ,20);
        this.regenarateHPNodeLeaf.SubcribeNotifyBack(this);

        this.regenarateStaminaNodeLeaf = new RegenarateGaugeNodeLeaf
            (
            ()=> this.player.staminaGauge._gauge < this.player.staminaGauge.maxGauge
            && (
            (this as INodeManager).GetCurNodeLeaf() is RestrainGunFuStateNodeLeaf
            || (this as INodeManager).GetCurNodeLeaf() is HumanShield_GunFu_NodeLeaf
            || (this as INodeManager).GetCurNodeLeaf() is GunFuHitDownNodeLeaf
            || (this as INodeManager).GetCurNodeLeaf() is GunFuReloadNodeLeaf
            ) == false
            ,this.player.staminaGauge
            ,this.player.staminaGauge.maxGauge
            ,50
            );

        this.regenarateExecuteGaugeNodeLeaf = new RegenarateGaugeNodeLeaf
            (
            () => true,
            this.player.executeGauge
            , this.player.executeGauge.maxGauge
            , 2
            );


        this._nodeComponentManager.AddNode(this.regenarateHPNodeLeaf);
        this._nodeComponentManager.AddNode(this.regenarateStaminaNodeLeaf);
        this._nodeComponentManager.AddNode(this.regenarateExecuteGaugeNodeLeaf);
    }
    #endregion
    public void ChangeNode(PlayerStateNodeLeaf playerStateNodeLeaf)
    {
        curNodeLeaf.Exit();
        curNodeLeaf = playerStateNodeLeaf;
        curNodeLeaf.Enter();
    }

    public void OnNotifyBack<T>(INode node, T var)
    {
        if(node is RegenarateGaugeNodeLeaf regenHpNodeLeaf 
            && regenHpNodeLeaf.isDelay == false)
        {
            this.player.NotifyObserver(this.player, SubjectPlayer.NotifyEvent.HealthRegen);
        }
    }
}
