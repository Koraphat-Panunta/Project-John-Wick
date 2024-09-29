using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInCover : MoveState,IObserverPlayer
{
    protected CoverDetection coverDetection;
    private bool isAiming;
    public MoveInCover(Player player) : base(player)
    {
        coverDetection = player.coverDetection;
    }
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdateState(PlayerStateManager stateManager)
    {
        base.FrameUpdateState(stateManager);
    }

    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(playerAction == SubjectPlayer.PlayerAction.Aim)
        {
            isAiming = true;
        }
        else if (playerAction == SubjectPlayer.PlayerAction.LowReady)
        {
            isAiming = false;
        }
    }

    public override void PhysicUpdateState(PlayerStateManager stateManager)
    {
        Debug.Log("Move in Cover");
        PlayerMovement playerMovement = base.player.playerMovement;
        playerMovement.OMNI_DirMovingCharacter();
        if (isAiming == false)
        {
            playerMovement.RotateCharacter(coverDetection.obstacleSurfaceDir * -1, 6);
        }
        else
        {
            playerMovement.RotateCharacter(Camera.main.transform.forward, 6);
        }
    }

    protected override void InputPerformed()
    {
        base.InputPerformed();
    }
}
