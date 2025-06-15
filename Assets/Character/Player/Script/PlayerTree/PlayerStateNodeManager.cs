using UnityEngine;
using static Player;

public class PlayerStateNodeManager : INodeManager
{
    public INodeLeaf curNodeLeaf { get; set; }
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

    public GunFuExecuteNodeLeaf gunFuExecuteNodeLeaf { get; private set; }
    public Hit1GunFuNode Hit1gunFuNode { get; private set; }
    public HumanShield_GunFuInteraction_NodeLeaf humanShield_GunFuInteraction_NodeLeaf { get; private set; }
    public RestrictGunFuStateNodeLeaf restrictGunFuStateNodeLeaf { get; private set; }
    public PlayerSelectorStateNode weaponDisarmSelector { get; private set; }
    public WeaponDisarm_GunFuInteraction_NodeLeaf primary_WeaponDisarm_GunFuInteraction_NodeLeaf { get; private set; }
    public WeaponDisarm_GunFuInteraction_NodeLeaf secondart_WeaponDisarm_GunFuInteraction_NodeLeaf { get; private set; }
    public HumanThrowGunFuInteractionNodeLeaf humanThrow_GunFuInteraction_NodeLeaf { get; private set; }
    public Hit2GunFuNode Hit2GunFuNode { get; private set; }
    public KnockDown_GunFuNode knockDown_GunFuNode { get; private set; }
    public DodgeSpinKicklGunFuNodeLeaf dodgeSpinKicklGunFuNodeLeaf { get; private set; }
    public void InitailizedNode()
    {
        startNodeSelector = new PlayerSelectorStateNode(this.player, () => true);

        deadNodeLeaf = new PlayerDeadNodeLeaf(this.player, () => player.isDead);

        stanceSelectorNode = new PlayerSelectorStateNode(this.player,
            () => { return true; });
        playerDodgeRollStateNodeLeaf = new PlayerDodgeRollStateNodeLeaf(player, () => player.triggerDodgeRoll);

        standSelectorNode = new PlayerSelectorStateNode(this.player,
            () => { return this.player.playerStance == PlayerStance.stand || player.isSprint; });
        playerSprintNode = new PlayerSprintNode(this.player, () => { return this.player.isSprint; });

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
            () => this.player.playerStance == PlayerStance.crouch);

        playerCrouch_Move_NodeLeaf = new PlayerCrouch_Move_NodeLeaf(this.player,
           () => this.player.inputMoveDir_Local.magnitude > 0);

        playerCrouch_Idle_NodeLeaf = new PlayerCrouch_Idle_NodeLeaf(this.player,
            () => this.player.inputMoveDir_Local.magnitude <= 0 || true);


        proneStanceSelector = new PlayerSelectorStateNode(this.player, 
            () => this.player.playerStance == PlayerStance.prone);
        playerGetUpStateNodeLeaf = new PlayerGetUpStateNodeLeaf(player.PlayerGetUpStateScriptableObject, this.player, 
            () => player.playerStance == PlayerStance.prone || true);

        gotGunFuAttackSelectorNodeLeaf = new PlayerSelectorStateNode(this.player, () => player._triggerHitedGunFu);
        playerBrounceOffGotAttackGunFuNodeLeaf = new PlayerBrounceOffGotAttackGunFuNodeLeaf(player.PlayerBrounceOffGotAttackGunFuScriptableObject, this.player,
            () => player.curAttackerGunFuNode is EnemySpinKickGunFuNodeLeaf);


