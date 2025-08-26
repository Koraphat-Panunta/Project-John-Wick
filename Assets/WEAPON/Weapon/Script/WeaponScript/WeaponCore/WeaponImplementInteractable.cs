using UnityEngine;

public partial class Weapon : I_Interactable
{
    public Collider _collider { get; set; }
    public bool isBeenInteractAble { get 
        {
            if(userWeapon != null)
                return false;

            return true;
        }set { } }

    public Transform _transform { get => transform; set { } }

    public void DoInteract(I_Interacter i_Interacter)
    {
        
    }
}
