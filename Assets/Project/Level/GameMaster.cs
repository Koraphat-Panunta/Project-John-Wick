using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GameMaster : MonoBehaviour,INodeManager,IInitializedAble
{
    public GameManager gameManager { get ; set ; }
    private INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager.curNodeLeaf { get ; set ; }

    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }
    public DataBased dataBased 
    { 
        get 
        {
            if(gameManager != null)
                return gameManager.dataBased;

            return this.DataBased;
        } 
    }
    private DataBased DataBased;

    public static readonly float lookSensitivityDefault = 50;
    public static readonly float adsSensitivityDefault = 30;

    public static readonly float masterVolumeDefault = 100;
    public static readonly float musicVolumeDefault = 100;
    public static readonly float sfxVolumeDefault = 100;
    public virtual void Initialized()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        this.DataBased = new DataBased();
        this.dataBased.settingData.mouseSensitivivty = lookSensitivityDefault;
        this.dataBased.settingData.mouseAimDownSightSensitivity = adsSensitivityDefault;

        this.dataBased.settingData.volumeMaster = masterVolumeDefault;
        this.dataBased.settingData.volumeEffect = sfxVolumeDefault;
        this.dataBased.settingData.volumeMusic = musicVolumeDefault;

        nodeManagerBehavior = new NodeManagerBehavior();
        this.InitailizedNode();
    }

    public abstract void FixedUpdateNode();
    

    public abstract void InitailizedNode();


    public abstract void UpdateNode();
    

   

    protected virtual void Update()
    {
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
