using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintNode : PlayerActionNodeLeaf
{
    public override List<PlayerNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    private PlayerMovement playerMovement;
    private float sprintMaxSpeed => player.sprintMaxSpeed;
    private float sprintAcceletion => player.sprintAccelerate;
    private float sprintRotateSpeed => player.sprintRotateSpeed;

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
        playerMovement.MoveToDirWorld(player.transform.forward, sprintAcceletion, sprintMaxSpeed, IMovementCompoent.MoveMode.IgnoreMomenTum);
        playerMovement.RotateToDirWorld(player.inputMoveDir_World.normalized, sprintRotateSpeed );

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
        if (player.isSwapShoulder)
        {
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwapShoulder);
        }
    }
   
}
