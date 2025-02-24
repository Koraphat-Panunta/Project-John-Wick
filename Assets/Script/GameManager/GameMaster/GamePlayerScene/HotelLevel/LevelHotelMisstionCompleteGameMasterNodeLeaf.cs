using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LevelHotelOpeningGameMasterNodeLeaf;

public class LevelHotelMisstionCompleteGameMasterNodeLeaf : GameMasterNodeLeaf<LevelHotelGameMaster>,IGameManagerSendNotifyAble
{

    private User user => gameMaster.user;
    private Canvas misstionCompleteCanvasUI => gameMaster.missionCompleteCanvasUI;
    private Image fadeImage => gameMaster.missionCompleteImageFade;
    private Image titleImagePanel => gameMaster.misstionCompletePanelTitle;
    private TextMeshProUGUI titleText => gameMaster.misstionCompleteTitle;
    private Button continueButton => gameMaster.misstionCompleteContinueButton;
    private Button restartButton => gameMaster.misstionCompleteRestartButton;

    public GameManager gameManager { get => gameMaster.gameManager; set { } }

    private bool notifyAlready;

    private float eplapesTime;
    private SetAlphaColorUI setAlphaColorUI;

    public enum MissionCompletePhase
    {
        FadeIn,
        Stay,
        FadeOutRestart,
        FadeOutContinue
    }
    public MissionCompletePhase curPhase;
    public LevelHotelMisstionCompleteGameMasterNodeLeaf(LevelHotelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        setAlphaColorUI = new SetAlphaColorUI();
    }

    public override void Enter()
    {
        curPhase = MissionCompletePhase.FadeIn;
        eplapesTime = 0f;
        notifyAlready = false;
        misstionCompleteCanvasUI.gameObject.SetActive(true);
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
        
        if (curPhase == MissionCompletePhase.FadeIn)
        {
            eplapesTime += Time.deltaTime;
            Debug.Log("Opening eplapesTime fadeIn = " + eplapesTime);
            setAlphaColorUI.SetColorAlpha<Image>(titleImagePanel, eplapesTime / 1f);
            setAlphaColorUI.SetColorAlpha<TextMeshProUGUI>(titleText, eplapesTime / 1f);

            if (eplapesTime >= 1f)
            {
                curPhase = MissionCompletePhase.Stay;
                continueButton.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
                user.DisableInput();
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else if (curPhase == MissionCompletePhase.Stay)
        {
            if (gameMaster.isTriggerContinue){
                curPhase = MissionCompletePhase.FadeOutContinue;
                return;
            }
            if (gameMaster.isTriggerRestart){
                curPhase = MissionCompletePhase.FadeOutRestart;
                return;
            }
        }
        else if (curPhase == MissionCompletePhase.FadeOutRestart)
        {
            eplapesTime += Time.deltaTime;
            setAlphaColorUI.SetColorAlpha(fadeImage, (eplapesTime - 1) / 1);
            setAlphaColorUI.SetColorAlpha(titleImagePanel, (2 - eplapesTime) / 1f);
            setAlphaColorUI.SetColorAlpha(titleText, (2 - eplapesTime) / 1);
            if (eplapesTime >= 2 && notifyAlready == false)
            {
                gameManager.OnNotify(this);
                notifyAlready = true;
            }
        }
        else if(curPhase == MissionCompletePhase.FadeOutContinue)
        {
            eplapesTime += Time.deltaTime;
            setAlphaColorUI.SetColorAlpha(fadeImage, (eplapesTime - 1) / 1);
            setAlphaColorUI.SetColorAlpha(titleImagePanel, (2 - eplapesTime) / 1f);
            setAlphaColorUI.SetColorAlpha(titleText, (2 - eplapesTime) / 1);
            if (eplapesTime >= 2 && notifyAlready == false)
            {
                gameManager.OnNotify(this);
                notifyAlready = true;
            }
        }
    }
}
