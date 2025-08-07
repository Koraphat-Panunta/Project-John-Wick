using UnityEngine;

public class FindingInteractable 
{
    public static bool FindingInteractableObject(LayerMask layerMask,float distanceDetect,float sphereCastRadius, Vector3 startCast,Vector3 dirCast,out I_Interactable i_Interactable)
    {
        i_Interactable = null;

        // Raycast check
        if (Physics.Raycast(startCast, dirCast, out RaycastHit rayHit, distanceDetect, layerMask,QueryTriggerInteraction.Ignore))
        {
            if (rayHit.collider.TryGetComponent<I_Interactable>(out i_Interactable)
                && i_Interactable.isBeenInteractAble)
                return true;
        }

        // SphereCast fallback
        if (Physics.SphereCast(startCast, sphereCastRadius, dirCast, out RaycastHit sphereHit, distanceDetect, layerMask,QueryTriggerInteraction.Ignore))
        {
            if (sphereHit.collider.TryGetComponent<I_Interactable>(out i_Interactable)
                && i_Interactable.isBeenInteractAble)
                return true;
        }

        return false;
    }
}
