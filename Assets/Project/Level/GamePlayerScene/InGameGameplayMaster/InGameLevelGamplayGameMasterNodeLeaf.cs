
using System;
using UnityEngine;

public class InGameLevelGamplayGameMasterNodeLeaf<T> : InGameLevelGameMasterNodeLeaf<T> where T : InGameLevelGameMaster
{
    protected GamePlayUICanvas gameplayCanvasUI => gameMaster.gamePlayUICanvas;
    protected User user => gameMaster.user;

    protected Player player => gameMaster.player;
    public GameManager gameManager { get => gameMaster.gameManager; set { } }

    public InGameLevelGamplayGameMasterNodeLeaf(T gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        
    }

    public override void Enter()
    {


        gameplayCanvasUI.EnableGameplayUI();

        Cursor.lockState = CursorLockMode.Locked;

        user.userInput.PlayerAction.Enable();
        gameMaster.NotifyObserver<InGameLevelGamplayGameMasterNodeLeaf<T>>(gameMaster,this);
    }

    public override void Exit()
    {
        gameplayCanvasUI.enabled=false;
        user.userInput.PlayerAction.Disable();
        gameMaster.NotifyObserver<InGameLevelGamplayGameMasterNodeLeaf<T>>(gameMaster, this);
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
    public virtual void RestartCheckPoint() { }

   
}
