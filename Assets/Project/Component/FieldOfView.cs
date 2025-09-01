using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldOfView
{
    private float distance;
    private float angleInDegrees;
    public Transform viewOrigin { get; private set; }
    private LayerMask collideLayerMask;

    public FieldOfView(float distance, float angleInDegrees, Transform viewOrigin)
        : this(distance, angleInDegrees, viewOrigin, LayerMask.GetMask("Default")) { }

    public FieldOfView(float distance, float angleInDegrees, Transform viewOrigin, LayerMask collideLayerMask)
    {
        this.distance = distance;
        this.angleInDegrees = angleInDegrees;
        this.viewOrigin = viewOrigin;
        this.collideLayerMask.value = collideLayerMask.value;
    }

    // Existing method (unchanged)
    public GameObject FindSingleTarget(LayerMask targetMask, Vector3? offset = null, Vector3? customLookDir = null, float? customAngle = null)
    {
        Collider[] hits = Physics.OverlapSphere(viewOrigin.position, distance, targetMask.value);
        if (hits.Length == 0) return null;

        Vector3 origin = viewOrigin.position + (offset ?? Vector3.zero);
        Vector3 forward = customLookDir ?? viewOrigin.forward;
        float angleLimit = (customAngle ?? angleInDegrees) / 2f;

        foreach (Collider target in hits)
        {
            Vector3 toTarget = (target.transform.position - origin).normalized;
            if (Vector3.Angle(forward, toTarget) > angleLimit) continue;

            if (Physics.Raycast(origin, toTarget, out RaycastHit hit, distance, collideLayerMask.value | targetMask.value))
            {
                if (hit.collider.gameObject == target.gameObject)
                    return target.gameObject;
            }
        }
        return null;
    }

    // ✅ New overload with QueryTriggerInteraction
    public GameObject FindSingleTarget(LayerMask targetMask, QueryTriggerInteraction triggerInteraction, Vector3? offset = null, Vector3? customLookDir = null, float? customAngle = null)
    {
        Collider[] hits = Physics.OverlapSphere(viewOrigin.position, distance, targetMask.value, triggerInteraction);
        if (hits.Length == 0) return null;

        Vector3 origin = viewOrigin.position + (offset ?? Vector3.zero);
        Vector3 forward = customLookDir ?? viewOrigin.forward;
        float angleLimit = (customAngle ?? angleInDegrees) / 2f;

        foreach (Collider target in hits)
        {
            Vector3 toTarget = (target.transform.position - origin).normalized;
            if (Vector3.Angle(forward, toTarget) > angleLimit) continue;

            if (Physics.Raycast(origin, toTarget, out RaycastHit hit, distance, collideLayerMask.value | targetMask.value, triggerInteraction))
            {
                if (hit.collider.gameObject == target.gameObject)
                    return target.gameObject;
            }
        }
        return null;
    }

    // Existing method (unchanged)
    public bool TryFindSingleTarget(LayerMask targetMask, out GameObject targetObj, Vector3? offset = null, Vector3? customLookDir = null, float? customAngle = null)
    {
        targetObj = FindSingleTarget(targetMask, offset, customLookDir, customAngle);
        return targetObj != null;
    }

    // ✅ New overload with QueryTriggerInteraction
    public bool TryFindSingleTarget(LayerMask targetMask, out GameObject targetObj, QueryTriggerInteraction triggerInteraction, Vector3? offset = null, Vector3? customLookDir = null, float? customAngle = null)
    {
        targetObj = FindSingleTarget(targetMask, triggerInteraction, offset, customLookDir, customAngle);
        return targetObj != null;
    }

    // Existing method (unchanged)
    public List<GameObject> FindMultipleTargetsInView(LayerMask targetMask)
    {
        return FindMultipleTargets(targetMask, inViewOnly: true);
    }

    public List<GameObject> FindMultipleTargetsInArea(LayerMask targetMask, float? customRadius = null)
    {
        return FindMultipleTargets(targetMask, customRadius ?? distance, false);
    }

    // ✅ New overload
    public List<GameObject> FindMultipleTargetsInView(LayerMask targetMask, QueryTriggerInteraction triggerInteraction)
    {
        return FindMultipleTargets(targetMask, distance, true, triggerInteraction);
    }

    public List<GameObject> FindMultipleTargetsInArea(LayerMask targetMask, QueryTriggerInteraction triggerInteraction, float? customRadius = null)
    {
        return FindMultipleTargets(targetMask, customRadius ?? distance, false, triggerInteraction);
    }

    // Existing internal method (unchanged)
    private List<GameObject> FindMultipleTargets(LayerMask targetMask, float searchRadius = -1f, bool inViewOnly = true)
    {
        return FindMultipleTargets(targetMask, searchRadius, inViewOnly, QueryTriggerInteraction.UseGlobal);
    }

    // ✅ New internal overload
    private List<GameObject> FindMultipleTargets(LayerMask targetMask, float searchRadius, bool inViewOnly, QueryTriggerInteraction triggerInteraction)
    {
        List<GameObject> results = new List<GameObject>();
        float actualRadius = searchRadius > 0 ? searchRadius : distance;
        Collider[] hits = Physics.OverlapSphere(viewOrigin.position, actualRadius, targetMask.value, triggerInteraction);

        if (hits.Length == 0) return results;

        foreach (Collider target in hits)
        {
            Vector3 toTarget = (target.transform.position - viewOrigin.position).normalized;
            if (inViewOnly && Vector3.Angle(viewOrigin.forward, toTarget) > angleInDegrees / 2f)
                continue;

            if (Physics.Raycast(viewOrigin.position, toTarget, out RaycastHit hit, actualRadius, collideLayerMask.value | targetMask.value, triggerInteraction))
            {
                if (hit.collider.gameObject == target.gameObject)
                    results.Add(target.gameObject);
            }
        }
        return results;
    }
}
