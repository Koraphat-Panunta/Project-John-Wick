using UnityEngine;
using static TriggerBox;

public class TriggerBoxExitEvent : VirtualEventNode, IObserverActor
{
    [SerializeField] public TriggerBox triggerBoxActor;
    [SerializeField] protected bool isTriggerOnce;

    protected bool isAlreadyTrigger = false;

    private void Awake()
    {
        this.triggerBoxActor.AddActorObserver(this);
    }

    public override void Execute()
    {
        if (this.isAlreadyTrigger
            && this.isTriggerOnce)
            return;

        if (this.isAlreadyTrigger == false)
            this.isAlreadyTrigger = true;

        base.Execute();
    }
    protected override void OnDrawGizmos()
    {
        if (this.triggerBoxActor != null)
        {
            onDrawGizmosTriggerEvent.DrawSphere(this.triggerBoxActor.transform.position, this.transform.position, color);
        }
        base.OnDrawGizmos();
    }



    public void OnNotifyActor<T>(Actor actor, T var)
    {
        if (actor is TriggerBox triggerBox && var is TriggerBoxEvent.Exit)
        {
            this.Execute();
            if (isTriggerOnce)
                triggerBox.RemoveActorObserver(this);
        }
    }
}
