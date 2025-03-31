using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class InGameLevelGameMaster : GameMaster
{
    public OpeningUICanvas openingUICanvas;

    public GamePlayUICanvas gamePlayUICanvas;

    public GameOverUICanvas gameOverUICanvas;

    public MissionCompleteUICanvas missionCompleteUICanvas;

    public PauseUICanvas pauseCanvasUI;

    public Objective curObjective { 
        get { return objective; } 
        set{ 
            objective = value; 
            OnObjectiveUpdate.Invoke(objective);
        } 
    }
    private Objective objective;

    public Action<Objective> OnObjectiveUpdate;

    public User user;
    public Player player;

    private bool isCompleteLoad = false;

    private IEnumerator DelaySceneLoaded()
    {
        yield return new WaitForSeconds(1.7f);
        isCompleteLoad = true;
    }
   
    protected override void Awake()
    {
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
        isTriggerExit = false;
        isTriggerRestart = false;
        isTriggerContinue = false;
    }

    public abstract InGameLevelOpeningGameMasterNodeLeaf levelHotelOpeningGameMasterNodeLeaf { get; protected set; }
    public abstract InGameLevelMisstionCompleteGameMasterNodeLeaf levelHotelMisstionCompleteGameMasterNodeLeaf { get; protected set; }
    public abstract GameOverGameMasterNodeLeaf gameOverGameMasterNodeLeaf { get; protected set; }    
    public abstract InGameLevelRestGameMasterNodeLeaf levelHotelRestGameMasterNodeLeaf { get;protected set; }
   

    public bool isTriggerRestart;
    public bool isTriggerContinue;
    public bool isTriggerExit;
    public void TriggerRestert()
    {
        isTriggerRestart = true;
    }
    public void TriggerContinue()
    {
        isTriggerContinue = true;
    }
    public void TriggerExit() 
    {
        isTriggerExit = true;

        if (isPause)
        {
            pauseCanvasUI.gameObject.SetActive(false);
            Time.timeScale = 1;
            isPause = false;
        }
    }
    public void Resume()
    {
        pauseCanvasUI.gameObject.SetActive(false);
        Time.timeScale = 1;
        isPause = false;
        Cursor.lockState = CursorLockMode.Locked;
        user.EnableInput();
    }

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

    private void OnValidate()
    {
        openingUICanvas = FindAnyObjectByType<OpeningUICanvas>();
        gamePlayUICanvas = FindAnyObjectByType<GamePlayUICanvas>();

        gameOverUICanvas = FindAnyObjectByType<GameOverUICanvas>(FindObjectsInactive.Include);
        gameOverUICanvas.gameOverExitButton.onClick.AddListener(TriggerExit);
        gameOverUICanvas.gameOverRestartButton.onClick.AddListener(TriggerRestert);

        missionCompleteUICanvas = FindAnyObjectByType<MissionCompleteUICanvas>(FindObjectsInactive.Include);
        missionCompleteUICanvas.continueButton.onClick.AddListener(TriggerContinue);
        missionCompleteUICanvas.restartButton.onClick.AddListener(TriggerRestert);

        pauseCanvasUI = FindAnyObjectByType<PauseUICanvas>(FindObjectsInactive.Include);
        pauseCanvasUI.resume.onClick.AddListener(Resume);
        pauseCanvasUI.exit.onClick.AddListener(TriggerExit);

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


