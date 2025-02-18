using System;
using TMPro;
using UnityEngine;

public class SplashSceneFrontSceneMasterNodeLeaf : GameMasterNodeLeaf<FrontSceneGameMaster>
{
    private bool isComplete;
    private Canvas splashSceneCanvas => gameMaster.splashScene;
    private TextMeshProUGUI tile => gameMaster.titleGame;
    private float elapseTime;

    private SetAlphaColorUI setAlphaColorUI;

    public enum SplashScenePhase
    {
        FadeIn,
        Stay,
        FadeOut
    }

    public SplashScenePhase curPhase;

    public SplashSceneFrontSceneMasterNodeLeaf(FrontSceneGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        setAlphaColorUI = new SetAlphaColorUI();
    }

    public override void Enter()
    {
        elapseTime = 0;
        curPhase = SplashScenePhase.FadeIn;
        splashSceneCanvas.enabled = true;
        isComplete = false;

        setAlphaColorUI.SetColorAlpha<TextMeshProUGUI>(tile,0);
    }

    public override void Exit()
    {
        splashSceneCanvas.enabled = false;
    }

    public override void FixedUpdateNode()
    {
       
    }

    public override bool IsComplete()
    {
        return isComplete;
    }

    public override void UpdateNode()
    {
        elapseTime += Time.deltaTime;
        switch (curPhase)
        {
            case SplashScenePhase.FadeIn:
                {
                    setAlphaColorUI.SetColorAlpha(tile,elapseTime/0.5f);

                    if(elapseTime >= 0.5f)
                        curPhase = SplashScenePhase.Stay;
                }
                break;
            case SplashScenePhase.Stay:
                {
                    if(elapseTime >= 1.5f)
                        curPhase= SplashScenePhase.FadeOut;
                }
                break;
            case SplashScenePhase.FadeOut: 
                {
                    setAlphaColorUI.SetColorAlpha(tile,2-elapseTime);

                    if (elapseTime >= 2)
                    {
                        gameMaster.frontSceneState = FrontSceneGameMaster.FrontSceneState.menu;
                        isComplete = true;
                    }
                }
                break;
        }

    }
}
