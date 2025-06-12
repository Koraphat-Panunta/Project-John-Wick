using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GameMaster : MonoBehaviour,INodeManager
{
    public GameManager gameManager { get ; set ; }
    public INodeLeaf curNodeLeaf { get ; set ; }
    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }

    public abstract void FixedUpdateNode();
    

    public abstract void InitailizedNode();


    public abstract void UpdateNode();
    

    protected virtual void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    protected virtual void Start()
    {
        nodeManagerBehavior = new NodeManagerBehavior();

        this.InitailizedNode();
    }

    protected virtual void Update()
    {
        if(gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();

        this.UpdateNode();
    }
    protected virtual void FixedUpdate()
    {
        this.FixedUpdateNode();
    }

}

public abstract class GameMasterNode : INode
{
    public Func<bool> preCondition { get; set ; }
    public INode parentNode { get ; set ; }
    protected GameMaster gameMaster { get; set ; }

    public GameMasterNode(GameMaster gameMaster,Func<bool> preCondition)
    {
        this.preCondition = preCondition;
        this.gameMaster = gameMaster;   
    }   

    public virtual bool Precondition()=> preCondition.Invoke();
    
}
public abstract class GameMasterNode<T> : INode where T : GameMaster
{
    public Func<bool> preCondition { get; set; }
    public INode parentNode { get; set; }
    public T gameMaster { get;protected set; }

    public GameMasterNode(T gameMaster, Func<bool> preCondition)
    {
        this.preCondition = preCondition;
        this.gameMaster = gameMaster;
    }

    public virtual bool Precondition() => preCondition.Invoke();

}

public class GameMasterNodeSelector : GameMasterNode, INodeSelector
{
    public GameMasterNodeSelector(GameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        childNode = new List<INode>();
        nodePrecondition = new Dictionary<INode, Func<bool>>();
        nodeSelectorBehavior = new NodeSelectorBehavior();
    }

    public List<INode> childNode { get; set ; }
    public Dictionary<INode, Func<bool>> nodePrecondition { get ; set ; }
    public NodeSelectorBehavior nodeSelectorBehavior { get; set; }

    public void AddtoChildNode(INode childNode) => nodeSelectorBehavior.AddtoChildNode(childNode,this);

    public bool FindingNode(out INodeLeaf nodeLeaf) => nodeSelectorBehavior.FindingNode(out nodeLeaf, this);

    public void RemoveNode(INode childNode) => nodeSelectorBehavior.RemoveChildNode(childNode,this);
    
}
public class GameMasterNodeSelector<T> : GameMasterNode<T>, INodeSelector where T : GameMaster
{
    public GameMasterNodeSelector(T gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        childNode = new List<INode>();
        nodePrecondition = new Dictionary<INode, Func<bool>>();
        nodeSelectorBehavior = new NodeSelectorBehavior();
    }

    public List<INode> childNode { get; set; }
    public Dictionary<INode, Func<bool>> nodePrecondition { get; set; }
    public NodeSelectorBehavior nodeSelectorBehavior { get; set; }

    public void AddtoChildNode(INode childNode) => nodeSelectorBehavior.AddtoChildNode(childNode, this);

    public bool FindingNode(out INodeLeaf nodeLeaf) => nodeSelectorBehavior.FindingNode(out nodeLeaf, this);

    public void RemoveNode(INode childNode) => nodeSelectorBehavior.RemoveChildNode(childNode, this);

   
}
public abstract class GameMasterNodeLeaf : GameMasterNode, INodeLeaf
{
    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set; }

    public GameMasterNodeLeaf(GameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        isReset = new List<Func<bool>>();
        nodeLeafBehavior = new NodeLeafBehavior();
    }

    public abstract void Enter();
   

    public abstract void Exit();


    public abstract void FixedUpdateNode();
    

    public abstract bool IsComplete();
   
    public virtual bool IsReset() => nodeLeafBehavior.IsReset(isReset);

    public abstract void UpdateNode();
    
}
public abstract class GameMasterNodeLeaf<T> : GameMasterNode<T>, INodeLeaf where T : GameMaster
{
    public List<Func<bool>> isReset { get; set; }
    public NodeLeafBehavior nodeLeafBehavior { get; set; }

    public GameMasterNodeLeaf(T gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        isReset = new List<Func<bool>>();
        nodeLeafBehavior = new NodeLeafBehavior();
    }

    public abstract void Enter();


    public abstract void Exit();


    public abstract void FixedUpdateNode();


    public abstract bool IsComplete();

    public virtual bool IsReset() => nodeLeafBehavior.IsReset(isReset);

    public abstract void UpdateNode();

}
