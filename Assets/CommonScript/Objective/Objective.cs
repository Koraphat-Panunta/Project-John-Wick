using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Objective 
{
    protected LevelManager Level;
    public enum ObjectiveStatus
    {
        Hold,
        Activate,
        Complete,
        Failed
    }
    public ObjectiveStatus status;
    public string ObjDescribe { get; set; }
    public Objective(LevelManager level)
    {
        status = ObjectiveStatus.Hold;
        this.Level = level;
    }
    public virtual bool PerformedDone(Player player)
    {
        //Check Player is dead
        if (player.GetHP() <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene
            (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            return true;
        }
        else
        {
            return false;
        }
    }    
}
