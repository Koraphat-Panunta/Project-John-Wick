using UnityEngine;

[CreateAssetMenu(fileName = "TwoBoneIKBlendingConstrainScriptableObject", menuName = "ScriptableObjects/ConstrainObject/TwoBoneIK_ConstraintSCRP/TwoBoneIKBlendingConstrainScriptableObject")]
public class TwoBoneIKBlendingConstrainScriptableObject : TwoBoneIK_ConstraintSCRP, BlendingConstrainScriptableObject<TwoBoneIK_ConstraintSCRP>
{
    public BledningWeightSCRP<TwoBoneIK_ConstraintSCRP>[] _bledningWeightSCRP => bledningWeightSCRP;
    [SerializeField]
    private BledningWeightSCRP<TwoBoneIK_ConstraintSCRP>[] bledningWeightSCRP;

    public float weight;

    public void SetWeight(float w)
    {
        this.weight = w;
        GetBlendData();
    }
    public TwoBoneIK_ConstraintSCRP GetBlendData()
    {

        var data = bledningWeightSCRP;

        if (data == null || data.Length == 0)
            return this;

        if (data.Length == 1)
        {
            var src = data[0].scrp;

            this.positionOffset = src.positionOffset;
            this.rotationEulerOffset = src.rotationEulerOffset;
            this.hintPositionOffset = src.hintPositionOffset;
            this.rotateRefDirOffset = src.rotateRefDirOffset;
            this.maxHorizontalHandAimDeg = src.maxHorizontalHandAimDeg;
            this.maxVerticalHandAimDeg = src.maxVerticalHandAimDeg;

            return this;
        }

        BledningWeightSCRP<TwoBoneIK_ConstraintSCRP> a = data[0];
        BledningWeightSCRP<TwoBoneIK_ConstraintSCRP> b = data[data.Length - 1];

        for (int i = 0; i < data.Length - 1; i++)
        {
            if (this.weight >= data[i].weight && this.weight <= data[i + 1].weight)
            {
                a = data[i];
                b = data[i + 1];
                break;
            }
        }

        float t = Mathf.InverseLerp(a.weight, b.weight, weight);

        var sa = a.scrp;
        var sb = b.scrp;

        this.positionOffset = Vector3.Lerp(sa.positionOffset, sb.positionOffset, t);
        this.rotationEulerOffset = Vector3.Lerp(sa.rotationEulerOffset, sb.rotationEulerOffset, t);
        this.hintPositionOffset = Vector3.Lerp(sa.hintPositionOffset, sb.hintPositionOffset, t);
        this.rotateRefDirOffset = Vector3.Lerp(sa.rotateRefDirOffset, sb.rotateRefDirOffset, t);
        this.maxHorizontalHandAimDeg = Mathf.Lerp(sa.maxHorizontalHandAimDeg, sb.maxHorizontalHandAimDeg, t);
        this.maxVerticalHandAimDeg = Mathf.Lerp(sa.maxVerticalHandAimDeg, sb.maxVerticalHandAimDeg, t);

        return this;
    }
}
