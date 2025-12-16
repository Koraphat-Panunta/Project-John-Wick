using System;
using UnityEngine;

public class InGameLevelCompleteGameMasterNodeLeaf : InGameLevelGameMasterNodeLeaf<InGameLevelGameMaster>
{

    private UserActor user => gameMaster.user;
    private LevelCompleteUICanvas misstionCompleteUICanvas;

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
    public MissionCompletePhase curMissionCompletePhase;
    public InGameLevelCompleteGameMasterNodeLeaf(InGameLevelGameMaster gameMaster,LevelCompleteUICanvas missionCompleteUICanvas, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        this.misstionCompleteUICanvas = missionCompleteUICanvas;

        this.misstionCompleteUICanvas.continueButton.onClick.AddListener(() => TriggerContinue());
        this.misstionCompleteUICanvas.restartButton.onClick.AddListener(() => TriggerRestart());
    }

    public override void Enter()
    {
        curMissionCompletePhase = MissionCompletePhase.FadeIn;
        eplapesTime = 0f;
        notifyAlready = false;
        isTriggerContinue = false;
        isTriggerRestart = false;

        misstionCompleteUICanvas.gameObject.SetActive(true);
        misstionCompleteUICanvas.PlayFadeIn();
        //gameManager.soundTrackManager.StopSoundTrack(2);

        gameMaster.NotifyObserver(gameMaster,this);
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
        
        if (curMissionCompletePhase == MissionCompletePhase.FadeIn)
        {
            eplapesTime += Time.deltaTime;

            if (eplapesTime >= 1f)
            {
                curMissionCompletePhase = MissionCompletePhase.Stay;
                user.DisableInput();
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else if (curMissionCompletePhase == MissionCompletePhase.Stay)
        {
            if (isTriggerContinue){
                curMissionCompletePhase = MissionCompletePhase.FadeOutContinue;
                misstionCompleteUICanvas.PlayFadeOut();
                return;
            }
            if (isTriggerRestart){
                curMissionCompletePhase = MissionCompletePhase.FadeOutRestart;
                misstionCompleteUICanvas.PlayFadeOut();
                return;
            }
        }
        else if (curMissionCompletePhase == MissionCompletePhase.FadeOutRestart)
        {
            eplapesTime += Time.deltaTime;
            if (eplapesTime >= 2 && notifyAlready == false)
            {
                gameManager.RestartScene();
                notifyAlready = true;
            }
        }
        else if(curMissionCompletePhase == MissionCompletePhase.FadeOutContinue)
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
