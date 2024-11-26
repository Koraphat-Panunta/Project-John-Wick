using System.Collections.Generic;
using UnityEngine;

public abstract class ActionNode : INode
{
    public List<INode> children { get; set; }
    public List<INode> SubNode { get; set; }
    public abstract void FixedUpdate();
   

    public abstract bool IsReset();


    public abstract bool PreCondition();


    public abstract void Update();
    public abstract bool IsComplete();
    protected abstract bool FindSubNode(out INode node);
   
}
