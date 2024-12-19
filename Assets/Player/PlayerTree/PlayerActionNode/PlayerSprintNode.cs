using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintNode : PlayerActionNode
{
    public override List<PlayerNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    private PlayerMovement playerMovement;

    public PlayerSprintNode(Player player) : base(player) 
    {
        playerMovement = player.playerMovement;
    }
    public override void Enter()
    {
        player.playerStance = Player.PlayerStance.stand;
        base.Enter();
    }
    public override void Update()
    {
        InputPerformed();
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.Sprint);
        base.Update();
    }
    public override void FixedUpdate()
    {
        playerMovement.RotateCharacter(playerMovement.inputVelocity_World.normalized, playerMovement.rotate_Speed * 0.67f);
        playerMovement.ONE_DirMovingCharacter();
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if(player.isSprint == false
            &&player.playerStance != Player.PlayerStance.stand)
            return true;
        else
            return false;
    }

    public override bool PreCondition()
    {
        return player.isSprint;
    }
    private  void InputPerformed()
    {
        if (player.isReload)
        {
            player.weaponCommand.Reload(player.weaponBelt.ammoProuch);
        }
        if (player.isSwapShoulder)
        {
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwapShoulder);
        }
        player.weaponCommand.LowReady();
    }
}
