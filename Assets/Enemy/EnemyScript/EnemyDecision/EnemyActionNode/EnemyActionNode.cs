using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyActionNode : INode
{
    protected Enemy enemy;
    protected EnemyCommandAPI enemyCommandAPI;
    public EnemyActionNode(Enemy enemy,EnemyCommandAPI enemyCommandAPI,Func<bool> preCondition)
    {
        this.enemy = enemy;
        this.enemyCommandAPI = enemyCommandAPI;
        this.preCondition = preCondition;
    }
    public Func<bool> preCondition { get ; set ; }
    public INode parentNode { get; set; }

    public bool Precondition() => preCondition.Invoke();
    
}
public class EnemyActionSelectorNode : EnemyActionNode, INodeSelector
{
    public List<INode> childNode { get; set; }
    public Dictionary<INode, Func<bool>> nodePrecondition { get; set; }
    public NodeSelectorBehavior nodeSelectorBehavior { get; set; }

    public EnemyActionSelectorNode(Enemy enemy, EnemyCommandAPI enemyCommandAPI, Func<bool> preCondition) : base(enemy, enemyCommandAPI, preCondition)
    {
        nodeSelectorBehavior = new NodeSelectorBehavior();
        childNode = new List<INode>();
        nodePrecondition = new Dictionary<INode, Func<bool>>();
    }



    public void AddtoChildNode(INode childNode) => nodeSelectorBehavior.AddtoChildNode(childNode,this);


    public void FindingNode(out INodeLeaf nodeLeaf) => nodeSelectorBehavior.FindingNode(out nodeLeaf, this);
    

    public void RemoveNode(INode childNode)=>nodeSelectorBehavior.RemoveChildNode(childNode,this);
   
}
public abstract class EnemyActionNodeLeaf : EnemyActionNode, INodeLeaf
{
    bool isComplete;
    public EnemyActionNodeManager enemyActionNodeManager { get; set; }
    protected EnemyActionNodeLeaf(Enemy enemy, EnemyCommandAPI enemyCommandAPI, Func<bool> preCondition, EnemyActionNodeManager enemyActionNodeManager) : base(enemy, enemyCommandAPI, preCondition)
    {
        isReset = new List<Func<bool>>();
        nodeLeafBehavior = new NodeLeafBehavior();
        this.enemyActionNodeManager = enemyActionNodeManager;
    }

    public List<Func<bool>> isReset { get; set ; }
    public NodeLeafBehavior nodeLeafBehavior { get ; set ; }

    public virtual void Enter()
    {
        Debug.Log("Enter "+this);
        isComplete = false;
    }

    public virtual void Exit()
    {
        Debug.Log("Exit " + this);
    }

    public virtual void FixedUpdateNode()
    {
        
    }

    public virtual bool IsComplete()
    {
        return isComplete;
    }

    public virtual bool IsReset()=>nodeLeafBehavior.IsReset(isReset);
    

    public virtual void UpdateNode()
    {
        
    }
}
