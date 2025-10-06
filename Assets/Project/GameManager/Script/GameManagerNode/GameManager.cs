
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
        Prologue,
    }
    public GameplayLevel gameplayLevelData;

    public SoundTrackManager soundTrackManager;
    public AudioClip gamePlaySoundTrack { get; set; }

    private INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager._curNodeLeaf { get => curNodeLeaf; set => curNodeLeaf = value; }
    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }
    public FrontSceneGameManagerNodeLeaf frontSceneGameManagerNodeLeaf { get; set ; }

    public GameManagerNodeSelector ingameGameManagerNodeSelector { get; set; }
    public GameManagerSceneNodeLeaf prologue_GameManagerSceneNodeLeaf { get; set; }
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
        this.prologue_GameManagerSceneNodeLeaf = new GameManagerSceneNodeLeaf("Scene_ProlougeLevel", this, () => gameplayLevelData == GameplayLevel.Prologue);



        startNodeSelector.AddtoChildNode(this.frontSceneGameManagerNodeLeaf);
        startNodeSelector.AddtoChildNode(ingameGameManagerNodeSelector);

        ingameGameManagerNodeSelector.AddtoChildNode(this.prologue_GameManagerSceneNodeLeaf);


        nodeManagerBehavior.SearchingNewNode(this);
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
            case GameplayLevel.None: 
                gameplayLevelData 
                    = GameplayLevel.Prologue; 
                break;
            case GameplayLevel.Prologue:
                {
                    gameManagerSceneData = GameManagerState.ForntScene;
                    gameplayLevelData = GameplayLevel.None;
                    break;
                }
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
