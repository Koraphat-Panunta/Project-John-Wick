using MK.Toon;
using UnityEngine;
[CreateAssetMenu(fileName = "LegsIKConstrainScriptableObject", menuName = "ScriptableObjects/ConstrainObject/TwoBoneIK_ConstraintSCRP/LegsIKConstrainScriptableObject/LegsBlendingConstrainScriptableObject")]
public class LegsBlendingConstrainScriptableObject : LegsIKConstrainScriptableObject, BlendingConstrainScriptableObject<LegsIKConstrainScriptableObject>
{
    public BledningWeightSCRP<LegsIKConstrainScriptableObject>[] _bledningWeightSCRP => this.bledningWeightSCRP;
    [SerializeField]
    private BledningWeightSCRP<LegsIKConstrainScriptableObject>[] bledningWeightSCRP;

    public LegsIKConstrainScriptableObject GetBlendData(float weight)
    {
        var data = this.bledningWeightSCRP;

        if (data == null || data.Length == 0)
            return this;

        if (data.Length == 1)
        {
            var src = data[0].scrp;

            this.leftLegPositionOffset = src.leftLegPositionOffset;
            this.leftLegRotationEulerOffset = src.leftLegRotationEulerOffset;
            this.leftLegHintPositionOffset = src.leftLegHintPositionOffset;

            this.rightLegPositionOffset = src.rightLegPositionOffset;
            this.rightLegRotationEulerOffset = src.rightLegRotationEulerOffset;
            this.rightLegHintPositionOffset = src.rightLegHintPositionOffset;

            this.rotateRefDirOffset = src.rotateRefDirOffset;

            this.maxHorizontalHandAimDeg = src.maxHorizontalHandAimDeg;
            this.maxVerticalHandAimDeg = src.maxVerticalHandAimDeg;

            return this;
        }

        BledningWeightSCRP<LegsIKConstrainScriptableObject> a = data[0];
        BledningWeightSCRP<LegsIKConstrainScriptableObject> b = data[data.Length - 1];

        for (int i = 0; i < data.Length - 1; i++)
        {
            if (weight >= data[i].weight && weight <= data[i + 1].weight)
            {
                a = data[i];
                b = data[i + 1];
                break;
            }
        }

        float t = Mathf.InverseLerp(a.weight, b.weight, weight);

        var sa = a.scrp;
        var sb = b.scrp;

        this.leftLegPositionOffset = Vector3.Lerp(sa.leftLegPositionOffset, sb.leftLegPositionOffset, t);
        this.leftLegRotationEulerOffset = Vector3.Lerp(sa.leftLegRotationEulerOffset, sb.leftLegRotationEulerOffset, t);
        this.leftLegHintPositionOffset = Vector3.Lerp(sa.leftLegHintPositionOffset, sb.leftLegHintPositionOffset, t);

        this.rightLegPositionOffset = Vector3.Lerp(sa.rightLegPositionOffset, sb.rightLegPositionOffset, t);
        this.rightLegRotationEulerOffset = Vector3.Lerp(sa.rightLegRotationEulerOffset, sb.rightLegRotationEulerOffset, t);
        this.rightLegHintPositionOffset = Vector3.Lerp(sa.rightLegHintPositionOffset, sb.rightLegHintPositionOffset, t);

        this.rotateRefDirOffset = Vector3.Lerp(sa.rotateRefDirOffset, sb.rotateRefDirOffset, t);

        this.maxHorizontalHandAimDeg = Mathf.Lerp(sa.maxHorizontalHandAimDeg, sb.maxHorizontalHandAimDeg, t);
        this.maxVerticalHandAimDeg = Mathf.Lerp(sa.maxVerticalHandAimDeg, sb.maxVerticalHandAimDeg, t);

        return this;
    }
}


