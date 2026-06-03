using UnityEngine;

public class HandRecoilModifier
{
    private float _posTimer;
    private float _rotTimer;
    public float weight;
    public void Reset()
    {
        _posTimer = 1;
        _rotTimer = 1;
        this.weight = 0;
    }

    public void Trigger()
    {
        _posTimer = 0f;
        _rotTimer = 0f;
        this.weight = 0;
    }

    public void UpdateWeights(WeaponHandRecoilSCRP data)
    {
        if (data == null)
            return;

        _posTimer = Mathf.Min(_posTimer + Time.deltaTime, data.positionRecoilDurattion);
        _rotTimer = Mathf.Min(_rotTimer + Time.deltaTime, data.rotationRecoilDurattion);
    }

    public Vector3 ApplyPosition(Vector3 position, Transform recoilDir, WeaponHandRecoilSCRP data,float constrainWeight)
    {
        if (data == null)
            return position;

        float t = data.positionRecoilDurattion > 0f ? _posTimer / data.positionRecoilDurattion : 1f;
        float weight = data.positionRecoilCurve.Evaluate(t);

        Vector3 recoilPos = position
            + recoilDir.forward * data.additionalPositionOffset.z
            + recoilDir.up    * data.additionalPositionOffset.y
            + recoilDir.right  * data.additionalPositionOffset.x;


        return Vector3.Lerp(position, Vector3.Lerp(position, recoilPos, weight), constrainWeight) ;
    }

    public Quaternion ApplyRotation(Quaternion rotation, WeaponHandRecoilSCRP data, float constrainWeight)
    {
        if (data == null)
            return rotation;

        float t = data.rotationRecoilDurattion > 0f ? _rotTimer / data.rotationRecoilDurattion : 1f;
        this.weight = data.rotationRecoilCurve.Evaluate(t);

        Quaternion recoilRot = rotation * Quaternion.Euler(data.additionalRotationEulerOffset);
        return Quaternion.Lerp(rotation, Quaternion.Lerp(rotation, recoilRot, weight), constrainWeight) ;
    }
}
