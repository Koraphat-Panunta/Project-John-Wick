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

    private const float splashDelay = 0.5f;

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
        Debug.Log("Splash Enter");
        elapseTime = 0;
        curPhase = SplashScenePhase.FadeIn;
        splashSceneCanvas.enabled = true;
        isComplete = false;
        if (gameMaster.gameManager.TryGetComponent<SoundTrackManager>(out SoundTrackManager soundTrack))
        {
            soundTrack.TriggerSoundTrack(soundTrack.openingTrack);
        }
        setAlphaColorUI.SetColorAlpha<TextMeshProUGUI>(tile,0);
    }

    public override void Exit()
    {
        splashSceneCanvas.enabled = false;
    }

    public override void FixedUpdateNode()
    {
        Debug.Log("Splash FixUpate");
    }

    public override bool IsComplete()
    {
        return isComplete;
    }

    float elapsTimeDelay = 0;
    public override void UpdateNode()
    {
        float fadeInduration = 1;
        float stayDuration = 2;
        float fadeOutduration = 1;
        if (elapsTimeDelay < splashDelay)
        {
            elapsTimeDelay += Time.deltaTime;
           return;
        }
        elapseTime += Time.deltaTime;
        switch (curPhase)
        {
            case SplashScenePhase.FadeIn:
                {
                    setAlphaColorUI.SetColorAlpha(tile,elapseTime/fadeInduration);

                    if(elapseTime >= fadeInduration)
                        curPhase = SplashScenePhase.Stay;
                }
                break;
            case SplashScenePhase.Stay:
                {
                    if(elapseTime >= fadeInduration + stayDuration)
                        curPhase= SplashScenePhase.FadeOut;
                }
                break;
            case SplashScenePhase.FadeOut: 
                {
                    setAlphaColorUI.SetColorAlpha(tile, fadeOutduration - (elapseTime -(fadeInduration + stayDuration)) / fadeOutduration);

                    if (elapseTime >= fadeInduration + stayDuration + fadeOutduration)
                    {
                        gameMaster.frontSceneState = FrontSceneGameMaster.FrontSceneState.menu;
                        isComplete = true;
                    }
                }
                break;
        }

    }
}
