using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSelectorNode : EnemyStateNode, INodeSelector
{
    public List<INode> childNode { get; set; }
    public Dictionary<INode, Func<bool>> nodePrecondition { get ; set ; }
    public NodeSelectorBehavior nodeSelectorBehavior { get; set; }

    public EnemyStateSelectorNode(Enemy enemy, Func<bool> preCondition) : base(enemy, preCondition)
    {
        this.childNode = new List<INode>();
        this.nodePrecondition = new Dictionary<INode, Func<bool>>();
        this.nodeSelectorBehavior = new NodeSelectorBehavior();
    }

  
    public void AddtoChildNode(INode childNode)=>nodeSelectorBehavior.AddtoChildNode(childNode,this);


    public void FindingNode(out INodeLeaf nodeLeaf) => nodeSelectorBehavior.FindingNode(out nodeLeaf, this);
   

    public void RemoveNode(INode childNode)=> nodeSelectorBehavior.RemoveChildNode(childNode,this);
   
}
