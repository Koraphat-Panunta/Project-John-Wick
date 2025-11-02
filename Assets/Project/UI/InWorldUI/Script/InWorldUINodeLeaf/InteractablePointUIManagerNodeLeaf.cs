using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class InteractablePointUIManagerNodeLeaf : InWorldUINodeLeaf
{
    protected FieldOfView fieldOfView;
    protected InWorldUI inWorldUI;
    protected Camera camera;
    protected ObjectPooling<InWorldUI> objectPooling;
    protected LayerMask interactableMask;
    protected Dictionary<I_Interactable, InWorldUI> assignInWorldInteractable;
    protected I_Interacter interacter;
    protected Vector3 offset;

    protected virtual float searchRadius { get => 7.5f; }
    public InteractablePointUIManagerNodeLeaf(Func<bool> preCondition, InWorldUI inWorldUI, Camera camera,I_Interacter i_Interacter,LayerMask interactAbleMask,Vector3 offset) : base(preCondition)
    {
        this.interacter = i_Interacter;
        this.inWorldUI = inWorldUI;
        this.camera = camera;
        this.fieldOfView = new FieldOfView(searchRadius, camera.fieldOfView,camera.transform);
        this.objectPooling = new ObjectPooling<InWorldUI>(this.inWorldUI,12,5,camera.transform.position);
        this.assignInWorldInteractable = new Dictionary<I_Interactable, InWorldUI>();
        this.interactableMask = interactAbleMask;
        this.offset = offset;
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
    protected virtual void UpdateInteractableDetected()
    {
        List<I_Interactable> interactableDetected = new List<I_Interactable>();
        foreach(I_Interactable i_Interactable in FindInteractAbleObject())
        {
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
    protected virtual void UpdateAssignedUI()
    {
        bool isFoundCurrentInteractAble = false;
        List<I_Interactable> interactables = assignInWorldInteractable.Keys.ToList();

        if(assignInWorldInteractable.Count <=0)
            return;

        for (int i = 0; i < interactables.Count; i++) 
        {
            Vector3 setPos = interactables[i]._transform.position 
                + interactables[i]._transform.forward * offset.z
                + interactables[i]._transform.up * offset.y
                + interactables[i]._transform.right * offset.x;

            assignInWorldInteractable[interactables[i]].SetAnchorPosition(setPos);

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
    protected List<I_Interactable> FindInteractAbleObject()
    {
        List<I_Interactable> i_Interactables = new List<I_Interactable>();


        Collider[] hits = Physics.OverlapSphere(camera.transform.position, searchRadius, interactableMask.value, QueryTriggerInteraction.Collide);

        if (hits.Length == 0) 
            return i_Interactables;


        foreach (Collider target in hits)
        {
            if (target.TryGetComponent<I_Interactable>(out I_Interactable interactAbleObject) == false)
                continue;

            Vector3 toTarget = (interactAbleObject._transform.position - camera.transform.position).normalized;

            if ( Vector3.Angle(camera.transform.forward, toTarget) > camera.fieldOfView / 2f)
                continue;

            if (Physics.Raycast(camera.transform.position, toTarget, out RaycastHit hit, searchRadius, LayerMask.GetMask("Default") | interactableMask.value, QueryTriggerInteraction.Collide))
            {
                if (hit.collider.gameObject == target.gameObject)
                    i_Interactables.Add(interactAbleObject);
            }
        }
        return i_Interactables;
    }

}
