using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldOfView
{
    private float radius;
    private float angleInDegrees;
    private Transform viewOrigin;
    private LayerMask defaultLayerMask;

    public FieldOfView(float radius, float angleInDegrees, Transform viewOrigin)
    {
        this.radius = radius;
        this.angleInDegrees = angleInDegrees;
        this.viewOrigin = viewOrigin;
        this.defaultLayerMask = LayerMask.GetMask("Default");
    }

    public GameObject FindSingleTarget(LayerMask targetMask, Vector3? offset = null, Vector3? customLookDir = null, float? customAngle = null)
    {
        Collider[] hits = Physics.OverlapSphere(viewOrigin.position, radius, targetMask);
        if (hits.Length == 0) return null;

        Vector3 origin = viewOrigin.position + (offset ?? Vector3.zero);
        Vector3 forward = customLookDir ?? viewOrigin.forward;
        float angleLimit = (customAngle ?? angleInDegrees) / 2f;

        foreach (Collider target in hits)
        {
            Vector3 toTarget = (target.transform.position - origin).normalized;
            if (Vector3.Angle(forward, toTarget) > angleLimit) continue;

            if (Physics.Raycast(origin, toTarget, out RaycastHit hit, radius, defaultLayerMask | targetMask))
            {
                if (hit.collider.gameObject == target.gameObject)
                    return target.gameObject;
            }
        }
        return null;
    }

    public bool TryFindSingleTarget(LayerMask targetMask, out GameObject targetObj, Vector3? offset = null, Vector3? customLookDir = null, float? customAngle = null)
    {
        targetObj = FindSingleTarget(targetMask, offset, customLookDir, customAngle);
        return targetObj != null;
    }

    public List<GameObject> FindMultipleTargetsInView(LayerMask targetMask)
    {
        return FindMultipleTargets(targetMask, inViewOnly: true);
    }

    public List<GameObject> FindMultipleTargetsInArea(LayerMask targetMask, float? customRadius = null)
    {
        return FindMultipleTargets(targetMask, customRadius ?? radius, false);
    }

    private List<GameObject> FindMultipleTargets(LayerMask targetMask, float searchRadius = -1f, bool inViewOnly = true)
    {
        List<GameObject> results = new List<GameObject>();
        float actualRadius = searchRadius > 0 ? searchRadius : radius;
        Collider[] hits = Physics.OverlapSphere(viewOrigin.position, actualRadius, targetMask);

        if (hits.Length == 0) return results;

        foreach (Collider target in hits)
        {
            Vector3 toTarget = (target.transform.position - viewOrigin.position).normalized;
            if (inViewOnly && Vector3.Angle(viewOrigin.forward, toTarget) > angleInDegrees / 2f)
                continue;

            if (Physics.Raycast(viewOrigin.position, toTarget, out RaycastHit hit, actualRadius, defaultLayerMask | targetMask))
            {
                if (hit.collider.gameObject == target.gameObject)
                    results.Add(target.gameObject);
            }
        }
        return results;
    }
}
