using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class InGameLevelGameMaster : GameMaster
{

    [SerializeField] protected PauseUICanvas pauseCanvasUI;
    [SerializeField] protected OptionUICanvas optionCanvasUI;
    public GamePlayUICanvas gamePlayUICanvas;
    public User user;
    public Player player;

    protected bool isCompleteLoad = false;

    public bool isComplete { get; protected set; }

    public Dictionary<Func<bool>, Action> gameMasterEvent = new Dictionary<Func<bool>, Action>();
    public IEnumerator DelaySceneLoaded()
    {
        yield return new WaitForSeconds(1.7f);
        isCompleteLoad = true;
    }


    public override void Initialized()
    {
        isComplete = false;
        base.Initialized();
    }

    protected virtual void Start()
    {
        StartCoroutine(DelaySceneLoaded());
    }
    public override void FixedUpdateNode()
    {
        _nodeManagerBehavior.FixedUpdateNode(this);
    }

    public override void UpdateNode()
    {
        _nodeManagerBehavior.UpdateNode(this);
    }
    public NodeSelector pausingSelector { get; protected set; }
    public  MenuInGameGameMasterNodeLeaf menuInGameGameMasterNodeLeaf { get; protected set; }
    public  OptionMenuSettingInGameGameMasterNodeLeaf optionMenuSettingInGameGameMasterNode { get; protected set; }
    public InGameLevelRestGameMasterNodeLeaf levelRestGameMasterNodeLeaf { get;protected set; }
    
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
    public void CompleteLevel()
    {
        this.isComplete = true;
    }
    protected virtual void OnValidate()
    {
        user = FindAnyObjectByType<User>();

        player = FindAnyObjectByType<Player>();

     
        gamePlayUICanvas = FindAnyObjectByType<GamePlayUICanvas>();
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


