using UnityEngine;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;

public static class CastFinding 
{
    public static bool FindObectInViewByComponent<T>(Vector3 startCast,Vector3 castDir,float castDistance,float castRaduis,LayerMask castLayerMask, out T detect)
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


        for (int i = 0;i < colliders.Length; i++)
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
}
