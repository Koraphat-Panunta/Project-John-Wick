using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class HandArmIKConstraintManager : MonoBehaviour,IConstraintManager
{
    public Side side;

    [SerializeField] public TwoBoneIKConstraint twoBoneIKConstraint;

    [SerializeField] private Transform handHint;
    [SerializeField] private Transform handTarget;

    protected Vector3 handTargetPosition;
    protected Quaternion handTargetRotation;

    protected Vector3 hintHandTargetPosition;
   
    private void Update()
    {
        if (twoBoneIKConstraint.weight <= 0)
        {
            handTarget.position = twoBoneIKConstraint.data.tip.position;
            handTarget.rotation = twoBoneIKConstraint.data.tip.rotation;
            handHint.position   = twoBoneIKConstraint.data.mid.position;
        }
    }

    private void LateUpdate()
    {
        this.SteadyTransform();
    }

    public void SteadyTransform()
    {
        this.handTarget.position = Vector3.Lerp(this.handTarget.position, this.handTargetPosition,Time.deltaTime);
        this.handTarget.rotation = Quaternion.Lerp(this.handTarget.rotation,this.handTargetRotation,Time.deltaTime);

        this.handHint.position = Vector3.Lerp(this.handHint.position, this.hintHandTargetPosition, Time.deltaTime);
    }

    public float GetWeight() => twoBoneIKConstraint.weight;
    public void SetWeight(float w) => twoBoneIKConstraint.weight = w;

    public void SetHintHandPosition(Vector3 worldHintPosition)
    {
        handHint.position = worldHintPosition;

        this.hintHandTargetPosition = worldHintPosition;
    }

    public void SetTargetHand(Vector3 worldPosition, Quaternion worldRotation)
    {
        this.handTargetPosition = worldPosition;
        this.handTargetRotation = worldRotation;

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
