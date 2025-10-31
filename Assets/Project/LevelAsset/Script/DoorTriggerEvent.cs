using UnityEngine;
using UnityEngine.Events;

public class DoorTriggerEvent : MonoBehaviour, IInitializedAble,IObserverDoor
{
    [SerializeField] private Door door;

    public void Initialized()
    {
        this.door.AddOberver(this);
    }

    [SerializeField] UnityEvent OnDoor_Open_TriggerEventOnce;
    [SerializeField] UnityEvent OnDoor_Close_TriggerEventOnce;
    [SerializeField] UnityEvent OnDoor_Open_TriggerEvent;
    [SerializeField] UnityEvent OnDoor_Close_TriggerEvent;

    public void OnNotify(Door door)
    {
        if (this.door.isOpen)
        {
            if(this.OnDoor_Open_TriggerEventOnce != null)
            {
                this.OnDoor_Open_TriggerEventOnce.Invoke();
                this.OnDoor_Open_TriggerEventOnce = null;
            }

            if(this.OnDoor_Open_TriggerEvent != null)
                this.OnDoor_Open_TriggerEvent.Invoke();
        }
        else if(this.door.isOpen == false)
        {
            if (this.OnDoor_Close_TriggerEventOnce != null)
            {
                this.OnDoor_Close_TriggerEventOnce.Invoke();
                this.OnDoor_Close_TriggerEventOnce = null;
            }

            if (this.OnDoor_Close_TriggerEvent != null)
                this.OnDoor_Close_TriggerEvent.Invoke();
        }
    }
}
