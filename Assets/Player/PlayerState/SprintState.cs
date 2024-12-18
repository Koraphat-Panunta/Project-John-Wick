using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SprintState : CharacterState
{
    Transform cameraTrans;
    private PlayerStateManager playerStateManager;
    private PlayerMovement playerMovement;
    public SprintState(Player player)
    {
        base.player = player;
        this.playerStateManager = player.playerStateManager;
        this.playerMovement = player.playerMovement;
    }
    public override void EnterState()
    {
        if(cameraTrans == null)
        {
            cameraTrans = Camera.main.transform;
        }
    }

    public override void ExitState()
    {

    }
    public override void FrameUpdateState(PlayerStateManager stateManager)
    {
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.Sprint);
    }

    public override void PhysicUpdateState(PlayerStateManager stateManager)
    {
        InputPerformed();
        playerMovement.RotateCharacter(playerMovement.inputVelocity_World.normalized,playerMovement.rotate_Speed*0.67f);
        playerMovement.ONE_DirMovingCharacter();
        //RotateTowards(TransformDirectionObject(new Vector3(playerController.input.movement.ReadValue<Vector2>().x,0,playerController.input.movement.ReadValue<Vector2>().y),Camera.main.transform.forward));
    }
    protected float rotationSpeed = 5.0f;
    protected void RotateTowards(Vector3 direction)
    {
        // Ensure the direction is normalized
        direction.Normalize();

        // Flatten the direction vector to the XZ plane to only rotate around the Y axis
        direction.y = 0;

        // Check if the direction is not zero to avoid setting a NaN rotation
        if (direction != Vector3.zero)
        {
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target rotation
            base.player.transform.rotation = Quaternion.Slerp(base.player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    protected override void InputPerformed()
    {

        if (player.isSprint==false)
        {
            this.playerStateManager.ChangeState(this.playerStateManager.move);
        }
        if(player.isReload)
        {
            player.weaponCommand.Reload(player.weaponBelt.ammoProuch);
        }
        if(player.isSwapShoulder)
        {
            player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwapShoulder);
        }
        player.weaponCommand.LowReady();
    }
}
