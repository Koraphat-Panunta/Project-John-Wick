using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class HandArmIKConstraintManager : MonoBehaviour,IConstraintManager
{
    [SerializeField] private TwoBoneIKConstraint twoBoneIKConstraint;

    [SerializeField] private Transform leftHandHint;
    [SerializeField] private ParentConstraint leftHandHintParentConstraint;
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private ParentConstraint leftHandTargetParentConstraint;



    public float GetWeight() => twoBoneIKConstraint.weight;
    public void SetWeight(float w) => twoBoneIKConstraint.weight = w;  
    
    public void SetHintHandPosition(Vector3 hintPosition)
    {
        this.leftHandHint.transform.position = hintPosition;
    }

    public void SetTargetHand(Vector3 targetHandPosition,Quaternion targetHandRotation)
    {
        this.leftHandTarget.transform.position = targetHandPosition;
        this.leftHandTarget.transform.rotation = targetHandRotation;
    }

    public void SetHintHandParentConstraint(Transform hintTransform)
    {
        ParentConstraintAttachBehavior.Attach(this.leftHandHintParentConstraint, hintTransform);
    }
    public void RemoveHintHandParentConstraint()
    {
        ParentConstraintAttachBehavior.Detach(this.leftHandHintParentConstraint);
    }
    public void SetTargetHandParentConstraint(Transform targetHandTransform)
    {
        ParentConstraintAttachBehavior.Attach(this.leftHandTargetParentConstraint, targetHandTransform);
    }
    public void RemoveTargetHandParentConstraint()
    {
        ParentConstraintAttachBehavior.Detach(this.leftHandTargetParentConstraint);
    }
    // Update is called once per frame

}
