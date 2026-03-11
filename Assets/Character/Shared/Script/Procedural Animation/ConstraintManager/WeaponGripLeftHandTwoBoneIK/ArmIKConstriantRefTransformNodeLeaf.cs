using System;
using UnityEngine;

public class ArmIKConstriantRefTransformNodeLeaf : AnimationConstrainNodeLeaf
{

    protected TwoBoneIK_ConstraintSCRP handIKOffsetSCRP;
    protected Transform refTransformDir;
    protected Transform refTransformPos;
    protected HandArmIKConstraintManager handArmIKConstraintManager;

    public float weight;
    protected float transitionSpeed = 10;
    public Vector3 getTargetHandPosition 
    { 
        get
        {
            Vector3 position = this.refTransformPos.position
                + (this.refTransformDir.forward * this.handIKOffsetSCRP.positionOffset.z)
                + (this.refTransformDir.up * this.handIKOffsetSCRP.positionOffset.y)
                + (this.refTransformDir.right * this.handIKOffsetSCRP.positionOffset.x);

            return position;
        } 
    }




    public Quaternion getTargetHandRotation 
    { 
        get
        {
            return this.refTransformPos.rotation * Quaternion.Euler(this.handIKOffsetSCRP.rotationEulerOffset);
        }
    }


    public Vector3 getHintPosition
    {
        get
        {
            Transform targetHandTransform = handArmIKConstraintManager.GetTargetHandTransform();

            return targetHandTransform.position
                + (targetHandTransform.forward * this.handIKOffsetSCRP.hintPositionOffset.z)
                + (targetHandTransform.up * this.handIKOffsetSCRP.hintPositionOffset.y)
                + (targetHandTransform.right * this.handIKOffsetSCRP.hintPositionOffset.x);
        }
    }        
   

    public ArmIKConstriantRefTransformNodeLeaf(         
        Func<bool> precondition,
        HandArmIKConstraintManager handArmIKConstraintManager
        , Transform refTransformPos
        , Transform refTransformDir
        , TwoBoneIK_ConstraintSCRP transformOffsetSCRP
        ) : base( precondition)
    {
        this.handArmIKConstraintManager = handArmIKConstraintManager;
        this.refTransformPos = refTransformPos;
        this.refTransformDir = refTransformDir;
        this.handIKOffsetSCRP = transformOffsetSCRP;
    }

    public override void Enter()
    {
        this.weight = 0;
        base.Enter();
    }

    public void SetWeight(float w) => this.weight = w;

    public override void UpdateNode()
    {
        this.weight += Time.deltaTime * this.transitionSpeed;

        Vector3 targetPos = Vector3.Lerp(
            this.handArmIKConstraintManager.GetTargetHandTransform().position
            ,this.getTargetHandPosition
            , this.weight);

        Quaternion targerRot = Quaternion.Lerp(
            this.handArmIKConstraintManager.GetTargetHandTransform().rotation
            , this.getTargetHandRotation
            , this.weight);

        Vector3 targetHintPos = Vector3.Lerp
            (
            this.handArmIKConstraintManager.GetHintHandTransform().position
            , this.getHintPosition
            , this.weight
            );

        this.handArmIKConstraintManager.SetTargetHand(targetPos, targerRot);
        this.handArmIKConstraintManager.SetHintHandPosition(targetHintPos);
        base.UpdateNode();
    }


    
}
