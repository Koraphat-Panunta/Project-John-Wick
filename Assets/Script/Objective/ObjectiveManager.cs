using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public Objective curObjective;
    public Queue<Objective> listObjecttive;
    
    public bool allQuestClear => listObjecttive.Count <= 0 && curObjective == null;

    void Start()
    {
        listObjecttive = new Queue<Objective>();
    }

    void Update()
    {
       UpdateObjective();
    }
    public void AddObjective(Objective objective)
    {
        this.listObjecttive.Enqueue(objective);
    }
    public void StartPlayObjective()
    {
        if (this.listObjecttive.Count <= 0)
            throw new System.Exception(" there are no objective in queue");

        curObjective = this.listObjecttive.Dequeue();   
    }

    private void UpdateObjective()
    {
        if(curObjective == null)
            return;

        if (curObjective.PerformedDone() == false)
            return;

        if (this.listObjecttive.Count <= 0)
        {
            curObjective = null;
            return;
        }

        curObjective = this.listObjecttive.Dequeue();
    }
}

