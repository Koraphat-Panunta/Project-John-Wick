using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class HandArmIKConstraintManager : MonoBehaviour,IConstraintManager
{
    public Side side;

    [SerializeField] public TwoBoneIKConstraint twoBoneIKConstraint;

    [SerializeField] private Transform handHint;
    [SerializeField] private Transform handTarget;

    private void Update()
    {
        if (twoBoneIKConstraint.weight <= 0)
        {
            handTarget.position = twoBoneIKConstraint.data.tip.position;
            handTarget.rotation = twoBoneIKConstraint.data.tip.rotation;
            handHint.position   = twoBoneIKConstraint.data.mid.position;
        }
    }

    public float GetWeight() => twoBoneIKConstraint.weight;
    public void SetWeight(float w) => twoBoneIKConstraint.weight = w;

    public void SetHintHandPosition(Vector3 worldHintPosition)
    {
        handHint.position = worldHintPosition;
    }

    public void SetTargetHand(Vector3 worldPosition, Quaternion worldRotation)
    {
        handTarget.position = worldPosition;
        handTarget.rotation = worldRotation;
    }

    public Transform GetTargetHandTransform() => this.handTarget;
    public Transform GetHintHandTransform() => this.handHint;

    public TwoBoneIKConstraint GetTwoBoneIKConstraint() => this.twoBoneIKConstraint;

    public void AssignBone(HumanoidBone humanoidBone)
    {
        if (this.side == Side.Left)
        {
            this.GetTwoBoneIKConstraint().data.root = humanoidBone._leftArmBone;
            this.GetTwoBoneIKConstraint().data.mid = humanoidBone._leftForeArmBone;
            this.GetTwoBoneIKConstraint().data.tip = humanoidBone._leftHandBone;
        }
        else
        {
            this.GetTwoBoneIKConstraint().data.root = humanoidBone._rightArmBone;
            this.GetTwoBoneIKConstraint().data.mid = humanoidBone._rightForeArmBone;
            this.GetTwoBoneIKConstraint().data.tip = humanoidBone._rightHandBone;
        }
    }
}
