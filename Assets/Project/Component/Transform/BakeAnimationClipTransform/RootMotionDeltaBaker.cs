using System.Collections.Generic;
using UnityEngine;

public static class RootMotionBaker
{
    /// <summary>
    /// Bakes root motion from animation clip into keyframes sampled at 'frameRate' FPS.
    /// Returns list of RootMotionKeyframe.
    /// </summary>
    public static List<RootMotionKeyframe> BakeRootMotion(AnimationClip clip,GameObject sampleDummy ,float frameRate = 60f)
    {
        if (clip == null) return null;

        List<RootMotionKeyframe> keyframes = new List<RootMotionKeyframe>();

        float clipLength = clip.length;
        int totalFrames = Mathf.CeilToInt(clipLength * frameRate);

        for (int i = 0; i <= totalFrames; i++)
        {
            float time = (i / frameRate);
            float normalizedTime = Mathf.Clamp01(time / clipLength);

            // Match dummy to identity each sample (ensures absolute root pose)
            sampleDummy.transform.position = Vector3.zero;
            sampleDummy.transform.rotation = Quaternion.identity;

            clip.SampleAnimation(sampleDummy, time);

            keyframes.Add(new RootMotionKeyframe
            {
                position = sampleDummy.transform.position,
                rotation = sampleDummy.transform.rotation,
                time = normalizedTime,
            });
        }

        return keyframes;
    }
}
