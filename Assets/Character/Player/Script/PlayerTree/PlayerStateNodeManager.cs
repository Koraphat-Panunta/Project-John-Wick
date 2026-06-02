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
    public PlayerSprintChangeDirectionNode playerSprintChangeDirectionNode { get; private set; }
    public PlayerSlideNodeLeaf playerSlideNodeLeaf { get; private set; }
    public NodeSelector dolphinDiveSelector { get; private set; }
    public ObstacleJumpDolphinDiveNodeLeaf obstacleJumpDolphinDiveNodeLeaf { get; private set; }
    public WallJumpReversDolphinDiveNodeLeaf wallJumpReversDolphinDiveNodeLeaf { get; private set; }
    public WallJumpForwardDolphinDiveNodeLeaf wallJumpForwardDolphinDiveNodeLeaf { get; private set; }
    public PlayerDolphinDiveStateNodeLeaf playerDolphinDiveStateNodeLeaf { get; private set; }

    public PlayerStandIdleNodeLeaf playerStandIdleNode { get; private set; }
    public PlayerStandMoveNodeLeaf playerStandMoveNode { get; private set; }

    public PlayerSelectorStateNode crouchSelectorNode { get; private set; }
    public PlayerCrouch_Move_NodeLeaf playerCrouch_Move_NodeLeaf { get; private set; }
    public PlayerCrouch_Idle_NodeLeaf playerCrouch_Idle_NodeLeaf { get; private set; }

    public PlayerSelectorStateNode proneStanceSelector { get; private set; }
    public PlayerProneStateNodeLeaf proneStateNodeLeaf { get; private set; }
    public PlayerGetUpStateNodeLeaf playerGetUpStateNodeLeaf { get; private set; }

    public PlayerPokePickUpWeaponNodeLeaf playerPokePickUpWeaponNodeLeaf { get; private set; }

    public PlayerThrowWeaponNodeLeaf playerThrowWeaponNodeLeaf { get; private set; }

    public PlayerSelectorStateNode PainStateSelectorNodeLeaf { get; private set; }
    public PlayerBrounceOffNodeLeaf playerBrounceOffNodeLeaf { get; private set; }

    public NodeSelector executeGunFuSelector { get; set; }

    public NodeSelector gunFuExecute_Single_Secondary_Selector;
    public OCM_Execute_Single_NodeLeaf gunFuExecute_Single_Secondary_NodeLeaf_I { get; set; }

    public NodeSelector gunFuExecute_Single_Primary_Selector;
    public OCM_Execute_Single_NodeLeaf gunFuExecute_Single_Primary_NodeLeaf_II { get; set; }
    public OCM_Execute_Single_NodeLeaf gunFuExecute_Single_Primary_Dodge_NodeLeaf_I { get; set; }
    public OCM_Execute_Single_NodeLeaf gunFuExecute_Single_Secondary_Dodge_NodeLeaf_I { get; set; }

    public QuickShootRangeWeaponNodeLeaf quickShootRangeWeaponNodeLeaf { get; set; }

    public NodeSelector executeGunFuOnGroundSelector { get; set; }
    public OCM_Execute_Single_NodeLeaf gunFuExecute_OnGround { get; protected set; }

    public ParryNodeLeaf parryNodeLeaf { get; private set; }

    public NodeSelector triggerHitGunFuSelector { get; private set; }
    public GunFuHitDownNodeLeaf hitDownNodeLeaf { get; private set; }
    public GunFuHitNodeLeaf hit1gunFuNodeLeaf { get; private set; }
    public OCM_KnockDown_NodeLeaf ocmKnockDownNodeLeaf { get; private set; }
    public OCMReloadNodeLeaf gunFuReloadNodeLeaf { get; private set; }
    public HumanShield_GunFu_NodeLeaf humanShield_GunFuInteraction_NodeLeaf { get; private set; }
    public HumanShieldExit_GunFu_NodeLeaf humanShieldExit_GunFu_NodeLeaf { get; private set; }
    public RestrainGunFuStateNodeLeaf restrainGunFuStateNodeLeaf { get; private set; }
    public GunFuHitNodeLeaf Hit2GunFuNodeLeaf { get; private set; }
    public GunFuHitNodeLeaf Hit3GunFuNodeLeaf { get; private set; }
    public GunFuHitNodeLeaf dodgeSpinKicklGunFuNodeLeaf { get; private set; }
    public MeleeExecute_NodeLeaf meleeExecuteNodeLeaf_I { get; private set; }

    public void InitailizedNode()
    {
        startNodeSelector = new PlayerSelectorStateNode(this.player, () => true);

        deadNodeLeaf = new PlayerDeadNodeLeaf(this.player, () => player.isDead);

        stanceSelectorNode = new PlayerSelectorStateNode(this.player,
            () => { return true; });
        this.playerDodgeRollStateNodeLeaf = new PlayerDodgeRollStateNodeLeaf(player,
            () =>
            this.player.triggerDodgeRoll
            && this.player.inputMoveDir_World.magnitude > 0
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
            () => true );
        this.playerSprintNode = new PlayerSprintNode(this.player,this, () => this.player.isSprint && player.inputMoveDir_World.magnitude > 0 );

        const float CHANGE_DIR_DOT_THRESHOLD = -.5f;
        const float CHANGE_DIR_MIN_SPEED_RATIO = 0.5f;
        this.playerSprintChangeDirectionNode = new PlayerSprintChangeDirectionNode(
            this.player,
            () => this.player.isSprint
                && this.player.inputMoveDir_World.magnitude > 0
                && this.player._movementCompoent.curMoveVelocity_World.magnitude > this.player.StandMoveMaxSpeed * CHANGE_DIR_MIN_SPEED_RATIO
                && Vector3.Dot(this.player.inputMoveDir_World.normalized, this.player._movementCompoent.curMoveVelocity_World.normalized) < CHANGE_DIR_DOT_THRESHOLD,
            this.player.sprintChangeDirSCRP);

        this.playerSlideNodeLeaf = new PlayerSlideNodeLeaf(
            this.player, this, this.player.slideSCRP,
            () => this.player.isTriggerCrouchStand 
            && this.player._movementCompoent.curMoveVelocity_World.magnitude >= this.player.sprintMaxSpeed * .9f);

        this.dolphinDiveSelector = new NodeSelector(
            () => this.player.triggerDodgeRoll);

        this.obstacleJumpDolphinDiveNodeLeaf = new ObstacleJumpDolphinDiveNodeLeaf(this.player
            , () => true);

        this.wallJumpReversDolphinDiveNodeLeaf = new WallJumpReversDolphinDiveNodeLeaf(this.player
            ,()=> true);

        this.wallJumpForwardDolphinDiveNodeLeaf = new WallJumpForwardDolphinDiveNodeLeaf(this.player
            , () => true);

        this.playerDolphinDiveStateNodeLeaf = new PlayerDolphinDiveStateNodeLeaf(this.player
            , () => true);

   
        playerStandMoveNode = new PlayerStandMoveNodeLeaf(this.player,
            () => { return this.player.inputMoveDir_Local.magnitude > 0; });

        playerStandIdleNode = new PlayerStandIdleNodeLeaf(this.player,
            () => true);


        crouchSelectorNode = new PlayerSelectorStateNode(this.player,
            () => (this.player.stance == Stance.crouch && this.player.isTriggerCrouchStand == false)
            || (this.player.stance != Stance.crouch && this.player.isTriggerCrouchStand)
            
            );

        playerCrouch_Move_NodeLeaf = new PlayerCrouch_Move_NodeLeaf(this.player,
           () => this.player.inputMoveDir_Local.magnitude > 0);

        playerCrouch_Idle_NodeLeaf = new PlayerCrouch_Idle_NodeLeaf(this.player,
            () => this.player.inputMoveDir_Local.magnitude <= 0 || true);


        this.proneStanceSelector = new PlayerSelectorStateNode(this.player, 
            () => this.player.stance == Stance.prone);
        this.proneStateNodeLeaf = new PlayerProneStateNodeLeaf(this.player,this
            ,()=> true);
        this.playerGetUpStateNodeLeaf = new PlayerGetUpStateNodeLeaf( this.player, 
            () => (this.player.inputMoveDir_World.magnitude > 0 && this.player.isSprint) || (this.player.triggerDodgeRoll));

        playerPokePickUpWeaponNodeLeaf = new PlayerPokePickUpWeaponNodeLeaf(
            this.player, this.player.pokePickUpAnimationSCRP, this.player.humanoidBone._rightFootBone,
            () => (player._isInteractCommand == true || player.commandBufferManager.TryGetCommand(nameof(player._isInteractCommand)))
            && this.player.currentInteractable is RangeWeapon 
            && player._weaponManuverManager.isPickingUpWeaponManuverAble
            );

        playerThrowWeaponNodeLeaf = new PlayerThrowWeaponNodeLeaf(this.player
            , this.player.throwObjectAnimationTriggerEventSCRP,
            () => player._isTriggerThrowCommand 
            && player._currentWeapon != null
            );

        PainStateSelectorNodeLeaf = new PlayerSelectorStateNode(this.player, 
            () => player._triggerEnterGotAttacked_OCM);
        playerBrounceOffNodeLeaf = new PlayerBrounceOffNodeLeaf( this.player,
            () => true);

        executeGunFuSelector = new NodeSelector(
            ()=> player._triggerExecute
            && player.executeGauge._gauge >= this.player.executeGauge.maxGauge
            && player.executedAbleGunFu != null
            && player._currentWeapon != null
            && player._currentWeapon.chamber.isReadyShoot );

        this.quickShootRangeWeaponNodeLeaf = new QuickShootRangeWeaponNodeLeaf(
            this.player
            , .67f
            ,this.player.castFindingScriptableObject
            ,this.player.quickShotAnimationTriggerEventSCRP
            , () => this.player.commandBufferManager.TryGetCommand(nameof(this.player.isTriggerQuickShot))
            && this.player._currentWeapon != null
            && this.player._currentWeapon.chamber.isReadyShoot
            && this.player._weaponManuverManager.aimingWeight < this.quickShootRangeWeaponNodeLeaf.aimingWeightQuickShot);

        executeGunFuOnGroundSelector = new NodeSelector(
            () => player._triggerExecute
            && player.executeGauge._gauge >= this.player.executeGauge.maxGauge
            && player.executedAbleGunFu != null
            && player.executedAbleGunFu._character is IRagdollAble downGetUpAble 
            && downGetUpAble._isFallDown);

        gunFuExecute_Single_Primary_Dodge_NodeLeaf_I = new OCM_Execute_Single_NodeLeaf(player,
            () => (player._triggerExecute
            && player.executeGauge._gauge >= this.player.executeGauge.maxGauge
            && player.executedAbleGunFu != null
            && player._currentWeapon != null
            && player._currentWeapon.chamber.isReadyShoot 
            && player._currentWeapon is PrimaryWeapon
            && (player.executedAbleGunFu._character as IRagdollAble)._isFallDown == false)
            , player.gunFuExecute_Single_Primary_Dodge_ScriptableObject_I
            ,GunFuExecuteStateName.GunFu_Execute_Dodge_Primary
            );
        gunFuExecute_Single_Secondary_Dodge_NodeLeaf_I = new OCM_Execute_Single_NodeLeaf(player,
            ()=> (player._triggerExecute
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

        gunFuExecute_Single_Primary_NodeLeaf_II = new OCM_Execute_Single_NodeLeaf(
            player,
            () => true
            , player.gunFuExecute_Single_Primary_ScriptableObject_II
            ,GunFuExecuteStateName.GunFu_Execute_Single_Primary_II
            );

        gunFuExecute_Single_Secondary_Selector = new NodeSelector(
            ()=> player._currentWeapon is SecondaryWeapon);

        gunFuExecute_Single_Secondary_NodeLeaf_I = new OCM_Execute_Single_NodeLeaf(
            player,
            () => true
            , player.gunFuExecute_Single_Secondary_ScriptableObject_I
            ,GunFuExecuteStateName.GunFu_Execute_Single_Secondary_I
            );
       

        gunFuExecute_OnGround = new OCM_Execute_Single_NodeLeaf(player,
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
            ,GunFuExecuteStateName.GunFu_Single_Execute_OnGround_I
            );
       
       

        this.parryNodeLeaf = new ParryNodeLeaf(
            this.player,
            () => (this.player._triggerAttack || this.player.commandBufferManager.TryGetCommand(nameof(player._triggerAttack)))
            && this.player._currentWeapon != null
            && this.player.meleeAttackerAble != null
            && (this.player.meleeAttackerAble._curAttackPhase == MeleeAttackingPhase.Anticipate
            || this.player.meleeAttackerAble._curAttackPhase == MeleeAttackingPhase.PreAttack
                || this.player.meleeAttackerAble._curAttackPhase == MeleeAttackingPhase.Attacking),
            this.player.parryScriptableObject);

        this.triggerHitGunFuSelector = new NodeSelector(
            () =>this.player.attackedAbleGunFu != null
            && this.player.attackedAbleGunFu._character.isDead == false
            && (this.player._triggerAttack || player.commandBufferManager.TryGetCommand(nameof(player._triggerAttack))));
        this.hitDownNodeLeaf = new GunFuHitDownNodeLeaf(this.player,this.player.gunFuHitDownScriptableObject,this.player.hit1
            ,() =>  this.player.attackedAbleGunFu != null
            && this.player._triggerAttack
            && this.player.attackedAbleGunFu._character.stance == Stance.prone
            && this.player.attackedAbleGunFu._character.isDead == false);

        this.hit1gunFuNodeLeaf = new GunFuHitNodeLeaf(this.player,
            () => true
            , this.player.hit1);

        this.ocmKnockDownNodeLeaf = new OCM_KnockDown_NodeLeaf(this.player,
            () => this.player.attackedAbleGunFu != null
            &&( this.player.isTriggerCrouchStand || this.player.commandBufferManager.TryGetCommand(nameof(this.player.isTriggerCrouchStand)))
            && this.player.attackedAbleGunFu.CanTakeAttack(this.ocmKnockDownNodeLeaf)
            ,this.player.ocmKnockDownScripatableObject
            );

        this.gunFuReloadNodeLeaf = new OCMReloadNodeLeaf(this.player,
            () => this.player.attackedAbleGunFu != null
            && (this.player._isReloadCommand || this.player.commandBufferManager.TryGetCommand(nameof(this.player._isReloadCommand)))
            && this.player.attackedAbleGunFu._character.isDead == false
            ,this.player.gunFuReloadScripatableObject
            );

        this.restrainGunFuStateNodeLeaf = new RestrainGunFuStateNodeLeaf(player.restrictScriptableObject, player,
            () =>
            {
                if (player._isAimingCommand
                && this.player.attackedAbleGunFu != null
                && this.player.attackedAbleGunFu.CanTakeAttack(this.restrainGunFuStateNodeLeaf)
                && this.player.attackedAbleGunFu._character.stance != Stance.prone
                && player._currentWeapon != null)
                    return true;
                return false;
            });

        humanShield_GunFuInteraction_NodeLeaf = new HumanShield_GunFu_NodeLeaf(this.player,
            () => this.player._isAimingCommand
            && this.player.attackedAbleGunFu != null
            && this.player.attackedAbleGunFu._character.stance != Stance.prone
            && this.player.attackedAbleGunFu._character.isDead == false
            && this.player.attackedAbleGunFu.CanTakeAttack(this.humanShield_GunFuInteraction_NodeLeaf)
            , this.player.humanShieldSCRP
            ,this.player.humanShieldTargetAdjustTransform);

        this.humanShieldExit_GunFu_NodeLeaf = new HumanShieldExit_GunFu_NodeLeaf(this.player
            ,this.player.humanShield_Exit_SCRP
            ,() => true);
        
        Hit2GunFuNodeLeaf = new GunFuHitNodeLeaf(this.player,
            () => (this.player._triggerAttack || this.player.commandBufferManager.TryGetCommand(nameof(player._triggerAttack)))
            && this.player.attackedAbleGunFu != null
            && this.player.attackedAbleGunFu._character.stance != Stance.prone
            , this.player.hit2);
        Hit3GunFuNodeLeaf = new GunFuHitNodeLeaf(this.player,
            () =>
            {
                if((this.player._triggerAttack
                || this.player.commandBufferManager.TryGetCommand(nameof(player._triggerAttack)))
                && this.player.attackedAbleGunFu != null
                && this.player.attackedAbleGunFu._character.stance != Stance.prone)
                    return true;

                else return false;
            }
        , this.player.hit3);
        dodgeSpinKicklGunFuNodeLeaf = new GunFuHitNodeLeaf(this.player,
            () => (this.player._triggerAttack || this.player.commandBufferManager.TryGetCommand(nameof(player._triggerAttack)))
       , player.dodgeSpinKick);

        this.meleeExecuteNodeLeaf_I = new MeleeExecute_NodeLeaf(player,
            () =>
            {
                Debug.Log("meleeExecuteNodeLeaf_I 1");
                if((player._triggerAttack || player.commandBufferManager.TryGetCommand(nameof(player._triggerAttack))) == false)
                    return false;

                Debug.Log("meleeExecuteNodeLeaf_I 2");
                if (this.player.attackedAbleGunFu == null
                || this.player.attackedAbleGunFu.CanTakeAttack(this.meleeExecuteNodeLeaf_I) == false)
                    return false;

                IHPDamageVisitor damagedCheck = this.hit1gunFuNodeLeaf;

                Debug.Log("meleeExecuteNodeLeaf_I 4");
                if (this.curNodeLeaf == hit1gunFuNodeLeaf
                ||this.curNodeLeaf == this.dodgeSpinKicklGunFuNodeLeaf
                )
                    damagedCheck = this.Hit2GunFuNodeLeaf;


                Debug.Log("meleeExecuteNodeLeaf_I 5");
                return HPDamageBeahavior.CheckDamageAfterTaken(damagedCheck, player.attackedAbleGunFu._character) <= 0;
            },
            player.meleeExecuteSCRP,
            GunFuExecuteStateName.GunFu_MeleeExecute_I);


        startNodeSelector.AddtoChildNode(deadNodeLeaf);
        startNodeSelector.AddtoChildNode(stanceSelectorNode);

        stanceSelectorNode.AddtoChildNode(PainStateSelectorNodeLeaf);
        stanceSelectorNode.AddtoChildNode(vaultingNodeLeaf);
        stanceSelectorNode.AddtoChildNode(climbHighNodeLeaf);
        stanceSelectorNode.AddtoChildNode(climbLowNodeLeaf);
        stanceSelectorNode.AddtoChildNode(this.fallingStateNodeLeaf);
        stanceSelectorNode.AddtoChildNode(playerDodgeRollStateNodeLeaf);
        stanceSelectorNode.AddtoChildNode(this.quickShootRangeWeaponNodeLeaf);
        stanceSelectorNode.AddtoChildNode(this.executeGunFuOnGroundSelector);
        stanceSelectorNode.AddtoChildNode(this.executeGunFuSelector);
        stanceSelectorNode.AddtoChildNode(this.parryNodeLeaf);
        stanceSelectorNode.AddtoChildNode(this.triggerHitGunFuSelector);
        stanceSelectorNode.AddtoChildNode(playerThrowWeaponNodeLeaf);
        stanceSelectorNode.AddtoChildNode(playerPokePickUpWeaponNodeLeaf);
        stanceSelectorNode.AddtoChildNode(this.proneStanceSelector);
        stanceSelectorNode.AddtoChildNode(this.playerSprintNode);
        stanceSelectorNode.AddtoChildNode(this.crouchSelectorNode);
        stanceSelectorNode.AddtoChildNode(standSelectorNode);



        this.fallingStateNodeLeaf.AddTransitionNode(this.landingRollStateNodeLeaf);
        this.fallingStateNodeLeaf.AddTransitionNode(this.landingStandStateNodeLeaf);
// 
        standSelectorNode.AddtoChildNode(playerStandMoveNode);
        standSelectorNode.AddtoChildNode(playerStandIdleNode);

        this.playerSprintNode.AddTransitionNode(this.playerSprintChangeDirectionNode);
        this.playerSprintNode.AddTransitionNode(this.playerSlideNodeLeaf);
        this.playerSprintNode.AddTransitionNode(this.dolphinDiveSelector);

        this.playerSlideNodeLeaf.AddTransitionNode(this.dodgeSpinKicklGunFuNodeLeaf);

        this.dolphinDiveSelector.AddtoChildNode(this.obstacleJumpDolphinDiveNodeLeaf);
        this.dolphinDiveSelector.AddtoChildNode(this.wallJumpReversDolphinDiveNodeLeaf);
        this.dolphinDiveSelector.AddtoChildNode(this.wallJumpForwardDolphinDiveNodeLeaf);
        this.dolphinDiveSelector.AddtoChildNode(this.playerDolphinDiveStateNodeLeaf);

        playerDodgeRollStateNodeLeaf.AddTransitionNode(this.hitDownNodeLeaf);
        playerDodgeRollStateNodeLeaf.AddTransitionNode(dodgeSpinKicklGunFuNodeLeaf);
        playerDodgeRollStateNodeLeaf.AddTransitionNode(gunFuExecute_Single_Primary_Dodge_NodeLeaf_I);
        playerDodgeRollStateNodeLeaf.AddTransitionNode(gunFuExecute_Single_Secondary_Dodge_NodeLeaf_I);


        this.dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(this.gunFuReloadNodeLeaf);
        this.dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(this.executeGunFuSelector);
        this.dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(this.restrainGunFuStateNodeLeaf);
        this.dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(this.Hit2GunFuNodeLeaf);

        PainStateSelectorNodeLeaf.AddtoChildNode(playerBrounceOffNodeLeaf);

        this.triggerHitGunFuSelector.AddtoChildNode(this.hitDownNodeLeaf);
        this.triggerHitGunFuSelector.AddtoChildNode(this.meleeExecuteNodeLeaf_I);
        this.triggerHitGunFuSelector.AddtoChildNode(this.hit1gunFuNodeLeaf);

        this.hitDownNodeLeaf.AddTransitionNode(this.gunFuReloadNodeLeaf);
        this.hitDownNodeLeaf.AddTransitionNode(this.restrainGunFuStateNodeLeaf);

        this.hit1gunFuNodeLeaf.AddTransitionNode(this.meleeExecuteNodeLeaf_I);
        this.hit1gunFuNodeLeaf.AddTransitionNode(this.executeGunFuSelector);
        this.hit1gunFuNodeLeaf.AddTransitionNode(this.ocmKnockDownNodeLeaf);
        this.hit1gunFuNodeLeaf.AddTransitionNode(this.gunFuReloadNodeLeaf);
        this.hit1gunFuNodeLeaf.AddTransitionNode(this.Hit2GunFuNodeLeaf);
        this.hit1gunFuNodeLeaf.AddTransitionNode(this.restrainGunFuStateNodeLeaf);

        this.Hit2GunFuNodeLeaf.AddTransitionNode(this.meleeExecuteNodeLeaf_I);
        this.Hit2GunFuNodeLeaf.AddTransitionNode(this.executeGunFuSelector);
        this.Hit2GunFuNodeLeaf.AddTransitionNode(this.ocmKnockDownNodeLeaf);
        this.Hit2GunFuNodeLeaf.AddTransitionNode(this.gunFuReloadNodeLeaf);
        this.Hit2GunFuNodeLeaf.AddTransitionNode(this.Hit3GunFuNodeLeaf);
        this.Hit2GunFuNodeLeaf.AddTransitionNode(this.humanShield_GunFuInteraction_NodeLeaf);


        this.humanShield_GunFuInteraction_NodeLeaf.AddTransitionNode(this.gunFuReloadNodeLeaf);
        this.humanShield_GunFuInteraction_NodeLeaf.AddTransitionNode(this.humanShieldExit_GunFu_NodeLeaf);

        this.restrainGunFuStateNodeLeaf.AddTransitionNode(this.gunFuReloadNodeLeaf);
        this.restrainGunFuStateNodeLeaf.AddTransitionNode(this.Hit3GunFuNodeLeaf);

        crouchSelectorNode.AddtoChildNode(playerCrouch_Move_NodeLeaf);
        crouchSelectorNode.AddtoChildNode(playerCrouch_Idle_NodeLeaf);

        this.proneStanceSelector.AddtoChildNode(this.proneStateNodeLeaf);

        this.proneStateNodeLeaf.AddTransitionNode(this.dodgeSpinKicklGunFuNodeLeaf);
        this.proneStateNodeLeaf.AddTransitionNode(this.playerGetUpStateNodeLeaf);

        executeGunFuSelector.AddtoChildNode(gunFuExecute_Single_Secondary_Selector);
        executeGunFuSelector.AddtoChildNode(gunFuExecute_Single_Primary_Selector);

        gunFuExecute_Single_Primary_Selector.AddtoChildNode(gunFuExecute_Single_Primary_NodeLeaf_II);
        gunFuExecute_Single_Secondary_Selector.AddtoChildNode(gunFuExecute_Single_Secondary_NodeLeaf_I);

        executeGunFuOnGroundSelector.AddtoChildNode(gunFuExecute_OnGround);

        _nodeManagerBehavior.SearchingNewNode(this);
    }
    #endregion

    #region NodeComponent
    public RegenarateGaugeNodeLeaf regenarateHPNodeLeaf;
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

        this.regenarateExecuteGaugeNodeLeaf = new RegenarateGaugeNodeLeaf
            (
            () => true,
            this.player.executeGauge
            , this.player.executeGauge.maxGauge
            , 2
            );


        this._nodeComponentManager.AddNode(this.regenarateHPNodeLeaf);
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
