using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveInCover : MoveState,IObserverPlayer
{
    protected CoverDetection coverDetection;
    private bool isAiming;
    private bool warping;
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
        if (base.player.coverDetection.CheckingObstacleToward(base.player.RayCastPos.transform.position, base.player.RayCastPos.transform.forward) == false)
        {
         
            playerStateManager.move = playerStateManager.normalMove;
            playerStateManager.ChangeState(playerStateManager.move);

        }
        base.FrameUpdateState(stateManager);
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

    public override void PhysicUpdateState(PlayerStateManager stateManager)
    {
        PlayerMovement playerMovement = base.player.playerMovement;
        MovementWarping movementWarping = stateManager.player.playerMovement.movementWarping;
        playerMovement.OMNI_DirMovingCharacter();
        if (isAiming == false)
        {
        
            //Debug.Log("Move in Cover Aiming = false");
            playerMovement.RotateCharacter(coverDetection.obstacleSurfaceDir * -1, 6);
            if (player.curentWeapon != null)
            {
                if (warping == true)
                {
                    //Debug.Log("Move in Cover Warping = true");
                    Vector3 warpDesPos = new Vector3(player.coverDetection.coverPos.x, player.transform.position.y, player.coverDetection.coverPos.z);
                    Vector3 warpDesOffsetPos = player.coverDetection.obstacleSurfaceDir.normalized * 0.6f;
                    playerMovement.WarpingMovementCharacter(warpDesPos, warpDesOffsetPos, 2f);

                    if (Vector3.Distance(player.transform.position, warpDesPos + warpDesOffsetPos) < 0.07f || playerMovement.inputDirection_World != Vector3.zero)
                    {
                        //Debug.Log("Move in Cover Warping finish or cancle");
                        warping = false;
                    }
                }
                else if (player.curentWeapon.weapon_StanceManager.AimingWeight > 0
                    && playerMovement.inputDirection_World == Vector3.zero
                    && coverDetection.GetAimPos(player.curShoulderSide))
                {
                    //Debug.Log("Move in Cover Warping ");
                    warping = true;
                }
            }
        }
        else
        {
           
            if (player.curentWeapon != null)
            {
                if (warping == true)
                {
                    Vector3 warpDesPos = new Vector3(player.coverDetection.aimPos.x, player.transform.position.y, player.coverDetection.aimPos.z);
                    Vector3 warpDesOffsetPos = player.coverDetection.obstacleSurfaceDir.normalized * 0.6f;
                    playerMovement.WarpingMovementCharacter(warpDesPos, warpDesOffsetPos, 2f);

                    if (Vector3.Distance(player.transform.position, warpDesPos + warpDesOffsetPos) < 0.07f || playerMovement.inputDirection_World != Vector3.zero)
                    {
                        warping = false;
                    }
                }
                else if (player.curentWeapon.weapon_StanceManager.AimingWeight < 1
                    && playerMovement.inputDirection_World == Vector3.zero
                    && coverDetection.GetAimPos(player.curShoulderSide))
                {
                    warping = true;
                }
            }
            playerMovement.RotateCharacter(Camera.main.transform.forward, 6);
        }
    }

    protected override void InputPerformed()
    {
        base.InputPerformed();
    }
}
