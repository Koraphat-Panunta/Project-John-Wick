using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SplashSceneFrontSceneMasterNodeLeaf : GameMasterNodeLeaf<FrontSceneGameMaster>
{
    public bool isComplete { get; private set; }

    private SplashUICanvas splashUICanvas; 

    private UIElementFader elementFader;


    public SplashSceneFrontSceneMasterNodeLeaf(
        FrontSceneGameMaster gameMaster
        , SplashUICanvas splashUICanvas
        , Func<bool> preCondition) 
        : base(gameMaster, preCondition)
    {
        this.splashUICanvas = splashUICanvas;
        this.elementFader = new UIElementFader();
    }

    public override void Enter()
    {
        this.isComplete = false;
        SplashSceneEvent();
    }

    public override void Exit()
    {

    }

    public override void FixedUpdateNode()
    {
    }

    public override bool IsComplete()
    {
        return isComplete;
    }

    float elapsTimeDelay = 0;
    public override void UpdateNode()
    {

    }

    private const float splashDelay = 0.5f;
    private float fadeInDuration = 1;
    private float stayDuration = 2;
    private float fadeOutDuration = 1;  
    private async void SplashSceneEvent()
    {
        this.splashUICanvas.gameObject.SetActive(true);
        this.elementFader.SetAlphaSceneFade(this.splashUICanvas.text, 0);
        await Task.Delay((int)(1000 * splashDelay));
        await elementFader.FadeAppear(this.splashUICanvas.text, this.fadeInDuration);
        await Task.Delay((int)(1000 * stayDuration));
        await elementFader.FadeDisappear(this.splashUICanvas.text, this.fadeOutDuration);
        this.splashUICanvas.gameObject.SetActive(false);
        this.isComplete = true;
    }
}
