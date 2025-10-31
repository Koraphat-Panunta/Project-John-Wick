using System;
using UnityEngine;
using System.Collections.Generic;

public class Door : MonoBehaviour,I_Interactable
{
    [SerializeField] private Animator animator;
    public virtual bool isOpen { get; private set; }
    public virtual Collider _collider { get => this.collider; set => this.collider = value; }
    [SerializeField]  private Collider collider;
    public virtual bool isBeenInteractAble { get => isInteractAble; set => isInteractAble = value; }
    [SerializeField] private bool isInteractAble;
    [SerializeField] protected bool lockedValue;
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
    public Transform _transform { get => transform; set {} }

    
    public Action<Door> doorTriggerEvent;
    public void Open()
    {
        if(isOpen)
            return;

        animator.SetTrigger("DoorTrigger");
        isOpen = true;
        if (doorTriggerEvent != null)
            doorTriggerEvent.Invoke(this);

        this.NotifyObserver();
    }
    public void Close()
    {
        if(isOpen == false)
            return;

        animator.SetTrigger("DoorTrigger");
        isOpen = false;
        if (doorTriggerEvent != null)
            doorTriggerEvent.Invoke(this);

        this.NotifyObserver();
    }
    public void DoInteract()
    {
        if (isLocked)
            return;

        if (isOpen)
            Close();
        else
            Open();
    }
    public virtual void DoInteract(I_Interacter i_Interacter)
    {
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
}
public interface IObserverDoor
{
    public void OnNotify(Door door);
}
