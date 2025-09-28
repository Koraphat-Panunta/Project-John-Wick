using UnityEngine;
using System.Collections.Generic;
public class TimeControlManager : MonoBehaviour, INodeManager,IInitializedAble
{
    public static readonly float fixDeltaTimeOnSlowMotion = 0.02f * 0.1f;
    public static readonly float fixDeltaTimeDefault = 0.02f ;
    public NodeManagerBehavior nodeManagerBehavior { get ; set; }

    private INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager.curNodeLeaf { get => this.curNodeLeaf; set => curNodeLeaf = value; }

    protected bool isSystemStopTime;

    public void Initialized()
    {
        nodeManagerBehavior = new NodeManagerBehavior();
        this.observerTimeControlManagers = new List<IObserverTimeControlManager>();
        InitailizedNode();
    }
    private void Update()
    {
        if(isSystemStopTime == false)
            this.UpdateNode();
    }
    private void FixedUpdate()
    {
        if(isSystemStopTime == false)
            this.FixedUpdateNode();
    }
    public void UpdateNode() => nodeManagerBehavior.UpdateNode(this);
    public void FixedUpdateNode() => nodeManagerBehavior.FixedUpdateNode(this);

    public INodeSelector startNodeSelector { get; set; }
    public TimeScaleSetNodeLeaf timeDefaultNodeLeaf { get; set; }
    public void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true, "startNodeSelector TimeControlManager");

        timeDefaultNodeLeaf = new TimeScaleSetNodeLeaf(()=> true,this,1,1);
    }

    #region ObserverPattern
    protected List<IObserverTimeControlManager> observerTimeControlManagers { get; set; }

    public void NotifyObserver<T>(TimeControlManager timeControlManager, T Var)
    {
        if (this.observerTimeControlManagers.Count <= 0)
            return;

        for (int i = 0; i < this.observerTimeControlManagers.Count; i++)
        {
            this.observerTimeControlManagers[i].OnNotifyObserver(timeControlManager, Var);
        }
    }
        
    public void AddObserver(IObserverTimeControlManager observerTimeControlManager)
    {
        this.observerTimeControlManagers.Add(observerTimeControlManager);
    }
    public void RemoveObsever(IObserverTimeControlManager observerTimeControlManager)
    {
        this.observerTimeControlManagers.Remove(observerTimeControlManager);    
    }
    #endregion

}
