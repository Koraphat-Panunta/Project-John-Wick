
using System;
using UnityEngine;

public abstract class InGameLevelGamplayGameMasterNodeLeaf<T> : GameMasterNodeLeaf<T>,IObserveObjective where T : InGameLevelGameMaster
{
    protected GamePlayUICanvas gameplayCanvasUI => gameMaster.gamePlayUICanvas;
    protected User user => gameMaster.user;

    protected PauseUICanvas pauseCanvasUI => gameMaster.pauseCanvasUI;

    protected Player player => gameMaster.player;

    public GameManager gameManager { get => gameMaster.gameManager; set { } }

    public InGameLevelGamplayGameMasterNodeLeaf(T gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        
    }

    public override void Enter()
    {
        gameMaster.NotifyObserver(gameMaster);

        gameplayCanvasUI.EnableGameplayUI();

        Cursor.lockState = CursorLockMode.Locked;

        user.userInput.PlayerAction.Enable();
    }

    public override void Exit()
    {
        gameplayCanvasUI.enabled=false;
        user.userInput.PlayerAction.Disable();
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
    }
    public virtual void GetNotifyObjectiveUpdate(Objective objective)
    {
        if(gameMaster.OnObjectiveUpdate != null)
            gameMaster.OnObjectiveUpdate(objective);
    }
   
}
