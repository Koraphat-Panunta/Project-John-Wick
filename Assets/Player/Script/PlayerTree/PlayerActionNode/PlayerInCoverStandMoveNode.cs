using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInCoverStandMoveNode : PlayerActionNodeLeaf
{
    public override List<PlayerNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    private bool warping;

    public PlayerInCoverStandMoveNode(Player player) : base(player) { }
    public override void Enter()
    {
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.Move);
        base.Enter();
    }
    public override void Update()
    {
        InputPerformed();
        base.Update();
    }
    public override void FixedUpdate()
    {
        PlayerMovement playerMovement = base.player.playerMovement;
        bool isAiming = player.weaponManuverManager.curWeaponManuverLeafNode is AimDownSightWeaponManuverNodeLeaf;
        CoverDetection coverDetection = player.coverDetection;

        if (isAiming == false)
            WarpingToCoverPos();
        else
            WarpingToAimPos();

        playerMovement.MoveToDirLocal(player.inputMoveDir_Local, player.moveAccelerate, player.moveMaxSpeed, IMovementCompoent.MoveMode.MaintainMomentum);
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if (player._triggerGunFu)
            return true;

        if (player.playerStance != Player.PlayerStance.stand
           || player.isSprint == true
           || player.isInCover == false
           || player.inputMoveDir_Local.magnitude <= 0)
            return true;
        else
            return false;
    }

    public override bool PreCondition()
    {
        return player.inputMoveDir_Local.magnitude >0;
    }
    private void WarpingToAimPos()
    {
        PlayerMovement playerMovement = player.playerMovement;
        CoverDetection coverDetection = player.coverDetection;

        if (player.currentWeapon == null)
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
