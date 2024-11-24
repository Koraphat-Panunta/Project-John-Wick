using UnityEngine;
using UnityEngine.Animations;

public abstract class WeaponAttachment :MonoBehaviour,IWeaponAttachAble
{
   
    public abstract Transform anchor { get; set; }
    protected abstract Vector3 bulletSpawnerPos { get; set; }
    public ParentConstraint constraint { get; set; }
    public Transform center { get; set ; }
    protected abstract AttachmentSlot myAttachmentSlot { get; set; }

    public virtual void Attach(Weapon weapon)
    {
       
        SetAttachmentPos(weapon.weaponSlotPos[myAttachmentSlot]);
    }

    public void SetAttachmentPos(Transform weaponSlotPosition)
    {
        Vector3 anchorPos = anchor.localPosition;
        Vector3 anchorRot = anchor.localRotation.eulerAngles;

        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = weaponSlotPosition;
        source.weight = 1;
        if (constraint.sourceCount > 0)
        {
            constraint.RemoveSource(0);
        }
        constraint.AddSource(source);
        constraint.constraintActive = true;
        constraint.translationAtRest = Vector3.zero + anchorPos;
        constraint.rotationAtRest = Vector3.zero;
        constraint.constraintActive = true;
    }
}
