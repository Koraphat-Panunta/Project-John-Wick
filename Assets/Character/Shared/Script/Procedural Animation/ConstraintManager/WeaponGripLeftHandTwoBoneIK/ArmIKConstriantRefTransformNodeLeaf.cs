using System;
using UnityEngine;

public class ArmIKConstriantRefTransformNodeLeaf : AnimationConstrainNodeLeaf
{

    protected HandIK_ConstraintSCRP handIKOffsetSCRP;
    protected Transform refTransformDir;
    protected Transform refTransformPos;
    protected HandArmIKConstraintManager handArmIKConstraintManager;

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
        , HandIK_ConstraintSCRP transformOffsetSCRP
        ) : base( precondition)
    {
        this.handArmIKConstraintManager = handArmIKConstraintManager;
        this.refTransformPos = refTransformPos;
        this.refTransformDir = refTransformDir;
        this.handIKOffsetSCRP = transformOffsetSCRP;
    }

    public override void UpdateNode()
    {

        this.handArmIKConstraintManager.SetTargetHand(this.getTargetHandPosition, this.getTargetHandRotation);
        this.handArmIKConstraintManager.SetHintHandPosition(this.getHintPosition);
        base.UpdateNode();
    }


    
}
