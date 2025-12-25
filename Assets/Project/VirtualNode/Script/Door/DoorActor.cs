using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Collections;

public class DoorActor : Actor,I_Interactable
{

    Coroutine Coroutine;
    public enum DoorEvent
    {
        Open, 
        Close
    }

    // 1 : Open to the back
    // -1 : Open to the front 

    [SerializeField] protected float curDoorWeight; // -1>0>1
    protected float targetDoorWeight; // -1>0>1

    public bool isOpen { get => curDoorWeight == 0? true:false; }
    public virtual Collider _collider { get => this.collider; set => this.collider = value; }
    [SerializeField]  private Collider collider;
    public virtual bool isBeenInteractAble { get => isInteractAble; set => isInteractAble = value; }
    [SerializeField] private bool isInteractAble;
    [SerializeField] protected bool lockedValue;
    [SerializeField] private Transform referenceInteractAbleTransform;
    public Transform _transform { get => referenceInteractAbleTransform; set { } }
    public virtual bool isLocked { 
        get 
        {
            if (isOpen)
            {
                lockedValue = false;
                return false;
            }
            return lockedValue;
        } 
        set 
        { 
            lockedValue = value; 
        } 
    }


    

    public void Open()
    {
        this.Open(1);
    }
    public void Open(float weight)
    {
        this.targetDoorWeight = weight;

        if (this.Coroutine != null)
            this.Coroutine = StartCoroutine(DoorEventUpdate());

        base.NotifyObserver(DoorEvent.Open);
        this.NotifyObserver();
    }
    public void Close()
    {
        this.curDoorWeight = 0;

        if (this.Coroutine != null)
            this.Coroutine = StartCoroutine(DoorEventUpdate());

        base.NotifyObserver(DoorEvent.Close);
        this.NotifyObserver();
    }
   public void DoInteract()
    {
        this.DoInteract(null);
    }
    public virtual void DoInteract(I_Interacter i_Interacter)
    {

        if(isLocked)
            return;

        if(isOpen)
            Close();
        else
        {

            if(i_Interacter is Transform transform)
            {
                if (Vector3.Dot(this.transform.forward, (transform.position - this.transform.position).normalized) > 0)
                    this.Open(1);
                else
                    this.Open(-1);
            }

            Open();
        }

    }
    protected List<IObserverDoor> observerDoors = new List<IObserverDoor>();
    public void AddOberver(IObserverDoor observerDoor)
    {
        this.observerDoors.Add(observerDoor);
    }
    public void RemoveOberver(IObserverDoor removedObserver) 
    {
        this.observerDoors.Remove(removedObserver);
    }
    public void NotifyObserver()
    {
        if(this.observerDoors.Count <= 0)
            return;

        for (int i = 0; i < this.observerDoors.Count; i++) 
        {
            this.observerDoors[i].OnNotify(this);
        }
    }

    protected override void OnDrawGizmos()
    {
        if (this.isEnableGizmos == false)
            return;

        Gizmos.color = color;
        Gizmos.DrawCube(_transform.position, Vector3.one * .3f);
        Vector3 cameraPos;
        if (Application.isPlaying)
        {
            cameraPos = Camera.main.transform.position;
        }
        else
        {
            cameraPos = SceneView.lastActiveSceneView.camera.transform.position;
        }
        if (Vector3.Distance(cameraPos, this.transform.position) < base.drawNameDistance)
        {
            Handles.Label(_transform.position + (Vector3.up * .35f), this.name);
        }
        
        Gizmos.DrawRay(_transform.position, Vector3.up * .35f);
    }

    protected IEnumerator DoorEventUpdate()
    {

        while(this.curDoorWeight != this.targetDoorWeight)
        {
            this.curDoorWeight = Mathf.MoveTowards(this.curDoorWeight, this.targetDoorWeight, Time.deltaTime);
            yield return null;
        }

        this.Coroutine = null;
    }
}

public interface IObserverDoor
{
    public void OnNotify(DoorActor door);
}
