using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class InGameLevelGameMaster : GameMaster
{

    public GamePlayUICanvas gamePlayUICanvas;

    public User user;
    public Player player;

    protected bool isCompleteLoad = false;

    public IEnumerator DelaySceneLoaded()
    {
        yield return new WaitForSeconds(1.7f);
        isCompleteLoad = true;
    }
   
    protected override void Awake()
    {

        InitailizedUserInput();

        base.Awake();
    }
    protected override void Start()
    {
        StartCoroutine(DelaySceneLoaded());
        base.Start();
    }
    public override void FixedUpdateNode()
    {
        nodeManagerBehavior.FixedUpdateNode(this);
    }

    public override void UpdateNode()
    {
        nodeManagerBehavior.UpdateNode(this);
    }
    protected virtual void LateUpdate()
    {
        //isTriggerExit = false;
        //isTriggerRestart = false;
        //isTriggerContinue = false;
    }
   
    public abstract InGameLevelRestGameMasterNodeLeaf levelRestGameMasterNodeLeaf { get;protected set; }
    
    public void GetNotify(Player player)
    {
        this.player = player;
    }

    private List<IGameLevelMasterObserver> gameLevelMasterObservers = new List<IGameLevelMasterObserver>();
    public void AddObserver(IGameLevelMasterObserver gameLevelMasterObserver)=>this.gameLevelMasterObservers.Add(gameLevelMasterObserver);
    public void RemoveObserver(IGameLevelMasterObserver gameLevelMasterObserver)=> this.gameLevelMasterObservers.Remove(gameLevelMasterObserver);  
    public void NotifyObserver(InGameLevelGameMaster inGameLevelGameMaster)
    {
        if(gameLevelMasterObservers.Count <= 0)
            return;
        foreach(IGameLevelMasterObserver gameLevelMasterObserver in this.gameLevelMasterObservers)
        {
            gameLevelMasterObserver.OnNotify(inGameLevelGameMaster);
        }
    }
    protected virtual void OnValidate()
    {
        user = FindAnyObjectByType<User>();

        player = FindAnyObjectByType<Player>();

     
        gamePlayUICanvas = FindAnyObjectByType<GamePlayUICanvas>();

       
    }
    protected void InitailizedUserInput()
    {
        user.EnableInput();
    }
}
public interface IGameLevelMasterObserver
{
    public void OnNotify(InGameLevelGameMaster inGameLevelGameMaster);
}
public class InGameLevelRestGameMasterNodeLeaf : GameMasterNodeLeaf<InGameLevelGameMaster>
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


