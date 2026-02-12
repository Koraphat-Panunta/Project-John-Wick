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

    public TwoBoneIKConstraint GetLeftLegTwoBoneIKConstrain() => this.leftLegTwoBoneIKConstrain;
    public TwoBoneIKConstraint GetRightLegTwoBoneIKConstrain() => this.rightLegTwoBoneIKConstrain;

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
        this.leftLeg_Target_Foot.rotation = rotation;
    }

    public void SetLeftLeg_Target_Foot(Vector3 pos)
    {
        this.leftLeg_Target_Foot.position = pos;
    }

    public void SetLeftLeg_Hint_FootPos(Vector3 pos)
    {
        this.leftLeg_Hint_Foot.position = pos;
    }

    public Transform GetLeftLeg_Target_Transform() => this.leftLeg_Target_Foot;
    public Transform GetLeftLeg_Hint_Transform()=> this.leftLeg_Hint_Foot;

    public void SetRightLeg_Target_Foot(Vector3 pos, Quaternion rotation)
    {
        this.SetRightLeg_Target_Foot(pos);
        this.rightLeg_Target_Foot.rotation = rotation;
    }
    public void SetRightLeg_Target_Foot(Vector3 pos)
    {
        this.rightLeg_Target_Foot.position = pos;
    }

    public void SetRightLeg_Hint_FootPos(Vector3 pos)
    {
        this.rightLeg_Hint_Foot.position = pos;
    }

    public Transform GetRightLeg_Target_Transform() => this.rightLeg_Target_Foot;
    public Transform GetRightLeg_Hint_Transform() => this.rightLeg_Hint_Foot;

}
