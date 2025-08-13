using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class DoorInteractablePointUIManagerNodeLeaf : InteractablePointUIManagerNodeLeaf
{
    public DoorInteractablePointUIManagerNodeLeaf(Func<bool> preCondition, InWorldUI inWorldUI, Camera camera, I_Interacter i_Interacter, LayerMask interactAbleMask) : base(preCondition, inWorldUI, camera, i_Interacter, interactAbleMask)
    {
    }
    protected override void UpdateAssignedUI()
    {
        if (assignInWorldInteractable.Count <= 0)
            return;

        bool isFoundCurrentInteractAble = false;

        List<Door> doors = new List<Door>();
        foreach (I_Interactable item in assignInWorldInteractable.Keys)
        {
            if(item is Door door)
                doors.Add(door);
        }

        for (int i = 0; i < doors.Count; i++) 
        {
            Vector3 setPos = doors[i]._collider.transform.position 
                + doors[i]._collider.transform.forward * offset.z
                + doors[i]._collider.transform.up * offset.y
                + doors[i]._collider.transform.right * offset.x;

            assignInWorldInteractable[doors[i]].SetAnchorPosition(setPos);

            if (doors[i].isBeenInteractAble == false)
            {
                objectPooling.ReturnToPool(assignInWorldInteractable[doors[i]]);
                assignInWorldInteractable.Remove(doors[i]);
                continue;
            }

            if(isFoundCurrentInteractAble)
            {
                assignInWorldInteractable[doors[i]].PlayAnimation("PointingAppear");
                continue;
            }

            if(interacter.currentInteractable is Door curDoor 
                && curDoor == doors[i])
            {
                isFoundCurrentInteractAble = true;
                if(curDoor.isLocked == false)
                    assignInWorldInteractable[doors[i]].PlayAnimation("InteractableAppear");
                else
                    assignInWorldInteractable[doors[i]].PlayAnimation("DoorLockedAppear");

                Debug.Log("Door is locked = " + curDoor.isLocked);

                continue;
            }
            else
            {
                assignInWorldInteractable[doors[i]].PlayAnimation("PointingAppear");
            }
        }
    }
}
