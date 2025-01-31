using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandIdleNode : PlayerActionNodeLeaf
{
    private float breakSpeed = 0.4f;
    public PlayerStandIdleNode(Player player) : base(player) { }

    public override List<PlayerNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.Idle);
        base.Enter();
    }
    public override void FixedUpdate()
    {
        PlayerMovement playerMovement = base.player.playerMovement;

        playerMovement.MoveToDirWorld(Vector3.zero, breakSpeed,breakSpeed);
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if (player._triggerGunFu)
            return true;

        if (player.playerStance != Player.PlayerStance.stand
            ||player.isInCover == true
            ||player.inputMoveDir_Local.magnitude > 0
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
