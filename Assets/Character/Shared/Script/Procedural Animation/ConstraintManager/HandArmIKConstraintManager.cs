using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class HandArmIKConstraintManager : MonoBehaviour,IConstraintManager
{
    [SerializeField] public TwoBoneIKConstraint twoBoneIKConstraint;

    [SerializeField] private Transform leftHandHint;
    [SerializeField] private MountComponent leftHandHintMountComponent;
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private MountComponent leftHandTargetMountComponent;



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

    public void SetHintHandParentConstraint(Transform hintTransform,Vector3 offsetPosition,Vector3 offsetRotation)
    {
        this.leftHandHintMountComponent.Attach(hintTransform, offsetPosition, Quaternion.Euler(offsetRotation));
    }
    public void SetHintHandParentConstraint(Transform hintTransform) => this.SetHintHandParentConstraint(hintTransform, Vector3.zero, Vector3.zero);
    public void RemoveHintHandParentConstraint()
    {
        leftHandHintMountComponent.Detach();
    }
    public void SetTargetHandParentConstraint(Transform targetHandTransform, Vector3 offsetPosition, Vector3 offsetRotation)
    {
        leftHandTargetMountComponent.Attach(targetHandTransform, offsetPosition, Quaternion.Euler(offsetRotation));
    }
    public void SetTargetHandParentConstraint(Transform targetHandTransform) => this.SetTargetHandParentConstraint(targetHandTransform, Vector3.zero, Vector3.zero);

    public void RemoveTargetHandParentConstraint()
    {
        leftHandTargetMountComponent.Detach();
    }


    // Update is called once per frame

}
