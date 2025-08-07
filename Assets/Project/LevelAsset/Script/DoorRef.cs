using UnityEngine;

public class DoorRef : MonoBehaviour,I_Interactable
{
    [SerializeField] public Door refDoor;

    public Collider _collider { get => refDoor._collider; set => refDoor._collider = value; }
    public bool isBeenInteractAble { get => refDoor.isBeenInteractAble; set => refDoor.isBeenInteractAble = value; }

    public void DoInteract(I_Interacter i_Interacter)
    {
      refDoor.DoInteract(i_Interacter);
    }
}
