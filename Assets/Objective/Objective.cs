using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective 
{
    public enum ObjectiveStatus
    {
        Hold,
        Activate,
        Complete,
        Failed
    }
    public ObjectiveStatus status;
    public Objective()
    {
        status = ObjectiveStatus.Hold;
    }
    public virtual bool PerformedDone(Player player)
    {
        //Check Player is dead
        if (player.GetHP() <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
