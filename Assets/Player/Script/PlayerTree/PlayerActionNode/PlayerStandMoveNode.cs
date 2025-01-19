using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandMoveNode : PlayerActionNodeLeaf
{
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
        playerMovement.OMNI_DirMovingCharacter();
        playerMovement.RotateCharacter(Camera.main.transform.forward, player.playerMovement.rotate_Speed);

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
        new WeaponInput().InputWeaponUpdate(player);
        if (player.isSwapShoulder)
        {
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwapShoulder);
        }
    }
}
