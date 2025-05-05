
using System;
using System.Threading.Tasks;

public class InGameLevelOpeningGameMasterNodeLeaf : GameMasterNodeLeaf<InGameLevelGameMaster>
{
    private OpeningUICanvas openingCanvasUI => gameMaster.openingUICanvas;
    public bool isComplete { get; protected set; }
    private Player player => gameMaster.player;
    public InGameLevelOpeningGameMasterNodeLeaf(InGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {

    }

    public override void Enter()
    {
        //gameMaster.player.gameObject.SetActive(false);
        gameMaster.gamePlayUICanvas.DisableGameplayUI();


        isComplete = false;
        openingCanvasUI.PlayOpeningAnimationUI();

        gameMaster.NotifyObserver(gameMaster);

        DisableInput();

        OpeningDelay();
    }

    public override void Exit()
    {
        EnableInput();

        player.gameObject.SetActive(true);
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
        gameMaster.openingUICanvas.gameObject.SetActive(false);
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
