using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface INode 
{
   public List<INode> children { get; set; }
    public void Update();
    public void FixedUpdate();
    public bool PreCondition();
    public bool IsReset();
}
