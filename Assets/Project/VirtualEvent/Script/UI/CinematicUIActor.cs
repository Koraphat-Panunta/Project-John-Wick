using UnityEngine;

public class CinematicUIActor : Actor,IObserverCinematicUICanvas
{
    [SerializeField] protected CinematicUICanvas cinematicUICanvas;

   
    private void Awake()
    {
        this.cinematicUICanvas.AddObserver(this);
    }

    public void Play()
    {
        this.cinematicUICanvas.gameObject.SetActive(true);
        this.cinematicUICanvas.Play();
    }

    public override void EnableActor()
    {
        this.cinematicUICanvas.gameObject.SetActive(true);
        base.EnableActor();
    }
    public override void DisableActor()
    {
        this.cinematicUICanvas.gameObject.SetActive(false);
        base.DisableActor();
    }
    
    public override void DestroyActor()
    {
        Destroy(this.cinematicUICanvas.gameObject);
        base.DestroyActor();
    }

    public void OnNotifyObserverCinematicUICanvas<T>(CinematicUICanvas cinematicUICanvas, T var)
    {
        if(var is CinematicUICanvas.CinematicUIEvent cinematicEvent)
        {
            base.NotifyObserver<CinematicUICanvas.CinematicUIEvent>(cinematicEvent);
        }
    }

}
