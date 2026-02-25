using System.Collections.Generic;
using UnityEngine;
using static Player;

public class PlayerStateNodeManager : INodeManager
{
    private INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager._curNodeLeaf { get; set; }
    public INodeSelector startNodeSelector { get; set; }
    public NodeManagerBehavior _nodeManagerBehavior { get; set; }
    public List<INodeManager> _parallelNodeManahger { get; set; }

    public Player player;
    public PlayerStateNodeManager(Player player) 
    { 
        this.player = player;
        this._nodeManagerBehavior = new NodeManagerBehavior();
        this._parallelNodeManahger = new List<INodeManager>();
        InitailizedNode();
    }
    public void FixedUpdateNode() => _nodeManagerBehavior.FixedUpdateNode(this);
   
    public void UpdateNode()=>_nodeManagerBehavior.UpdateNodeAndCheckFindingNode(this);
   
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
    public GunFuExecute_Single_NodeLeaf gunFuExecute_Single_Secondary_NodeLeaf_II { get; set; }
    public GunFuExecute_Single_NodeLeaf gunFuExecute_Single_Secondary_NodeLeaf_III { get; set; }
    public GunFuExecute_Single_NodeLeaf gunFuExecute_Single_Secondary_NodeLeaf_IV { get; set; }

    public NodeSelector gunFuExecute_Single_Primary_Selector;
    public GunFuExecute_Single_NodeLeaf gunFuExecute_Single_Primary_NodeLeaf_I { get; set; }
    public GunFuExecute_Single_NodeLeaf gunFuExecute_Single_Primary_NodeLeaf_II { get; set; }
    public GunFuExecute_Single_NodeLeaf gunFuExecute_Single_Primary_Dodge_NodeLeaf_I { get; set; }
    public GunFuExecute_Single_NodeLeaf gunFuExecute_Single_Secondary_Dodge_NodeLeaf_I { get; set; }
    public NodeSelector executeGunFuOnGroundSelector { get; set; }
    public GunFuExecute_Single_NodeLeaf gunFuExecute_OnGround_Secondary_I_NodeLeaf { get; private set; }
    public GunFuExecute_Single_NodeLeaf gunFuExecute_OnGround_Primary_I_NodeLeaf { get; private set; }

