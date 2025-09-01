using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverEnemy 
{
    public void Notify<T>(Enemy enemy, T node);
}
