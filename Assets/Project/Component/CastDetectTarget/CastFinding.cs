using System.Collections.Generic;
using UnityEngine;
public static class CastFinding 
{
    public static bool FindObectInViewByComponent<T>(
        Vector3 startCast
        , Vector3 castDir
        , float castDistance
        , float castRaduis
        , LayerMask castLayerMask
        , out T detect)
    {
        detect = default(T);
        if (Physics.Raycast(startCast, castDir, out RaycastHit hitInfo, castDistance, castLayerMask | LayerMask.GetMask("Default")))
        {
            if (hitInfo.collider.TryGetComponent<T>(out T component))
            {
                detect = component;
                return true;
            }
        }

        Collider[] colliders = Physics.OverlapCapsule(startCast, startCast + (castDir * castDistance), castRaduis, castLayerMask);


        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent<T>(out T colliderComponentDetect) == false)
                continue;


            Vector3 toTarget = (colliders[i].transform.position - startCast).normalized;

            if (Physics.Raycast(startCast, toTarget, out RaycastHit hit, castDistance, castLayerMask | LayerMask.GetMask("Default")))
            {
                if (hit.collider.gameObject == colliders[i].gameObject)
                {
                    detect = colliderComponentDetect;
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Find the single object inside a cone of vision whose direction is closest to <paramref name="castDir"/>.
    /// Uses an OverlapSphere then filters by angle, with an optional line-of-sight ray to reject blocked targets.
    /// </summary>
    /// <param name="origin">Cone apex (e.g. camera or player position).</param>
    /// <param name="castDir">Cone forward axis (will be normalized).</param>
    /// <param name="castDistance">Max range of the cone.</param>
    /// <param name="halfAngleDegrees">Half-angle of the cone in degrees (e.g. 30 = 60° FOV).</param>
    /// <param name="castLayerMask">Layers to consider for targets.</param>
    /// <param name="detect">Closest-to-axis component found, or default if none.</param>
    /// <param name="obstacleLayerMask">Layers that block line of sight. Pass 0 to skip the LOS check.</param>
    public static bool FindObjectInConeByComponent<T>(
        Vector3 origin,
        Vector3 castDir,
        float castDistance,
        float halfAngleDegrees,
        LayerMask castLayerMask,
        out T detect,
        LayerMask obstacleLayerMask = default)
    {
        detect = default;
        if (castDir.sqrMagnitude < Mathf.Epsilon)
            return false;

        castDir.Normalize();
        float cosHalf = Mathf.Cos(halfAngleDegrees * Mathf.Deg2Rad);
        bool checkLOS = obstacleLayerMask.value != 0;

        Collider[] colliders = Physics.OverlapSphere(origin, castDistance, castLayerMask);

        float bestDot = -1f;
        T bestComponent = default;
        bool found = false;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent<T>(out T component) == false)
                continue;

            Vector3 toTarget = colliders[i].transform.position - origin;
            float dist = toTarget.magnitude;
            if (dist < Mathf.Epsilon || dist > castDistance)
                continue;

            Vector3 dir = toTarget / dist;
            float dot = Vector3.Dot(castDir, dir);
            if (dot < cosHalf)
                continue;

            if (checkLOS && Physics.Raycast(origin, dir, out RaycastHit hit, dist, obstacleLayerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.gameObject != colliders[i].gameObject)
                    continue;
            }

            if (dot > bestDot)
            {
                bestDot = dot;
                bestComponent = component;
                found = true;
            }
        }

        if (found)
            detect = bestComponent;
        return found;
    }

    /// <summary>
    /// Find every object inside the cone of vision. Results are sorted by closeness to the cone axis (most centered first).
    /// </summary>
    public static bool FindAllObjectsInConeByComponent<T>(
        Vector3 origin,
        Vector3 castDir,
        float castDistance,
        float halfAngleDegrees,
        LayerMask castLayerMask,
        out List<T> detects,
        LayerMask obstacleLayerMask = default)
    {
        detects = new List<T>();
        if (castDir.sqrMagnitude < Mathf.Epsilon)
            return false;

        castDir.Normalize();
        float cosHalf = Mathf.Cos(halfAngleDegrees * Mathf.Deg2Rad);
        bool checkLOS = obstacleLayerMask.value != 0;

        Collider[] colliders = Physics.OverlapSphere(origin, castDistance, castLayerMask);

        List<(T component, float dot)> hits = new List<(T, float)>(colliders.Length);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent<T>(out T component) == false)
                continue;

            Vector3 toTarget = colliders[i].transform.position - origin;
            float dist = toTarget.magnitude;
            if (dist < Mathf.Epsilon || dist > castDistance)
                continue;

            Vector3 dir = toTarget / dist;
            float dot = Vector3.Dot(castDir, dir);
            if (dot < cosHalf)
                continue;

            if (checkLOS && Physics.Raycast(origin, dir, out RaycastHit hit, dist, obstacleLayerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.gameObject != colliders[i].gameObject)
                    continue;
            }

            hits.Add((component, dot));
        }

        hits.Sort((a, b) => b.dot.CompareTo(a.dot));
        for (int i = 0; i < hits.Count; i++)
            detects.Add(hits[i].component);

        return detects.Count > 0;
    }
}
