
using UnityEngine;

public abstract partial class MovementCompoent : INodeManager
{
    public MonoBehaviour userMovement { get; set; }
    public CharacterController characterController { get;protected set; }
    public Vector3 moveInputVelocity_World { get; set; }
    public Vector3 curMoveVelocity_World { get;  set; }
    public Vector3 moveInputVelocity_Local { get => TransformWorldToLocalVector(moveInputVelocity_World, transform.forward); }
    public Vector3 curMoveVelocity_Local { get => TransformWorldToLocalVector(curMoveVelocity_World, transform.forward); }
    public Vector3 forwardDir { get => transform.forward; }
    public Transform transform { get; protected set; }
    public virtual bool isGround { get 
        {
            if (Physics.Raycast(transform.position, Vector3.down, 1, GetGroundLayerMask()))
                return true;
            else return false;
        } }
    public abstract bool isOnUpdateEnable { get;protected set; }
    public enum MoveMode
    {
        MaintainMomentum,
        IgnoreMomenTum,
    }
    INodeLeaf INodeManager.curNodeLeaf { get => curNodeLeaf; set => curNodeLeaf = value; }
    private INodeLeaf curNodeLeaf;
    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior nodeManagerBehavior { get ; set ; }

    public MovementCompoent(Transform transform,MonoBehaviour myMovement,CharacterController characterController)
    {
        this.transform = transform;
        this.characterController = characterController;
        this.userMovement = myMovement;
        nodeManagerBehavior = new NodeManagerBehavior();
        InitailizedNode();
    }

    public virtual void UpdateNode()
    {
        nodeManagerBehavior.UpdateNode(this);
    }

    public virtual void FixedUpdateNode()
    {
        nodeManagerBehavior.FixedUpdateNode(this);
    }

    public abstract void InitailizedNode();
  

    public void MoveToDirWorld(Vector3 dirWorldNormalized,float speed,float maxSpeed, MoveMode moveMode)
    {
        moveInputVelocity_World = new Vector3(dirWorldNormalized.x, 0, dirWorldNormalized.z);

        switch (moveMode)
        {
            case MovementCompoent.MoveMode.MaintainMomentum:
                {
                    curMoveVelocity_World = Vector3.Lerp(curMoveVelocity_World, moveInputVelocity_World * maxSpeed, speed * Time.deltaTime);
                }
                break;
            case MovementCompoent.MoveMode.IgnoreMomenTum:
                {
                    curMoveVelocity_World = moveInputVelocity_World
                        * Mathf.Lerp(curMoveVelocity_World.magnitude, maxSpeed, speed * Time.deltaTime);
                }
                break;
        }
    }
    public void MoveToDirLocal(Vector3 dirLocalNormalized,float speed,float maxSpeed, MoveMode moveMode)
    {
        moveInputVelocity_World = TransformLocalToWorldVector(
         new Vector3(dirLocalNormalized.x, 0, dirLocalNormalized.y),
         forwardDir);

        switch (moveMode)
        {
            case MovementCompoent.MoveMode.MaintainMomentum:
                {
                    curMoveVelocity_World = Vector3.Lerp(curMoveVelocity_World, moveInputVelocity_World.normalized * maxSpeed, speed * Time.deltaTime);
                }
                break;
            case MovementCompoent.MoveMode.IgnoreMomenTum:
                {
                    curMoveVelocity_World = moveInputVelocity_World
                        * Mathf.Lerp(curMoveVelocity_World.magnitude, maxSpeed, speed * Time.deltaTime);
                }
                break;
        }
    }
    public void RotateToDirWorld(Vector3 lookDirWorldNomalized,float rotateSpeed)
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
            transform.gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
    public void RotateToDirWorldSlerp(Vector3 dir, float t)
    {
        Quaternion targetRotation = Quaternion.LookRotation(dir);

        transform.gameObject.transform.rotation = Quaternion.Lerp(transform.gameObject.transform.rotation, targetRotation, t);
    }
    public void SetPosition(Vector3 position)
    {
        characterController.Move(position - transform.position);
    }
    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
    public void SetTransform(Vector3 position,Quaternion rotation)
    {
        SetPosition(position);
        SetRotation(rotation);
    }
    public void CancleMomentum() => curMoveVelocity_World = Vector3.zero;
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
    public LayerMask GetGroundLayerMask()
    {
        LayerMask mask = +LayerMask.GetMask("Ground") + LayerMask.GetMask("Default");
        return mask;
    }

}
