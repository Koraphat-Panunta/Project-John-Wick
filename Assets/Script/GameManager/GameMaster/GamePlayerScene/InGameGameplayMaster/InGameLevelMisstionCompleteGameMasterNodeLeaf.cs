using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InGameLevelOpeningGameMasterNodeLeaf;

public class InGameLevelMisstionCompleteGameMasterNodeLeaf : GameMasterNodeLeaf<InGameLevelGameMaster>,IGameManagerSendNotifyAble
{

    private User user => gameMaster.user;
    private MissionCompleteUICanvas misstionCompleteUICanvas => gameMaster.missionCompleteUICanvas;

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
    public InGameLevelMisstionCompleteGameMasterNodeLeaf(InGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        setAlphaColorUI = new SetAlphaColorUI();
    }

    public override void Enter()
    {
        curPhase = MissionCompletePhase.FadeIn;
        eplapesTime = 0f;
        notifyAlready = false;
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
            if (gameMaster.isTriggerContinue){
                curPhase = MissionCompletePhase.FadeOutContinue;
                misstionCompleteUICanvas.PlayFadeOut();
                return;
            }
            if (gameMaster.isTriggerRestart){
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
                gameManager.OnNotify(this);
                notifyAlready = true;
            }
        }
        else if(curPhase == MissionCompletePhase.FadeOutContinue)
        {
            eplapesTime += Time.deltaTime;
            if (eplapesTime >= 2 && notifyAlready == false)
            {
                gameManager.OnNotify(this);
                notifyAlready = true;
            }
        }
    }
}
