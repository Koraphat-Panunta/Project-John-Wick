using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using static SubjectEnemy;

public class Elimination : Objective,IObserverEnemy
{
    public List<Enemy> targets;
    public int targetRemain { get; protected set; }
    public override string ObjDescribe { get; set ; }

    public Elimination(List<Enemy> targets)
    {
        this.targets = targets;
        this.targetRemain = targets.Count;
        this.targets.ForEach(target => { target.AddObserver(this); });
        ObjDescribe = "Eliminate All target" + "There is " + targetRemain + " target remain";

    }
    public override bool PerformedDone()
    {
        for (int i = targets.Count - 1; i >= 0; i--)
        {
            if (targets[i] == null)
            {
                continue;
            }
            if (targets[i].isDead)
            {
                targets.RemoveAt(i);
                targetRemain -= 1;
                UpdateObjectiveDescription();
                NotifyObserver(this);
            }
            targetRemain = this.targets.Count;
        }

        if (targets.Count <= 0)
        {
            base.status = ObjectiveStatus.Complete;
            NotifyObserver(this);
            return true;
        }

        return false;
    }
    private void UpdateObjectiveDescription()
    {
        ObjDescribe = "Eliminate All target" + "There is " + targetRemain + " target remain";
    }

   

    public void Notify<T>(Enemy enemy, T node)
    {
        if (node is EnemyDeadStateNode)
        {
            PerformedDone();
            enemy.RemoveObserver(this);
        }
    }
}
