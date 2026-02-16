using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverEnemy 
{
    public void OnNotify<T>(Enemy enemy, T node);
}