        gunFuExecuteNodeLeaf = new GunFuExecuteNodeLeaf(player,
            () => 
            {
                if(player._triggerExecuteGunFu == false)
                    return false;
                if (player._currentWeapon == null || player._currentWeapon.bulletStore[BulletStackType.Chamber] <= 0)
                    return false;
                if (player.executedAbleGunFu == null)
                    return false;
                return true;
            }
            );
        Hit1gunFuNode = new Hit1GunFuNode(this.player, 
            () => this.player._triggerGunFu 
            && this.player.attackedAbleGunFu != null
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
                if(player._isPickingUpWeaponCommand && player.attackedAbleGunFu != null)
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
            , this.player.humanShield);
        humanThrow_GunFuInteraction_NodeLeaf = new HumanThrowGunFuInteractionNodeLeaf(this.player,
            () => this.player._isAimingCommand == false 
            || humanShield_GunFuInteraction_NodeLeaf.attackedAbleGunFu._isDead
            || humanShield_GunFuInteraction_NodeLeaf.isComplete,
            player.humanThrow);
        Hit2GunFuNode = new Hit2GunFuNode(this.player, 
            () => this.player._triggerGunFu
            && this.player.attackedAbleGunFu != null
            , this.player.hit2);
        knockDown_GunFuNode = new KnockDown_GunFuNode(this.player, () => this.player._triggerGunFu 
        && this.player.attackedAbleGunFu != null
        , this.player.knockDown);
        dodgeSpinKicklGunFuNodeLeaf = new DodgeSpinKicklGunFuNodeLeaf(this.player, () => this.player._triggerGunFu
        && this.player.attackedAbleGunFu != null, player.dodgeSpinKick);


        startNodeSelector.AddtoChildNode(deadNodeLeaf);
        startNodeSelector.AddtoChildNode(stanceSelectorNode);

        stanceSelectorNode.AddtoChildNode(gotGunFuAttackSelectorNodeLeaf);
        stanceSelectorNode.AddtoChildNode(playerDodgeRollStateNodeLeaf);
        stanceSelectorNode.AddtoChildNode(standSelectorNode);
        stanceSelectorNode.AddtoChildNode(crouchSelectorNode);
        stanceSelectorNode.AddtoChildNode(proneStanceSelector);

        standSelectorNode.AddtoChildNode(gunFuExecuteNodeLeaf);
        standSelectorNode.AddtoChildNode(Hit1gunFuNode);
        standSelectorNode.AddtoChildNode(playerSprintNode);
        standSelectorNode.AddtoChildNode(standIncoverSelector);
        standSelectorNode.AddtoChildNode(playerStandMoveNode);
        standSelectorNode.AddtoChildNode(playerStandIdleNode);

        weaponDisarmSelector.AddtoChildNode(primary_WeaponDisarm_GunFuInteraction_NodeLeaf);
        weaponDisarmSelector.AddtoChildNode(secondart_WeaponDisarm_GunFuInteraction_NodeLeaf);

        playerDodgeRollStateNodeLeaf.AddTransitionNode(dodgeSpinKicklGunFuNodeLeaf);
        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(restrictGunFuStateNodeLeaf);
        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(weaponDisarmSelector);
        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(Hit2GunFuNode);

        gotGunFuAttackSelectorNodeLeaf.AddtoChildNode(playerBrounceOffGotAttackGunFuNodeLeaf);

        Hit1gunFuNode.AddTransitionNode(Hit2GunFuNode);
        Hit1gunFuNode.AddTransitionNode(weaponDisarmSelector);
        Hit1gunFuNode.AddTransitionNode(restrictGunFuStateNodeLeaf);
        humanShield_GunFuInteraction_NodeLeaf.AddTransitionNode(humanThrow_GunFuInteraction_NodeLeaf);
        Hit2GunFuNode.AddTransitionNode(knockDown_GunFuNode);
        Hit2GunFuNode.AddTransitionNode(weaponDisarmSelector);
        Hit2GunFuNode.AddTransitionNode(humanShield_GunFuInteraction_NodeLeaf);

        crouchSelectorNode.AddtoChildNode(playerCrouch_Move_NodeLeaf);
        crouchSelectorNode.AddtoChildNode(playerCrouch_Idle_NodeLeaf);

        proneStanceSelector.AddtoChildNode(playerGetUpStateNodeLeaf);

        standIncoverSelector.AddtoChildNode(playerInCoverStandMoveNode);
        standIncoverSelector.AddtoChildNode(playerInCoverStandIdleNode);

        nodeManagerBehavior.SearchingNewNode(this);
    }
    public void ChangeNode(PlayerStateNodeLeaf playerStateNodeLeaf)
    {
        curNodeLeaf.Exit();
        curNodeLeaf = playerStateNodeLeaf;
        curNodeLeaf.Enter();
    }
}
