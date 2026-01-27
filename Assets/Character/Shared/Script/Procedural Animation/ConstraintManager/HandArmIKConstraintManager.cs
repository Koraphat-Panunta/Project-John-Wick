using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class HandArmIKConstraintManager : MonoBehaviour,IConstraintManager
{
    [SerializeField] public TwoBoneIKConstraint twoBoneIKConstraint;

    [SerializeField] private Transform handHint;
    [SerializeField] private MountComponent handHintMountComponent;
    [SerializeField] private Transform handTarget;
    [SerializeField] private MountComponent handTargetMountComponent;

    
    private void LateUpdate()
    {
        if (twoBoneIKConstraint.weight < 1)
        {
            this.handTarget.position = twoBoneIKConstraint.data.tip.position;
            this.handTarget.rotation = twoBoneIKConstraint.data.tip.rotation;
        }
    }

    public float GetWeight() => twoBoneIKConstraint.weight;
    public void SetWeight(float w) => twoBoneIKConstraint.weight = w;  
    
    public void SetHintHandPosition(Vector3 hintPosition)
    {
        this.handHint.transform.position = hintPosition;
    }

    public void SetTargetHand(Vector3 targetHandPosition,Quaternion targetHandRotation)
    {
        this.handTarget.transform.position = targetHandPosition;
        this.handTarget.transform.rotation = targetHandRotation;
    }

    public Transform GetTargetHandTransform() => this.handTarget;
    public Transform GetHintHandTransform() => this.handHint;

    public void SetHintHandParentConstraint(Transform hintTransform,Vector3 offsetPosition,Vector3 offsetRotation)
    {
        this.handHintMountComponent.Attach(hintTransform, offsetPosition, Quaternion.Euler(offsetRotation));
    }
    public void SetHintHandParentConstraint(Transform hintTransform) => this.SetHintHandParentConstraint(hintTransform, Vector3.zero, Vector3.zero);
    public void RemoveHintHandParentConstraint()
    {
        handHintMountComponent.Detach();
    }
    public void SetTargetHandParentConstraint(Transform targetHandTransform, Vector3 offsetPosition, Vector3 offsetRotation)
    {
        handTargetMountComponent.Attach(targetHandTransform, offsetPosition, Quaternion.Euler(offsetRotation));
    }
    public void SetTargetHandParentConstraint(Transform targetHandTransform) => this.SetTargetHandParentConstraint(targetHandTransform, Vector3.zero, Vector3.zero);

    public void RemoveTargetHandParentConstraint()
    {
        handTargetMountComponent.Detach();
    }


    // Update is called once per frame

}
