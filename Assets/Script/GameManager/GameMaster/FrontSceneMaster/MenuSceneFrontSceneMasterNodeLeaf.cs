using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneFrontSceneMasterNodeLeaf : GameMasterNodeLeaf<FrontSceneGameMaster>,IGameManagerSendNotifyAble
{
    public bool isTriggerNewGame;
    private float fadeInTimer;
    private float fadeOutTimer;
    public enum MenuPhase
    {
        FadeIn,
        Stay,
        FadeOut
    }
    public MenuPhase phase;

    public Image fadeImgame => gameMaster.fadeImage;
    private SetAlphaColorUI fadeColorUI;
    public MenuSceneFrontSceneMasterNodeLeaf(FrontSceneGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        phase = MenuPhase.FadeIn;
        fadeInTimer = 0;
        fadeOutTimer = 0;
        fadeColorUI = new SetAlphaColorUI();
    }
    public GameManager gameManager { get => gameMaster.gameManager; set { } }

    public override void Enter()
    {
        isTriggerNewGame = false;
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
        float fadeInduration = 1.5f;
        float fadeOutduration = 2;

        switch (phase)
        {
            case MenuPhase.FadeIn: 
                {
                    fadeInTimer += Time.deltaTime;
                    fadeColorUI.SetColorAlpha<Image>(fadeImgame,1- (fadeInTimer / fadeInduration));
                    if(fadeInTimer >= fadeInduration)
                        phase = MenuPhase.Stay;
                }
                break;
            case MenuPhase.Stay: 
                { 
                    fadeImgame.enabled = false;
                    if (isTriggerNewGame)
                    {
                        fadeImgame.enabled = true;
                        phase = MenuPhase.FadeOut;
                    }
                }
                break;
            case MenuPhase.FadeOut: 
                {
                    fadeOutTimer += Time.deltaTime;
                    fadeColorUI.SetColorAlpha<Image>(fadeImgame, fadeOutTimer / fadeOutduration);

                    if (fadeOutTimer >= fadeOutduration)
                    {
                        gameManager.OnNotify(this);
                    }
                }
                break;
        }
    }
}
