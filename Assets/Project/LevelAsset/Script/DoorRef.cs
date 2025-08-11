using UnityEngine;

public class DoorRef : Door,I_Interactable
{
    [SerializeField] public Door refDoor;

    public override bool isLocked { get => refDoor.isLocked; set 
        {
            refDoor.isLocked = value;
            base.lockedValue = value;
        } }
    public override bool isOpen => refDoor.isOpen;
    public override Collider _collider { get => refDoor._collider; set => refDoor._collider = value; }
    public override bool isBeenInteractAble { get => refDoor.isBeenInteractAble; set => refDoor.isBeenInteractAble = value; }
    public override void DoInteract(I_Interacter i_Interacter)
    {
      refDoor.DoInteract(i_Interacter);
    }
}
