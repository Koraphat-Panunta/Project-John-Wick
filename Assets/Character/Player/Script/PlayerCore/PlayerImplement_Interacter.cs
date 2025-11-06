using UnityEngine;

public partial class Player : I_Interacter
{
    public bool _isInteractCommand { get; set; }
    public I_Interactable currentInteractable { get; set; }
    [SerializeField] private LayerMask findInteractAbleObject;

    [Range(0, 10)]
    [SerializeField] private float interacter_distaceDetect;

    [Range(0, 10)]
    [SerializeField] private float interacter_sphereCastDetect;

    [SerializeField] private string interactableName;

    [SerializeField] public AnimationClip pokePickUpClip;

    [SerializeField] public Transform rightFootss;
    private void UpdateFindingInteractableObject()
    {
        Vector3 castPos = this.centreTransform.position;
        Vector3 castDir = (weaponAdvanceUser._pointingPos - castPos).normalized;

        if(FindingInteractable.FindingInteractableObject(
            this.findInteractAbleObject
            ,this.interacter_distaceDetect
            ,this.interacter_sphereCastDetect
            ,castPos
            ,castDir
            ,out I_Interactable i_Interactable))
            currentInteractable = i_Interactable;
        else
            currentInteractable = null;

        if(currentInteractable != null)
            interactableName = currentInteractable.ToString();
        else
            interactableName = " none";
    }

    public void Interact()
    {
        if (currentInteractable != null && currentInteractable.isBeenInteractAble)
        {
            currentInteractable.DoInteract(this);
        }
    }
}
