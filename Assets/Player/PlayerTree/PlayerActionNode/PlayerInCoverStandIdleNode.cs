using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInCoverStandIdleNode : PlayerActionNode
{
    public PlayerInCoverStandIdleNode(Player player) : base(player) { }
    public override List<PlayerNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    private bool warping;

    public override void FixedUpdate()
    {
        PlayerMovement playerMovement = base.player.playerMovement;
        MovementWarping movementWarping = playerMovement.movementWarping;
        bool isAiming = player.isAiming;
        CoverDetection coverDetection = player.coverDetection;

        if (isAiming == false)
            WarpingToCoverPos();
        else
            WarpingToAimPos();

        player.NotifyObserver(player, SubjectPlayer.PlayerAction.Idle);
        playerMovement.FreezingCharacter();

        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if (player.playerStance != Player.PlayerStance.stand
            || player.isSprint == true
            || player.isInCover == false
            || player.inputMoveDir_Local.magnitude > 0)
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

    private void WarpingToAimPos()
    {
        PlayerMovement playerMovement = player.playerMovement;
        CoverDetection coverDetection = player.coverDetection;

        if(player.currentWeapon == null){
            playerMovement.RotateCharacter(Camera.main.transform.forward, 6);
            return;
        }

        if (warping == true){

            Vector3 warpDesPos = new Vector3(
                player.coverDetection.aimPos.x, 
                player.transform.position.y, 
                player.coverDetection.aimPos.z);

            Vector3 warpDesOffsetPos = player.coverDetection.obstacleSurfaceDir.normalized * 0.6f;

            playerMovement.WarpingMovementCharacter(warpDesPos, warpDesOffsetPos, 2f);

            if (Vector3.Distance(player.transform.position, warpDesPos + warpDesOffsetPos) < 0.07f)
                warping = false;
        }
        else if (player.currentWeapon.aimingWeight < 1
            && coverDetection.GetAimPos(player.curShoulderSide)
            )
            warping = true;

        playerMovement.RotateCharacter(Camera.main.transform.forward, 6);
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

            playerMovement.WarpingMovementCharacter(warpDesPos, warpDesOffsetPos, 2f);

            if (Vector3.Distance(player.transform.position, warpDesPos + warpDesOffsetPos) < 0.07f
                || playerMovement.inputVelocity_World != Vector3.zero)
                warping = false;
            
            
        }
        else if (player.currentWeapon.aimingWeight > 0
            && playerMovement.inputVelocity_World == Vector3.zero
            && coverDetection.GetAimPos(player.curShoulderSide))
            warping = true;
        

        playerMovement.RotateCharacter(coverDetection.obstacleSurfaceDir * -1, 6);
    }

    private  void InputPerformed()
    {
        new WeaponInput().InputWeaponUpdate(player);
        if (player.isSwapShoulder)
        {
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwapShoulder);
        }
    }
}
