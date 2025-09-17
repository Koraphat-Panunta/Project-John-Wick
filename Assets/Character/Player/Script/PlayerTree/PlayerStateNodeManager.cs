using UnityEngine;
using static Player;

public class PlayerStateNodeManager : INodeManager
{
    private INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager.curNodeLeaf { get; set; }
    public INodeSelector startNodeSelector { get; set; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }

    public Player player;
    public PlayerStateNodeManager(Player player) 
    { 
        this.player = player;
        this.nodeManagerBehavior = new NodeManagerBehavior();
        InitailizedNode();
    }
    public void FixedUpdateNode() => nodeManagerBehavior.FixedUpdateNode(this);
   
    public void UpdateNode()=>nodeManagerBehavior.UpdateNode(this);
   
    public PlayerSelectorStateNode stanceSelectorNode { get; private set; }
    public PlayerDeadNodeLeaf deadNodeLeaf { get; private set; }
    public PlayerSelectorStateNode standSelectorNode { get; private set; }
    public PlayerDodgeRollStateNodeLeaf playerDodgeRollStateNodeLeaf { get; private set; }
    public VaultingNodeLeaf vaultingNodeLeaf { get; private set; }
    public ClimbParkourNodeLeaf climbLowNodeLeaf { get; private set; }
    public ClimbParkourNodeLeaf climbHighNodeLeaf { get; private set; }
    public PlayerSprintNode playerSprintNode { get; private set; }
    public PlayerSelectorStateNode standIncoverSelector { get; private set; }
    public PlayerStandIdleNodeLeaf playerStandIdleNode { get; private set; }
    public PlayerStandMoveNodeLeaf playerStandMoveNode { get; private set; }

    public PlayerSelectorStateNode crouchSelectorNode { get; private set; }
    public PlayerCrouch_Move_NodeLeaf playerCrouch_Move_NodeLeaf { get; private set; }
    public PlayerCrouch_Idle_NodeLeaf playerCrouch_Idle_NodeLeaf { get; private set; }
    public PlayerInCoverStandMoveNodeLeaf playerInCoverStandMoveNode { get; private set; }
    public PlayerInCoverStandIdleNodeLeaf playerInCoverStandIdleNode { get; private set; }

    public PlayerSelectorStateNode proneStanceSelector { get; private set; }
    public PlayerGetUpStateNodeLeaf playerGetUpStateNodeLeaf { get; private set; }

    public PlayerSelectorStateNode gotGunFuAttackSelectorNodeLeaf { get; private set; }
    public PlayerBrounceOffGotAttackGunFuNodeLeaf playerBrounceOffGotAttackGunFuNodeLeaf { get; private set; }

