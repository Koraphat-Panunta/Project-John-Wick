using System;
using UnityEngine;

public interface INode
{
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }
    public bool Precondition();
}

