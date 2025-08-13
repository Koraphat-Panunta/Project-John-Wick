using UnityEngine;

public interface I_Interactable 
{
    public Collider _collider { get; set; }
    public bool isBeenInteractAble { get; set; }
    public void DoInteract(I_Interacter i_Interacter);
}
