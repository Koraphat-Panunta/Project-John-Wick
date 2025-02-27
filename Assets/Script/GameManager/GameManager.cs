using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour,INodeManager
{
    public enum GameManagerState
    {
        ForntScene,
        Gameplay
    }
    public GameManagerState gameManagerSceneData;
    public enum GameplayLevel
    {
        None,
        Mansion,
        Hotel, 
        Level2
    }
    public GameplayLevel gameplayLevelData;

    public SoundTrackManager soundTrackManager;
    public AudioClip gamePlaySoundTrack { get; set; }

    public INodeLeaf curNodeLeaf { get  ; set ; }
    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }
    public FrontSceneGameManagerNodeLeaf frontSceneGameManagerNodeLeaf { get; set ; }

    public GameManagerNodeSelector ingameGameManagerNodeSelector { get; set; }
    public LevelHotelGameManagerNodeLeaf levelHotelGameManagerNodeLeaf { get; set; }  
    public LevelMansionGameManagerNodeLeaf levelMansionGameManagerNodeleaf { get; set ; }



    public void InitailizedNode()
    {
        startNodeSelector = new GameManagerNodeSelector(() => true);

        this.frontSceneGameManagerNodeLeaf = new FrontSceneGameManagerNodeLeaf("FrontScene", this,()=> gameManagerSceneData == GameManagerState.ForntScene);

        this.ingameGameManagerNodeSelector = new GameManagerNodeSelector(() => gameManagerSceneData == GameManagerState.Gameplay);

        this.levelMansionGameManagerNodeleaf = new LevelMansionGameManagerNodeLeaf("MansionLevel", this, () => gameplayLevelData == GameplayLevel.Mansion);
        this.levelHotelGameManagerNodeLeaf = new LevelHotelGameManagerNodeLeaf("HotelLevel", this, () => gameplayLevelData == GameplayLevel.Hotel);



        startNodeSelector.AddtoChildNode(this.frontSceneGameManagerNodeLeaf);
        startNodeSelector.AddtoChildNode(ingameGameManagerNodeSelector);

        ingameGameManagerNodeSelector.AddtoChildNode(levelMansionGameManagerNodeleaf);
        ingameGameManagerNodeSelector.AddtoChildNode(levelHotelGameManagerNodeLeaf);

        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf;

        curNodeLeaf.Enter();
    }

    public void FixedUpdateNode()
    {
        nodeManagerBehavior.FixedUpdateNode(this);
    }
    public void UpdateNode()
    {
        nodeManagerBehavior.UpdateNode(this);
    }

    private void Awake()
    {
        soundTrackManager = GetComponent<SoundTrackManager>();
        nodeManagerBehavior = new NodeManagerBehavior();
        Application.targetFrameRate = 60; // Match Editor
        QualitySettings.vSyncCount = 1;  // Prevent high FPS affecting physics
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {

        this.gameManagerSceneData = GameManagerState.ForntScene;
        this.gameplayLevelData = GameplayLevel.None;

        InitailizedNode();
    }

    private void Update()
    {
        this.UpdateNode();
    }
    private void FixedUpdate()
    {
        this.FixedUpdateNode();
    }
    
    public void OnNotify(IGameManagerSendNotifyAble gameManagerSendNotifyAble)
    {
        switch (gameManagerSendNotifyAble)
        {
            case MenuSceneFrontSceneMasterNodeLeaf menuSceneFrontSceneMasterNodeLeaf: 
                {
                    gameManagerSceneData = GameManagerState.Gameplay;
                    gameplayLevelData = GameplayLevel.Mansion;
                }
                break;
            case InGameLevelMisstionCompleteGameMasterNodeLeaf levelHotelMisstionCompleteGameMasterNodeLeaf:
                {
                    if(levelHotelMisstionCompleteGameMasterNodeLeaf.curPhase == InGameLevelMisstionCompleteGameMasterNodeLeaf.MissionCompletePhase.FadeOutRestart)
                        (curNodeLeaf as GameManagerNodeLeaf).Enter();

                    if (levelHotelMisstionCompleteGameMasterNodeLeaf.curPhase == InGameLevelMisstionCompleteGameMasterNodeLeaf.MissionCompletePhase.FadeOutContinue) 
                    {
                        if (curNodeLeaf is LevelMansionGameManagerNodeLeaf)
                            gameplayLevelData = GameplayLevel.Hotel;

                        if (curNodeLeaf is LevelHotelGameManagerNodeLeaf)
                            gameplayLevelData = GameplayLevel.Mansion;
                    }
                }
                break;
            case GameOverGameMasterNodeLeaf gameOverGameMasterNodeLeaf: 
                {
                    if(gameOverGameMasterNodeLeaf.gameOverPhase == GameOverGameMasterNodeLeaf.GameOverPhase.FadeOutRestart)
                        (curNodeLeaf as GameManagerNodeLeaf).Enter();

                    if(gameOverGameMasterNodeLeaf.gameOverPhase == GameOverGameMasterNodeLeaf.GameOverPhase.FadeOutExit)
                        gameManagerSceneData = GameManagerState.ForntScene;
                }
                break;
            case InGameLevelGamplayGameMasterNodeLeaf levelHotelGamplayGameMasterNodeLeaf: 
                {
                    if (levelHotelGamplayGameMasterNodeLeaf.gameMaster.isTriggerExit)
                        gameManagerSceneData = GameManagerState.ForntScene;
                }
                break;
        }   
    }
}
public interface IGameManagerSendNotifyAble
{
    public GameManager gameManager { get; set; }
    public void SendNotify(IGameManagerSendNotifyAble gameManagerSender) => gameManager.OnNotify(gameManagerSender);
}
