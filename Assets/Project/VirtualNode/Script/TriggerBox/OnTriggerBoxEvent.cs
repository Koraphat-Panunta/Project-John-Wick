using UnityEngine;
using static TriggerBoxActor;

public class OnTriggerBoxEvent : VirtualEventNode,IObserverActor
{
    [SerializeField] public TriggerBoxActor triggerBoxActor;
    [SerializeField] protected bool isTriggerOnce;

    [SerializeField] protected TriggerBoxEvent triggerBoxEvent;

    protected bool isAlreadyTrigger = false;

    private void Awake()
    {
        this.triggerBoxActor.AddActorObserver(this);
    }

    public override void Execute()
    {
        if(this.isAlreadyTrigger
            && this.isTriggerOnce)
            return;

        if(this.isAlreadyTrigger == false)
            this.isAlreadyTrigger = true;

        base.Execute();
    }
    protected override void OnDrawGizmos()
    {
        if (this.triggerBoxActor != null)
        {
            onDrawGizmosTriggerEvent.DrawLine(this.triggerBoxActor.transform.position, this.transform.position, color);
        }
        base.OnDrawGizmos();
    }

    

    public void OnNotifyActor<T>(Actor actor, T var)
    {
        if (actor is TriggerBoxActor triggerBox 
            && var is TriggerBoxEvent triggerBoxEvent
            && triggerBoxEvent == this.triggerBoxEvent)
        {
            this.Execute();
            if(isTriggerOnce)
                triggerBox.RemoveActorObserver(this);
        }
    }
}
