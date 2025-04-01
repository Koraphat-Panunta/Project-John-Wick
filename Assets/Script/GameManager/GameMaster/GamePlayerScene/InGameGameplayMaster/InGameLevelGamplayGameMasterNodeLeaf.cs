
using System;
using UnityEngine;

public abstract class InGameLevelGamplayGameMasterNodeLeaf<T> : GameMasterNodeLeaf<T>,IGameManagerSendNotifyAble,IObserveObjective where T : InGameLevelGameMaster
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

        user.EnableInput();
    }

    public override void Exit()
    {
        gameplayCanvasUI.enabled=false;
        user.DisableInput();
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameMaster.isPause)
            {
                Time.timeScale = 1;
                gameMaster.isPause = false;
                pauseCanvasUI.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                user.EnableInput();
            }
            else
            {
                Time.timeScale = 0;
                gameMaster.isPause = true;
                pauseCanvasUI.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                user.DisableInput();
            }

        }

        if(gameMaster.isPause)
            { return; }

        if (gameMaster.isTriggerExit) 
        {
            gameManager.OnNotify(this);
        }


    }
    public virtual void GetNotifyObjectiveUpdate(Objective objective)
    {
        if(gameMaster.OnObjectiveUpdate != null)
            gameMaster.OnObjectiveUpdate(objective);
    }
   
}
