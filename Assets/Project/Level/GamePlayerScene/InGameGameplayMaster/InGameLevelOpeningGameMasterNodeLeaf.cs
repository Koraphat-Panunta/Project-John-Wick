
using System;
using System.Threading.Tasks;

public class InGameLevelOpeningGameMasterNodeLeaf : InGameLevelGameMasterNodeLeaf<InGameLevelGameMaster>
{
    private OpeningUICanvas openingCanvasUI;
    private Player player => gameMaster.player;
    public InGameLevelOpeningGameMasterNodeLeaf(InGameLevelGameMaster gameMaster,OpeningUICanvas openingUICanvas, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        this.openingCanvasUI = openingUICanvas;
    }

    public override void Enter()
    {
        gameMaster.gamePlayUICanvas.DisableGameplayUI();


        isComplete = false;
        openingCanvasUI.PlayOpeningAnimationUI();

        DisableInput();

        OpeningDelay();

        gameMaster.NotifyObserver<InGameLevelOpeningGameMasterNodeLeaf>(gameMaster, this);
    }

    public override void Exit()
    {
        EnableInput();

        player.gameObject.SetActive(true);

        gameMaster.NotifyObserver<InGameLevelOpeningGameMasterNodeLeaf>(gameMaster, this);
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
    protected virtual async void  OpeningDelay()
    {
        await Task.Delay(3000);
        gameMaster.gamePlayUICanvas.EnableGameplayUI();
        this.isComplete = true;
        await Task.Delay(6000);
        try
        {
            openingCanvasUI.gameObject.SetActive(false);
        }
        catch
        {
            throw new Exception("gameMaster.openingUICanvas been destroy");
        }
    }

    private void DisableInput()
    {
        gameMaster.user.userInput.PauseAction.Disable();
        gameMaster.user.userInput.PlayerAction.Disable();
    }
    private void EnableInput()
    {
        gameMaster.user.userInput.PauseAction.Enable();
        gameMaster.user.userInput.PlayerAction.Enable();
    }
  
}
