using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InGameLevelOpeningGameMasterNodeLeaf;

public class InGameLevelMisstionCompleteGameMasterNodeLeaf : GameMasterNodeLeaf<InGameLevelGameMaster>
{

    private User user => gameMaster.user;
    private MissionCompleteUICanvas misstionCompleteUICanvas;

    public GameManager gameManager { get => gameMaster.gameManager; set { } }

    private bool notifyAlready;

    private float eplapesTime;

    private bool isTriggerRestart;
    private bool isTriggerContinue;

    public enum MissionCompletePhase
    {
        FadeIn,
        Stay,
        FadeOutRestart,
        FadeOutContinue
    }
    public MissionCompletePhase curPhase;
    public InGameLevelMisstionCompleteGameMasterNodeLeaf(InGameLevelGameMaster gameMaster,MissionCompleteUICanvas missionCompleteUICanvas, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        this.misstionCompleteUICanvas = missionCompleteUICanvas;

        this.misstionCompleteUICanvas.continueButton.onClick.AddListener(() => TriggerContinue());
        this.misstionCompleteUICanvas.restartButton.onClick.AddListener(() => TriggerRestart());
    }

    public override void Enter()
    {
        curPhase = MissionCompletePhase.FadeIn;
        eplapesTime = 0f;
        notifyAlready = false;
        isTriggerContinue = false;
        isTriggerRestart = false;

        misstionCompleteUICanvas.gameObject.SetActive(true);
        misstionCompleteUICanvas.PlayFadeIn();
        gameManager.soundTrackManager.StopSoundTrack(2);

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
    public void TriggerContinue() => isTriggerContinue = true;
  
    public void TriggerRestart() => isTriggerRestart = true;    
   
    public override void UpdateNode()
    {
        
        if (curPhase == MissionCompletePhase.FadeIn)
        {
            eplapesTime += Time.deltaTime;
            Debug.Log("Opening eplapesTime fadeIn = " + eplapesTime);

            if (eplapesTime >= 1f)
            {
                curPhase = MissionCompletePhase.Stay;
                user.DisableInput();
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else if (curPhase == MissionCompletePhase.Stay)
        {
            if (isTriggerContinue){
                curPhase = MissionCompletePhase.FadeOutContinue;
                misstionCompleteUICanvas.PlayFadeOut();
                return;
            }
            if (isTriggerRestart){
                curPhase = MissionCompletePhase.FadeOutRestart;
                misstionCompleteUICanvas.PlayFadeOut();
                return;
            }
        }
        else if (curPhase == MissionCompletePhase.FadeOutRestart)
        {
            eplapesTime += Time.deltaTime;
            if (eplapesTime >= 2 && notifyAlready == false)
            {
                gameManager.RestartScene();
                notifyAlready = true;
            }
        }
        else if(curPhase == MissionCompletePhase.FadeOutContinue)
        {
            eplapesTime += Time.deltaTime;
            if (eplapesTime >= 2 && notifyAlready == false)
            {
                gameManager.ContinueGameplayScene();
                notifyAlready = true;
            }
        }
    }
}
