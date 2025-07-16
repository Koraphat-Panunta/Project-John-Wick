using System;
using UnityEngine;

public interface INode
{
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }
    public bool Precondition();
}
public static class NodeBehavior
{
    public static bool TryGetRootNodeAs<T>(INode node,out T rootNodeLeaf) where T : INodeLeaf
    {
        rootNodeLeaf = default(T);

        switch (node)
        {
            case INodeSelector nodeSelector:
                {
                    if(nodeSelector is T)
                    {
                        rootNodeLeaf = (T)nodeSelector;
                        return true;
                    }

                    if (nodeSelector.curNodeLeaf is INodeCombine combineNode)
                        return NodeBehavior.TryGetRootNodeAs<T>(combineNode, out rootNodeLeaf);
                    else if (nodeSelector.curNodeLeaf is INodeSequence sequenceNode)
                        return NodeBehavior.TryGetRootNodeAs<T>(sequenceNode, out rootNodeLeaf);
                    else if (nodeSelector.curNodeLeaf is INodeLeaf nodeLeaf)
                        return NodeBehavior.TryGetRootNodeAs<T>(nodeLeaf, out rootNodeLeaf);
                    else if (nodeSelector.curNodeLeaf is INodeSelector ChildnodeSelector)
                        return NodeBehavior.TryGetRootNodeAs<T>(ChildnodeSelector,out rootNodeLeaf);

                    return false;
                }    
            case INodeCombine combineNode:
                {
                    if(combineNode is T)
                    {
                        rootNodeLeaf = (T)combineNode;
                        return true;
                    }

                    foreach (INode childNodeCombine in combineNode.combineNodeActivate.Keys)
                    {
                        if (combineNode.combineNodeActivate[childNodeCombine] == false)
                            continue;

                        if (childNodeCombine is INodeCombine)
                            return NodeBehavior.TryGetRootNodeAs<T>(childNodeCombine, out rootNodeLeaf);
                        else if (childNodeCombine is INodeSequence sequenceNode)
                            return NodeBehavior.TryGetRootNodeAs<T>(sequenceNode, out rootNodeLeaf);
                        else if (childNodeCombine is INodeLeaf nodeLeaf)
                            return NodeBehavior.TryGetRootNodeAs<T>(nodeLeaf, out rootNodeLeaf);
                        else if(childNodeCombine is INodeSelector nodeSelector)
                            return NodeBehavior.TryGetRootNodeAs(nodeSelector, out rootNodeLeaf);

                    }
                    return false;
                }
            case INodeSequence nodeSequence:
                {
                    if (nodeSequence is T)
                    {
                        rootNodeLeaf = (T)nodeSequence;
                        return true;
                    }

                    if (nodeSequence.curNodeLeaf is INodeCombine combineNode)
                        return NodeBehavior.TryGetRootNodeAs<T>(combineNode, out rootNodeLeaf);
                    else if (nodeSequence.curNodeLeaf is INodeSequence sequenceNode)
                        return NodeBehavior.TryGetRootNodeAs<T>(sequenceNode, out rootNodeLeaf);
                    else if (nodeSequence.curNodeLeaf is INodeLeaf nodeLeaf)
                        return NodeBehavior.TryGetRootNodeAs<T>(nodeLeaf, out rootNodeLeaf);
                    else if (nodeSequence.curNodeLeaf is INodeSelector nodeSelector)
                        return NodeBehavior.TryGetRootNodeAs(nodeSelector, out rootNodeLeaf);

                    return false;
                }
            case INodeLeaf nodeLeaf: 
                {
                    if(nodeLeaf is T )
                    {
                        rootNodeLeaf = (T)nodeLeaf;
                        return true;
                    }
                    return false;
                }
        }
        return false;


    }
}
