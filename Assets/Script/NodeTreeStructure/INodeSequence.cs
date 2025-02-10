using System.Collections.Generic;
using UnityEngine;

public interface INodeSequence :INodeLeaf
{
    public INodeLeaf curNodeLeaf { get; set; }

    public List<INodeLeaf> childNode { get; set; }
    public int curNodeIndex { get; set; }
    public NodeSequenceBehavior nodeSequenceBehavior { get; set; }
    public void AddChildNode(INodeLeaf nodeLeaf);
}
public class NodeSequenceBehavior
{
    public void Enter(INodeSequence nodeSequence)
    {
        nodeSequence.curNodeLeaf = null;
        nodeSequence.curNodeIndex = 0;
    }
    public void Exit(INodeSequence nodeSequence)
    {
        nodeSequence.curNodeLeaf = null;
    }

    public void UpdateNodeSequence(INodeSequence nodeSequence)
    {
        //Begin
        if (nodeSequence.curNodeLeaf == null)
        {
            nodeSequence.curNodeLeaf = nodeSequence.childNode[nodeSequence.curNodeIndex];
            if (nodeSequence.curNodeLeaf != null)
                nodeSequence.curNodeLeaf.Enter();
        }


        if (nodeSequence.curNodeLeaf.IsComplete())
        {
            nodeSequence.curNodeIndex += 1;

            nodeSequence.curNodeLeaf.Exit();
            nodeSequence.curNodeLeaf = null;

            if (nodeSequence.curNodeIndex < nodeSequence.childNode.Count)
                nodeSequence.curNodeLeaf = nodeSequence.childNode[nodeSequence.curNodeIndex];
            if (nodeSequence.curNodeLeaf != null)
                nodeSequence.curNodeLeaf.Enter();
        }
        if (nodeSequence.curNodeLeaf != null)
            nodeSequence.curNodeLeaf.UpdateNode();
    }
    public void FixedUpdateNodeSequencee(INodeSequence nodeSequence)
    {
        if (nodeSequence.curNodeLeaf != null)
            nodeSequence.curNodeLeaf.FixedUpdateNode();
    }

    public bool IsComplete(INodeSequence nodeSequence)
    {
        if (nodeSequence.curNodeIndex >= nodeSequence.childNode.Count)
            return true;
        else return false;
    }

    public bool IsReset(INodeSequence nodeSequence)
    {
        if(IsComplete(nodeSequence) == true)
            return false;

        return nodeSequence.nodeLeafBehavior.IsReset(nodeSequence.isReset);
    }
    public void AddNode(INodeSequence nodeSequence,INodeLeaf nodeLeaf)
    {
        nodeSequence.childNode.Add(nodeLeaf);
    }

}
