using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class DoorActor : Actor,I_Interactable
{
    public enum DoorEvent
    {
        Open, 
        Close
    }

    [SerializeField] private Animator animator;
    public virtual bool isOpen { get; private set; }
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
        if(isOpen)
            return;

        animator.SetTrigger("DoorTrigger");
        isOpen = true;

        base.NotifyObserver(DoorEvent.Open);
        this.NotifyObserver();
    }
    public void Close()
    {
        if(isOpen == false)
            return;

        animator.SetTrigger("DoorTrigger");
        isOpen = false;
       
        base.NotifyObserver(DoorEvent.Close);
        this.NotifyObserver();
    }
   public void DoInteract()
    {
        this.DoInteract(null);
    }
    public virtual void DoInteract(I_Interacter i_Interacter)
    {
        Debug.Log("DoInteract");

        if(isLocked)
            return;

        if(isOpen)
            Close();
        else
            Open();
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
}

public interface IObserverDoor
{
    public void OnNotify(DoorActor door);
}
