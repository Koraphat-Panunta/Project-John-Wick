using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static TriggerBoxActor;

public abstract class Actor : MonoBehaviour
{
    [SerializeField] List<IObserverActor> observer = new List<IObserverActor>();
    public virtual void DestroyActor()
    {
        Destroy(this.gameObject);
    }
    public virtual void EnableActor()
    {
        this.gameObject.SetActive(true);
    }
    public virtual void DisableActor()
    {
        this.gameObject.SetActive(false);
    }
    public void AddActorObserver(IObserverActor observerTriggerBox)
    {
        this.observer.Add(observerTriggerBox);
    }
    public void RemoveActorObserver(IObserverActor observerTriggerBox)
    {
        this.observer.Remove(observerTriggerBox);
    }
    protected void NotifyObserver<T>(T var)
    {
        if (this.observer.Count <= 0)
            return;

        for (int i = 0; i < this.observer.Count; i++)
        {
            this.observer[i].OnNotifyActor(this, var);
        }
    }
    [SerializeField] protected bool isEnableGizmos;
    [SerializeField] protected Color color;

    public float drawNameDistance = 10f;
    protected virtual void OnDrawGizmos()
    {
        if(this.isEnableGizmos == false)
            return;

        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, Vector3.one * .3f);
        Vector3 cameraPos;
        if (Application.isPlaying){
            cameraPos = Camera.main.transform.position;
        }
        else{
            cameraPos = SceneView.lastActiveSceneView.camera.transform.position;
        }
        if (Vector3.Distance(cameraPos, this.transform.position) < drawNameDistance)
        {
            Handles.Label(transform.position + (Vector3.up * .35f), this.name);
        }
        Gizmos.DrawRay(transform.position, Vector3.up * .35f);

        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, Vector3.one * .3f);
    }
}
