using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CinematicUICanvas : MonoBehaviour
{
    public enum CinematicUIEvent
    {
        Play,
        Complete
    }

    [SerializeField] protected Animation animation;
    public bool isComplete { get; private set; }
    private void OnValidate()
    {
        animation = GetComponent<Animation>();
    }
    public void Play()
    {
        isComplete = false;
        animation.enabled = true;
        animation.Play();
        this.NotifyObserver<CinematicUIEvent>(CinematicUIEvent.Play);
        StartCoroutine(this.UpdateAnimation());
    }
    IEnumerator UpdateAnimation()
    {
        yield return new WaitForSeconds(animation.clip.length);
        this.CinematicComplete();
    }
    public void CinematicComplete()
    {
        isComplete=true;
        animation.enabled = false;
        this.NotifyObserver<CinematicUIEvent>(CinematicUIEvent.Complete);
    }
    protected List<IObserverCinematicUICanvas> observerCinematicUICanvas = new List<IObserverCinematicUICanvas>();
    public void AddObserver(IObserverCinematicUICanvas observer) 
    {
        this.observerCinematicUICanvas.Add(observer);
    }
    public void RemoveObserver(IObserverCinematicUICanvas observer)
    {
        this.observerCinematicUICanvas.Remove(observer);
    }
    private void NotifyObserver<T>(T var)
    {
        if(this.observerCinematicUICanvas.Count <=0)
            return;

        for (int i = 0; i < this.observerCinematicUICanvas.Count; i++) 
        {
            this.observerCinematicUICanvas[i].OnNotifyObserverCinematicUICanvas<T>(this,var);
        }
    }
  
}
public interface IObserverCinematicUICanvas
{
    public void OnNotifyObserverCinematicUICanvas<T>(CinematicUICanvas cinematicUICanvas, T var);
   
}

