using System;
using UnityEngine;

public class PlayerSprintChangeDirectionNode : PlayerStateNodeLeaf
{
    private PlayerMovement playerMovement => player._movementCompoent as PlayerMovement;
    private readonly AnimationTriggerEventPlayer _animTriggerPlayer;

    private Vector3 _targetDir;
    private bool _isSprintOutPhase;
    private float _rotateRate;
    private float _sprintWeight;

    private float sprintMaxSpeed => player.sprintMaxSpeed;
    private float sprintAccelerate => player.sprintAccelerate;
    private float breakDecelerate => player.breakDecelerate;

    private const string SPRINT_OUT_EVENT = "SprintOut";
    private const float ROTATE_RAMP_SPEED_BRAKE = 1f;
    private const float ROTATE_RAMP_SPEED_SPRINT_OUT = 2f;

    public PlayerSprintChangeDirectionNode(Player player, Func<bool> preCondition,
        AnimationTriggerEventSCRP changeDirSCRP) : base(player, preCondition)
    {
        _animTriggerPlayer = new AnimationTriggerEventPlayer(changeDirSCRP);
        _animTriggerPlayer.SubscribeEvent(SPRINT_OUT_EVENT, OnSprintOutPhase);
    }

    public override void Enter()
    {
        _targetDir = player.inputMoveDir_World;
        _isSprintOutPhase = false;
        _rotateRate = 0;
        _sprintWeight = playerMovement.movementAttribute.stanceRate;
        _animTriggerPlayer.Rewind();
        base.Enter();
    }

    public override void UpdateNode()
    {
        _animTriggerPlayer.UpdatePlay(Time.deltaTime);
        if (_animTriggerPlayer.IsPlayFinish())
            isComplete = true;
        base.UpdateNode();
    }

    public override void FixedUpdateNode()
    {
        if (_isSprintOutPhase)
        {
            _rotateRate = Mathf.Clamp01(_rotateRate + Time.fixedDeltaTime * ROTATE_RAMP_SPEED_SPRINT_OUT);
            SprintOut();
        }
        else
        {
            _rotateRate = Mathf.Clamp01(_rotateRate + Time.fixedDeltaTime * ROTATE_RAMP_SPEED_BRAKE);
            Brake();
        }
        base.FixedUpdateNode();
    }

    public override bool IsReset()
    {
        if (player.isDead) return true;
        return IsComplete();
    }

    private void Brake()
    {
        Vector3 brakingVelocity = Vector3.MoveTowards(
            playerMovement.curMoveVelocity_World,
            Vector3.zero,
            breakDecelerate * Time.fixedDeltaTime);

        playerMovement.UpdateMoveToDirWorld(brakingVelocity, breakDecelerate, MoveMode.IgnoreMomentumDirection);
        playerMovement.SetRotateToDirWorldSlerp(_targetDir.normalized, _rotateRate * 0.5f);
    }

    private void SprintOut()
    {
        playerMovement.SetStanceWeight(_sprintWeight);
        playerMovement.UpdateMoveToDirWorld(
            _targetDir * sprintMaxSpeed * _sprintWeight,
            sprintAccelerate * _sprintWeight,
            MoveMode.IgnoreMomentumDirection);
        playerMovement.SetRotateToDirWorldSlerp(_targetDir.normalized, _rotateRate);
    }

    private void OnSprintOutPhase()
    {
        _targetDir = player.inputMoveDir_World.magnitude > 0 ? player.inputMoveDir_World : _targetDir;
        _isSprintOutPhase = true;
        _rotateRate = 0;
    }
}
