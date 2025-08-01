using UnityEngine;

public interface I_Interactable 
{
    public Collider _collider { get; set; }
    public bool isBeenInteractAble { get; set; }
}
