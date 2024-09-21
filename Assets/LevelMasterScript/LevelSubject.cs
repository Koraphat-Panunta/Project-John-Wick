using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSubject : MonoBehaviour
{
    private List<IObseverLevel> _levelObsevers = new List<IObseverLevel>();
    public enum LevelEvent
    {
        ObjectiveComplete,ObjectiveFialed,LevelClear
    }
    public void AddObserver(IObseverLevel obsever)
    {
        _levelObsevers.Add(obsever);
    }
    public void RemoveObserver(IObseverLevel obsever)
    {
        _levelObsevers.Remove(obsever);
    }
    public void NotifyObserver(LevelManager level,LevelEvent levelEvent)
    {
        foreach(IObseverLevel obsever in _levelObsevers)
        {
            obsever.OnNotify(level,levelEvent);
        }
    }
}
