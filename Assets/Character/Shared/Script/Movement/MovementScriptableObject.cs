using UnityEngine;

[CreateAssetMenu(fileName = "MovementScriptableObject", menuName = "ScriptableObjects/MovementScriptableObject")]
public class MovementScriptableObject : ScriptableObject
{
    [Range(0, 100)]
    public float StandMoveAccelerate;
    [Range(0, 100)]
    public float StandMoveMaxSpeed;

    [Range(0, 100)]
    public float CrouchMoveAccelerate;
    [Range(0, 100)]
    public float CrouchMoveMaxSpeed;

    [Range(0, 100)]
    public float moveRotateSpeed;

    [Range(0, 100)]
    public float sprintAccelerate;
    [Range(0, 100)]
    public float sprintMaxSpeed;
    [Range(0, 100)]
    public float sprintRotateSpeed;

    [Range(0, 100)]
    public float breakDecelerate;
    [Range(0, 100)]
    public float breakMaxSpeed;

    [Range(0, 100)]
    public float dodgeImpluseForce;
    [Range(0, 100)]
    public float dodgeInAirStopForce;
    [Range(0, 100)]
    public float dodgeOnGroundStopForce;
}
