
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
        Tutorial,
        Mansion,
        Hotel, 
    }
    public GameplayLevel gameplayLevelData;

    public SoundTrackManager soundTrackManager;
    public AudioClip gamePlaySoundTrack { get; set; }

    private INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager.curNodeLeaf { get; set; }
    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }
    public FrontSceneGameManagerNodeLeaf frontSceneGameManagerNodeLeaf { get; set ; }

    public GameManagerNodeSelector ingameGameManagerNodeSelector { get; set; }
    public TutorialGameManagerNodeLeaf tutorialGameManagerNodeLeaf{ get; set; }
    public LevelMansionGameManagerNodeLeaf levelMansionGameManagerNodeLeaf { get; set; }
    public LevelHotelGameManagerNodeLeaf levelHotelGameManagerNodeLeaf { get; set; }  
    public DataBased dataBased { get; set; }
    private void Awake()
    {
        soundTrackManager = GetComponent<SoundTrackManager>();
        nodeManagerBehavior = new NodeManagerBehavior();
        Application.targetFrameRate = 60; // Match Editor
        QualitySettings.vSyncCount = 1;  // Prevent high FPS affecting physics
        dataBased = new DataBased();
        DontDestroyOnLoad(gameObject);
    }
    public void InitailizedNode()
    {
        startNodeSelector = new GameManagerNodeSelector(() => true);

        this.frontSceneGameManagerNodeLeaf = new FrontSceneGameManagerNodeLeaf("FrontScene", this,()=> gameManagerSceneData == GameManagerState.ForntScene);

        this.ingameGameManagerNodeSelector = new GameManagerNodeSelector(() => gameManagerSceneData == GameManagerState.Gameplay);
        this.tutorialGameManagerNodeLeaf = new TutorialGameManagerNodeLeaf("Tutorial_New", this,()=> gameplayLevelData == GameplayLevel.Tutorial);
        this.levelMansionGameManagerNodeLeaf = new LevelMansionGameManagerNodeLeaf("MansionLevel", this,()=> gameplayLevelData == GameplayLevel.Mansion);
        this.levelHotelGameManagerNodeLeaf = new LevelHotelGameManagerNodeLeaf("HotelLevel", this, () => gameplayLevelData == GameplayLevel.Hotel);



        startNodeSelector.AddtoChildNode(this.frontSceneGameManagerNodeLeaf);
        startNodeSelector.AddtoChildNode(ingameGameManagerNodeSelector);

        ingameGameManagerNodeSelector.AddtoChildNode(tutorialGameManagerNodeLeaf);
        ingameGameManagerNodeSelector.AddtoChildNode(levelMansionGameManagerNodeLeaf);
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
    
    public void RestartScene()
    {
        (curNodeLeaf as GameManagerNodeLeaf).Enter();
    }
    public void StartGameplayScene(GameplayLevel gameplayLevel)
    {
        gameManagerSceneData = GameManagerState.Gameplay;
        gameplayLevelData = gameplayLevel;
    }
    public void ContinueGameplayScene()
    {
        gameManagerSceneData = GameManagerState.Gameplay;

        switch (gameplayLevelData)
        {
            case GameplayLevel.None: gameplayLevelData = GameplayLevel.Tutorial; break;

            case GameplayLevel.Tutorial: gameplayLevelData = GameplayLevel.Hotel; break;

            case GameplayLevel.Hotel: RestartScene(); break;
        }
    }
    public void ExitToMainMenu()
    {
        gameManagerSceneData = GameManagerState.ForntScene;
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }
    //public void OnNotify()
    //{
    //    switch (gameManagerSendNotifyAble)
    //    {
    //        case MenuSceneFrontSceneMasterNodeLeaf menuSceneFrontSceneMasterNodeLeaf: 
    //            {
    //                gameManagerSceneData = GameManagerState.Gameplay;
    //                gameplayLevelData = GameplayLevel.Tutorial;
    //            }
    //            break;
    //        case InGameLevelGameOverGameMasterNodeLeaf gameOverGameMasterNodeLeaf: 
    //            {
    //                if(gameOverGameMasterNodeLeaf.gameOverPhase == InGameLevelGameOverGameMasterNodeLeaf.GameOverPhase.FadeOutRestart)
    //                    (curNodeLeaf as GameManagerNodeLeaf).Enter();

    //                if(gameOverGameMasterNodeLeaf.gameOverPhase == InGameLevelGameOverGameMasterNodeLeaf.GameOverPhase.FadeOutExit)
    //                    gameManagerSceneData = GameManagerState.ForntScene;
    //            }
    //            break;
    //        case TutorialTitleGameMasterNodeLeaf tutorialTitleGameMasterNodeLeaf:
    //            {
    //                gameManagerSceneData = GameManagerState.Gameplay;
    //                gameplayLevelData = GameplayLevel.Hotel;
    //            }
    //            break;
    //    }   
    //}
}
