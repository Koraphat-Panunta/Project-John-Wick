using System.Collections.Generic;
using UnityEngine;

public interface INodeCombine : INodeLeaf
{
    public Dictionary<INode, bool> combineNodeActivate { get; set; } //Combine Node is Activate
    public Dictionary<INode, INodeLeaf> combineNodeLeaf { get; set; } //NodeLeaf of Combine ,can be selfNode or childNode
    public NodeCombineBehavior nodeCombineBehavior { get; set; }
    public void AddCombineNode(INode addCombineNode);
    public void RemoveCombineNode(INode removeCombineNode);
}
public class NodeCombineBehavior
{
    public void AddCombineNode(INodeCombine nodeCombine,INode addCombineNode)
    {
        nodeCombine.combineNodeActivate.Add(addCombineNode, false);
        nodeCombine.combineNodeLeaf.Add(addCombineNode, null);
    }
    public void RemoveCombineNode(INodeCombine nodeCombine,INode removeCombineNode) 
    {
        nodeCombine.combineNodeActivate.Remove(removeCombineNode);
        nodeCombine.combineNodeLeaf.Remove(removeCombineNode);
    }
    public void Enter(INodeCombine nodeCombine)
    {
        if (nodeCombine.combineNodeActivate == null || nodeCombine.combineNodeActivate.Count <= 0)
            return;

        foreach (INode node in nodeCombine.combineNodeActivate.Keys)
        {
            if (node.Precondition())
            {
                if(node is INodeLeaf nodeLeaf)
                {
                    nodeLeaf.Enter();
                    nodeCombine.combineNodeActivate[node] = true;
                    nodeCombine.combineNodeLeaf[node] = nodeLeaf;   
                }
                else if(node is INodeSelector nodeSelector)
                {
                    nodeSelector.FindingNode(out INodeLeaf outNodeLeaf);
                    outNodeLeaf.Enter();
                    nodeCombine.combineNodeActivate[node] = true;
                    nodeCombine.combineNodeLeaf[node] = outNodeLeaf;
                }
            }
        }
    }
    public void Exit(INodeCombine nodeCombine) 
    {
        if(nodeCombine.combineNodeActivate == null || nodeCombine.combineNodeActivate.Count <= 0)
            return;

        foreach(INode node in nodeCombine.combineNodeActivate.Keys)
        {
            if (nodeCombine.combineNodeActivate[node] == false)
                continue;

            if (nodeCombine.combineNodeLeaf[node] != null)
            {
                nodeCombine.combineNodeLeaf[node].Exit();
                nodeCombine.combineNodeActivate[node] = false;
                nodeCombine.combineNodeLeaf = null;
            }
        }
    }
    public void Update(INodeCombine nodeCombine)
    {
        foreach (INode node in nodeCombine.combineNodeActivate.Keys)
        {
            if (nodeCombine.combineNodeActivate[node] == true)
            {
                if (node is INodeLeaf nodeLeaf)
                {
                    if (nodeLeaf.IsReset())
                    {
                        nodeLeaf.Exit();
                        nodeCombine.combineNodeActivate[node] = false;
                        nodeCombine.combineNodeLeaf[node] = null;
                    }
                } //Case NodeCombine is NodeLeaf
                if (node is INodeSelector nodeSelector)
                {
                    if (nodeCombine.combineNodeLeaf[node].IsReset())
                    {
                        nodeCombine.combineNodeLeaf[node].Exit();
                        if (nodeSelector.Precondition())
                        {
                            nodeSelector.FindingNode(out INodeLeaf outNodeLeaf);
                            nodeCombine.combineNodeLeaf[node] = outNodeLeaf;
                            nodeCombine.combineNodeLeaf[node].Enter();
                        }
                        else
                        {
                            nodeCombine.combineNodeActivate[node] = false;
                            nodeCombine.combineNodeLeaf[node] = null;
                        }
                    }
                } //Case NodeCombine is NodeSelector

                if (nodeCombine.combineNodeLeaf[node] != null)
                    nodeCombine.combineNodeLeaf[node].UpdateNode();
            } // NodeCombine is Active

            else if (nodeCombine.combineNodeActivate[node] == false)
            {
                if (node is INodeLeaf nodeLeaf)
                {
                    if (nodeLeaf.Precondition() == false)
                        continue;

                    nodeLeaf.Enter();
                    nodeCombine.combineNodeLeaf[node] = nodeLeaf;
                    nodeCombine.combineNodeActivate[node] = true;
                }//Case NodeCombine is NodeLeaf
                if (node is INodeSelector nodeSelector)
                {
                    if (nodeSelector.Precondition() == false)
                        continue;

                    nodeSelector.FindingNode(out INodeLeaf outNodeLeaf);
                    outNodeLeaf.Enter();
                    nodeCombine.combineNodeLeaf[node] = outNodeLeaf;
                    nodeCombine.combineNodeActivate[node] = true;
                } //Case NodeCombine is NodeSelector

                if (nodeCombine.combineNodeLeaf[node] != null)
                    nodeCombine.combineNodeLeaf[node].UpdateNode();
            }// NodeCombine is InActive

        }
    }
    public void FixedUpdate(INodeCombine nodeCombine)
    {
        foreach(INode node in nodeCombine.combineNodeActivate.Keys)
        {
            if (nodeCombine.combineNodeActivate[node])
            {
                if (nodeCombine.combineNodeLeaf[node] != null)
                    nodeCombine.combineNodeLeaf[node].FixedUpdateNode();
            }
        }
    }
    private void UpdateNodeCombineManager(INodeCombine nodeCombine)
    {
       
    }
}
