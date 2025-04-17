using UnityEngine;
using UnityEngine.Animations;

public interface IWeaponAttachment<T> where T : IWeaponAttachmentAttachAble
{
    public bool isAttaching { get; set; }
    public ParentConstraint parentConstraint { get; set; }
   public void Attach(T attachmentAble);
   public void Detach(T attachmentAble);
}
