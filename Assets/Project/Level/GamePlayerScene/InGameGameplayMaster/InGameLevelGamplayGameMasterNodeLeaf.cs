
using System;
using UnityEngine;

public class InGameLevelGamplayGameMasterNodeLeaf<T> : InGameLevelGameMasterNodeLeaf<T> where T : InGameLevelGameMaster
{
    protected GamePlayUICanvas gameplayCanvasUI => gameMaster.gamePlayUICanvas;
    protected UserInputActor user => gameMaster.user;
    protected Player player => gameMaster.player;

    protected PlayerInputAPI playerInputAPI;
    public GameManager gameManager { get => gameMaster.gameManager; set { } }

    public InGameLevelGamplayGameMasterNodeLeaf(T gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        
    }

    public override void Enter()
    {
        if(this.playerInputAPI == null)
            this.playerInputAPI  = player.GetComponent<PlayerInputAPI>();

        gameplayCanvasUI.EnableGameplayUI();

        Cursor.lockState = CursorLockMode.Locked;
        this.playerInputAPI.EnablePlayerInputAPI();
        gameMaster.NotifyObserver<InGameLevelGamplayGameMasterNodeLeaf<T>>(gameMaster,this);
    }

    public override void Exit()
    {
        gameplayCanvasUI.DisableGameplayUI();
        this.playerInputAPI.DisablePlayerInputAPI();
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
