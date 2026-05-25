using System;
using System.Collections.Generic;
using UnityEngine;

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

    private LayerMask _defaultAndInteractableMask;
    private readonly Collider[] _overlapBuffer = new Collider[32];
    private readonly List<I_Interactable> _foundInteractables = new List<I_Interactable>();
    private readonly List<I_Interactable> _detectedThisFrame = new List<I_Interactable>();
    private readonly List<I_Interactable> _assignedKeysBuffer = new List<I_Interactable>();

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
        _defaultAndInteractableMask = LayerMask.GetMask("Default") | interactAbleMask.value;
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
        _detectedThisFrame.Clear();
        FindInteractAbleObject(_foundInteractables);
        foreach(I_Interactable i_Interactable in _foundInteractables)
        {
            if(_detectedThisFrame.Contains(i_Interactable))
                continue;

            if(i_Interactable.isBeenInteractAble == false)
                continue;

            _detectedThisFrame.Add(i_Interactable);

            if (this.assignInWorldInteractable.ContainsKey(i_Interactable) == false)
            {
                InWorldUI inWorldUI = objectPooling.Get();
                this.assignInWorldInteractable.Add(i_Interactable, inWorldUI);
                assignInWorldInteractable[i_Interactable].PlayAnimation("PointingAppear");
            }
        }

        _assignedKeysBuffer.Clear();
        foreach (I_Interactable key in assignInWorldInteractable.Keys)
            _assignedKeysBuffer.Add(key);

        for (int i = 0; i < _assignedKeysBuffer.Count; i++)
        {
            if (_detectedThisFrame.Contains(_assignedKeysBuffer[i]) == false)
            {
                objectPooling.ReturnToPool(assignInWorldInteractable[_assignedKeysBuffer[i]]);
                assignInWorldInteractable.Remove(_assignedKeysBuffer[i]);
            }
        }
    }
    protected virtual void UpdateAssignedUI()
    {
        if(assignInWorldInteractable.Count <= 0)
            return;

        _assignedKeysBuffer.Clear();
        foreach (I_Interactable key in assignInWorldInteractable.Keys)
            _assignedKeysBuffer.Add(key);

        bool isFoundCurrentInteractAble = false;
        for (int i = 0; i < _assignedKeysBuffer.Count; i++)
        {
            I_Interactable interactable = _assignedKeysBuffer[i];
            Vector3 setPos = interactable._transform.position
                + interactable._transform.forward * offset.z
                + interactable._transform.up * offset.y
                + interactable._transform.right * offset.x;

            assignInWorldInteractable[interactable].SetAnchorPosition(setPos);

            if (interactable.isBeenInteractAble == false)
            {
                objectPooling.ReturnToPool(assignInWorldInteractable[interactable]);
                assignInWorldInteractable.Remove(interactable);
                continue;
            }

            if(isFoundCurrentInteractAble)
            {
                assignInWorldInteractable[interactable].PlayAnimation("PointingAppear");
                continue;
            }

            if(interacter.currentInteractable == interactable)
            {
                isFoundCurrentInteractAble = true;
                assignInWorldInteractable[interactable].PlayAnimation("InteractableAppear");
            }
            else
            {
                assignInWorldInteractable[interactable].PlayAnimation("PointingAppear");
            }
        }
    }
    protected void FindInteractAbleObject(List<I_Interactable> results)
    {
        results.Clear();

        int count = Physics.OverlapSphereNonAlloc(camera.transform.position, searchRadius, _overlapBuffer, interactableMask.value, QueryTriggerInteraction.Collide);

        if (count == 0)
            return;

        float halfFOV = camera.fieldOfView / 2f;
        for (int i = 0; i < count; i++)
        {
            if (_overlapBuffer[i].TryGetComponent<I_Interactable>(out I_Interactable interactAbleObject) == false)
                continue;

            Vector3 toTarget = (interactAbleObject._transform.position - camera.transform.position).normalized;

            if (Vector3.Angle(camera.transform.forward, toTarget) > halfFOV)
                continue;

            if (Physics.Raycast(camera.transform.position, toTarget, out RaycastHit hit, searchRadius, _defaultAndInteractableMask, QueryTriggerInteraction.Collide))
            {
                if (hit.collider.gameObject == _overlapBuffer[i].gameObject)
                    results.Add(interactAbleObject);
            }
        }
    }

}
