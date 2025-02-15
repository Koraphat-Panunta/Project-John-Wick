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

    public PlayerSelectorStateNode standSelectorNode { get; private set; }

    public PlayerSprintNode playerSprintNode { get; private set; }
    public PlayerSelectorStateNode standIncoverSelector { get; private set; }
    public PlayerStandIdleNode playerStandIdleNode { get; private set; }
    public PlayerStandMoveNode playerStandMoveNode { get; private set; }
    public PlayerSelectorStateNode crouchSelectorNode { get; private set; }
    public PlayerCrouch_Move_NodeLeaf playerCrouch_Move_NodeLeaf { get; private set; }
    public PlayerCrouch_Idle_NodeLeaf playerCrouch_Idle_NodeLeaf { get; private set; }
    public PlayerInCoverStandMoveNode playerInCoverStandMoveNode { get; private set; }
    public PlayerInCoverStandIdleNode playerInCoverStandIdleNode { get; private set; }

    public Hit1GunFuNode Hit1gunFuNode { get; private set; }
    public HumanShield_GunFuInteraction_NodeLeaf humanShield_GunFuInteraction_NodeLeaf { get; private set; }
    public Hit2GunFuNode Hit2GunFuNode { get; private set; }
    public KnockDown_GunFuNode knockDown_GunFuNode { get; private set; }
    public void InitailizedNode()
    {
        startNodeSelector = new PlayerSelectorStateNode(this.player, () => true);

        stanceSelectorNode = new PlayerSelectorStateNode(this.player,
            () => { return true; });

        standSelectorNode = new PlayerSelectorStateNode(this.player,
            () => { return this.player.playerStance == PlayerStance.stand; });

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

       

        Hit1gunFuNode = new Hit1GunFuNode(this.player, 
            () => this.player._triggerGunFu 
            && this.player.attackedAbleGunFu != null
            ,this.player.hit1);
        humanShield_GunFuInteraction_NodeLeaf = new HumanShield_GunFuInteraction_NodeLeaf(this.player,
            () => this.player.isAimingCommand
            && this.player.attackedAbleGunFu != null
            , this.player.humanShield);
        Hit2GunFuNode = new Hit2GunFuNode(this.player, 
            () => this.player._triggerGunFu
            && this.player.attackedAbleGunFu != null
            , this.player.hit2);
        knockDown_GunFuNode = new KnockDown_GunFuNode(this.player, () => this.player._triggerGunFu 
        && this.player.attackedAbleGunFu != null
        , this.player.knockDown);

        startNodeSelector.AddtoChildNode(stanceSelectorNode);

        stanceSelectorNode.AddtoChildNode(standSelectorNode);
        stanceSelectorNode.AddtoChildNode(crouchSelectorNode);

        standSelectorNode.AddtoChildNode(Hit1gunFuNode);
        standSelectorNode.AddtoChildNode(playerSprintNode);
        standSelectorNode.AddtoChildNode(standIncoverSelector);
        standSelectorNode.AddtoChildNode(playerStandMoveNode);
        standSelectorNode.AddtoChildNode(playerStandIdleNode);

        Hit1gunFuNode.AddTransitionNode(Hit2GunFuNode);
        Hit1gunFuNode.AddTransitionNode(humanShield_GunFuInteraction_NodeLeaf);
        Hit2GunFuNode.AddTransitionNode(knockDown_GunFuNode);

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
