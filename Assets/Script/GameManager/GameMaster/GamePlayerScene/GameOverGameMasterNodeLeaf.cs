using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverGameMasterNodeLeaf : GameMasterNodeLeaf<LevelHotelGameMaster>, IGameManagerSendNotifyAble
{
    private Canvas gameOverCanvasUI => gameMaster.gameOverCanvasUI;
    private Image titlePanel => gameMaster.gameOverTitlePanel;
    private TextMeshProUGUI titlePanelText => gameMaster.gameOverTitlePanelText;
    private Button restartButton => gameMaster.gameOverRestartButton;
    private Button exitButton => gameMaster.gameOverExitButton;
    private Image fadeImage => gameMaster.gameOverFadeImage;

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

    public GameOverGameMasterNodeLeaf(LevelHotelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        setAlphaColorUI = new SetAlphaColorUI();
    }

    public override void Enter()
    {
        gameOverPhase = GameOverPhase.FadeIn;
        eplapesTime = 0;
        this.gameOverCanvasUI.gameObject.SetActive(true);
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
            Debug.Log("GameOver eplapesTime fadeIn = " + eplapesTime);
            setAlphaColorUI.SetColorAlpha<Image>(titlePanel, eplapesTime / 1f);
            setAlphaColorUI.SetColorAlpha<TextMeshProUGUI>(titlePanelText, eplapesTime / 1f);

            if (eplapesTime >= 1f)
            {
                gameOverPhase = GameOverPhase.Stay;
                exitButton.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
                gameMaster.user.DisableInput();
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else if (gameOverPhase == GameOverPhase.Stay)
        {
            if (gameMaster.isTriggerExit)
            {
                gameOverPhase = GameOverPhase.FadeOutExit;
                return;
            }
            if (gameMaster.isTriggerRestart)
            {
                gameOverPhase = GameOverPhase.FadeOutRestart;
                return;
            }
        }
        else if (gameOverPhase == GameOverPhase.FadeOutRestart)
        {
            eplapesTime += Time.deltaTime;
            setAlphaColorUI.SetColorAlpha(fadeImage, (eplapesTime - 1) / 1);
            setAlphaColorUI.SetColorAlpha(titlePanel, (2 - eplapesTime) / 1f);
            setAlphaColorUI.SetColorAlpha(titlePanelText, (2 - eplapesTime) / 1);
            if (eplapesTime >= 2)
            {
                gameManager.OnNotify(this);
            }
        }
        else if (gameOverPhase == GameOverPhase.FadeOutExit)
        {
            eplapesTime += Time.deltaTime;
            setAlphaColorUI.SetColorAlpha(fadeImage, (eplapesTime - 1) / 1);
            setAlphaColorUI.SetColorAlpha(titlePanelText, (2 - eplapesTime) / 1f);
            setAlphaColorUI.SetColorAlpha(titlePanel, (2 - eplapesTime) / 1);
            if (eplapesTime >= 2)
            {
                gameManager.OnNotify(this);
            }
        }
    }
}
