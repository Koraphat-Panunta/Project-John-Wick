using UnityEngine;

public class UICanvasActor : Actor
{
    [SerializeField] protected GameObject uiCanvasObj;
    public enum UICanvasActorEvent
    {
        ShowEvent,
        HideEvent
    }
    public void Show()
    {
        this.uiCanvasObj.SetActive(true);
        base.NotifyObserver<UICanvasActorEvent>(UICanvasActorEvent.ShowEvent);
    }
    public void Hide() 
    {
        this.uiCanvasObj.SetActive(false);
        base.NotifyObserver<UICanvasActorEvent>(UICanvasActorEvent.HideEvent);
    }
}
