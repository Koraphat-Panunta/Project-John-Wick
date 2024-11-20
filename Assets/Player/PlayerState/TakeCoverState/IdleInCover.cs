using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleInCover : IdleState,IObserverPlayer
{
    private CoverDetection coverDetection;
    private bool isAiming;
    private bool warping;
    public IdleInCover(Player player) : base(player)
    {
        coverDetection = player.coverDetection;
        player.AddObserver(this);
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
        Debug.Log("IdleinCover");
        if (base.player.coverDetection.CheckingObstacleToward(base.player.RayCastPos.transform.position, base.player.RayCastPos.transform.forward * 0.3f) == false)
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
        MovementWarping movementWarping = stateManager.player.playerMovement.movementWarping;
        if (isAiming == false)
        {
            Debug.Log("Is Aiming = true");
            if (warping == true)
            {
                Vector3 warpDesPos = new Vector3(player.coverDetection.coverPos.x, player.transform.position.y, player.coverDetection.coverPos.z);
                Vector3 warpDesOffsetPos = player.coverDetection.obstacleSurfaceDir.normalized * 0.6f;
                playerMovement.WarpingMovementCharacter(warpDesPos, warpDesOffsetPos, 2f);

                if (Vector3.Distance(player.transform.position, warpDesPos + warpDesOffsetPos) < 0.07f || playerMovement.inputDirection_World != Vector3.zero)
                {
                    warping = false;
                }
            }
            else if (player.currentWeapon.weapon_StanceManager.AimingWeight > 0
                && playerMovement.inputDirection_World == Vector3.zero
                && coverDetection.GetAimPos(player.curShoulderSide))
            {
                warping = true;
            }
            playerMovement.RotateCharacter(coverDetection.obstacleSurfaceDir * -1, 6);
        }
        else
        {
            Debug.Log("Is Aiming = true");
            if (player.currentWeapon != null)
            {
                if (warping == true)
                {
                    Vector3 warpDesPos = new Vector3(player.coverDetection.aimPos.x, player.transform.position.y, player.coverDetection.aimPos.z);
                    Vector3 warpDesOffsetPos = player.coverDetection.obstacleSurfaceDir.normalized * 0.6f;
                    playerMovement.WarpingMovementCharacter(warpDesPos, warpDesOffsetPos, 2f);

                    if (Vector3.Distance(player.transform.position, warpDesPos + warpDesOffsetPos) < 0.07f)
                    {
                        warping = false;
                    }
                }
                else if (player.currentWeapon.weapon_StanceManager.AimingWeight < 1
                    && coverDetection.GetAimPos(player.curShoulderSide))
                {
                    warping = true;
                }
            }
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
