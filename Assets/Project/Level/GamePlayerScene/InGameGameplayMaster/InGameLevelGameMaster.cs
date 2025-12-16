using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class InGameLevelGameMaster : GameMaster
{
    [SerializeField] protected GameOverUICanvas gameOverUICanvas;
    [SerializeField] protected PauseUICanvas pauseCanvasUI;
    [SerializeField] protected OptionUICanvas optionCanvasUI;
    [SerializeField] protected LevelCompleteUICanvas missionCompleteUICanvas;
    public GamePlayUICanvas gamePlayUICanvas;
    public UserActor user;
    public Player player;

    protected bool isCompleteLoad = false;
    public bool isActiveFreeState { get; set; }
    public bool isLevelComplete { get; set; }

    public Dictionary<Func<bool>, Action> gameMasterEvent = new Dictionary<Func<bool>, Action>();
    public IEnumerator DelaySceneLoaded()
    {
        yield return new WaitForSeconds(1.7f);
        isCompleteLoad = true;
    }
    public override void Initialized()
    {
        this.isLevelComplete = false;
        this.isActiveFreeState = false;
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

    public InGameLevelFreeStateGameMasterNodeLeaf freeStateGameMasterNodeLeaf { get; protected set; }
    public InGameLevelGameOverGameMasterNodeLeaf InGameLevelGameOverGameMasterNodeLeaf { get; protected set; }
    public NodeSelector pausingSelector { get; protected set; }
    public  MenuInGameGameMasterNodeLeaf menuInGameGameMasterNodeLeaf { get; protected set; }
    public  OptionMenuSettingInGameGameMasterNodeLeaf optionMenuSettingInGameGameMasterNode { get; protected set; }
    public InGameLevelCompleteGameMasterNodeLeaf levelCompleteGameMasterNodeLeaf { get; protected set; }
    public InGameLevelGamplayGameMasterNodeLeaf<InGameLevelGameMaster> inGameLevelGamplayGameMasterNodeLeaf { get; protected set; }


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
        this.isLevelComplete = true;
    }
    protected virtual void OnValidate()
    {
        user = FindAnyObjectByType<UserActor>();

        player = FindAnyObjectByType<Player>();

        gamePlayUICanvas = FindAnyObjectByType<GamePlayUICanvas>();
    }

    public override void InitailizedNode()
    {
        this.startNodeSelector = new NodeSelector(() => true, "PrologueStartNodeSelector");

        this.freeStateGameMasterNodeLeaf = new InGameLevelFreeStateGameMasterNodeLeaf(this,()=> isActiveFreeState);    
        this.InGameLevelGameOverGameMasterNodeLeaf = new InGameLevelGameOverGameMasterNodeLeaf(this, gameOverUICanvas, () => player.isDead);
        this.pausingSelector = new NodeSelector(() => this.menuInGameGameMasterNodeLeaf.isMenu);
        this.menuInGameGameMasterNodeLeaf = new MenuInGameGameMasterNodeLeaf(this, pauseCanvasUI, () => true);
        this.optionMenuSettingInGameGameMasterNode = new OptionMenuSettingInGameGameMasterNodeLeaf(this, optionCanvasUI, () => menuInGameGameMasterNodeLeaf.isTriggerToSetting);

        this.levelCompleteGameMasterNodeLeaf = new InGameLevelCompleteGameMasterNodeLeaf(this, missionCompleteUICanvas, () => this.isLevelComplete);
        this.inGameLevelGamplayGameMasterNodeLeaf = new InGameLevelGamplayGameMasterNodeLeaf<InGameLevelGameMaster>(this, () => true);

        this.startNodeSelector.AddtoChildNode(this.freeStateGameMasterNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.InGameLevelGameOverGameMasterNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.pausingSelector);
        this.startNodeSelector.AddtoChildNode(this.levelCompleteGameMasterNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.inGameLevelGamplayGameMasterNodeLeaf);

        this.pausingSelector.AddtoChildNode(this.optionMenuSettingInGameGameMasterNode);
        this.pausingSelector.AddtoChildNode(this.menuInGameGameMasterNodeLeaf);

        _nodeManagerBehavior.SearchingNewNode(this);

    }
}
public interface IGameLevelMasterObserver
{
    public void OnNotify<T>(InGameLevelGameMaster inGameLevelGameMaster,T var);
}
public class InGameLevelFreeStateGameMasterNodeLeaf : InGameLevelGameMasterNodeLeaf<InGameLevelGameMaster>
{
    public InGameLevelFreeStateGameMasterNodeLeaf(InGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
       base .Exit();
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


