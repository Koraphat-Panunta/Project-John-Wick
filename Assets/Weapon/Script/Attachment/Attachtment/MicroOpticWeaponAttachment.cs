using UnityEngine;
using UnityEngine.Animations;

public class MicroOpticWeaponAttachment :MonoBehaviour, IWeaponAttachment<IMicroOpticAttachAble>
{

    public float min_Precision_PN { get => microOpticWeaponAttachmentScriptableObject.min_Precision_PN; }
    public float max_Precision_PN { get => microOpticWeaponAttachmentScriptableObject.max_Precision_PN; }
    public float accuracy_PN { get => microOpticWeaponAttachmentScriptableObject.accuracy_PN; }
    public float aimDownSightSpeed_N { get => microOpticWeaponAttachmentScriptableObject.aimDownSightSpeed_N; }
    public bool isAttaching { get; set ; }
    public ParentConstraint parentConstraint { get => ParentConstraint; set => ParentConstraint = value; }
    public Transform anchor;
    [SerializeField] protected MicroOpticWeaponAttachmentScriptableObject microOpticWeaponAttachmentScriptableObject;
    [SerializeField] private ParentConstraint ParentConstraint;
    public void Attach(IMicroOpticAttachAble attachmentAble)
    {
        if (isAttaching == true) 
        {
            Detach(attachmentAble);
        }


        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = attachmentAble._microOpticSocket;
        source.weight = 1;
        if (parentConstraint.sourceCount > 0)
        {
            parentConstraint.RemoveSource(0);
        }
        parentConstraint.AddSource(source);

        Vector3 offset = anchor.localPosition*-1;

        parentConstraint.constraintActive = true;
        parentConstraint.translationAtRest = Vector3.zero;
        parentConstraint.rotationAtRest = Vector3.zero;

        //parentConstraint.SetTranslationOffset(0, offset);
        parentConstraint.constraintActive = true;

        parentConstraint.weight = 1;

        attachmentAble._microOptic = this;
        //attachmentAble._min_PrecisionAdditional += min_Precision_PN;
        //attachmentAble._max_PrecisionAdditional += max_Precision_PN;
        //attachmentAble._accuracyAdditional += accuracy_PN;
        //attachmentAble._aimDownSightSpeedAdditional += aimDownSightSpeed_N;
        isAttaching = true ;
    }

    public void Detach(IMicroOpticAttachAble attachmentAble)
    {
        if (parentConstraint.sourceCount > 0)
        {
            parentConstraint.RemoveSource(0);
            parentConstraint.constraintActive = true;
            parentConstraint.constraintActive = true;
            parentConstraint.weight = 1;
        }

        if (attachmentAble._microOptic == this)
        {
            //attachmentAble._min_PrecisionAdditional += min_Precision_PN;
            //attachmentAble._max_PrecisionAdditional += max_Precision_PN;
            //attachmentAble._accuracyAdditional += accuracy_PN;
            //attachmentAble._aimDownSightSpeedAdditional += aimDownSightSpeed_N;
            attachmentAble = null;
        }

        isAttaching = false;
    }
}


