using UnityEngine;
using UnityEngine.AI;

public class BlindSpotFinder 

{
    /// <summary>
    /// Walks the NavMesh path from <paramref name="targetCurrentPos"/> to <paramref name="targetFinalPos"/>,
    /// sampling at <paramref name="sampleSpacing"/>-meter intervals, and returns true if line of
    /// sight from <paramref name="observerPos"/> breaks somewhere along it.
    /// </summary>
    /// <param name="observerPos">Where the enemy is looking from (head/eye position, not feet).</param>
    /// <param name="targetCurrentPos">The target's current position (path start).</param>
    /// <param name="targetFinalPos">The candidate destination (path end).</param>
    /// <param name="obstructionMask">Layers that should block sight. Must EXCLUDE the target and observer themselves.</param>
    /// <param name="lastVisiblePoint">
    /// Output: the last path sample where line of sight was still clear.
    /// Valid only when the method returns true.
    /// </param>
    /// <param name="firstBlockedPoint">
    /// Output: the first path sample where line of sight broke. Valid only when the method returns true.
    /// Useful if the AI wants to aim slightly past the cover threshold.
    /// </param>
    /// <param name="sampleSpacing">Distance between samples along the path (meters). Smaller = more accurate, more raycasts. 0.5 is a good default.</param>
    /// <param name="targetEyeHeight">Height above the path to test sight to (the target's body, not their feet). 1.6 is humanoid eye level.</param>
    /// <param name="areaMask">NavMesh area mask. Default is all areas.</param>
    /// <returns>
    /// True if a blind spot was found.
    /// False if (a) no path exists, (b) the target is already invisible at start, or
    /// (c) the entire path is visible (no cover gained — AI shouldn't expect them to hide).
    /// </returns>
    public static bool TryFindCoverEntryAlongPath(
        Vector3 observerPos,
        Vector3 targetCurrentPos,
        Vector3 targetFinalPos,
        LayerMask obstructionMask,
        out Vector3 lastVisiblePoint,
        out Vector3 firstBlockedPoint,
        float sampleSpacing = 0.5f,
        float targetEyeHeight = 1.6f,
        int areaMask = NavMesh.AllAreas)
    {
        lastVisiblePoint = targetCurrentPos;
        firstBlockedPoint = targetFinalPos;

        // 1. Compute the NavMesh path.
        var path = new NavMeshPath();
        if (!NavMesh.CalculatePath(targetCurrentPos, targetFinalPos, areaMask, path))
            return false;
        if (path.corners == null || path.corners.Length < 2)
            return false;

        // 2. Verify the target is currently visible at all. If not, no useful prediction.
        Vector3 startEye = path.corners[0] + Vector3.up * targetEyeHeight;
        if (!HasLineOfSight(observerPos, startEye, obstructionMask))
            return false;

        lastVisiblePoint = path.corners[0];
        bool foundBlock = false;

        // 3. Walk corner-pair segments, subdividing each by sampleSpacing.
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Vector3 a = path.corners[i];
            Vector3 b = path.corners[i + 1];
            float segmentLength = Vector3.Distance(a, b);
            int sampleCount = Mathf.Max(1, Mathf.CeilToInt(segmentLength / sampleSpacing));

            for (int s = 1; s <= sampleCount; s++)
            {
                float t = (float)s / sampleCount;
                Vector3 sample = Vector3.Lerp(a, b, t);
                Vector3 sampleEye = sample + Vector3.up * targetEyeHeight;

                if (HasLineOfSight(observerPos, sampleEye, obstructionMask))
                {
                    lastVisiblePoint = sample;
                }
                else
                {
                    firstBlockedPoint = sample;
                    foundBlock = true;
                    return true; // first occlusion wins
                }
            }
        }

        // 4. Walked the whole path without losing sight — target stays exposed.
        return foundBlock;
    }

    /// <summary>Pure raycast helper. Triggers ignored by default to avoid trigger-volume false positives.</summary>
    private static bool HasLineOfSight(Vector3 from, Vector3 to, LayerMask mask)
    {
        Vector3 delta = to - from;
        float dist = delta.magnitude;
        if (dist < 0.001f) return true;
        return !Physics.Raycast(from, delta / dist, dist, mask, QueryTriggerInteraction.Ignore);
    }

    // ----- Optional: same logic but draws Gizmo lines for debugging --------

    public static bool TryFindCoverEntryAlongPath_Debug(
        Vector3 observerPos,
        Vector3 targetCurrentPos,
        Vector3 targetFinalPos,
        LayerMask obstructionMask,
        out Vector3 lastVisiblePoint,
        out Vector3 firstBlockedPoint,
        float sampleSpacing = 0.5f,
        float targetEyeHeight = 1.6f,
        int areaMask = NavMesh.AllAreas)
    {
        bool found = TryFindCoverEntryAlongPath(
            observerPos, targetCurrentPos, targetFinalPos,
            obstructionMask, out lastVisiblePoint, out firstBlockedPoint,
            sampleSpacing, targetEyeHeight, areaMask);

        Debug.DrawLine(observerPos, targetCurrentPos + Vector3.up * targetEyeHeight, Color.gray, 0.5f);
        if (found)
        {
            Debug.DrawLine(observerPos, lastVisiblePoint + Vector3.up * targetEyeHeight, Color.green, 0.5f);
            Debug.DrawLine(observerPos, firstBlockedPoint + Vector3.up * targetEyeHeight, Color.red, 0.5f);
            Debug.DrawLine(lastVisiblePoint, firstBlockedPoint, Color.yellow, 0.5f);
        }
        return found;
    }
}
