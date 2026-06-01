using UnityEngine;

/// <summary>
/// Single responsibility: weapon recoil for a hand IK target. Holds the decaying recoil
/// weights, gets kicked by <see cref="Trigger"/> on fire, and applies an additive offset
/// (driven by a <see cref="WeaponHandRecoilSCRP"/>) on top of a hand position/rotation.
/// All methods no-op when no data is supplied.
/// </summary>
public class HandRecoilModifier
{
    private float weightPos;
    private float weightRot;

    public void Reset()
    {
        this.weightPos = 0;
        this.weightRot = 0;
    }

    public void Trigger(float weight)
    {
        this.weightPos = Mathf.Clamp01(weight);
        this.weightRot = Mathf.Clamp01(weight);
    }

    public void UpdateWeights(WeaponHandRecoilSCRP data)
    {
        if (data == null)
            return;

        this.weightPos = Mathf.Clamp01(this.weightPos - Time.deltaTime * data.positionRecoverySpeed);
        this.weightRot = Mathf.Clamp01(this.weightRot - Time.deltaTime * data.rotationRecoverySpeed);
    }

    public Vector3 ApplyPosition(Vector3 position, Transform recoilDir, WeaponHandRecoilSCRP data)
    {
        if (data == null)
            return position;

        Vector3 recoilPos = position
            + recoilDir.forward * data.additionalPositionOffset.z
            + recoilDir.up * data.additionalPositionOffset.y
            + recoilDir.right * data.additionalPositionOffset.x;

        return Vector3.Lerp(position, recoilPos, this.weightPos);
    }

    public Quaternion ApplyRotation(Quaternion rotation, WeaponHandRecoilSCRP data)
    {
        if (data == null)
            return rotation;

        Quaternion recoilRot = rotation * Quaternion.Euler(data.additionalRotationEulerOffset);
        return Quaternion.Lerp(rotation, recoilRot, this.weightRot);
    }
}
