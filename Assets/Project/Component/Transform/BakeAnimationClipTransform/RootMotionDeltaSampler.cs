using System.Collections.Generic;
using UnityEngine;

public static class RootMotionDeltaSampler
{
    /// <summary>
    /// Interpolates deltaPosition and deltaRotation from baked keyframes at normalizedTime ∈ [0,1].
    /// </summary>
    public static void GetDeltaAtTime(List<RootMotionKeyframe> keyframes, float normalizedTime, out Vector3 deltaPos, out Quaternion deltaRot)
    {
        deltaPos = Vector3.zero;
        deltaRot = Quaternion.identity;

        if (keyframes == null || keyframes.Count < 2) return;

        normalizedTime = Mathf.Clamp01(normalizedTime);

        for (int i = 0; i < keyframes.Count - 1; i++)
        {
            RootMotionKeyframe a = keyframes[i];
            RootMotionKeyframe b = keyframes[i + 1];

            if (normalizedTime >= a.time && normalizedTime <= b.time)
            {
                float t = Mathf.InverseLerp(a.time, b.time, normalizedTime);
                deltaPos = Vector3.Lerp(a.position, b.position, t);
                deltaRot = Quaternion.Slerp(a.rotation, b.rotation, t);
                return;
            }
        }

        // Beyond last keyframe → return last delta
        RootMotionKeyframe last = keyframes[keyframes.Count - 1];
        deltaPos = last.position;
        deltaRot = last.rotation;
    }
}