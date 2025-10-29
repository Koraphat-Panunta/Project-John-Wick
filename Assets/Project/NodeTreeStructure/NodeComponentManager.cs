using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeComponentManager 
{
    public Dictionary<INode, bool> combineNodeActivate { get; set; } //Combine Node is Activate
    public Dictionary<INode, INodeLeaf> combineNodeLeaf { get; set; } //NodeLeaf of Combine ,can be selfNode or childNode

    public NodeComponentManager() 
    {
        this.combineNodeActivate = new Dictionary<INode, bool>();
        this.combineNodeLeaf = new Dictionary<INode, INodeLeaf>();
    }

    public void AddNode(INode node)
    {
        this.combineNodeActivate.Add(node, false);
        this.combineNodeLeaf.Add(node, null);
        if (node is INodeLeaf nodeleaf)
        {
            nodeleaf.isReset.Add(() => !nodeleaf.Precondition());
        }
    }
    public void RemoveNode(INode removeNode)
    {
        this.combineNodeActivate.Remove(removeNode);
        this.combineNodeLeaf.Remove(removeNode);
    }
    public void Update()
    {
        var keys = this.combineNodeActivate.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            INode node = keys[i];
            if (this.combineNodeActivate[node])
            {
                if (node is INodeLeaf nodeLeaf)
                {
                    if (nodeLeaf.IsReset())
                    {
                        nodeLeaf.Exit();
                        this.combineNodeActivate[node] = false;
                        this.combineNodeLeaf[node] = null;
                        continue;
                    }
                }
                else if (node is INodeSelector nodeSelector)
                {
                    if (this.combineNodeLeaf[node].IsReset())
                    {

                        if (nodeSelector.Precondition())
                        {
                            nodeSelector.FindingNode(out INodeLeaf outNodeLeaf);
                            this.combineNodeLeaf[node].Exit();
                            this.combineNodeLeaf[node] = outNodeLeaf;
                            this.combineNodeLeaf[node].Enter();
                        }
                        else
                        {
                            this.combineNodeLeaf[node].Exit();
                            this.combineNodeActivate[node] = false;
                            this.combineNodeLeaf[node] = null;
                            continue;
                        }
                    }
                }
                this.combineNodeLeaf[node]?.UpdateNode();
            }
            else
            {
                if (node is INodeLeaf nodeLeaf && nodeLeaf.Precondition())
                {
                    nodeLeaf.Enter();
                    this.combineNodeLeaf[node] = nodeLeaf;
                    this.combineNodeActivate[node] = true;
                    nodeLeaf.UpdateNode();
                }
                else if (node is INodeSelector nodeSelector && nodeSelector.Precondition())
                {
                    nodeSelector.FindingNode(out INodeLeaf outNodeLeaf);
                    outNodeLeaf.Enter();
                    this.combineNodeLeaf[node] = outNodeLeaf;
                    this.combineNodeActivate[node] = true;
                    outNodeLeaf.UpdateNode();
                }
            }
        }
    }

    public void FixedUpdate()
    {
        var keys = this.combineNodeActivate.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            INode node = keys[i];
            if (this.combineNodeActivate[node] && this.combineNodeLeaf[node] != null)
            {
                this.combineNodeLeaf[node].FixedUpdateNode();
            }
        }
    }

}
