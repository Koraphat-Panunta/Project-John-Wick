using System.Collections.Generic;
using UnityEngine;

public interface INodeCombine : INodeLeaf
{
    public List<INodeLeaf> combineNodes { get; set; }
    public NodeCombineBehavior nodeCombineBehavior { get; set; }
    public void AddCombineNode(INodeLeaf addCombineNode);
    public void RemoveCombineNode(INodeLeaf removeCombineNode);
}
public class NodeCombineBehavior
{
    public void AddCombineNode(INodeCombine nodeCombine,INodeLeaf addCombineNode)
    {
        nodeCombine.combineNodes.Add(addCombineNode);
    }
    public void RemoveCombineNode(INodeCombine nodeCombine,INodeLeaf removeCombineNode) 
    {
        nodeCombine.combineNodes.Add(removeCombineNode);
    }
    public void Enter(INodeCombine nodeCombine)
    {
        if (nodeCombine.combineNodes.Count > 0)
            nodeCombine.combineNodes.ForEach(nodeLeaf => { nodeLeaf.Enter();});
    }
    public void Exit(INodeCombine nodeCombine) 
    {
        if (nodeCombine.combineNodes.Count > 0)
            nodeCombine.combineNodes.ForEach(nodeLeaf => { nodeLeaf.Exit(); });
    }
    public void Update(INodeCombine nodeCombine)
    {
        if (nodeCombine.combineNodes.Count > 0)
            nodeCombine.combineNodes.ForEach(nodeLeaf => { nodeLeaf.UpdateNode(); });
    }
    public void FixedUpdate(INodeCombine nodeCombine)
    {
        if (nodeCombine.combineNodes.Count > 0)
            nodeCombine.combineNodes.ForEach(nodeLeaf => { nodeLeaf.FixedUpdateNode(); });
    }
}
