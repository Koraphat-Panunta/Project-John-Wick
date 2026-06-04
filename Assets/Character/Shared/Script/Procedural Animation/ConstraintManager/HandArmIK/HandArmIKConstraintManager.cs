using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class HandArmIKConstraintManager : MonoBehaviour,IConstraintManager
{
    public Side side;

    [SerializeField] public TwoBoneIKConstraint twoBoneIKConstraint;

    [SerializeField] private Transform handHint;
    [SerializeField] private Transform handTarget;

    private Vector3    _handTargetLocalPos;
    private Quaternion _handTargetLocalRot = Quaternion.identity;
    private Vector3    _handHintLocalPos;

    public Vector3    curHandTargetPosition => this.transform.TransformPoint(_handTargetLocalPos);
    public Quaternion curHandTargetRotation => this.transform.rotation * _handTargetLocalRot;
    public Vector3    curHandHintPosition   => this.transform.TransformPoint(_handHintLocalPos);

    private void Update()
    {
        if (twoBoneIKConstraint.weight <= 0)
        {
            SetTargetHand(twoBoneIKConstraint.data.tip.position, twoBoneIKConstraint.data.tip.rotation);
            SetHintHandPosition(twoBoneIKConstraint.data.mid.position);
        }

        this.handTarget.position =  curHandTargetPosition;
        this.handTarget.rotation = curHandTargetRotation;
        this.handHint.position   = curHandHintPosition;
    }

    public float GetWeight() => twoBoneIKConstraint.weight;
    public void SetWeight(float w) => twoBoneIKConstraint.weight = w;

    public void SetHintHandPosition(Vector3 worldHintPosition)
    {
        _handHintLocalPos = this.transform.InverseTransformPoint(worldHintPosition);
    }

    public void SetTargetHand(Vector3 worldPosition, Quaternion worldRotation)
    {
        _handTargetLocalPos = this.transform.InverseTransformPoint(worldPosition);
        _handTargetLocalRot = Quaternion.Inverse(this.transform.rotation) * worldRotation;
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
