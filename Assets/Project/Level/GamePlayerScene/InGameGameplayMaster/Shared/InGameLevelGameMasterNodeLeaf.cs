using System;
using UnityEngine;

public class InGameLevelGameMasterNodeLeaf<T> : GameMasterNodeLeaf<T> where T : InGameLevelGameMaster
{
    public virtual bool isComplete { get;protected set; }
    public InGameLevelGameMasterNodeLeaf(T gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        
    }
    public enum InGameLevelPhase
    {
        Enter,
        Exit
    }
    public InGameLevelPhase curPhase { get; protected set; }
    public override void Enter()
    {
        curPhase = InGameLevelPhase.Enter;
        gameMaster.NotifyObserver(gameMaster,this);
    }

    public override void Exit()
    {
        curPhase = InGameLevelPhase.Exit;
        gameMaster.NotifyObserver(gameMaster,this);
    }
    public override void UpdateNode()
    {

    }
    public override void FixedUpdateNode()
    {
        
    }

    public override bool IsComplete()
    {
        return isComplete;
    }

 
}
