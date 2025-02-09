using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class EnemyStateNode:INode
{
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }
    public EnemyStateNode(Enemy enemy,Func<bool> preCondition)
    {
        this.enemy = enemy;
        this.preCondition = preCondition;
    }

    protected Enemy enemy { get; set; }
  
    public  bool Precondition() => preCondition.Invoke();
    



}
