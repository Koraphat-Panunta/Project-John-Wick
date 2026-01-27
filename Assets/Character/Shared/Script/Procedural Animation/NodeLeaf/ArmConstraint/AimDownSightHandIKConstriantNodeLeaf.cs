using System;
using UnityEngine;

public class AimDownSightHandIKConstriantNodeLeaf : ArmIKConstraintNodeLeaf
{

    protected IWeaponAdvanceUser weaponAdvanceUser;
    public Vector3 aimDirConstriant 
    {
        get 
        {
            Vector3 dir = (this.weaponAdvanceUser._pointingPos - base.rootIKHandRef.position).normalized;
            dir = ClampDirection.GetClampDirection(this.weaponAdvanceUser._userWeapon.transform.forward, dir,0, 60, this.weaponAdvanceUser._userWeapon.transform.up );
            dir = new Vector3(dir.x, dir.y * -1, dir.z);
            return dir;
        }
    }

    private Vector3 forward => this.aimDirConstriant.normalized;
    private Vector3 rightWard => Vector3.Cross(Vector3.up, this.aimDirConstriant).normalized;
    private Vector3 upWard => Vector3.Cross(forward, rightWard).normalized;
    public Vector3 targetAnchorHandPosition 
    {
        get 
        {
            //Debug.DrawRay(base.rootIKHandRef.position, forward,Color.blue);
            //Debug.DrawRay(base.rootIKHandRef.position, rightWard, Color.red);
            //Debug.DrawRay(base.rootIKHandRef.position, upWard, Color.green);

            return this.rootIKHandRef.position 
                + (forward * this.rightHandIK_ConstraintSCRP.positionOffset.z)
                + (rightWard * this.rightHandIK_ConstraintSCRP.positionOffset.x)
                + (upWard * this.rightHandIK_ConstraintSCRP.positionOffset.y);
        }
    }
    public Quaternion targetAnchorHandRotaion
    {
        get 
        {
            return Quaternion.LookRotation( - this.rightWard,this.forward) * Quaternion.Euler(this.rightHandIK_ConstraintSCRP.rotationEulerOffset);
        }
    }

    public float weight;
    public Vector3 targetHandPosition
    {
        get 
        {
            return Vector3.Lerp(this.handArmIKConstraintManager.GetTargetHandTransform().position,this.targetAnchorHandPosition,this.weight);
        }
    }
    public Quaternion targetHandRotation
    {
        get
        {
            return Quaternion.Lerp(this.handArmIKConstraintManager.GetTargetHandTransform().rotation, this.targetAnchorHandRotaion, this.weight);
        }
    }

    protected Transform rootHintHandTransform;

    protected HandIK_ConstraintSCRP rightHandIK_ConstraintSCRP;

    protected Transform aimingAtTransfrom;
    public AimDownSightHandIKConstriantNodeLeaf(
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform aimingAtTransform
        , Transform rootIKHandRef
        , Transform rootHintHand
        , HandIK_ConstraintSCRP rightHandIK_ConstraintSCRP
        , IWeaponAdvanceUser weaponAdvanceUser
        , Func<bool> precondition) : base(handArmIKConstraintManager, rootIKHandRef, precondition)
    {
        this.aimingAtTransfrom = aimingAtTransform;
        this.weaponAdvanceUser = weaponAdvanceUser;
        this.rightHandIK_ConstraintSCRP = rightHandIK_ConstraintSCRP;
        this.rootHintHandTransform = rootHintHand;
    }
    public override void Enter()
    {
        weight = 0;
        base.Enter();
    }

    public override void UpdateNode()
    {
        this.weight = Mathf.Clamp01(weight + Time.deltaTime );

        base.UpdateNode();
    }
    protected override void UpdateHintHandPotation()
    {

        Vector3 hintHandPos = this.rootHintHandTransform.position
             + this.forward * this.rightHandIK_ConstraintSCRP.hintPositionOffset.z
             + this.upWard * this.rightHandIK_ConstraintSCRP.hintPositionOffset.y
             + this.rightWard * this.rightHandIK_ConstraintSCRP.hintPositionOffset.x;

      

        this.handArmIKConstraintManager.SetHintHandPosition(Vector3.Lerp(this.handArmIKConstraintManager.GetHintHandTransform().position,hintHandPos,this.weight));
    }

    protected override void UpdateTargetHandPosition()
    {
        this.handArmIKConstraintManager.SetTargetHand(this.targetHandPosition, this.targetHandRotation);
        Debug.DrawLine(base.rootIKHandRef.position, this.targetHandPosition, Color.yellow);
    }
}