    public NodeSelector executeGunFuSelector { get; set; }
    public GunFuExecute_Single_NodeLeaf gunFuExecute_Single_Secondary_NodeLeaf_I { get; set; }
    public GunFuExecute_Single_NodeLeaf gunFuExecute_Single_Primary_NodeLeaf_I { get; set; }
    public GunFuExecute_Single_NodeLeaf gunFuExecute_Single_Secondary_Dodge_NodeLeaf_I { get; set; }
    public NodeSelector executeGunFuOnGroundSelector { get; set; }
    public GunFuExecute_OnGround_Single_NodeLeaf gunFuExecute_OnGround_Secondary_LayUp_I_NodeLeaf { get; private set; }
    public GunFuExecute_OnGround_Single_NodeLeaf gunFuExecute_OnGround_Secondary_LayDown_I_NodeLeaf { get; private set; }
    public GunFuExecute_OnGround_Single_NodeLeaf gunFuExecute_OnGround_Primary_LayUp_I_NodeLeaf { get; private set; }
    public GunFuExecute_OnGround_Single_NodeLeaf gunFuExecute_OnGround_Primary_LayDown_I_NodeLeaf { get; private set; }
    public GunFuHitNodeLeaf Hit1gunFuNodeLeaf { get; private set; }
    public HumanShield_GunFuInteraction_NodeLeaf humanShield_GunFuInteraction_NodeLeaf { get; private set; }
    public RestrictGunFuStateNodeLeaf restrictGunFuStateNodeLeaf { get; private set; }
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
        standSelectorNode = new PlayerSelectorStateNode(this.player,
            () => { return this.player.playerStance == Stance.stand || player.isSprint; });
        playerSprintNode = new PlayerSprintNode(this.player, () => this.player.isSprint && player.inputMoveDir_World.magnitude > 0 );

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
            () => this.player.playerStance == Stance.crouch);

        playerCrouch_Move_NodeLeaf = new PlayerCrouch_Move_NodeLeaf(this.player,
           () => this.player.inputMoveDir_Local.magnitude > 0);

        playerCrouch_Idle_NodeLeaf = new PlayerCrouch_Idle_NodeLeaf(this.player,
            () => this.player.inputMoveDir_Local.magnitude <= 0 || true);


        proneStanceSelector = new PlayerSelectorStateNode(this.player, 
            () => this.player.playerStance == Stance.prone);
        playerGetUpStateNodeLeaf = new PlayerGetUpStateNodeLeaf(player.PlayerGetUpStateScriptableObject, this.player, 
            () => player.playerStance == Stance.prone || true);

        gotGunFuAttackSelectorNodeLeaf = new PlayerSelectorStateNode(this.player, () => player._triggerHitedGunFu);
        playerBrounceOffGotAttackGunFuNodeLeaf = new PlayerBrounceOffGotAttackGunFuNodeLeaf(player.PlayerBrounceOffGotAttackGunFuScriptableObject, this.player,
            () => player.curAttackerGunFuNode is EnemySpinKickGunFuNodeLeaf);

        executeGunFuSelector = new NodeSelector(
            ()=> player._triggerExecuteGunFu
            && player.executedAbleGunFu != null
            && player._currentWeapon != null
            && player._currentWeapon.bulletStore[BulletStackType.Chamber] > 0);
        executeGunFuOnGroundSelector = new NodeSelector(
            () => player.executedAbleGunFu._character is IFallDownGetUpAble downGetUpAble 
            && downGetUpAble._isFallDown);
        gunFuExecute_Single_Secondary_Dodge_NodeLeaf_I = new GunFuExecute_Single_NodeLeaf(player,
            ()=> (player._triggerExecuteGunFu
            && player.executedAbleGunFu != null
            && player._currentWeapon != null
            && player._currentWeapon.bulletStore[BulletStackType.Chamber] > 0
            && player._currentWeapon is SecondaryWeapon
            && (player.executedAbleGunFu._character as IFallDownGetUpAble)._isFallDown == false)
            ,player.gunFuExecute_Single_Secondary_Dodge_ScriptableObject_I);
        gunFuExecute_Single_Primary_NodeLeaf_I = new GunFuExecute_Single_NodeLeaf(
            player,
            () => player._currentWeapon is PrimaryWeapon
            , player.gunFuExecute_Single_Primary_ScriptableObject_I);
        gunFuExecute_Single_Secondary_NodeLeaf_I = new GunFuExecute_Single_NodeLeaf(
            player,
            () => player._currentWeapon is SecondaryWeapon
            , player.gunFuExecute_Single_Secondary_ScriptableObject_I);
        gunFuExecute_OnGround_Secondary_LayUp_I_NodeLeaf = new GunFuExecute_OnGround_Single_NodeLeaf(player,
            () => 
            {
                if (player._currentWeapon == null 
                && player._currentWeapon.bulletStore[BulletStackType.Chamber] <= 0
                )
                    return false;
                if (
                player.executedAbleGunFu._character is IFallDownGetUpAble downGetUpAble
                && downGetUpAble._isFallDown && downGetUpAble._isFacingUp
                && player._currentWeapon is SecondaryWeapon)
                    return true;
                return false;
            }
            ,player.gunFu_Single_Execute_OnGround_Secondary_Layup_I
            );
        gunFuExecute_OnGround_Secondary_LayDown_I_NodeLeaf = new GunFuExecute_OnGround_Single_NodeLeaf(player, 
            () => 
            {
                if (player._currentWeapon == null
               && player._currentWeapon.bulletStore[BulletStackType.Chamber] <= 0
              )
                    return false;
                if (
                player.executedAbleGunFu._character is IFallDownGetUpAble downGetUpAble
                && downGetUpAble._isFallDown 
                && player._currentWeapon is SecondaryWeapon)
                    return true;
                return false;
            },player.gunFu_Single_Execute_OnGround_Secondary_Laydown_I
            );
        gunFuExecute_OnGround_Primary_LayUp_I_NodeLeaf = new GunFuExecute_OnGround_Single_NodeLeaf(player, 
            () => 
            {
                if (player._currentWeapon == null
               && player._currentWeapon.bulletStore[BulletStackType.Chamber] <= 0
               )
                    return false;
                if (
                player.executedAbleGunFu._character is IFallDownGetUpAble downGetUpAble
                && downGetUpAble._isFallDown && downGetUpAble._isFacingUp
                && player._currentWeapon is PrimaryWeapon)
                    return true;
                return false;
            },player.gunFu_Single_Execute_OnGround_Primary_Layup_I
            );
        gunFuExecute_OnGround_Primary_LayDown_I_NodeLeaf = new GunFuExecute_OnGround_Single_NodeLeaf(player,
            () =>
            {
                if (player._currentWeapon == null
              && player._currentWeapon.bulletStore[BulletStackType.Chamber] <= 0
              )
                    return false;
                if (
                player.executedAbleGunFu._character is IFallDownGetUpAble downGetUpAble
                && downGetUpAble._isFallDown
                && player._currentWeapon is PrimaryWeapon)
                    return true;
                return false;
            }
            , player.gunFu_Single_Execute_OnGround_Primary_Laydown_I
            );
        Hit1gunFuNodeLeaf = new GunFuHitNodeLeaf(this.player, 
            () => (this.player._triggerGunFu || player.commandBufferManager.TryGetCommand(nameof(player._triggerGunFu)) )
            && this.player.attackedAbleGunFu != null
            && this.player.attackedAbleGunFu._character.isDead == false
            ,this.player.hit1);

        restrictGunFuStateNodeLeaf = new RestrictGunFuStateNodeLeaf(player.restrictScriptableObject, player,
            () =>
            {
                if (player._isAimingCommand && player.attackedAbleGunFu != null)
                {
                    if (player._currentWeapon != null)
                        return true;
                }
                return false;
            });

        weaponDisarmSelector = new PlayerSelectorStateNode(this.player,
            () => 
            {
                if((player._isPickingUpWeaponCommand || player.commandBufferManager.TryGetCommand(nameof(player._isPickingUpWeaponCommand))) && player.attackedAbleGunFu != null)
                {
                    if(player.attackedAbleGunFu._weaponAdvanceUser._currentWeapon != null)
                        return true;
                }
                return false;
            }
            );

        primary_WeaponDisarm_GunFuInteraction_NodeLeaf = new WeaponDisarm_GunFuInteraction_NodeLeaf(this.player.primaryWeaponDisarmGunFuScriptableObject
            ,this.player
            , () => player.attackedAbleGunFu._weaponAdvanceUser._currentWeapon is PrimaryWeapon);
        secondart_WeaponDisarm_GunFuInteraction_NodeLeaf = new WeaponDisarm_GunFuInteraction_NodeLeaf(this.player.secondaryWeaponDisarmGunFuScriptableObject
            , this.player
            , () => player.attackedAbleGunFu._weaponAdvanceUser._currentWeapon is SecondaryWeapon);

        humanShield_GunFuInteraction_NodeLeaf = new HumanShield_GunFuInteraction_NodeLeaf(this.player,
            () => this.player._isAimingCommand
            && this.player.attackedAbleGunFu != null
            && this.player.attackedAbleGunFu._character.isDead == false

            , this.player.humanShield);
        
        Hit2GunFuNodeLeaf = new GunFuHitNodeLeaf(this.player, 
            () => (this.player._triggerGunFu || this.player.commandBufferManager.TryGetCommand(nameof(player._triggerGunFu)))
            && this.player.attackedAbleGunFu != null
            , this.player.hit2);
        Hit3GunFuNodeLeaf = new GunFuHitNodeLeaf(this.player, 
            () => (this.player._triggerGunFu || this.player.commandBufferManager.TryGetCommand(nameof(player._triggerGunFu))) 
        && this.player.attackedAbleGunFu != null
        , this.player.hit3);
        dodgeSpinKicklGunFuNodeLeaf = new GunFuHitNodeLeaf(this.player, 
            () => (this.player._triggerGunFu || this.player.commandBufferManager.TryGetCommand(nameof(player._triggerGunFu)))
        && this.player.attackedAbleGunFu != null, player.dodgeSpinKick);


        startNodeSelector.AddtoChildNode(deadNodeLeaf);
        startNodeSelector.AddtoChildNode(stanceSelectorNode);

        stanceSelectorNode.AddtoChildNode(gotGunFuAttackSelectorNodeLeaf);
        stanceSelectorNode.AddtoChildNode(vaultingNodeLeaf);
        stanceSelectorNode.AddtoChildNode(climbHighNodeLeaf);
        stanceSelectorNode.AddtoChildNode(climbLowNodeLeaf);
        stanceSelectorNode.AddtoChildNode(playerDodgeRollStateNodeLeaf);
        stanceSelectorNode.AddtoChildNode(standSelectorNode);
        stanceSelectorNode.AddtoChildNode(crouchSelectorNode);
        stanceSelectorNode.AddtoChildNode(proneStanceSelector);

        standSelectorNode.AddtoChildNode(executeGunFuSelector);
        standSelectorNode.AddtoChildNode(Hit1gunFuNodeLeaf); // 
        standSelectorNode.AddtoChildNode(playerSprintNode);
        standSelectorNode.AddtoChildNode(playerStandMoveNode);
        standSelectorNode.AddtoChildNode(playerStandIdleNode);

        weaponDisarmSelector.AddtoChildNode(primary_WeaponDisarm_GunFuInteraction_NodeLeaf);
        weaponDisarmSelector.AddtoChildNode(secondart_WeaponDisarm_GunFuInteraction_NodeLeaf);

        primary_WeaponDisarm_GunFuInteraction_NodeLeaf.AddTransitionNode(restrictGunFuStateNodeLeaf);
        primary_WeaponDisarm_GunFuInteraction_NodeLeaf.AddTransitionNode(Hit2GunFuNodeLeaf);

        secondart_WeaponDisarm_GunFuInteraction_NodeLeaf.AddTransitionNode(restrictGunFuStateNodeLeaf);
        secondart_WeaponDisarm_GunFuInteraction_NodeLeaf.AddTransitionNode(Hit2GunFuNodeLeaf);

        playerDodgeRollStateNodeLeaf.AddTransitionNode(dodgeSpinKicklGunFuNodeLeaf);
        playerDodgeRollStateNodeLeaf.AddTransitionNode(gunFuExecute_Single_Secondary_Dodge_NodeLeaf_I);

        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(executeGunFuSelector);
        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(restrictGunFuStateNodeLeaf);
        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(weaponDisarmSelector);
        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(Hit2GunFuNodeLeaf);

        gotGunFuAttackSelectorNodeLeaf.AddtoChildNode(playerBrounceOffGotAttackGunFuNodeLeaf);

        Hit1gunFuNodeLeaf.AddTransitionNode(executeGunFuSelector);
        Hit1gunFuNodeLeaf.AddTransitionNode(Hit2GunFuNodeLeaf);
        Hit1gunFuNodeLeaf.AddTransitionNode(weaponDisarmSelector);
        Hit1gunFuNodeLeaf.AddTransitionNode(restrictGunFuStateNodeLeaf);
        Hit2GunFuNodeLeaf.AddTransitionNode(executeGunFuSelector);
        Hit2GunFuNodeLeaf.AddTransitionNode(Hit3GunFuNodeLeaf);
        Hit2GunFuNodeLeaf.AddTransitionNode(weaponDisarmSelector);
        Hit2GunFuNodeLeaf.AddTransitionNode(humanShield_GunFuInteraction_NodeLeaf);

        restrictGunFuStateNodeLeaf.AddTransitionNode(Hit3GunFuNodeLeaf);

        crouchSelectorNode.AddtoChildNode(playerCrouch_Move_NodeLeaf);
        crouchSelectorNode.AddtoChildNode(playerCrouch_Idle_NodeLeaf);

        proneStanceSelector.AddtoChildNode(playerGetUpStateNodeLeaf);

        executeGunFuSelector.AddtoChildNode(executeGunFuOnGroundSelector);
        executeGunFuSelector.AddtoChildNode(gunFuExecute_Single_Primary_NodeLeaf_I);
        executeGunFuSelector.AddtoChildNode(gunFuExecute_Single_Secondary_NodeLeaf_I);

        executeGunFuOnGroundSelector.AddtoChildNode(gunFuExecute_OnGround_Secondary_LayUp_I_NodeLeaf);
        executeGunFuOnGroundSelector.AddtoChildNode(gunFuExecute_OnGround_Secondary_LayDown_I_NodeLeaf);
        executeGunFuOnGroundSelector.AddtoChildNode(gunFuExecute_OnGround_Primary_LayUp_I_NodeLeaf);
        executeGunFuOnGroundSelector.AddtoChildNode(gunFuExecute_OnGround_Primary_LayDown_I_NodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }
    public void ChangeNode(PlayerStateNodeLeaf playerStateNodeLeaf)
    {
        curNodeLeaf.Exit();
        curNodeLeaf = playerStateNodeLeaf;
        curNodeLeaf.Enter();
    }
}
