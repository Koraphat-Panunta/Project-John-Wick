using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanHouse : Objective
{
    private List<Character> ALLenemies = new List<Character>();
    public int targetRemain;
    public CleanHouse(LevelManager level, List<Character> ALLenemies) : base(level)
    {
        this.ALLenemies = ALLenemies;
        targetRemain = this.ALLenemies.Count;
        ObjDescribe = "Eliminate ALL enemies there is "+targetRemain+" remain";
    }

    public override bool PerformedDone(Player player)
    {
        foreach (Character target in ALLenemies)
        {
            if (target.GetHP() <= 0)
            {
                this.ALLenemies.Remove(target);
                targetRemain = this.ALLenemies.Count;
                UpdateObjectiveDescription();
                base.Level.NotifyObserver(base.Level, LevelSubject.LevelEvent.ObjectiveUpdate);
            }
            else
            {
                targetRemain = this.ALLenemies.Count;
            }
        }

        // Return Objective status
        if (ALLenemies.Count <= 0)
        {
            base.status = ObjectiveStatus.Complete;
            //Debug.Log("Elimination Complete");
            return true;
        }
        else
        {
            return base.PerformedDone(player);
        }
    }
    private void UpdateObjectiveDescription()
    {
        ObjDescribe = "Eliminate ALL enemies there is " + targetRemain + " remain";
    }
}
