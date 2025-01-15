using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : LevelSubject,IObserverPlayer
{
    protected List<Objective> levelObjective = new List<Objective>();
    protected Player player;
    protected virtual void Start()
    {
        player = FindAnyObjectByType<Player>();
        player.AddObserver(this);
    }
    protected virtual void Update()
    {
        ObjectiveUpdate();
    }
    protected virtual void ObjectiveUpdate()
    {    
        if (levelObjective.Count > 0)
        {
            for(int i = levelObjective.Count - 1; i >= 0; i--)
            {
                if (levelObjective[i] == null)
                {
                    levelObjective.RemoveAt(i);
                }
                else if (levelObjective[i].PerformedDone(player))
                {
                    levelObjective.RemoveAt(i);
                    NotifyObserver(this, LevelEvent.ObjectiveComplete);
                }
            }
            //foreach (Objective objective in levelObjective)
            //{
            //    if (objective.PerformedDone(player))
            //    {
            //        levelObjective.Remove(objective);
            //        NotifyObserver(this, LevelEvent.ObjectiveComplete);
            //    }
            //}
        }
        else
        {
            LevelClear();
            NotifyObserver(this, LevelEvent.LevelClear);
        }
    }
    protected virtual void LevelClear()
    {
        //Debug.Log("Level Clear");
    }
    public List<Objective> GetListObjective()
    {
        return levelObjective;
    }

    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(playerAction == SubjectPlayer.PlayerAction.Dead)
        {

        }
    }

    public void OnNotify(Player player)
    {
    }
}
