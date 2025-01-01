using UnityEngine;

public interface IMovementCompoent
{
    public GameObject userMovement { get; set; }
    public Vector2 moveVelocity_World { get; set; }
    public Vector2 moveVelocity_Local { get; set; }
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
    public void Move(Vector2 MoveDir, float velocity, Quaternion rotation);
    public void Sprint(Vector2 SprintDir);
    public void Freez();
    public void Stand();
    public void Crouch();
    public void Dodge();

}
