using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class InGameLevelGameMaster : GameMaster
{

    public GamePlayUICanvas gamePlayUICanvas;
    public User user;
    public Player player;

    protected bool isCompleteLoad = false;

    public Dictionary<Func<bool>, Action> gameMasterEvent = new Dictionary<Func<bool>, Action>();
    public IEnumerator DelaySceneLoaded()
    {
        yield return new WaitForSeconds(1.7f);
        isCompleteLoad = true;
    }


    public override void Initialized()
    {
        base.Initialized();
    }
    protected virtual void Start()
    {
        StartCoroutine(DelaySceneLoaded());
    }
    public override void FixedUpdateNode()
    {
        nodeManagerBehavior.FixedUpdateNode(this);
    }

    public override void UpdateNode()
    {
        nodeManagerBehavior.UpdateNode(this);
    }
    public abstract PauseInGameGameMasterNodeLeaf pauseInGameGameMasterNodeLeaf { get; protected set; }
    public abstract InGameLevelRestGameMasterNodeLeaf levelRestGameMasterNodeLeaf { get;protected set; }
    
    public void GetNotify(Player player)
    {
        this.player = player;
    }

    private List<IGameLevelMasterObserver> gameLevelMasterObservers = new List<IGameLevelMasterObserver>();
    public void AddObserver(IGameLevelMasterObserver gameLevelMasterObserver)=>this.gameLevelMasterObservers.Add(gameLevelMasterObserver);
    public void RemoveObserver(IGameLevelMasterObserver gameLevelMasterObserver)=> this.gameLevelMasterObservers.Remove(gameLevelMasterObserver);  
    public void NotifyObserver<T>(InGameLevelGameMaster inGameLevelGameMaster,T var)
    {
        if(gameLevelMasterObservers.Count <= 0)
            return;
        foreach(IGameLevelMasterObserver gameLevelMasterObserver in this.gameLevelMasterObservers)
        {
            gameLevelMasterObserver.OnNotify(inGameLevelGameMaster,var);
        }
    }
    protected virtual void OnValidate()
    {
        user = FindAnyObjectByType<User>();

        player = FindAnyObjectByType<Player>();

     
        gamePlayUICanvas = FindAnyObjectByType<GamePlayUICanvas>();
    }


    protected void UpdateingEvent()
    {
        if(gameMasterEvent == null || gameMasterEvent.Count <=0)
            return;

        List<Func<bool>> preConditionEvent = gameMasterEvent.Keys.ToList();

        for (int i = 0; i < preConditionEvent.Count; i++)
        {
            if (preConditionEvent[i].Invoke() == true)
            {
                gameMasterEvent[preConditionEvent[i]].Invoke();
                gameMasterEvent.Remove(preConditionEvent[i]);
            }
        }
    }

}
public interface IGameLevelMasterObserver
{
    public void OnNotify<T>(InGameLevelGameMaster inGameLevelGameMaster,T var);
}
public class InGameLevelRestGameMasterNodeLeaf : InGameLevelGameMasterNodeLeaf<InGameLevelGameMaster>
{
    public InGameLevelRestGameMasterNodeLeaf(InGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }

    public override void Enter()
    {
      
    }

    public override void Exit()
    {
       
    }

    public override void FixedUpdateNode()
    {
       
    }

    public override bool IsComplete()
    {
        return true;
    }

    public override void UpdateNode()
    {
       
    }
}


