using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandMoveNode : PlayerActionNodeLeaf
{
    private float moveSpeed = 0.4f;
    private float moveMaxSpeed = 2.8f;
    private float moveRotateSpeed = 3f;
    public override List<PlayerNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }
    public PlayerStandMoveNode(Player player) : base(player) { }
    public override void Enter()
    {
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.Move);
        base.Enter();
    }
    public override void FixedUpdate()
    {

        PlayerMovement playerMovement = base.player.playerMovement;
        playerMovement.MoveToDirLocal(player.inputMoveDir_Local, moveSpeed, moveMaxSpeed);
        playerMovement.RotateToDirWorld(Camera.main.transform.forward, moveRotateSpeed);

        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if (player._triggerGunFu)
            return true;

        if (player.playerStance != Player.PlayerStance.stand
            ||player.isSprint
            ||player.isInCover == true
            ||player.inputMoveDir_Local.magnitude<=0)
            return true;
        return false;
    }

    public override bool PreCondition()
    {
        return player.inputMoveDir_Local.magnitude > 0;
    }

    public override void Update()
    {
        InputPerformed();
        base.Update();
    }
    private void InputPerformed()
    {
        if (player.isSwapShoulder)
        {
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwapShoulder);
        }
    }
}
