using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SplashSceneFrontSceneMasterNodeLeaf : GameMasterNodeLeaf<FrontSceneGameMaster>
{
    public bool isComplete { get; private set; }
    private Canvas splashSceneCanvas => gameMaster.splashScene;
    private TextMeshProUGUI tile => gameMaster.titleGame;

    private UIElementFader elementFader;



    public SplashSceneFrontSceneMasterNodeLeaf(FrontSceneGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        elementFader = new UIElementFader();
    }

    public override void Enter()
    {
        isComplete = false;
        SplashSceneEvent();
        if (gameMaster.gameManager.TryGetComponent<SoundTrackManager>(out SoundTrackManager soundTrack))
            soundTrack.TriggerSoundTrack(soundTrack.openingTrack);
        

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
        splashSceneCanvas.gameObject.SetActive(true);
        elementFader.SetAlphaSceneFade(tile, 0);
        await Task.Delay((int)(1000 * splashDelay));
        await elementFader.FadeAppear(tile,this.fadeInDuration);
        await Task.Delay((int)(1000 * stayDuration));
        await elementFader.FadeDisappear(tile, this.fadeOutDuration);
        splashSceneCanvas.gameObject.SetActive(false);
        isComplete = true;
    }
}
