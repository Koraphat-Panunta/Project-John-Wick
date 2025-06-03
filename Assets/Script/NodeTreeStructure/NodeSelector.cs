using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeSelector : INode
{
    public Func<bool> preCondition { get; set ; }
    public INode parentNode { get; set ; }
    public List<INode> childNode { get; set; }
    public Dictionary<INode, Func<bool>> nodePrecondition { get; set; }
    public string nodeName { get; protected set; }

    public NodeSelector(Func<bool> preCondition, string nodeName)
    {
        this.preCondition = preCondition;
        this.childNode = new List<INode>();
        nodePrecondition = new Dictionary<INode, Func<bool>>();
        this.nodeName = nodeName;
    }
    
    //NDN : None declare name
    public NodeSelector(Func<bool> preCondition) : this(preCondition, "NDN Selector Node") { }
   

    public bool Precondition()
    {
        return this.preCondition.Invoke();
    }
    public void AddtoChildNode(INode childNode)
    {
        Dictionary<INode, Func<bool>> nodePrecondition = this.nodePrecondition;
        List<INode> childNodes = this.childNode;

        //Add ChildNode and preCondition of ChildNode
        childNodes.Add(childNode);
        nodePrecondition.Add(childNode, childNode.preCondition);

        //Set parent of ChildNode ot this Node
        if (childNode is NodeSelector Selector)
            Selector.parentNode = this;

        //IF ChildNode is NodeLeaf then PoppulateResetCondition of ChildNode
        if (childNode is INodeLeaf leafNode)
        {
            PopulatePreCondition(out List<Func<bool>> isReset, childNode);
            isReset.ForEach(resetCon => leafNode.isReset.Add(resetCon));
        }
    }
    public void RemoveNode(INode childNode) 
    {
        Dictionary<INode, Func<bool>> nodePrecondition = this.nodePrecondition;
        List<INode> childNodes = this.childNode;

        childNodes.Remove(childNode);
        nodePrecondition.Remove(childNode);
    }
    public bool FindingNode(out INode node,List<string> debugLog)
    {
        Dictionary<INode, Func<bool>> nodePrecondition = this.nodePrecondition;
        List<INode> childNodes = this.childNode;
        if(debugLog == null) 
            debugLog = new List<string>();

        node = null;

        foreach (INode NODE in this.childNode)
        {
            //Check PreCondition of that node is it true
            if (NODE.Precondition() == false)
                continue;

            //Debug.Log("Node " + nodeSelector + " -> " + node);

            if (NODE is INodeLeaf)//Node is NodeLeaf
            {
                //Debug.Log("Node " + node + " isNodeLeaf ");
                debugLog.Add("Node " + this.nodeName + " -> " + node);
                node = NODE as INodeLeaf;
                return true;
            }
            else if (NODE is NodeSelector SelectorNode)//If Node is NodeSelector Continue Searching for NodeLeaf in NodeSelector
            {
                //Debug.Log("Node " + node + " isNodeSelector ");
                debugLog.Add("Node " + this.nodeName + " -> " + SelectorNode.nodeName);
                node = SelectorNode;
                SelectorNode.FindingNode(out node,debugLog);
                return true;
            }
            //Debug.Log("Node " + node + " not both ");
        }
        if (node is INodeLeaf)
            return true;
        return false;
    }
    public bool FindingNode(out INode node) 
    {
        Dictionary<INode, Func<bool>> nodePrecondition = this.nodePrecondition;
        List<INode> childNodes = this.childNode;


        node = null;

        foreach (INode NODE in this.childNode)
        {
            //Check PreCondition of that node is it true
            if (NODE.Precondition() == false)
                continue;

            //Debug.Log("Node " + nodeSelector + " -> " + node);

            if (NODE is INodeLeaf)//Node is NodeLeaf
            {
                //Debug.Log("Node " + node + " isNodeLeaf ");
                node = NODE as INodeLeaf;
                return true;
            }
            else if (NODE is NodeSelector SelectorNode)//If Node is NodeSelector Continue Searching for NodeLeaf in NodeSelector
            {
                //Debug.Log("Node " + node + " isNodeSelector ");
                node = SelectorNode;
                SelectorNode.FindingNode(out node);
                return true;
            }
            //Debug.Log("Node " + node + " not both ");
        }
        if(node is INodeLeaf)
            return true;
        return false;
    }
    private void PopulatePreCondition(out List<Func<bool>> nodeLeafReset, INode nodePosition)
    {
        nodeLeafReset = new List<Func<bool>>();

        //Add All PreCondition Foward nodePosition as a ResetCondition of nodeLeaf
        foreach (INode Node in nodePrecondition.Keys)
        {
            if (Node == nodePosition)
                break;

            //Debug.Log("Add isReset form Node " + Node + " to " + nodePosition);

            nodeLeafReset.Add(Node.preCondition);
        }

        // Add invert PreCondition of nodePosition as ResetCondition of nodeLeaf
        nodeLeafReset.Add(() => !nodePosition.preCondition());

        //Debug.Log("Add isReset form Node " + nodePosition);

        if (parentNode == null)
            return;

        //Continue Populate ResetCondition if parent of nodePosition is NodeSelector
        if (parentNode is NodeSelector selectorNode)
        {
            selectorNode.PopulatePreCondition(out List<Func<bool>> parentLeafReset, selectorNode);

            foreach (Func<bool> pReset in parentLeafReset)
            {
                nodeLeafReset.Add((Func<bool>)pReset);
            }
        }


    }
}
