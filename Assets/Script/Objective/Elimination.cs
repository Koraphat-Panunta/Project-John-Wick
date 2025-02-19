using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Elimination : Objective
{
    public List<Character> targets;
    public int targetRemain { get; protected set; }
    public override string ObjDescribe { get; set ; }

    public Elimination(List<Character> targets)
    {
        this.targets = targets;
        this.targetRemain = targets.Count;
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
            }
            targetRemain = this.targets.Count;
        }

        if (targets.Count <= 0)
        {
            base.status = ObjectiveStatus.Complete;
            return true;
        }

        return false;
    }
    private void UpdateObjectiveDescription()
    {
        ObjDescribe = "Eliminate All target" + "There is " + targetRemain + " target remain";
    }
}
