using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintNode : PlayerActionNodeLeaf
{
    public override List<PlayerNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    private PlayerMovement playerMovement;
    private float sprintMaxSpeed = 5.6f;
    private float sprintAcceletion = 0.8f;
    private float sprintRotateSpeed = 3;

    public PlayerSprintNode(Player player) : base(player) 
    {
        playerMovement = player.playerMovement;
    }
    public override void Enter()
    {
        player.playerStance = Player.PlayerStance.stand;
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.Sprint);
        base.Enter();
    }
    public override void Update()
    {
        InputPerformed();

        base.Update();
    }
    public override void FixedUpdate()
    {
        playerMovement.RotateCharacter(playerMovement.moveInputVelocity_World.normalized, sprintRotateSpeed * 0.67f);
        playerMovement.MoveToDirWorld(playerMovement.forwardDir, sprintAcceletion, sprintMaxSpeed);
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if(player._triggerGunFu)
            return true;

        if(player.isSprint == false
            ||player.playerStance != Player.PlayerStance.stand)
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
        player.weaponCommand.CancleTrigger();
        player.weaponCommand.LowReady();
    }
}
