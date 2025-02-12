using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObseverLevel 
{
    public void OnNotify(LevelManager level,LevelSubject.LevelEvent levelEvent);
    //public void OnNotify(LevelManager level);
}

