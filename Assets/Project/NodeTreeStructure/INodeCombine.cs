using System.Collections.Generic;
using System.Linq;
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
    public void AddCombineNode(INodeCombine nodeCombine, INode addCombineNode)
    {
        nodeCombine.combineNodeActivate.Add(addCombineNode, false);
        nodeCombine.combineNodeLeaf.Add(addCombineNode, null);
        if(addCombineNode is INodeLeaf nodeleaf)
        {
            nodeleaf.isReset.Add(() => !nodeleaf.Precondition());
        }
    }

    public void RemoveCombineNode(INodeCombine nodeCombine, INode removeCombineNode)
    {
        nodeCombine.combineNodeActivate.Remove(removeCombineNode);
        nodeCombine.combineNodeLeaf.Remove(removeCombineNode);
    }

    public void Enter(INodeCombine nodeCombine)
    {

        if (nodeCombine.combineNodeActivate == null || nodeCombine.combineNodeActivate.Count <= 0)
            return;

        List<INode> keys = nodeCombine.combineNodeActivate.Keys.ToList<INode>();
        for (int i = 0; i < keys.Count; i++)
        {
            INode node = keys[i];
            if (node.Precondition())
            {
                if (node is INodeLeaf nodeLeaf)
                {
                    nodeLeaf.Enter();
                    nodeCombine.combineNodeActivate[node] = true;
                    nodeCombine.combineNodeLeaf[node] = nodeLeaf;
                }
                else if (node is INodeSelector nodeSelector)
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
        if (nodeCombine.combineNodeActivate == null || nodeCombine.combineNodeActivate.Count <= 0)
            return;

        var keys = nodeCombine.combineNodeActivate.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            INode node = keys[i];
            if (!nodeCombine.combineNodeActivate[node])
                continue;

            if (nodeCombine.combineNodeLeaf[node] != null)
            {
                nodeCombine.combineNodeLeaf[node].Exit();
                nodeCombine.combineNodeActivate[node] = false;
                nodeCombine.combineNodeLeaf[node] = null;
            }
        }
    }

    public void Update(INodeCombine nodeCombine)
    {
        var keys = nodeCombine.combineNodeActivate.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            INode node = keys[i];
            if (nodeCombine.combineNodeActivate[node])
            {
                if (node is INodeLeaf nodeLeaf)
                {
                    if (nodeLeaf.IsReset())
                    {
                        nodeLeaf.Exit();
                        nodeCombine.combineNodeActivate[node] = false;
                        nodeCombine.combineNodeLeaf[node] = null;
                        continue;
                    }
                }
                else if (node is INodeSelector nodeSelector)
                {
                    if (nodeCombine.combineNodeLeaf[node].IsReset())
                    {

                        if (nodeSelector.Precondition())
                        {
                            nodeSelector.FindingNode(out INodeLeaf outNodeLeaf);
                            nodeCombine.combineNodeLeaf[node].Exit();
                            nodeCombine.combineNodeLeaf[node] = outNodeLeaf;
                            nodeCombine.combineNodeLeaf[node].Enter();
                        }
                        else
                        {
                            nodeCombine.combineNodeLeaf[node].Exit();
                            nodeCombine.combineNodeActivate[node] = false;
                            nodeCombine.combineNodeLeaf[node] = null;
                            continue;
                        }
                    }
                }
                nodeCombine.combineNodeLeaf[node]?.UpdateNode();
            }
            else
            {
                if (node is INodeLeaf nodeLeaf && nodeLeaf.Precondition())
                {
                    nodeLeaf.Enter();
                    nodeCombine.combineNodeLeaf[node] = nodeLeaf;
                    nodeCombine.combineNodeActivate[node] = true;
                    nodeLeaf.UpdateNode();
                }
                else if (node is INodeSelector nodeSelector && nodeSelector.Precondition())
                {
                    nodeSelector.FindingNode(out INodeLeaf outNodeLeaf);
                    outNodeLeaf.Enter();
                    nodeCombine.combineNodeLeaf[node] = outNodeLeaf;
                    nodeCombine.combineNodeActivate[node] = true;
                    outNodeLeaf.UpdateNode();
                }
            }
        }
    }

    public void FixedUpdate(INodeCombine nodeCombine)
    {
        var keys = nodeCombine.combineNodeActivate.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            INode node = keys[i];
            if (nodeCombine.combineNodeActivate[node] && nodeCombine.combineNodeLeaf[node] != null)
            {
                nodeCombine.combineNodeLeaf[node].FixedUpdateNode();
            }
        }
    }
}
