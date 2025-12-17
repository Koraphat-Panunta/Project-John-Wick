using UnityEngine;

public class OnCinematicUIEvent : VirtualEventNode,IObserverActor
{
    [SerializeField] protected CinematicUIActor cinematicUIActor;

    [SerializeField] protected CinematicUICanvas.CinematicUIEvent onCinematicUIEvent;
    public void OnNotifyActor<T>(Actor actor, T var)
    {
        if (actor is CinematicUIActor
            && var is CinematicUICanvas.CinematicUIEvent cinematicUIEvent
            && cinematicUIEvent == this.onCinematicUIEvent)
            Execute();
    }

    private void Awake()
    {
        if(this.cinematicUIActor != null)
        this.cinematicUIActor.AddActorObserver(this);
    }

    protected override void OnDrawGizmos()
    {
        if(base.isEnableGizmos
            && this.cinematicUIActor != null)
        {
            this.onDrawGizmosTriggerEvent.DrawLine(this.cinematicUIActor.transform.position,this.transform.position,color);
        }
        base.OnDrawGizmos();
    }
}
