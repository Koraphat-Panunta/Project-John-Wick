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

        this.transform.SetParent(attachmentAble._microOpticSocket, false);
        this.transform.localPosition = anchor.localPosition * -1;

        attachmentAble._microOptic = this;
        isAttaching = true ;
    }

    public void Detach(IMicroOpticAttachAble attachmentAble)
    {
       
        if (attachmentAble._microOptic == this)
        {
            attachmentAble = null;
        }

        isAttaching = false;
    }
}


