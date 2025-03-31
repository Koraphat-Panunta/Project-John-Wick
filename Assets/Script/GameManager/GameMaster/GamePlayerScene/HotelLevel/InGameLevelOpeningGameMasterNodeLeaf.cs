using NUnit.Framework;
using System;
using System.Threading.Tasks;

public class InGameLevelOpeningGameMasterNodeLeaf : GameMasterNodeLeaf<InGameLevelGameMaster>
{
    private OpeningUICanvas openingCanvasUI => gameMaster.openingUICanvas;
    private bool isComplete;
    private Player player => gameMaster.player;
    public InGameLevelOpeningGameMasterNodeLeaf(InGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {

    }

    public override void Enter()
    {
        gameMaster.player.gameObject.SetActive(false);
        gameMaster.gamePlayUICanvas.DisableGameplayUI();


        isComplete = false;
        openingCanvasUI.PlayOpeningAnimationUI();

        gameMaster.NotifyObserver(gameMaster);

        if (gameMaster.gameManager != null)
        {
            if (gameMaster.gameManager.curNodeLeaf is LevelMansionGameManagerNodeLeaf)
                gameMaster.gameManager.soundTrackManager.TriggerSoundTrack(gameMaster.gameManager.gamePlaySoundTrack);
        }

        OpeningDelay();
    }

    public override void Exit()
    {
        player.gameObject.SetActive(true);
        gameMaster.gamePlayUICanvas.EnableGameplayUI();
    }

    public override void FixedUpdateNode()
    {
       
    }

    public override bool IsReset()
    {
        if(IsComplete())
            return true;

        return base.IsReset();
    }

    public override bool IsComplete()
    {
        return isComplete;
    }

    public override void UpdateNode()
    {
    }
    private async void  OpeningDelay()
    {
        await Task.Delay(1000);
        gameMaster.curLevelHotelPhase = InGameLevelGameMaster.LevelHotelPhase.Gameplay;
        this.isComplete = true;
    }
  
}
