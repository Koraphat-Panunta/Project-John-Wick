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
        Level0,
        Level1, 
        Level2
    }
    public GameplayLevel gameplayLevelData;


    public INodeLeaf curNodeLeaf { get  ; set ; }
    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }
    public FrontSceneGameManagerNodeLeaf frontSceneGameManagerNodeLeaf { get; set ; }
    public LevelHotelGameManagerNodeLeaf levelHotelGameManagerNodeLeaf { get; set; }  

    public void InitailizedNode()
    {
        startNodeSelector = new GameManagerNodeSelector(() => true);

        this.frontSceneGameManagerNodeLeaf = new FrontSceneGameManagerNodeLeaf("FrontScene", this,()=> gameManagerSceneData == GameManagerState.ForntScene);
        this.levelHotelGameManagerNodeLeaf = new LevelHotelGameManagerNodeLeaf("HotelLevel", this, () => gameManagerSceneData == GameManagerState.Gameplay);

        startNodeSelector.AddtoChildNode(this.frontSceneGameManagerNodeLeaf);
        startNodeSelector.AddtoChildNode(this.levelHotelGameManagerNodeLeaf);

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
        nodeManagerBehavior = new NodeManagerBehavior();
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
                }
                break;
            case LevelHotelMisstionCompleteGameMasterNodeLeaf levelHotelMisstionCompleteGameMasterNodeLeaf:
                {
                    if(levelHotelMisstionCompleteGameMasterNodeLeaf.curPhase == LevelHotelMisstionCompleteGameMasterNodeLeaf.MissionCompletePhase.FadeOutRestart)
                        (curNodeLeaf as LevelHotelGameManagerNodeLeaf).Enter();
                }
                break;
            case GameOverGameMasterNodeLeaf gameOverGameMasterNodeLeaf: 
                {
                    if(gameOverGameMasterNodeLeaf.gameOverPhase == GameOverGameMasterNodeLeaf.GameOverPhase.FadeOutRestart)
                        (curNodeLeaf as LevelHotelGameManagerNodeLeaf).Enter();

                    if(gameOverGameMasterNodeLeaf.gameOverPhase == GameOverGameMasterNodeLeaf.GameOverPhase.FadeOutExit)
                        gameManagerSceneData = GameManagerState.ForntScene;
                }
                break;
            case LevelHotelGamplayGameMasterNodeLeaf levelHotelGamplayGameMasterNodeLeaf: 
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
