
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour,INodeManager,IInitializedAble
{

    public static GameManager gameManagerInstance;


    private INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager._curNodeLeaf { get => curNodeLeaf; set => curNodeLeaf = value; }
    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior _nodeManagerBehavior { get; set; }
    public FrontSceneGameManagerNodeLeaf frontSceneGameManagerNodeLeaf { get; set ; }

    public GameManagerNodeSelector ingameGameManagerNodeSelector { get; set; }
    public GameManagerSceneNodeLeaf prologue_GameManagerSceneNodeLeaf { get; set; }
    public List<INodeManager> _parallelNodeManahger { get ; set ; }

   
    public void InitailizedNode()
    {
        startNodeSelector = new GameManagerNodeSelector(() => true);

        this.frontSceneGameManagerNodeLeaf = new FrontSceneGameManagerNodeLeaf("FrontScene", this, () => true);

        this.ingameGameManagerNodeSelector = new GameManagerNodeSelector(() => this.triggerEnter );
        this.prologue_GameManagerSceneNodeLeaf = new GameManagerSceneNodeLeaf("Scene_ProlougeLevel", this, () => true);

        startNodeSelector.AddtoChildNode(this.frontSceneGameManagerNodeLeaf);
        startNodeSelector.AddtoChildNode(ingameGameManagerNodeSelector);

        ingameGameManagerNodeSelector.AddtoChildNode(this.prologue_GameManagerSceneNodeLeaf);


        _nodeManagerBehavior.SearchingNewNode(this);
    }

    public void Initialized()
    {

        _nodeManagerBehavior = new NodeManagerBehavior();
        this._parallelNodeManahger = new List<INodeManager>();
        DontDestroyOnLoad(gameObject);

        gameManagerInstance = this;
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

    
    
    public void RestartScene()
    {
        (curNodeLeaf as GameManagerNodeLeaf).Enter();
    }

    public void GameManagerOnValidate()
    {
        this._nodeManagerBehavior.CheckFindingNode(this);
        this.triggerEnter = false;
    }
    public bool triggerEnter;
    public void ContinueGameplayScene()
    {
        this.triggerEnter = true;
        this.GameManagerOnValidate();
    }

    public void ExitToMainMenu()
    {
        this.GameManagerOnValidate();
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
