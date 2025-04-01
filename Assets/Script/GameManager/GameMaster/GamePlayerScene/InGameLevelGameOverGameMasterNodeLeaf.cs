using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameLevelGameOverGameMasterNodeLeaf : GameMasterNodeLeaf<InGameLevelGameMaster>, IGameManagerSendNotifyAble
{

    private GameOverUICanvas gameOverUICanvas => gameMaster.gameOverUICanvas;
    public enum GameOverPhase
    {
        FadeIn,
        Stay,
        FadeOutRestart,
        FadeOutExit,
    }
    public GameOverPhase gameOverPhase;

    private float eplapesTime;
    private SetAlphaColorUI setAlphaColorUI;

    public GameManager gameManager { get => gameMaster.gameManager ; set { } }

    public InGameLevelGameOverGameMasterNodeLeaf(InGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        setAlphaColorUI = new SetAlphaColorUI();
    }

    public override void Enter()
    {
        gameOverPhase = GameOverPhase.FadeIn;
        eplapesTime = 0;
        this.gameOverUICanvas.gameObject.SetActive(true);
        this.gameOverUICanvas.PlayFadeInGameOverCanvas();
        gameMaster.NotifyObserver(gameMaster);
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
        if (gameOverPhase == GameOverPhase.FadeIn)
        {
            eplapesTime += Time.deltaTime;
            if (eplapesTime >= 1f)
            {
                gameOverPhase = GameOverPhase.Stay;
                gameMaster.user.DisableInput();
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else if (gameOverPhase == GameOverPhase.Stay)
        {
            if (gameMaster.isTriggerExit)
            {
                this.gameOverUICanvas.PlayFadeOutGameOverCanvas();
                gameOverPhase = GameOverPhase.FadeOutExit;
                return;
            }
            if (gameMaster.isTriggerRestart)
            {
                this.gameOverUICanvas.PlayFadeOutGameOverCanvas();
                gameOverPhase = GameOverPhase.FadeOutRestart;
                return;
            }
        }
        else if (gameOverPhase == GameOverPhase.FadeOutRestart)
        {
            eplapesTime += Time.deltaTime;
            if (eplapesTime >= 2)
            {
                gameManager.OnNotify(this);
            }
        }
        else if (gameOverPhase == GameOverPhase.FadeOutExit)
        {
            eplapesTime += Time.deltaTime;
            if (eplapesTime >= 2)
            {
                gameManager.OnNotify(this);
            }
        }
    }
}
