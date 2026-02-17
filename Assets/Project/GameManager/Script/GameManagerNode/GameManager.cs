
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour,INodeManager
{

    public static GameManager gameManagerInstance;


   

    public SoundTrackManager soundTrackManager;
    public AudioClip gamePlaySoundTrack { get; set; }

    private INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager._curNodeLeaf { get => curNodeLeaf; set => curNodeLeaf = value; }
    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior _nodeManagerBehavior { get; set; }
    public FrontSceneGameManagerNodeLeaf frontSceneGameManagerNodeLeaf { get; set ; }

    public GameManagerNodeSelector ingameGameManagerNodeSelector { get; set; }
    public GameManagerSceneNodeLeaf prologue_GameManagerSceneNodeLeaf { get; set; }
    public List<INodeManager> _parallelNodeManahger { get ; set ; }

    private void Awake()
    {
        soundTrackManager = GetComponent<SoundTrackManager>();
        _nodeManagerBehavior = new NodeManagerBehavior();
        this._parallelNodeManahger = new List<INodeManager>();
        Application.targetFrameRate = 60; // Match Editor
        QualitySettings.vSyncCount = 1;  // Prevent high FPS affecting physics
        DontDestroyOnLoad(gameObject);

        gameManagerInstance = this;
    }
    public void InitailizedNode()
    {
        startNodeSelector = new GameManagerNodeSelector(() => true);

        //this.frontSceneGameManagerNodeLeaf = new FrontSceneGameManagerNodeLeaf("FrontScene", this,()=> gameManagerSceneData == GameManagerState.ForntScene);

        //this.ingameGameManagerNodeSelector = new GameManagerNodeSelector(() => gameManagerSceneData == GameManagerState.Gameplay);
        //this.prologue_GameManagerSceneNodeLeaf = new GameManagerSceneNodeLeaf("Scene_ProlougeLevel", this, () => gameplayLevelData == GameplayLevel.Prologue);



        startNodeSelector.AddtoChildNode(this.frontSceneGameManagerNodeLeaf);
        startNodeSelector.AddtoChildNode(ingameGameManagerNodeSelector);

        ingameGameManagerNodeSelector.AddtoChildNode(this.prologue_GameManagerSceneNodeLeaf);


        _nodeManagerBehavior.SearchingNewNode(this);
    }

    public void FixedUpdateNode()
    {
        _nodeManagerBehavior.FixedUpdateNode(this);
    }
    public void UpdateNode()
    {
        _nodeManagerBehavior.UpdateNodeAndCheckFindingNode(this);
    }

   
    private void Start()
    {

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

    public void ContinueGameplayScene()
    {
        
    }

    public void ExitToMainMenu()
    {
       
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
