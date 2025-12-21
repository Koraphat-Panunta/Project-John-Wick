using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameLevelGameOverGameMasterNodeLeaf : InGameLevelGameMasterNodeLeaf<InGameLevelGameMaster>
{
    private bool isTriggerExit;
    private bool isTriggerRestart;
    private GameOverUICanvas gameOverUICanvas;
    public enum GameOverPhase
    {
        FadeIn,
        Stay,
        FadeOutRestart,
        FadeOutExit,
    }
    public GameOverPhase gameOverPhase;

    private float eplapesTime;

    public GameManager gameManager { get => gameMaster.gameManager ; set { } }

    public InGameLevelGameOverGameMasterNodeLeaf(InGameLevelGameMaster gameMaster,GameOverUICanvas gameOverUICanvas, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        this.gameOverUICanvas = gameOverUICanvas;

        this.gameOverUICanvas.gameOverRestartButton.onClick.AddListener(() => isTriggerRestart = true);
        this.gameOverUICanvas.gameOverExitButton.onClick.AddListener(() => isTriggerExit = true);
    }

    public override void Enter()
    {
        isTriggerExit = false;
        isTriggerRestart = false;

        gameOverPhase = GameOverPhase.FadeIn;
        eplapesTime = 0;
        this.gameOverUICanvas.gameObject.SetActive(true);
        this.gameOverUICanvas.PlayFadeInGameOverCanvas();
        try
        {
            gameManager.soundTrackManager.StopSoundTrack(0.5f);
        }
        catch { }
        gameMaster.NotifyObserver(gameMaster,this);
    }

    public override void Exit()
    {
        gameMaster.NotifyObserver(gameMaster, this);
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
            Cursor.lockState = CursorLockMode.None;
            if (isTriggerExit)
            {
                this.gameOverUICanvas.PlayFadeOutGameOverCanvas();
                gameOverPhase = GameOverPhase.FadeOutExit;
                return;
            }
            if (isTriggerRestart)
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
                gameManager.RestartScene();
            }
        }
        else if (gameOverPhase == GameOverPhase.FadeOutExit)
        {
            eplapesTime += Time.deltaTime;
            if (eplapesTime >= 2)
            {
                gameManager.ExitToMainMenu();
            }
        }
    }
}
