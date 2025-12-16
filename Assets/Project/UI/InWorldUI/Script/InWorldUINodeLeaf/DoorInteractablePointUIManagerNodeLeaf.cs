using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class DoorInteractablePointUIManagerNodeLeaf : InteractablePointUIManagerNodeLeaf
{
    public DoorInteractablePointUIManagerNodeLeaf(Func<bool> preCondition, InWorldUI inWorldUI, Camera camera, I_Interacter i_Interacter, LayerMask interactAbleMask,Vector3 offset) : base(preCondition, inWorldUI, camera, i_Interacter, interactAbleMask,offset)
    {
    }
  
    protected override void UpdateAssignedUI()
    {
        if (assignInWorldInteractable.Count <= 0)
            return;

        bool isFoundCurrentInteractAble = false;

        List<DoorActor> doors = new List<DoorActor>();
        foreach (I_Interactable item in assignInWorldInteractable.Keys)
        {
            if(item is DoorActor door)
                doors.Add(door);
        }

        for (int i = 0; i < doors.Count; i++) 
        {
            Vector3 setPos = doors[i]._transform.position 
                + doors[i]._transform.forward * offset.z
                + doors[i]._transform.up * offset.y
                + doors[i]._transform.right * offset.x;

            assignInWorldInteractable[doors[i]].SetAnchorPosition(setPos);

            Debug.DrawLine(camera.transform.position, doors[i].transform.position,Color.red);

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

            if(interacter.currentInteractable is DoorActor curDoor 
                && curDoor == doors[i])
            {
                isFoundCurrentInteractAble = true;
                if(curDoor.isLocked == false)
                    assignInWorldInteractable[doors[i]].PlayAnimation("InteractableAppear");
                else
                    assignInWorldInteractable[doors[i]].PlayAnimation("DoorLockedAppear");

                continue;
            }
            else
            {
                assignInWorldInteractable[doors[i]].PlayAnimation("PointingAppear");
            }
        }
    }
}
