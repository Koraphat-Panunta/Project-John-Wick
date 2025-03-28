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
    public PlayerStandIdleNode playerStandIdleNode { get; private set; }
    public PlayerStandMoveNode playerStandMoveNode { get; private set; }
    public PlayerSelectorStateNode crouchSelectorNode { get; private set; }
    public PlayerCrouch_Move_NodeLeaf playerCrouch_Move_NodeLeaf { get; private set; }
    public PlayerCrouch_Idle_NodeLeaf playerCrouch_Idle_NodeLeaf { get; private set; }
    public PlayerInCoverStandMoveNode playerInCoverStandMoveNode { get; private set; }
    public PlayerInCoverStandIdleNode playerInCoverStandIdleNode { get; private set; }

    public GunFuExecuteNodeLeaf gunFuExecuteNodeLeaf { get; private set; }
    public Hit1GunFuNode Hit1gunFuNode { get; private set; }
    public HumanShield_GunFuInteraction_NodeLeaf humanShield_GunFuInteraction_NodeLeaf { get; private set; }
    public WeaponDisarm_GunFuInteraction_NodeLeaf weaponDisarm_GunFuInteraction_NodeLeaf { get; private set; }
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

        crouchSelectorNode = new PlayerSelectorStateNode(this.player,
            () => this.player.playerStance == PlayerStance.crouch);

        playerSprintNode = new PlayerSprintNode(this.player, () => { return this.player.isSprint; });

        standIncoverSelector = new PlayerSelectorStateNode(this.player,
            () => { return this.player.isInCover; });

        playerStandMoveNode = new PlayerStandMoveNode(this.player,
            () =>{ return this.player.inputMoveDir_Local.magnitude > 0; });

        playerStandIdleNode = new PlayerStandIdleNode(this.player, 
            () => true);

        playerCrouch_Move_NodeLeaf = new PlayerCrouch_Move_NodeLeaf(this.player,
           () => this.player.inputMoveDir_Local.magnitude > 0);

        playerCrouch_Idle_NodeLeaf = new PlayerCrouch_Idle_NodeLeaf(this.player,
            () => this.player.inputMoveDir_Local.magnitude <= 0 || true);

        playerInCoverStandMoveNode = new PlayerInCoverStandMoveNode(this.player,
            () =>{ return this.player.inputMoveDir_Local.magnitude > 0; });

        playerInCoverStandIdleNode = new PlayerInCoverStandIdleNode(this.player, 
            () => true);


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
        weaponDisarm_GunFuInteraction_NodeLeaf = new WeaponDisarm_GunFuInteraction_NodeLeaf(this.player,
            () => 
            {
                if(player.isAimingCommand && player.attackedAbleGunFu != null)
                {
                    if(player.attackedAbleGunFu._weaponAdvanceUser._currentWeapon != null)
                        return true;
                }
                return false;
            }
            );
        humanShield_GunFuInteraction_NodeLeaf = new HumanShield_GunFuInteraction_NodeLeaf(this.player,
            () => this.player.isAimingCommand
            && this.player.attackedAbleGunFu != null
            , this.player.humanShield);
        humanThrow_GunFuInteraction_NodeLeaf = new HumanThrowGunFuInteractionNodeLeaf(this.player,
            () => this.player.isAimingCommand == false 
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

        stanceSelectorNode.AddtoChildNode(playerDodgeRollStateNodeLeaf);
        stanceSelectorNode.AddtoChildNode(standSelectorNode);
        stanceSelectorNode.AddtoChildNode(crouchSelectorNode);

        standSelectorNode.AddtoChildNode(gunFuExecuteNodeLeaf);
        standSelectorNode.AddtoChildNode(Hit1gunFuNode);
        standSelectorNode.AddtoChildNode(playerSprintNode);
        standSelectorNode.AddtoChildNode(standIncoverSelector);
        standSelectorNode.AddtoChildNode(playerStandMoveNode);
        standSelectorNode.AddtoChildNode(playerStandIdleNode);

        playerDodgeRollStateNodeLeaf.AddTransitionNode(dodgeSpinKicklGunFuNodeLeaf);

        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(weaponDisarm_GunFuInteraction_NodeLeaf);
        dodgeSpinKicklGunFuNodeLeaf.AddTransitionNode(Hit2GunFuNode);

        Hit1gunFuNode.AddTransitionNode(Hit2GunFuNode);
        Hit1gunFuNode.AddTransitionNode(weaponDisarm_GunFuInteraction_NodeLeaf);
        humanShield_GunFuInteraction_NodeLeaf.AddTransitionNode(humanThrow_GunFuInteraction_NodeLeaf);
        Hit2GunFuNode.AddTransitionNode(knockDown_GunFuNode);
        Hit2GunFuNode.AddTransitionNode(humanShield_GunFuInteraction_NodeLeaf);

        crouchSelectorNode.AddtoChildNode(playerCrouch_Move_NodeLeaf);
        crouchSelectorNode.AddtoChildNode(playerCrouch_Idle_NodeLeaf);

        standIncoverSelector.AddtoChildNode(playerInCoverStandMoveNode);
        standIncoverSelector.AddtoChildNode(playerInCoverStandIdleNode);

        startNodeSelector.FindingNode(out INodeLeaf playerActionNode);
        curNodeLeaf = playerActionNode as PlayerStateNodeLeaf;
    }
    public void ChangeNode(PlayerStateNodeLeaf playerStateNodeLeaf)
    {
        curNodeLeaf.Exit();
        curNodeLeaf = playerStateNodeLeaf;
        curNodeLeaf.Enter();
    }
}
