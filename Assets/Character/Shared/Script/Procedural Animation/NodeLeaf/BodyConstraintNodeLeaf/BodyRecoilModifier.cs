using UnityEngine;

public class BodyRecoilModifier
{
    private float _timer;

    public void Reset() { _timer = 1f; }
    public void Trigger() { _timer = 0f; }

    public void UpdateWeights(BodyRecoilSCRP data)
    {
        if (data == null) return;
        _timer = Mathf.Min(_timer + Time.deltaTime, data.recoilDuration);
    }

    public Vector3 ApplyOffset(Vector3 offset, BodyRecoilSCRP data, float constrainWeight)
    {
        if (data == null) return offset;
        float t = data.recoilDuration > 0f ? _timer / data.recoilDuration : 1f;
        float weight = data.recoilCurve.Evaluate(t);
        Vector3 recoilOffset = offset + data.additionalRotationOffset;

        return Vector3.Lerp(offset, Vector3.Lerp(offset, recoilOffset, weight), constrainWeight);
    }
}
