using UnityEngine;

public partial class Enemy : I_Interacter
{
    public I_Interactable currentInteractable { get ; set ; }

    public void Interact()
    {
        if (this.currentInteractable != null)
            this.currentInteractable.DoInteract(this);
    }

    public bool FindInteractAble<T>(Vector3 findDir,float distance,out T i_Interactable)
    {
        i_Interactable = (default);

        //Debug.DrawRay(this._hipBone.position, findDir * distance,Color.blue,2);

        if(Physics.Raycast(this.humanoidBone.hips.position
            , findDir
            , out RaycastHit hit
            , distance
            , LayerMask.GetMask("InteractAble")
            , QueryTriggerInteraction.Collide)
            )
        {
            if (hit.collider.gameObject.TryGetComponent<I_Interactable>(out I_Interactable InteractAble)
                && InteractAble is T result)
            {
                i_Interactable = result;
                return true;
            }
                
        }

        return false;
    }
}
