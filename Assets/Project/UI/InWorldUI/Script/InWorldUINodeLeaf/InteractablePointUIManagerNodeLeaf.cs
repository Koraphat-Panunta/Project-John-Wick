using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractablePointUIManagerNodeLeaf : InWorldUINodeLeaf
{
    private FieldOfView fieldOfView;
    private InWorldUI inWorldUI;
    private Camera camera;
    private ObjectPooling<InWorldUI> objectPooling;
    private LayerMask interactableMask;
    public Dictionary<I_Interactable, InWorldUI> assignInWorldInteractable;
    private I_Interacter interacter;
    public InteractablePointUIManagerNodeLeaf(Func<bool> preCondition, InWorldUI inWorldUI, Camera camera,I_Interacter i_Interacter,LayerMask interactAbleMask) : base(preCondition)
    {
        this.interacter = i_Interacter;
        this.inWorldUI = inWorldUI;
        this.camera = camera;
        this.fieldOfView = new FieldOfView(7.5f,camera.fieldOfView,camera.transform);
        this.objectPooling = new ObjectPooling<InWorldUI>(this.inWorldUI,12,5,camera.transform.position);
        this.assignInWorldInteractable = new Dictionary<I_Interactable, InWorldUI>();
        this.interactableMask = interactAbleMask;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        this.UpdateAssignedUI();
        this.UpdateInteractableDetected();
        base.FixedUpdateNode();
    }
    private void UpdateInteractableDetected()
    {
        List<I_Interactable> interactableDetected = new List<I_Interactable>();
        foreach(GameObject obj in fieldOfView.FindMultipleTargetsInView(interactableMask))
        {
            if(obj.TryGetComponent<I_Interactable>(out I_Interactable i_Interactable) == false)
                continue;

            if(interactableDetected.Contains(i_Interactable))
                continue;

            if(i_Interactable.isBeenInteractAble == false)
                continue;

            interactableDetected.Add(i_Interactable);

            if (this.assignInWorldInteractable.ContainsKey(i_Interactable) == false)
            {
                InWorldUI inWorldUI = objectPooling.Get();
                this.assignInWorldInteractable.Add(i_Interactable, inWorldUI);
                assignInWorldInteractable[i_Interactable].PlayAnimation("PointingAppear");
            }

        }

        List<I_Interactable> interactablesAssigned = assignInWorldInteractable.Keys.ToList();
        for (int i = 0; i < interactablesAssigned.Count; i++)
        {
            if (interactableDetected.Contains(interactablesAssigned[i]) == false)
            {
                objectPooling.ReturnToPool(assignInWorldInteractable[interactablesAssigned[i]]);
                assignInWorldInteractable.Remove(interactablesAssigned[i]);
            }
        }


    }
    private void UpdateAssignedUI()
    {
        bool isFoundCurrentInteractAble = false;
        List<I_Interactable> interactables = assignInWorldInteractable.Keys.ToList();

        if(assignInWorldInteractable.Count <=0)
            return;

        for (int i = 0; i < interactables.Count; i++) 
        {
            assignInWorldInteractable[interactables[i]].SetAnchorPosition(interactables[i]._collider.transform.position);

            if (interactables[i].isBeenInteractAble == false)
            {
                objectPooling.ReturnToPool(assignInWorldInteractable[interactables[i]]);
                assignInWorldInteractable.Remove(interactables[i]);
                continue;
            }

            if(isFoundCurrentInteractAble)
            {
                assignInWorldInteractable[interactables[i]].PlayAnimation("PointingAppear");
                continue;
            }

            if(interacter.currentInteractable == interactables[i])
            {
                isFoundCurrentInteractAble = true;
                assignInWorldInteractable[interactables[i]].PlayAnimation("InteractableAppear");
                continue;
            }
            else
            {
                assignInWorldInteractable[interactables[i]].PlayAnimation("PointingAppear");
            }
        }
    }

}
