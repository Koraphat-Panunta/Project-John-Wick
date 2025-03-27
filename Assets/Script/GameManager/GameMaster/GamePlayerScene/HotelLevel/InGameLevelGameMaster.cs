using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameLevelGameMaster : GameMaster
{
    public OpeningUICanvas openingUICanvas;

    public Canvas gameplayCanvasUI;
    public User user;
    public CrosshairController crosshairController;
    public RawImage hpBar;
    public TextMeshProUGUI weaponInfo;

    public Canvas gameOverCanvasUI;
    public Image gameOverTitlePanel;
    public TextMeshProUGUI gameOverTitlePanelText;
    public Button gameOverRestartButton;
    public Button gameOverExitButton;
    public Image gameOverFadeImage;

    public Canvas missionCompleteCanvasUI;
    public Image missionCompleteImageFade;
    public Image misstionCompletePanelTitle;
    public TextMeshProUGUI misstionCompleteTitle;
    public Button misstionCompleteContinueButton;
    public Button misstionCompleteRestartButton;

    public Canvas pauseCanvasUI;

    public ObjectiveManager objectiveManager;
    public List<Character> targetEliminationQuest;
    public Transform destination;
    public Player player;

    private bool isCompleteLoad =false;
    public enum LevelHotelPhase
    {
        Opening,
        Gameplay,
        GameOver,
        MissionComplete
    }
    public LevelHotelPhase curLevelHotelPhase;
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
        curLevelHotelPhase = LevelHotelPhase.Opening;
        if(targetEliminationQuest.Count > 0)
        foreach (Character target in targetEliminationQuest)
        {
            target.gameObject.SetActive(false);
        }
        player.gameObject.SetActive(false);
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
    private void LateUpdate()
    {
        isTriggerExit = false;
        isTriggerRestart = false;
        isTriggerContinue = false;
    }

    public InGameLevelOpeningGameMasterNodeLeaf levelHotelOpeningGameMasterNodeLeaf { get; private set; }
    public InGameLevelGamplayGameMasterNodeLeaf levelHotelGamplayGameMasterNodeLeaf { get; private set; }
    public InGameLevelMisstionCompleteGameMasterNodeLeaf levelHotelMisstionCompleteGameMasterNodeLeaf { get; private set; }
    public GameOverGameMasterNodeLeaf gameOverGameMasterNodeLeaf { get; private set; }    
    private LevelHotelRestGameMasterNodeLeaf levelHotelRestGameMasterNodeLeaf;

    public override void InitailizedNode()
    {
        startNodeSelector = new GameMasterNodeSelector<InGameLevelGameMaster>(this,()=> true);

        levelHotelOpeningGameMasterNodeLeaf = new InGameLevelOpeningGameMasterNodeLeaf(this,()=> curLevelHotelPhase == LevelHotelPhase.Opening && isCompleteLoad);
        levelHotelGamplayGameMasterNodeLeaf = new InGameLevelGamplayGameMasterNodeLeaf(this, () => curLevelHotelPhase == LevelHotelPhase.Gameplay  && isCompleteLoad);
        levelHotelMisstionCompleteGameMasterNodeLeaf = new InGameLevelMisstionCompleteGameMasterNodeLeaf(this, () => curLevelHotelPhase == LevelHotelPhase.MissionComplete && isCompleteLoad);
        gameOverGameMasterNodeLeaf = new GameOverGameMasterNodeLeaf(this, () => curLevelHotelPhase == LevelHotelPhase.GameOver && isCompleteLoad);
        levelHotelRestGameMasterNodeLeaf = new LevelHotelRestGameMasterNodeLeaf(this, () => true);

        startNodeSelector.AddtoChildNode(levelHotelOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelHotelGamplayGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelHotelMisstionCompleteGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(gameOverGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelHotelRestGameMasterNodeLeaf);

        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf;
    }

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
}
public interface IGameLevelMasterObserver
{
    public void OnNotify(InGameLevelGameMaster inGameLevelGameMaster);
}
public class LevelHotelRestGameMasterNodeLeaf : GameMasterNodeLeaf<InGameLevelGameMaster>
{
    public LevelHotelRestGameMasterNodeLeaf(InGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
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


