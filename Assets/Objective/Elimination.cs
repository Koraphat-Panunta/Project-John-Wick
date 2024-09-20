using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Elimination : Objective
{
    public List<Character> targets;
    public int targetRemain { get; protected set; }
    public Elimination(List<Character> targets) 
    {
        this.targets = targets;
        ObjDescribe = "Eliminate All target";
        foreach (Character target in this.targets)
        {
            var targetUI = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            targetUI.transform.SetParent(target.transform);
            targetUI.transform.localPosition = Vector3.zero + new Vector3(0,2f,0);
            targetUI.transform.localScale = Vector3.one*0.4f;
            targetUI.GetComponent<MeshRenderer>().material.color = Color.red;
            targetUI.GetComponent<SphereCollider>().enabled = false;
        }
    }
    public override bool PerformedDone(Player player)
    {
  
        foreach (Character target in targets) 
        {
            if (target.GetHP()<=0)
            {
                this.targets.Remove(target);
            }
            targetRemain = this.targets.Count;
        }

        // Return Objective status
        if (targets.Count <= 0)
        {
            base.status = ObjectiveStatus.Complete;
            Debug.Log("Elimination Complete");
            return true;
        }
        else
        {
            return base.PerformedDone(player);
        }
    }
}
