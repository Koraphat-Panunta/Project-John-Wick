using UnityEngine;

public partial class Weapon : I_Interactable
{
    public Collider _collider { get => this.Collider; set => this.Collider = value; }
    [SerializeField] private Collider Collider;
    public bool isBeenInteractAble { get 
        {
            if(userWeapon != null)
                return false;
            if(_isBeenThrow)
                return false;

            return true;
        }set { } }

    public Transform _transform { get => transform; set { } }

    public void DoInteract(I_Interacter i_Interacter)
    {
        
    }
}
