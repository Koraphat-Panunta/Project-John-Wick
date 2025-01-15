using UnityEngine;

public interface IMovementCompoent
{
    public GameObject userMovement { get; set; }
    public Vector3 moveInputVelocity_World { get; set; }
    public Vector3 curMoveVelocity_World { get; set; }
    public Vector3 moveInputVelocity_Local { get; set; }
    public Vector3 curMoveVelocity_Local { get; set; }
    public Vector3 lookRotation { get; set; }

    public float _moveAccelerate { get; set; } // 1
    public float _moveMaxSpeed { get; set; }//3

    public float _sprintAccelerate { get; set; }//2.5
    public float _sprintMaxSpeed { get; set; }//5.5
    public float _rotateSpeed { get; set; }
    public EnemyStateSelectorNode stanceSelector { get; set; }
    public EnemyStateSelectorNode standStateSelector { get; set; }
    public EnemyStateSelectorNode crouchStateSelector { get; set; }
    public EnemyStandIdleStateNode standIdleState { get; set; }
    public EnemyStandMoveStateNode standMoveState { get; set; }
    public EnemySprintStateNode sprintState { get; set; }
    public enum Stance
    {
        Stand,
        Crouch,
        Prone
    }
    public Stance curStance { get; set; }

    public bool isSprint { get; set; }
    public void InitailizedMovementComponent();

}