    public NodeSelector triggerHitGunFuSelector { get; private set; }
    public GunFuHitDownNodeLeaf hitDownNodeLeaf { get; private set; }
    public GunFuHitNodeLeaf Hit1gunFuNodeLeaf { get; private set; }
    public HumanShield_GunFu_NodeLeaf humanShield_GunFuInteraction_NodeLeaf { get; private set; }
    public HumanShieldExit_GunFu_NodeLeaf humanShieldExit_GunFu_NodeLeaf { get; private set; }
    public RestrainGunFuStateNodeLeaf restrictGunFuStateNodeLeaf { get; private set; }
    public PlayerSelectorStateNode weaponDisarmSelector { get; private set; }
    public WeaponDisarm_GunFuInteraction_NodeLeaf primary_WeaponDisarm_GunFuInteraction_NodeLeaf { get; private set; }
    public WeaponDisarm_GunFuInteraction_NodeLeaf secondart_WeaponDisarm_GunFuInteraction_NodeLeaf { get; private set; }
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
            () => player.triggerDodgeRoll && player.inputMoveDir_World.magnitude > 0
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
        this.playerDolphinDiveStateNodeLeaf = new PlayerDolphinDiveStateNodeLeaf(this.player
            ,() => this.player.triggerDodgeRoll);

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
            this.player, this.player.pokePickUpAnimationSCRP, this.player.rightFootss,
            () => (player._isInteractCommand == true || player.commandBufferManager.TryGetCommand(nameof(player._isInteractCommand)))
            && this.player.currentInteractable is Weapon 
            && player._weaponManuverManager.isPickingUpWeaponManuverAble
            );

        playerThrowWeaponNodeLeaf = new PlayerThrowWeaponNodeLeaf(this.player
            , this.player.throwObjectAnimationTriggerEventSCRP,
            () => player._isTriggerThrowCommand 
            && player._currentWeapon != null
            );

        gotGunFuAttackSelectorNodeLeaf = new PlayerSelectorStateNode(this.player, () => player._triggerHitedGunFu);
        playerBrounceOffGotAttackGunFuNodeLeaf = new PlayerBrounceOffGotAttackGunFuNodeLeaf(player.PlayerBrounceOffGotAttackGunFuScriptableObject, this.player,
            () => player.curAttackerGunFuNode is EnemySpinKickGunFuNodeLeaf);

        executeGunFuSelector = new NodeSelector(
            ()=> player._triggerExecuteGunFu
            && player.executedAbleGunFu != null
            && player._currentWeapon != null
            && player._currentWeapon.chamber.isReadyShoot );
        executeGunFuOnGroundSelector = new NodeSelector(
            () => player.executedAbleGunFu._character is IRagdollAble downGetUpAble 
            && downGetUpAble._isFallDown);

        gunFuExecute_Single_Primary_Dodge_NodeLeaf_I = new GunFuExecute_Single_NodeLeaf(player,
            () => (player._triggerExecuteGunFu
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
            () => player.primaryExecuteGunFuRandomNumber.GetGunExecuteGuNumber() == 1
            , player.gunFuExecute_Single_Primary_ScriptableObject_I
            ,GunFuExecuteStateName.GunFu_Execute_Single_Primary_I
            );
        gunFuExecute_Single_Primary_NodeLeaf_II = new GunFuExecute_Single_NodeLeaf(
            player,
            () => player.primaryExecuteGunFuRandomNumber.GetGunExecuteGuNumber() == 2
            , player.gunFuExecute_Single_Primary_ScriptableObject_II
            ,GunFuExecuteStateName.GunFu_Execute_Single_Primary_II
            );

        gunFuExecute_Single_Secondary_Selector = new NodeSelector(
            ()=> player._currentWeapon is SecondaryWeapon);

        gunFuExecute_Single_Secondary_NodeLeaf_I = new GunFuExecute_Single_NodeLeaf(
            player,
            () => player.secondaryExecuteGunFuRandomNumber.GetGunExecuteGuNumber() == 1
            || player.secondaryExecuteGunFuRandomNumber.GetGunExecuteGuNumber() == 0
            , player.gunFuExecute_Single_Secondary_ScriptableObject_I
            ,GunFuExecuteStateName.GunFu_Execute_Single_Secondary_I
            );
        gunFuExecute_Single_Secondary_NodeLeaf_II = new GunFuExecute_Single_NodeLeaf(
            player,
            () => player.secondaryExecuteGunFuRandomNumber.GetGunExecuteGuNumber() == 2
            , player.gunFuExecute_Single_Secondary_ScriptableObject_II
            ,GunFuExecuteStateName.GunFu_Execute_Single_Secondary_II
            );
        gunFuExecute_Single_Secondary_NodeLeaf_III = new GunFuExecute_Single_NodeLeaf(
            player,
            () => player.secondaryExecuteGunFuRandomNumber.GetGunExecuteGuNumber() == 3
            , player.gunFuExecute_Single_Secondary_ScriptableObject_III
            ,GunFuExecuteStateName.GunFu_Execute_Single_Secondary_III
            );
        gunFuExecute_Single_Secondary_NodeLeaf_IV = new GunFuExecute_Single_NodeLeaf(
            player,
            () => player.secondaryExecuteGunFuRandomNumber.GetGunExecuteGuNumber() == 4
            , player.gunFuExecute_Single_Secondary_ScriptableObject_IV
            ,GunFuExecuteStateName.GunFu_Execute_Single_Secondary_IV
            );
        gunFuExecute_OnGround_Secondary_I_NodeLeaf = new GunFuExecute_Single_NodeLeaf(player,
            () => 
            {
                if (player._currentWeapon == null 
                && player._currentWeapon.chamber.isReadyShoot
                )
                    return false;
                if (
                player.executedAbleGunFu._character is IRagdollAble downGetUpAble
                && downGetUpAble._isFallDown 
                && player._currentWeapon is SecondaryWeapon)
                    return true;
                return false;
            }
            ,player.gunFu_Single_Execute_OnGround_Secondary_I
            ,GunFuExecuteStateName.GunFu_Single_Execute_OnGround_Secondary_I
            );
       
        
        gunFuExecute_OnGround_Primary_I_NodeLeaf = new GunFuExecute_Single_NodeLeaf(player,
            () =>
            {
                if (player._currentWeapon == null
              && player._currentWeapon.chamber.isReadyShoot == false
              )
                    return false;
                if (
                player.executedAbleGunFu._character is IRagdollAble downGetUpAble
                && downGetUpAble._isFallDown
                && player._currentWeapon is PrimaryWeapon)
                    return true;
                return false;
            }
            , player.gunFu_Single_Execute_OnGround_Primary_I
            ,GunFuExecuteStateName.GunFu_Single_Execute_OnGround_Primary_I
            );

        this.triggerHitGunFuSelector = new NodeSelector(
            () => (this.player._triggerGunFu || player.commandBufferManager.TryGetCommand(nameof(player._triggerGunFu))));
        this.hitDownNodeLeaf = new GunFuHitDownNodeLeaf(this.player,this.player.gunFuHitDownScriptableObject,this.player.hit1
            ,() =>  this.player.attackedAbleGunFu != null
            && this.player.attackedAbleGunFu._character.stance == Stance.prone
            && this.player.attackedAbleGunFu._character.isDead == false);

        Hit1gunFuNodeLeaf = new GunFuHitNodeLeaf(this.player,
            () => true
            //&& this.player.attackedAbleGunFu != null
            //&& this.player.attackedAbleGunFu._character.isDead == false
            ,this.player.hit1);

        restrictGunFuStateNodeLeaf = new RestrainGunFuStateNodeLeaf(player.restrictScriptableObject, player,
            () =>
            {
                if (player._isAimingCommand
                && this.player.attackedAbleGunFu != null
                && this.player.attackedAbleGunFu._character.stance != Stance.prone)
                {
                    if (player._currentWeapon != null)
                        return true;
                }
                return false;
            });

        weaponDisarmSelector = new PlayerSelectorStateNode(this.player,
            () => 
            {
                if((player._isInteractCommand || player.commandBufferManager.TryGetCommand(nameof(player._isInteractCommand))) 
                && this.player.attackedAbleGunFu != null
                && this.player.attackedAbleGunFu._character.stance != Stance.prone)
                {
                    if(player.attackedAbleGunFu._weaponAdvanceUser._currentWeapon != null)
                        return true;
                }
                return false;
            }
            );

        primary_WeaponDisarm_GunFuInteraction_NodeLeaf = new WeaponDisarm_GunFuInteraction_NodeLeaf(this.player.primaryWeaponDisarmGunFuScriptableObject
            , this.player
            , () => player.attackedAbleGunFu._weaponAdvanceUser._currentWeapon is PrimaryWeapon);
        secondart_WeaponDisarm_GunFuInteraction_NodeLeaf = new WeaponDisarm_GunFuInteraction_NodeLeaf(this.player.secondaryWeaponDisarmGunFuScriptableObject
            , this.player
            , () => player.attackedAbleGunFu._weaponAdvanceUser._currentWeapon is SecondaryWeapon);

        humanShield_GunFuInteraction_NodeLeaf = new HumanShield_GunFu_NodeLeaf(this.player,
            () => this.player._isAimingCommand
            && this.player.attackedAbleGunFu != null
            && this.player.attackedAbleGunFu._character.stance != Stance.prone
            && this.player.attackedAbleGunFu._character.isDead == false
            , this.player.humanShieldSCRP
            ,this.player.humanShieldTargetAdjustTransform);

        this.humanShieldExit_GunFu_NodeLeaf = new HumanShieldExit_GunFu_NodeLeaf(this.player
            ,this.player.humanShield_Exit_SCRP
            ,() => true);
        
        Hit2GunFuNodeLeaf = new GunFuHitNodeLeaf(this.player, 
            () => (this.player._triggerGunFu || this.player.commandBufferManager.TryGetCommand(nameof(player._triggerGunFu)))
            && this.player.attackedAbleGunFu != null
            && this.player.attackedAbleGunFu._character.stance != Stance.prone
            , this.player.hit2);
        Hit3GunFuNodeLeaf = new GunFuHitNodeLeaf(this.player, 
            () => 
            {

                if((this.player._triggerGunFu 
                || this.player.commandBufferManager.TryGetCommand(nameof(player._triggerGunFu)))
                && this.player.attackedAbleGunFu != null
                && this.player.attackedAbleGunFu._character.stance != Stance.prone)
                    return true;

                else return false;
            } 
        , this.player.hit3);
        dodgeSpinKicklGunFuNodeLeaf = new GunFuHitNodeLeaf(this.player, 
            () => (this.player._triggerGunFu || this.player.commandBufferManager.TryGetCommand(nameof(player._triggerGunFu)))
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

        this.playerSprintNode.AddTransitionNode(this.playerDolphinDiveStateNodeLeaf);

        weaponDisarmSelector.AddtoChildNode(primary_WeaponDisarm_GunFuInteraction_NodeLeaf);
        weaponDisarmSelector.AddtoChildNode(secondart_WeaponDisarm_GunFuInteraction_NodeLeaf);

        primary_WeaponDisarm_GunFuInteraction_NodeLeaf.AddTransitionNode(restrictGunFuStateNodeLeaf);
        primary_WeaponDisarm_GunFuInteraction_NodeLeaf.AddTransitionNode(Hit2GunFuNodeLeaf);

        secondart_WeaponDisarm_GunFuInteraction_NodeLeaf.AddTransitionNode(restrictGunFuStateNodeLeaf);
        secondart_WeaponDisarm_GunFuInteraction_NodeLeaf.AddTransitionNode(Hit2GunFuNodeLeaf);

        playerDodgeRollStateNodeLeaf.AddTransitionNode(dodgeSpinKicklGunFuNodeLeaf);
        playerDodgeRollStateNodeLeaf.AddTransitionNode(gunFuExecute_Single_Primary_Dodge_NodeLeaf_I);
        playerDodgeRollStateNodeLeaf.AddTransitionNode(gunFuExecute_Single_Secondary_Dodge_NodeLeaf_I);

        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(executeGunFuSelector);
        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(restrictGunFuStateNodeLeaf);
        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(weaponDisarmSelector);
        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(Hit2GunFuNodeLeaf);

        gotGunFuAttackSelectorNodeLeaf.AddtoChildNode(playerBrounceOffGotAttackGunFuNodeLeaf);

        this.triggerHitGunFuSelector.AddtoChildNode(this.hitDownNodeLeaf);
        this.triggerHitGunFuSelector.AddtoChildNode(this.Hit1gunFuNodeLeaf);

        this.hitDownNodeLeaf.AddTransitionNode(this.restrictGunFuStateNodeLeaf);
        this.hitDownNodeLeaf.AddTransitionNode(this.weaponDisarmSelector);

        Hit1gunFuNodeLeaf.AddTransitionNode(executeGunFuSelector);
        Hit1gunFuNodeLeaf.AddTransitionNode(Hit2GunFuNodeLeaf);
        Hit1gunFuNodeLeaf.AddTransitionNode(weaponDisarmSelector);
        Hit1gunFuNodeLeaf.AddTransitionNode(restrictGunFuStateNodeLeaf);
        Hit2GunFuNodeLeaf.AddTransitionNode(executeGunFuSelector);
        Hit2GunFuNodeLeaf.AddTransitionNode(Hit3GunFuNodeLeaf);
        Hit2GunFuNodeLeaf.AddTransitionNode(weaponDisarmSelector);
        Hit2GunFuNodeLeaf.AddTransitionNode(humanShield_GunFuInteraction_NodeLeaf);

        this.humanShield_GunFuInteraction_NodeLeaf.AddTransitionNode(humanShieldExit_GunFu_NodeLeaf);

        restrictGunFuStateNodeLeaf.AddTransitionNode(Hit3GunFuNodeLeaf);

        crouchSelectorNode.AddtoChildNode(playerCrouch_Move_NodeLeaf);
        crouchSelectorNode.AddtoChildNode(playerCrouch_Idle_NodeLeaf);

        this.proneStanceSelector.AddtoChildNode(this.proneStateNodeLeaf);
        this.proneStateNodeLeaf.AddTransitionNode(this.playerGetUpStateNodeLeaf);

        executeGunFuSelector.AddtoChildNode(executeGunFuOnGroundSelector);
        executeGunFuSelector.AddtoChildNode(gunFuExecute_Single_Secondary_Selector);
        executeGunFuSelector.AddtoChildNode(gunFuExecute_Single_Primary_Selector);

        gunFuExecute_Single_Primary_Selector.AddtoChildNode(gunFuExecute_Single_Primary_NodeLeaf_I);
        gunFuExecute_Single_Primary_Selector.AddtoChildNode(gunFuExecute_Single_Primary_NodeLeaf_II);

        gunFuExecute_Single_Secondary_Selector.AddtoChildNode(gunFuExecute_Single_Secondary_NodeLeaf_I);
        gunFuExecute_Single_Secondary_Selector.AddtoChildNode(gunFuExecute_Single_Secondary_NodeLeaf_II);
        gunFuExecute_Single_Secondary_Selector.AddtoChildNode(gunFuExecute_Single_Secondary_NodeLeaf_III);
        gunFuExecute_Single_Secondary_Selector.AddtoChildNode(gunFuExecute_Single_Secondary_NodeLeaf_IV);

        executeGunFuOnGroundSelector.AddtoChildNode(gunFuExecute_OnGround_Secondary_I_NodeLeaf);
        executeGunFuOnGroundSelector.AddtoChildNode(gunFuExecute_OnGround_Primary_I_NodeLeaf);

        _nodeManagerBehavior.SearchingNewNode(this);
    }
    public void ChangeNode(PlayerStateNodeLeaf playerStateNodeLeaf)
    {
        curNodeLeaf.Exit();
        curNodeLeaf = playerStateNodeLeaf;
        curNodeLeaf.Enter();
    }

   
}
