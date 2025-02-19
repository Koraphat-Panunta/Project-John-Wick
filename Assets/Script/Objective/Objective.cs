using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Objective 
{
    public enum ObjectiveStatus
    {
        Hold,
        Activate,
        Complete,
        Failed
    }
    public ObjectiveStatus status;
    public abstract string ObjDescribe { get; set; }
    public Objective()
    {
        status = ObjectiveStatus.Hold;
    }
    public abstract bool PerformedDone();
      
}
