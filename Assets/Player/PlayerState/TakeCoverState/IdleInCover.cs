using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleInCover : IdleState,IObserverPlayer
{
    private CoverDetection coverDetection;
    private bool isAiming;
    public IdleInCover(Player player) : base(player)
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
        if (base.player.coverDetection.CheckingObstacleToward(base.player.RayCastPos.transform.position, base.player.RayCastPos.transform.forward) == false)
        {
            Debug.Log("DetectCover");
            playerStateManager.idle = playerStateManager.normalIdle;
            playerStateManager.ChangeState(playerStateManager.idle);

        }
        base.FrameUpdateState(stateManager);
    }
    public override void PhysicUpdateState(PlayerStateManager stateManager)
    {
        Debug.Log("Idle in Cover");
        PlayerMovement playerMovement = base.player.playerMovement;
        if (isAiming == false)
        {
            playerMovement.RotateCharacter(coverDetection.obstacleSurfaceDir * -1, 6);
        }
        else
        {
            playerMovement.RotateCharacter(Camera.main.transform.forward, 6);
        }
        base.PhysicUpdateState(stateManager);
    }

    protected override void InputPerformed()
    {
        base.InputPerformed();
    }
    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if (playerAction == SubjectPlayer.PlayerAction.Aim)
        {
            isAiming = true;
        }
        else if (playerAction == SubjectPlayer.PlayerAction.LowReady)
        {
            isAiming = false;
        }
    }
}
