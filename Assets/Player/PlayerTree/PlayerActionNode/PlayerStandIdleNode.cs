using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandIdleNode : PlayerActionNode
{
    
    public PlayerStandIdleNode(Player player) : base(player) { }

    public override List<PlayerNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void FixedUpdate()
    {
        PlayerMovement playerMovement = base.player.playerMovement;
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.Idle);
        playerMovement.FreezingCharacter();
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if(player.playerStance != Player.PlayerStance.stand
            &&player.isInCover == true
            &&player.inputMoveDir_Local.magnitude >0
            )
            return true;
        else
        return false;
    }

    public override bool PreCondition()
    {
        return true;
    }

    public override void Update()
    {
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.Idle);
        base.Update();
    }

    private void InputPerformed()
    {
        new WeaponInput().InputWeaponUpdate(player);
        if (player.isSwapShoulder)
        {
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwapShoulder);
        }
    }
}
