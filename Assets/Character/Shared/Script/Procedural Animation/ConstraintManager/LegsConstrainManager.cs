using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LegsConstrainManager :MonoBehaviour, IConstraintManager
{
    [SerializeField] TwoBoneIKConstraint leftLegTwoBoneIKConstrain;
    [SerializeField] TwoBoneIKConstraint rightLegTwoBoneIKConstrain;

    [SerializeField] Transform leftLeg_Target_Foot;
    [SerializeField] Transform leftLeg_Hint_Foot;

    [SerializeField] Transform rightLeg_Target_Foot;
    [SerializeField] Transform rightLeg_Hint_Foot;

    public TransfromValue leftLegTransformValue;
    public Vector3 hintLeftLegPos;
    public TransfromValue rightLegTransformValue;
    public Vector3 hintRightLegPos;



    public TwoBoneIKConstraint GetLeftLegTwoBoneIKConstrain() => this.leftLegTwoBoneIKConstrain;
    public TwoBoneIKConstraint GetRightLegTwoBoneIKConstrain() => this.rightLegTwoBoneIKConstrain;

    private void FixedUpdate()
    {
        if (this.GetWeight() <= 0)
        {
            this.leftLegTransformValue.position = this.leftLegTwoBoneIKConstrain.data.tip.transform.position;
            this.leftLegTransformValue.rotationEuler = this.leftLegTwoBoneIKConstrain.data.tip.transform.rotation.eulerAngles;
            this.hintLeftLegPos = this.leftLegTwoBoneIKConstrain.data.mid.position;

            this.rightLegTransformValue.position = this.rightLegTwoBoneIKConstrain.data.tip.transform.position;
            this.rightLegTransformValue.rotationEuler = this.rightLegTwoBoneIKConstrain.data.tip.transform.rotation.eulerAngles;
            this.hintRightLegPos = this.rightLegTwoBoneIKConstrain.data.mid.transform.position;
        }

        this.leftLeg_Target_Foot.position = this.leftLegTransformValue.position;
        this.leftLeg_Target_Foot.rotation = Quaternion.Euler(this.leftLegTransformValue.rotationEuler);
        this.leftLeg_Hint_Foot.position = this.hintLeftLegPos;


        this.rightLeg_Target_Foot.position = this.rightLegTransformValue.position;
        this.rightLeg_Target_Foot.rotation = Quaternion.Euler(this.rightLegTransformValue.rotationEuler);
        this.rightLeg_Hint_Foot.position = this.hintRightLegPos;
    }

    private void Update()
    {
        //this.leftLeg_Target_Foot.position = this.leftLegTransformValue.position;
        //this.leftLeg_Target_Foot.rotation = Quaternion.Euler(this.leftLegTransformValue.rotationEuler);
        //this.leftLeg_Hint_Foot.position = this.hintLeftLegPos;


        //this.rightLeg_Target_Foot.position = this.rightLegTransformValue.position;
        //this.rightLeg_Target_Foot.rotation = Quaternion.Euler(this.rightLegTransformValue.rotationEuler);
        //this.rightLeg_Hint_Foot.position = this.hintRightLegPos;
    }

    public float GetWeight()
    {
        return this.leftLegTwoBoneIKConstrain.weight;
    }

    public void SetWeight(float w)
    {
        this.leftLegTwoBoneIKConstrain.weight = w;
        this.rightLegTwoBoneIKConstrain.weight = w;
    }

    public void SetLeftLeg_Target_Foot(Vector3 pos,Quaternion rotation)
    {
        this.SetLeftLeg_Target_Foot(pos);
        this.leftLegTransformValue.rotationEuler = rotation.eulerAngles;
    }

    public void SetLeftLeg_Target_Foot(Vector3 pos)
    {
        this.leftLegTransformValue.position = pos;


    }

    public void SetLeftLeg_Hint_FootPos(Vector3 pos)
    {
        this.hintLeftLegPos = pos;
    }

    public Transform GetLeftLeg_Target_Transform() => this.leftLeg_Target_Foot;
    public Transform GetLeftLeg_Hint_Transform()=> this.leftLeg_Hint_Foot;

    public void SetRightLeg_Target_Foot(Vector3 pos, Quaternion rotation)
    {
        this.SetRightLeg_Target_Foot(pos);
        this.rightLegTransformValue.rotationEuler = rotation.eulerAngles;

    }
    public void SetRightLeg_Target_Foot(Vector3 pos)
    {
        this.rightLegTransformValue.position = pos;
    }

    public void SetRightLeg_Hint_FootPos(Vector3 pos)
    {
        this.hintRightLegPos = pos;
    }

    public Transform GetRightLeg_Target_Transform() => this.rightLeg_Target_Foot;
    public Transform GetRightLeg_Hint_Transform() => this.rightLeg_Hint_Foot;

    public void AssignBone(HumanoidBone humanoidBone)
    {
        this.leftLegTwoBoneIKConstrain.data.root = humanoidBone._leftUpperLegBone;
        this.leftLegTwoBoneIKConstrain.data.mid = humanoidBone._leftLowerLegBone;
        this.leftLegTwoBoneIKConstrain.data.tip = humanoidBone._leftFootBone;

        this.rightLegTwoBoneIKConstrain.data.root = humanoidBone._rightUpperLegBone;
        this.rightLegTwoBoneIKConstrain.data.mid = humanoidBone._rightLowerLegBone;
        this.rightLegTwoBoneIKConstrain.data.tip = humanoidBone._rightFootBone;
    }
}
