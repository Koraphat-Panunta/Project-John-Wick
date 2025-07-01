using UnityEngine;

public static class AnimationRootMotionMover
{
    /// <summary>
    /// Moves the transform by sampling delta root motion from the animation clip at normalizedTimeClipLength.
    /// </summary>
    /// <param name="transform">Transform to move.</param>
    /// <param name="clip">Animation clip to sample.</param>
    /// <param name="normalizedTime">Normalized time (0-1) in the clip.</param>
    public static void MoveTransformByClipDelta(Transform transform, AnimationClip clip, float normalizedTime)
    {
        if (clip == null) return;

        // Convert normalized time to seconds in the clip
        float clipTime = Mathf.Clamp01(normalizedTime) * clip.length;
        float previousTime = Mathf.Clamp(clipTime - Time.deltaTime, 0f, clip.length);

        // Create dummy GameObjects to sample poses at previous and current times
        GameObject prevDummy = new GameObject("PrevDummy");
        GameObject currDummy = new GameObject("CurrDummy");

        // Match their initial transforms to current object
        prevDummy.transform.position = transform.position;
        prevDummy.transform.rotation = transform.rotation;

        currDummy.transform.position = transform.position;
        currDummy.transform.rotation = transform.rotation;

        // Sample animation poses
        clip.SampleAnimation(prevDummy, previousTime);
        clip.SampleAnimation(currDummy, clipTime);

        // Calculate delta between sampled poses
        Vector3 deltaPos = currDummy.transform.position - prevDummy.transform.position;
        Quaternion deltaRot = currDummy.transform.rotation * Quaternion.Inverse(prevDummy.transform.rotation);

        // Apply delta to actual transform
        transform.position += deltaPos;
        transform.rotation = transform.rotation * deltaRot;

        // Clean up dummy objects
#if UNITY_EDITOR
        GameObject.DestroyImmediate(prevDummy);
        GameObject.DestroyImmediate(currDummy);
#else
        GameObject.Destroy(prevDummy);
        GameObject.Destroy(currDummy);
#endif
    }
}
