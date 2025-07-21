using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverPlayer
{
    public void OnNotify<T>(Player player, T node);

    
}
