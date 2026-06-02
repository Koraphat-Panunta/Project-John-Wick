using UnityEngine;

/// <summary>
/// Single responsibility: muzzle-blocking for a hand IK target. Raycasts from the hand
/// along the aim direction toward the muzzle; while the path is obstructed it raises a
/// blocked weight that lerps the hand to a "blocked" pose (driven by a
/// <see cref="WeaponHandBlockSCRP"/>). All methods no-op when no data is supplied.
/// </summary>
public class HandBlockModifier
{
    private float blockedWeight;
    private float targetBlockWeight;

    public void Reset()
    {
        //this.blockedWeight = 0;
        this.targetBlockWeight = 0;
    }

    public void UpdateBlocking(Vector3 handPos, Vector3 aimDir, Vector3 muzzlePos, WeaponHandBlockSCRP data)
    {
        if (data == null)
            return;

        Vector3 castEnd = Vector3.Project(muzzlePos - handPos, aimDir) + handPos;
        Vector3 handToEnd = castEnd - handPos;

        if (Physics.Raycast(handPos, handToEnd.normalized, out _, handToEnd.magnitude, data.blockMask, QueryTriggerInteraction.Ignore))
        {
            this.targetBlockWeight = Mathf.Clamp01(this.targetBlockWeight + Time.deltaTime * data.raiseSpeed);
        }
        else if (Physics.Raycast(handPos, handToEnd.normalized, handToEnd.magnitude + data.clearBuffer, data.blockMask, QueryTriggerInteraction.Ignore) == false)
        {
            this.targetBlockWeight = Mathf.Clamp01(this.targetBlockWeight - Time.deltaTime * data.lowerSpeed);
        }

        this.blockedWeight = Mathf.Lerp(this.blockedWeight, this.targetBlockWeight, Time.deltaTime * data.smoothingSpeed);
    }

    public Vector3 ApplyPosition(Vector3 basePos, Vector3 handPos, Vector3 forward, Vector3 rightWard, Vector3 upWard, WeaponHandBlockSCRP data)
    {
        if (data == null)
            return basePos;

        Vector3 blockedPos = handPos
            + forward * data.positionOffset.z
            + rightWard * data.positionOffset.x
            + upWard * data.positionOffset.y;

        return Vector3.Lerp(basePos, blockedPos, this.blockedWeight);
    }

    public Quaternion ApplyRotation(Quaternion baseRot, Vector3 finalHandPos, Vector3 aimingAtPos, Vector3 refRotUp, WeaponHandBlockSCRP data)
    {
        if (data == null)
            return baseRot;

        Quaternion blockedRot = Quaternion.LookRotation((aimingAtPos - finalHandPos).normalized, refRotUp)
            * Quaternion.Euler(data.rotationEulerOffset);

        return Quaternion.Lerp(baseRot, blockedRot, this.blockedWeight);
    }

    public Vector3 ApplyHint(Vector3 baseHint, Vector3 finalHandPos, Transform hintHand, WeaponHandBlockSCRP data)
    {
        if (data == null)
            return baseHint;

        Vector3 blockedHint = finalHandPos
            + hintHand.forward * data.hintPositionOffset.z
            + hintHand.up * data.hintPositionOffset.y
            + hintHand.right * data.hintPositionOffset.x;

        return Vector3.Lerp(baseHint, blockedHint, this.blockedWeight);
    }
}
