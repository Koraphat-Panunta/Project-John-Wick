using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public abstract class WeaponAttachment :MonoBehaviour,IWeaponAttachAble
{
    public abstract Transform anchor { get; set; }
    public ParentConstraint constraint { get; set; }
    public Transform center { get => transform; set { } }
    public abstract AttachmentSlot myAttachmentSlot { get;protected set; }
    protected virtual void Start()
    {
       
    }
    protected virtual void Awake()
    {
        constraint = GetComponent<ParentConstraint>();
    }
    protected virtual void Update()
    {
    }
    public virtual void Attach(Weapon weapon)
    {
        SetAttachmentPos(weapon.weaponSlotPos[myAttachmentSlot]);
        weapon.Notify(weapon,WeaponSubject.WeaponNotifyType.AttachmentSetup);
    }

    public void SetAttachmentPos(Transform weaponSlotPosition)
    {
        transform.SetParent(weaponSlotPosition,false);
        transform.localPosition = Vector3.zero/* - anchor.position;*/;
        //constraint = GetComponent<ParentConstraint>();
        //Vector3 anchorPos = anchor.localPosition;
        //Vector3 anchorRot = anchor.localRotation.eulerAngles;
        //ConstraintSource source = new ConstraintSource();
        //source.sourceTransform = weaponSlotPosition;
        //source.weight = 1;

        //Debug.Log(source.sourceTransform);
        //if (constraint.sourceCount > 0)
        //{
        //    constraint.RemoveSource(0);
        //}
        //constraint.AddSource(source);
        //constraint.constraintActive = true;
        //constraint.translationAtRest = Vector3.zero + anchorPos;
        //constraint.rotationAtRest = Vector3.zero;
        //constraint.constraintActive = true;
        //constraint.weight = 1;

    }
}
