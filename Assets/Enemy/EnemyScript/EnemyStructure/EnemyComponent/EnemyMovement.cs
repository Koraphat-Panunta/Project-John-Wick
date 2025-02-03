using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : IMovementCompoent
{
    public MonoBehaviour userMovement { get; set ; }
    public Vector3 moveInputVelocity_World { get ; set ; }
    public Vector3 curMoveVelocity_World { get; set; }
    public Vector3 moveInputVelocity_Local { get; set; }
    public Vector3 curMoveVelocity_Local { get; set; }
    public Vector3 forwardDir { get; set; }
    public IMovementCompoent.Stance curStance { get; set; }
    public bool isGround { get; set; }
    public bool isSprint { get; set; }
    public NavMeshAgent agent { get; set; }

    public Enemy enemy { get; set; }
    public GravityMovement gravityMovement { get; set; }
    public EnemyMovement(NavMeshAgent agent,Enemy enemy)
    {
        this.agent = agent;
        this.gravityMovement = new GravityMovement();
        this.enemy = enemy;
    }

    public void GravityUpdate()
    {
        this.gravityMovement.GravityMovementUpdate(this);
    }

    public void MovementFixedUpdate()
    {
        GravityUpdate();
        agent.Move(curMoveVelocity_World * Time.deltaTime);

        if (Physics.Raycast(enemy.transform.position, Vector3.down, 1))
            isGround = true;
        else isGround = false;
    }

    public void MovementUpdate()
    {
        moveInputVelocity_Local = TransformWorldToLocalVector(moveInputVelocity_World, enemy.transform.forward);
        curMoveVelocity_Local = TransformWorldToLocalVector(curMoveVelocity_World, enemy.transform.forward);

        forwardDir = enemy.transform.forward;
    }

    public void MoveToDirLocal(Vector3 dirLocalNormalized, float speed, float maxSpeed)
    {
        moveInputVelocity_World = TransformLocalToWorldVector(
           new Vector3(dirLocalNormalized.x, 0, dirLocalNormalized.y),
           forwardDir);

        curMoveVelocity_World = Vector3.Lerp(curMoveVelocity_World, moveInputVelocity_World * maxSpeed, speed * Time.deltaTime);
    }

    public void MoveToDirWorld(Vector3 dirWorldNormalized, float speed, float maxSpeed)
    {
        moveInputVelocity_World = new Vector3(dirWorldNormalized.x, 0, dirWorldNormalized.z);
        curMoveVelocity_World = Vector3.Lerp(curMoveVelocity_World, moveInputVelocity_World * maxSpeed, speed * Time.deltaTime);
    }

    public void RotateToDirWorld(Vector3 lookDirWorldNomalized, float rotateSpeed)
    {
        lookDirWorldNomalized.Normalize();

        // Flatten the direction vector to the XZ plane to only rotate around the Y axis
        lookDirWorldNomalized.y = 0;

        // Check if the direction is not zero to avoid setting a NaN rotation
        if (lookDirWorldNomalized != Vector3.zero)
        {
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(lookDirWorldNomalized);

            // Smoothly rotate towards the target rotation
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
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
