using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneFrontSceneMasterNodeLeaf : GameMasterNodeLeaf<FrontSceneGameMaster>,INodeLeafTransitionAble
{
    public bool isTriggerOption { get; private set; }

    private float fadeInduration = 1.5f;
    private float fadeOutduration = 2;


    private MainMenuUICanvas mainMenuUICanvas;
    public Image fadeImgame => gameMaster.fadeImage;
    private UIElementFader UIElementFader;


    public MenuSceneFrontSceneMasterNodeLeaf(FrontSceneGameMaster gameMaster,MainMenuUICanvas mainMenuUICanvas, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        this.mainMenuUICanvas = mainMenuUICanvas;

        transitionAbleNode = new Dictionary<INode, bool>();
        nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();

        UIElementFader = new UIElementFader();

        this.mainMenuUICanvas.newGameButton.onClick.AddListener(() => { TriggerNewGame(); });
        this.mainMenuUICanvas.exitBotton.onClick.AddListener(this.TriggerExitGame);

    }
    public GameManager gameManager { get => gameMaster.gameManager; set { } }

    public INodeManager nodeManager { get => gameMaster; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    public override void Enter()
    {
        isTriggerOption = false;

        UIElementFader.FadeDisappear(this.fadeImgame, this.fadeInduration,
            ()=> 
            { 
                nodeLeafTransitionBehavior.TransitionAbleAll(this);
            }
            );
    }

    public override void Exit()
    {
    }

    public override void FixedUpdateNode()
    {
        
    }

    public override bool IsComplete()
    {
        return false;
    }

    public override void UpdateNode()
    {
        this.Transitioning();
    }

   
    private async void TriggerNewGame()
    {
        await UIElementFader.FadeApprear(fadeImgame, fadeOutduration);
        gameManager.ContinueGameplayScene();
    }
    public void TriggerOption() => isTriggerOption = true;
    private async void TriggerExitGame()
    {
        await UIElementFader.FadeApprear(fadeImgame, fadeInduration);
        gameManager.ExitGame();
    }

    public bool Transitioning() => nodeLeafTransitionBehavior.Transitioning(this);
    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);
   
}
