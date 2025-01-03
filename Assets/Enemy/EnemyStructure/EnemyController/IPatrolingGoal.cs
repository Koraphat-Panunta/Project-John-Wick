using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface IPatrolingGoal 
{
    public List<Dictionary<Transform, float>> patrolpoint { get; set; }
    public PatrolingGoal patrolingGoal { get; set; }
    public void InitailizedPatrolingGoal();
}
