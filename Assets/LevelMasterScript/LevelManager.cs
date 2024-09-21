using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : LevelSubject
{
    protected List<Objective> levelObjective = new List<Objective>();
    protected Player player;
    protected virtual void Start()
    {
        player = FindAnyObjectByType<Player>();
    }
    protected virtual void Update()
    {
        ObjectiveUpdate();
    }
    protected virtual void ObjectiveUpdate()
    {    
        if (levelObjective.Count > 0)
        {
            if (levelObjective[0].PerformedDone(player))
            {
                levelObjective.RemoveAt(0);
                NotifyObserver(this, LevelEvent.ObjectiveComplete);
            }
        }
        else
        {
            LevelClear();
            NotifyObserver(this, LevelEvent.LevelClear);
        }
    }
    protected virtual void LevelClear()
    {
        Debug.Log("Level Clear");
    }
    public List<Objective> GetListObjective()
    {
        return levelObjective;
    }
}
