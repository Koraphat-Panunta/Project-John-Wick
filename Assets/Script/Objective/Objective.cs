using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Objective 
{
    public enum ObjectiveStatus
    {
        Hold,
        Activate,
        Complete,
        Failed
    }
    public ObjectiveStatus status;
    public abstract string ObjDescribe { get; set; }
    protected List<IObserveObjective> observeObjectives;
    public Objective()
    {
        status = ObjectiveStatus.Hold;
        observeObjectives = new List<IObserveObjective>();
    }
    public abstract bool PerformedDone();
    public void NotifyObserver(Objective objective)
    {
        if(observeObjectives.Count <= 0)
            return;
        
        foreach(IObserveObjective observeObjective in this.observeObjectives)
        {
            observeObjective.GetNotifyObjectiveUpdate(objective);
        }
    }
    public void AddNotifyUpdateObjective(IObserveObjective observeObjective)
    {
        observeObjectives.Add(observeObjective);
    }
    public void RemoveNotifyUpdateObjective(IObserveObjective observeObjective) => observeObjectives.Remove(observeObjective);

}
public interface IObserveObjective 
{
    public void GetNotifyObjectiveUpdate(Objective objective);
}

