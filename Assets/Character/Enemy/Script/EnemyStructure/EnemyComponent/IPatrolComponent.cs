using System.Collections.Generic;
using UnityEngine;

public interface IPatrolComponent 
{
    public GameObject userPatrol { get; set; }
    public List<PatrolPoint> patrolPoints { get; set; }
    public int Index { get; set; }

    public void InitailizedPatrolComponent();
}
