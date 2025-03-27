using System;
using System.Collections.Generic;

using UnityEngine;

public class PlayerInCoverStandMoveNode : PlayerStateNodeLeaf
{
   

    private bool warping;

    public PlayerInCoverStandMoveNode(Player player, Func<bool> preCondition) : base(player, preCondition)
    {
    }

    public override void Enter()
    {
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.StandMove);
        base.Enter();
    }
    public override void UpdateNode()
    {
        InputPerformed();
        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        PlayerMovement playerMovement = base.player.playerMovement;
        

        CoverDetection coverDetection = player.coverDetection;

        playerMovement.MoveToDirWorld(player.inputMoveDir_World, player.StandMoveAccelerate, player.StandMoveMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
        base.FixedUpdateNode();
    }

    private void WarpingToAimPos()
    {
        PlayerMovement playerMovement = player.playerMovement;
        CoverDetection coverDetection = player.coverDetection;

        if (player._currentWeapon == null)
        {
            playerMovement.RotateToDirWorld(Camera.main.transform.forward, 6);
            return;
        }

        if (warping == true)
        {

            Vector3 warpDesPos = new Vector3(
                player.coverDetection.aimPos.x,
                player.transform.position.y,
                player.coverDetection.aimPos.z);

            Vector3 warpDesOffsetPos = player.coverDetection.obstacleSurfaceDir.normalized * 0.6f;

            playerMovement.SnapingMovement(warpDesPos, warpDesOffsetPos, 2f);

            if (Vector3.Distance(player.transform.position, warpDesPos + warpDesOffsetPos) < 0.07f)
                warping = false;
        }
        else if (player.weaponManuverManager.aimingWeight < 1
            && coverDetection.GetAimPos(player.curShoulderSide)
            )
            warping = true;

        playerMovement.RotateToDirWorld(Camera.main.transform.forward, 6);
    }
    private void WarpingToCoverPos()
    {
        PlayerMovement playerMovement = player.playerMovement;
        CoverDetection coverDetection = player.coverDetection;

        if (warping == true)
        {
            Vector3 warpDesPos = new Vector3(
                player.coverDetection.coverPos.x,
                player.transform.position.y,
                player.coverDetection.coverPos.z);

            Vector3 warpDesOffsetPos = player.coverDetection.obstacleSurfaceDir.normalized * 0.6f;

            playerMovement.SnapingMovement(warpDesPos, warpDesOffsetPos, 2f);

            if (Vector3.Distance(player.transform.position, warpDesPos + warpDesOffsetPos) < 0.07f
                || playerMovement.moveInputVelocity_World != Vector3.zero)
                warping = false;


        }
        else if (player.weaponManuverManager.aimingWeight > 0
            && playerMovement.moveInputVelocity_World == Vector3.zero
            && coverDetection.GetAimPos(player.curShoulderSide))
            warping = true;


        Vector3 coverStanceDir = coverDetection.obstacleSurfaceDir * -1;
        if (player.curShoulderSide == Player.ShoulderSide.Left)
            coverStanceDir = Quaternion.Euler(0, -45, 0) * coverDetection.obstacleSurfaceDir * -1;
        else if (player.curShoulderSide == Player.ShoulderSide.Right)
            coverStanceDir = Quaternion.Euler(0, 45, 0) * coverDetection.obstacleSurfaceDir * -1;

        playerMovement.RotateToDirWorld(coverStanceDir , 6);
    }

    private  void InputPerformed()
    {
        if (player.isSwapShoulder)
        {
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwapShoulder);
        }
    }
}
