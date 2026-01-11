using UnityEngine;

public class OnDoorTriggerEvent : VirtualEventNode, IObserverActor
{
    [SerializeField] DoorActor doorActor;
    [SerializeField] bool isTriggerOnce;
    protected bool isAlreadyTrigger;

    [SerializeField] DoorActor.DoorEvent DoorEvent;
    protected void Awake()
    {
        this.doorActor.AddActorObserver(this);
    }
    public override void Execute()
    {

        if(isTriggerOnce && isAlreadyTrigger)
            return;

        if (isAlreadyTrigger == false)
            isAlreadyTrigger = true;

        base.Execute();
    }
    public void OnNotifyActor<T>(Actor actor, T var)
    {
        if(actor is DoorActor && var is DoorActor.DoorEvent doorEvent && doorEvent == DoorEvent)
            this.Execute();
    }

    protected override void OnDrawGizmos()
    {
        if (base.isEnableGizmos == false)
            return;

        if (doorActor != null)
        {
            base.onDrawGizmosTriggerEvent.DrawLine(doorActor._transform.position, transform.position, color);
        }

        base.OnDrawGizmos();
    }
}
