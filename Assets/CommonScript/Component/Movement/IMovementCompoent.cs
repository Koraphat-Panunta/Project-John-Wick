using UnityEngine;
using static IMovementCompoent;

public interface IMovementCompoent
{
    public MonoBehaviour userMovement { get; set; }
    public Vector3 moveInputVelocity_World { get; set; }
    public Vector3 curMoveVelocity_World { get; set; }
    public Vector3 moveInputVelocity_Local { get; set; }
    public Vector3 curMoveVelocity_Local { get; set; }
    public Vector3 forwardDir { get; set; }
    public MoveTo moveTo { get; set; }
    public enum MoveMode
    {
        MaintainMomentum,
        IgnoreMomenTum,
    }
    public void MovementUpdate();
    public void MovementFixedUpdate();
    public void MoveToDirWorld(Vector3 dirWorldNormalized,float speed,float maxSpeed, MoveMode moveMode);
    public void MoveToDirLocal(Vector3 dirLocalNormalized,float speed,float maxSpeed, MoveMode moveMode);
    public void RotateToDirWorld(Vector3 lookDirWorldNomalized,float rotateSpeed);
    public void GravityUpdate();
    public void CancleMomentum() => curMoveVelocity_World = Vector3.zero;
    public enum Stance
    {
        Stand,
        Crouch,
        Prone
    }
    public Stance curStance { get; set; }
    public bool isGround { get; set; }
    //public bool isSprint { get; set; }

}
public class MoveTo
{
    IMovementCompoent movementCompoent;
    public MoveTo(IMovementCompoent movementCompoent)
    {
        this.movementCompoent = movementCompoent;
    }
    public void MoveToDirWorld(Vector3 dirWorldNormalized, float speed, float maxSpeed, MoveMode moveMode)
    {
        movementCompoent.moveInputVelocity_World = new Vector3(dirWorldNormalized.x, 0, dirWorldNormalized.z);

        switch (moveMode)
        {
            case IMovementCompoent.MoveMode.MaintainMomentum:
                {
                    movementCompoent.curMoveVelocity_World = Vector3.Lerp(movementCompoent.curMoveVelocity_World, movementCompoent.moveInputVelocity_World.normalized * maxSpeed, speed * Time.deltaTime);
                }
                break;
            case IMovementCompoent.MoveMode.IgnoreMomenTum:
                {
                    movementCompoent.curMoveVelocity_World = movementCompoent.moveInputVelocity_World
                        * Mathf.Lerp(movementCompoent.curMoveVelocity_World.magnitude, maxSpeed, speed * Time.deltaTime);
                }
                break;
        }
    }
    public void MoveToDirLocal(Vector3 dirLocalNormalized, float speed, float maxSpeed, MoveMode moveMode)
    {
        movementCompoent.moveInputVelocity_World = TransformLocalToWorldVector(
          new Vector3(dirLocalNormalized.x, 0, dirLocalNormalized.y),
          movementCompoent.forwardDir);

        switch (moveMode)
        {
            case IMovementCompoent.MoveMode.MaintainMomentum:
                {
                    movementCompoent.curMoveVelocity_World = Vector3.Lerp(movementCompoent.curMoveVelocity_World, movementCompoent.moveInputVelocity_World.normalized * maxSpeed, speed * Time.deltaTime);
                }
                break;
            case IMovementCompoent.MoveMode.IgnoreMomenTum:
                {
                    movementCompoent.curMoveVelocity_World = movementCompoent.moveInputVelocity_World
                        * Mathf.Lerp(movementCompoent.curMoveVelocity_World.magnitude, maxSpeed, speed * Time.deltaTime);
                }
                break;
        }
    }
    private Vector3 TransformLocalToWorldVector(Vector3 dirChild, Vector3 dirParent)
    {
        float zeta;

        Vector3 Direction;
        zeta = Mathf.Atan2(dirParent.z, dirParent.x) - Mathf.Deg2Rad * 90;
        Direction.x = dirChild.x * Mathf.Cos(zeta) - dirChild.z * Mathf.Sin(zeta);
        Direction.z = dirChild.x * Mathf.Sin(zeta) + dirChild.z * Mathf.Cos(zeta);
        Direction.y = 0;

        return Direction;
    }
    private Vector3 TransformWorldToLocalVector(Vector3 dirChild, Vector3 dirParent)
    {
        Vector3 Direction = Vector3.zero;
        float zeta;
        zeta = Mathf.Atan2(dirParent.z, dirParent.x) - Mathf.Deg2Rad * 90;
        zeta = -zeta;
        Direction.x = dirChild.x * Mathf.Cos(zeta) - dirChild.z * Mathf.Sin(zeta);
        Direction.z = dirChild.x * Mathf.Sin(zeta) + dirChild.z * Mathf.Cos(zeta);
        Direction.y = 0;

        return Direction;
    }
}
