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
}
