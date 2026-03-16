
using System.Collections.Generic;
using UnityEngine;

public abstract partial class MovementCompoent : INodeManager
{
    public MonoBehaviour userMovement { get; set; }
    public float inputAngularVelocity { get; set; }
    public float curAngularVelocity { get; set; }
    public Vector3 moveInputVelocity_World { get; set; }
    public Vector3 curMoveVelocity_World { get;  set; }
    public Vector3 moveInputVelocity_Local { get => TransformWorldToLocalVector(moveInputVelocity_World, transform.forward); }
    public Vector3 curMoveVelocity_Local { get => TransformWorldToLocalVector(curMoveVelocity_World, transform.forward); }
    public Vector3 forwardDir { get => transform.forward; }
    public Transform transform { get; protected set; }
    public bool isOnUpdateEnable { get; set; }
   
    INodeLeaf INodeManager._curNodeLeaf { get => curNodeLeaf; set => curNodeLeaf = value; }
    private INodeLeaf curNodeLeaf;
    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior _nodeManagerBehavior { get ; set ; }
    public OnUpdateMovementNodeLeaf onUpdateMovementNodeLeaf { get; set; }
    public List<INodeManager> _parallelNodeManahger { get;set; }

    public abstract Vector3 curPosition { get; } 
    public abstract Quaternion curRotation { get; }

    public Vector3 proneDir { get; protected set; }

    public MovementCompoent(Transform transform,MonoBehaviour myMovement)
    {
        isOnUpdateEnable = true;
        this.transform = transform;
        this.userMovement = myMovement;
        _nodeManagerBehavior = new NodeManagerBehavior();
        _parallelNodeManahger = new List<INodeManager>();
        InitailizedNode();
    }

    public virtual void UpdateNode()
    {
        _nodeManagerBehavior.UpdateNodeAndCheckFindingNode(this);
        //Debug.DrawRay(this.userMovement.transform.position + Vector3.up, moveInputVelocity_World, Color.blue);
        //Debug.DrawRay(this.userMovement.transform.position + Vector3.up, curMoveVelocity_World, Color.yellow);
    }

    public virtual void FixedUpdateNode()
    {
        _nodeManagerBehavior.FixedUpdateNode(this);
    }

    public abstract void InitailizedNode();
  
    public void UpdateAngularVelocity(float targetAngularVelocity,float accelerateAngularVelocity, MoveMode moveMode)
    {
        this.inputAngularVelocity = targetAngularVelocity;
        switch (moveMode)
        {
            case MoveMode.MaintainMomentumDirection:
                {
                    this.curAngularVelocity = Mathf.MoveTowards(this.curAngularVelocity, targetAngularVelocity, accelerateAngularVelocity * Time.deltaTime);
                    break;
                }
            case MoveMode.IgnoreMomentumDirection:
                {
                    this.curAngularVelocity = targetAngularVelocity;
                    break;
                }
        }
    }
    public void SetVelocityWorld(Vector3 velocity)
    {
        this.curMoveVelocity_World = velocity;
    }
    public void UpdateMoveToDirWorld(Vector3 dirWorldVelocity,float accelerate, MoveMode moveMode)
    {
        moveInputVelocity_World = new Vector3(dirWorldVelocity.x, 0, dirWorldVelocity.z);

        

        switch (moveMode)
        {
            case MoveMode.MaintainMomentumDirection:
                {
                    this.curMoveVelocity_World = Vector3.MoveTowards(this.curMoveVelocity_World, this.moveInputVelocity_World, accelerate * Time.deltaTime);
                }
                break;
            case MoveMode.IgnoreMomentumDirection:
                {
                    if(this.curMoveVelocity_World.magnitude <= 0 || this.moveInputVelocity_World.magnitude <= 0)
                        this.curMoveVelocity_World = Vector3.MoveTowards(this.curMoveVelocity_World, this.moveInputVelocity_World, accelerate * Time.deltaTime);
                    else
                    {
                        float dot = Mathf.Clamp01(Vector3.Dot(this.curMoveVelocity_World.normalized, this.moveInputVelocity_World.normalized));
                        this.curMoveVelocity_World 
                            = this.moveInputVelocity_World.normalized 
                            * Mathf.MoveTowards
                            (this.curMoveVelocity_World.magnitude * dot
                            , this.moveInputVelocity_World.magnitude
                            , accelerate * Time.deltaTime);
                    }
                }
                break;
        }
    }
    public void UpdateMoveToDirLocal(Vector3 dirLocalNormalized,float speed, MoveMode moveMode)
    {
        this.moveInputVelocity_World = TransformLocalToWorldVector(
         new Vector3(dirLocalNormalized.x, 0, dirLocalNormalized.y),
         forwardDir);

        this.UpdateMoveToDirWorld(moveInputVelocity_World, speed, moveMode);
    }
    public void SetRotateToDirWorld(Vector3 lookDirWorldNomalized,float rotateSpeed)
    {
        lookDirWorldNomalized.Normalize();

        // Flatten the direction vector to the XZ plane to only rotate around the Y axis
        lookDirWorldNomalized.y = 0;

        // Check if the direction is not zero to avoid setting a NaN rotation
        if (lookDirWorldNomalized != Vector3.zero)
        {
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(lookDirWorldNomalized,Vector3.up);

            // Smoothly rotate towards the target rotation
            this.SetRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime));
        }
    }
    public void SetRotateToDirWorldSlerp(Vector3 dir, float t)
    {
        Quaternion targetRotation = Quaternion.LookRotation(dir);

        Quaternion resault = Quaternion.Lerp(this.curRotation, targetRotation, t);
        SetRotation(resault);
    }
    public abstract void Move(Vector3 position);
    public void SetPosition(Vector3 position)
    {
        this.Move(position - curPosition);
    }
    public abstract void SetRotation(Quaternion rotation);
    public void SetTransform(Vector3 position,Quaternion rotation)
    {
        SetPosition(position);
        SetRotation(rotation);
    }
    public void CancleMomentum() 
    {
        this.curMoveVelocity_World = Vector3.zero;
        this.curAngularVelocity = 0;
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
    public LayerMask GetGroundLayerMask()
    {
        LayerMask mask = +LayerMask.GetMask("Ground") + LayerMask.GetMask("Default");
        return mask;
    }
    private float castCheckIsGroundOffserUp = 2;
    public bool IsGround(out Vector3 hitGroundPosition)
    {
        hitGroundPosition = Vector3.zero;

        if (Physics.Raycast(curPosition + (Vector3.up * castCheckIsGroundOffserUp), Vector3.down,out RaycastHit hitGroundPos,castCheckIsGroundOffserUp + .2f, GetGroundLayerMask()))
        {
            hitGroundPosition = hitGroundPos.point;
            Debug.DrawLine(curPosition + (Vector3.up * castCheckIsGroundOffserUp), hitGroundPosition, Color.blue);
            return true;
        }
        else
        {
            Debug.DrawLine(curPosition + (Vector3.up * castCheckIsGroundOffserUp)
                , curPosition + (Vector3.up * castCheckIsGroundOffserUp) + (Vector3.down * (castCheckIsGroundOffserUp + .12f))
                , Color.blue);
        }
        return false;
    }
}
public enum MoveMode
{
    MaintainMomentumDirection,
    IgnoreMomentumDirection,
}
