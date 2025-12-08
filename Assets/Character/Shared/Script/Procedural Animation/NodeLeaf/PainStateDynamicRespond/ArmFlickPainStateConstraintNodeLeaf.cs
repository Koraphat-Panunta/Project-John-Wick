using System;
using UnityEngine;
using static UnityEditor.Recorder.OutputPath;

public class ArmFlickPainStateConstraintNodeLeaf : ArmIKConstraintNodeLeaf
{

    public Vector3 painLookAtPos;
    protected Vector3 root_DirOffset;

    public Vector3 rootDir => Quaternion.Euler(this.root_DirOffset) * base.rootIKHandRef.forward;
    public Vector3 rootRight => Vector3.Cross(this.rootDir, Vector3.up);
    public Vector3 rootUp => Vector3.Cross(this.rootDir, this.rootRight);

    public Vector3 curVelocity;
    public float accelToBalancePoint => 20f * Mathf.Clamp(Vector3.Distance(painLookAtPos,balancePoint)/0.7f,0,1);

    private TransformOffsetSCRP transformAnchorOffset;
    private TransformOffsetSCRP transformBalancePountOffset;

    public Vector3 anchorRootPosition => this.rootIKHandRef.position
        + (this.rootDir * this.transformAnchorOffset.postitionOffset.z)
        + (this.rootRight * this.transformAnchorOffset.postitionOffset.x)
        + (this.rootUp * this.transformAnchorOffset.postitionOffset.y);

    public Vector3 balancePoint => this.rootIKHandRef.position
        + (this.rootDir * this.transformBalancePountOffset.postitionOffset.z)
        + (this.rootRight * this.transformBalancePountOffset.postitionOffset.x)
        + ( this.rootUp * this.transformBalancePountOffset.postitionOffset.y);

    public Vector3 handToBalancePointDir => (this.balancePoint - this.painLookAtPos).normalized;

    public ArmFlickPainStateConstraintNodeLeaf(
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform rootIKHandRef
        , Func<bool> precondition
        , TransformOffsetSCRP transformAnchorOffset
        , TransformOffsetSCRP transformBalancePountOffset
        , Vector3 root_DirOffset
        ) 
        : base(
            handArmIKConstraintManager
            , rootIKHandRef
            , precondition
            )
    {
        this.root_DirOffset = root_DirOffset;
        this.transformAnchorOffset = transformAnchorOffset; 
        this.transformBalancePountOffset = transformBalancePountOffset;
    }

    int dirMulltiply => (Vector3.Dot(base.rootIKHandRef.right, this.rootDir) > 0 ? 1 : -1);

    public override void Enter()
    {
        this.painLookAtPos = this.balancePoint;
        this.curVelocity = Vector3.zero;
        base.Enter();
    }
  
    public override void UpdateNode()
    {
        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        this.curVelocity += this.accelToBalancePoint * this.handToBalancePointDir * Time.fixedDeltaTime;
        this.curVelocity = curVelocity.normalized * Mathf.Clamp(this.curVelocity.magnitude,0f,2f);
        base.FixedUpdateNode();
    }
    protected override void UpdateTargetHandPosition()
    {

        this.painLookAtPos += this.curVelocity * Time.deltaTime;

        Vector3 targetHandPosition = this.anchorRootPosition 
            + ((this.painLookAtPos - this.anchorRootPosition).normalized * Mathf.Clamp(Vector3.Distance(painLookAtPos, anchorRootPosition),0,.45f) );
        this.handArmIKConstraintManager.SetTargetHand(targetHandPosition,base.handArmIKConstraintManager.twoBoneIKConstraint.data.mid.rotation);

        Debug.DrawRay(this.painLookAtPos, this.curVelocity,Color.red);
        Debug.DrawLine(this.anchorRootPosition, targetHandPosition, Color.blue);
    }
    public void TriggerForcePush(Vector3 forcePushDir,float force)
    {
        this.curVelocity = forcePushDir.normalized * force;
        Debug.DrawRay(painLookAtPos, this.curVelocity,Color.red,2);
    }
    public void TriggerForcePull(Vector3 pullPosition, float force)
    {
        this.curVelocity = (pullPosition - this.painLookAtPos) * force;
    }
    protected override void UpdateHintHandPotation()
    {
        Vector3 hintPosition = Vector3.Lerp(this.anchorRootPosition, base.handArmIKConstraintManager.GetTargetHandTransform().position, .5f) + (rootIKHandRef.forward * -.75f);

        base.handArmIKConstraintManager.SetHintHandPosition(hintPosition);
    }
}

