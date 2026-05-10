using UnityEngine;

[CreateAssetMenu(fileName = "BodyRotationScriptableObjectBlend", menuName = "ScriptableObjects/ConstrainObject/BodyRotationScriptableObjectBlend")]
public class BodyRotationScriptableObjectBlend : BodyRotationConstrainScriptableObject, BlendingConstrainScriptableObject<BodyRotationConstrainScriptableObject>
{
    public BledningWeightSCRP<BodyRotationConstrainScriptableObject>[] _bledningWeightSCRP => this.bledningWeightSCRP;

    [SerializeField]
    private BledningWeightSCRP<BodyRotationConstrainScriptableObject>[] bledningWeightSCRP;

    public BodyRotationConstrainScriptableObject GetBlendData(float weight)
    {
        var data = this.bledningWeightSCRP;

        if (data == null || data.Length == 0)
            return this;

        if (data.Length == 1)
        {
            var src = data[0].scrp;

            this.weightConstraint = src.weightConstraint;
            this.offsetConstraint = src.offsetConstraint;

            this.weightConstraint1 = src.weightConstraint1;
            this.offsetConstraint1 = src.offsetConstraint1;

            this.weightConstraint2 = src.weightConstraint2;
            this.offsetConstraint2 = src.offsetConstraint2;

            this.rotateRefDirOffset = src.rotateRefDirOffset;
            this.maxHorizontalDeg = src.maxHorizontalDeg;
            this.maxVerticalDeg = src.maxVerticalDeg;
            this.offsetChangedRate = src.offsetChangedRate;

            return this;
        }

        BledningWeightSCRP<BodyRotationConstrainScriptableObject> a = data[0];
        BledningWeightSCRP<BodyRotationConstrainScriptableObject> b = data[data.Length - 1];

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

        this.weightConstraint = Mathf.Lerp(sa.weightConstraint, sb.weightConstraint, t);
        this.offsetConstraint = Vector3.Lerp(sa.offsetConstraint, sb.offsetConstraint, t);

        this.weightConstraint1 = Mathf.Lerp(sa.weightConstraint1, sb.weightConstraint1, t);
        this.offsetConstraint1 = Vector3.Lerp(sa.offsetConstraint1, sb.offsetConstraint1, t);

        this.weightConstraint2 = Mathf.Lerp(sa.weightConstraint2, sb.weightConstraint2, t);
        this.offsetConstraint2 = Vector3.Lerp(sa.offsetConstraint2, sb.offsetConstraint2, t);

        this.rotateRefDirOffset = Vector3.Lerp(sa.rotateRefDirOffset, sb.rotateRefDirOffset, t);
        this.maxHorizontalDeg = Mathf.Lerp(sa.maxHorizontalDeg, sb.maxHorizontalDeg, t);
        this.maxVerticalDeg = Mathf.Lerp(sa.maxVerticalDeg, sb.maxVerticalDeg, t);
        this.offsetChangedRate = Mathf.Lerp(sa.offsetChangedRate, sb.offsetChangedRate, t);

        return this;
    }
}
