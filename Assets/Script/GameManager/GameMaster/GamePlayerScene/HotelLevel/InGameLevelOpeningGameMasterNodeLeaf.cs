using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameLevelOpeningGameMasterNodeLeaf : GameMasterNodeLeaf<InGameLevelGameMaster>
{
    private Canvas openingCanvasUI => gameMaster.openingCanvasUI;

    private Image titleHotelLevelImage => gameMaster.titleHotelLevelImage;
    private TextMeshProUGUI titleLevelHotel => gameMaster.titleLevelHotel;

    private Image fadeInImage => gameMaster.fadeInImage;

    private PlayerSpawner playerSpawner => gameMaster.playerSpawner;

    private bool isComplete;

    private SetAlphaColorUI setAlphaColorUI;
    public enum LevelHotelOpeningPhase
    {
        FadeIn,
        Stay,
        FadeOut
    }
    public LevelHotelOpeningPhase curPhase;

    private float eplapesTime;
    public InGameLevelOpeningGameMasterNodeLeaf(InGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        setAlphaColorUI = new SetAlphaColorUI();
    }

    public override void Enter()
    {
        isComplete = false;
        curPhase = LevelHotelOpeningPhase.FadeIn;
        openingCanvasUI.enabled = true;
        eplapesTime = 0;

        if(gameMaster.gameManager.curNodeLeaf is LevelMansionGameManagerNodeLeaf)
        gameMaster.gameManager.soundTrackManager.TriggerSoundTrack(gameMaster.gameManager.gamePlaySoundTrack);
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

    public override void UpdateNode()
    {
        eplapesTime += Time.deltaTime;
        if(curPhase == LevelHotelOpeningPhase.FadeIn)
        {
            setAlphaColorUI.SetColorAlpha<Image>(titleHotelLevelImage, eplapesTime / 0.5f);
            setAlphaColorUI.SetColorAlpha<TextMeshProUGUI>(titleLevelHotel, eplapesTime / 0.5f);

            if (eplapesTime >= 0.5f)
            {
                curPhase = LevelHotelOpeningPhase.Stay;
            }
        }
        else if(curPhase == LevelHotelOpeningPhase.Stay)
        {
            if (eplapesTime >= 1.5f)
            {
                curPhase = LevelHotelOpeningPhase.FadeOut;
                playerSpawner.SpawnPlayer();
            }
        }
        else if(curPhase == LevelHotelOpeningPhase.FadeOut)
        {
            setAlphaColorUI.SetColorAlpha(fadeInImage, (2-eplapesTime)/0.5f );
            if (eplapesTime >= 2)
            {
                gameMaster.StartCoroutine(FadeOutTitle());
                gameMaster.curLevelHotelPhase = InGameLevelGameMaster.LevelHotelPhase.Gameplay;
            }
        }
    }

    public IEnumerator FadeOutTitle()
    {
        
        while (eplapesTime < 3)
        {
            eplapesTime += Time.deltaTime;
            setAlphaColorUI.SetColorAlpha(this.titleLevelHotel, (3 - eplapesTime) / 1);
            setAlphaColorUI.SetColorAlpha(this.titleHotelLevelImage, (3 - eplapesTime) / 1);

            yield return null;
        }
        openingCanvasUI.enabled = false;
    }
}
