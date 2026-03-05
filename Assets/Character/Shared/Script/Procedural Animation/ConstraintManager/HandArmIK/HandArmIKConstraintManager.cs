using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class HandArmIKConstraintManager : MonoBehaviour,IConstraintManager
{
    [SerializeField] public TwoBoneIKConstraint twoBoneIKConstraint;

    [SerializeField] private Transform handHint;
    [SerializeField] private Transform handTarget;
    
    private void LateUpdate()
    {
        if (twoBoneIKConstraint.weight < 1)
        {
            this.handTarget.position = twoBoneIKConstraint.data.tip.position;
            this.handTarget.rotation = twoBoneIKConstraint.data.tip.rotation;

            this.handHint.position = twoBoneIKConstraint.data.mid.position;
        }
    }

   

    public float GetWeight() => twoBoneIKConstraint.weight;
    public void SetWeight(float w) => twoBoneIKConstraint.weight = w;  
    
    public void SetHintHandPosition(Vector3 hintPosition)
    {
        this.handHint.position = hintPosition;
    }

    public void SetTargetHand(Vector3 targetHandPosition,Quaternion targetHandRotation)
    {
        this.handTarget.position = targetHandPosition;
        this.handTarget.rotation = targetHandRotation;
    }

    public Transform GetTargetHandTransform() => this.handTarget;
    public Transform GetHintHandTransform() => this.handHint;

   

    // Update is called once per frame

}
