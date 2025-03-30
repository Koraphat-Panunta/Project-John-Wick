
using System;
using UnityEngine;

public class InGameLevelGamplayGameMasterNodeLeaf : GameMasterNodeLeaf<InGameLevelGameMaster>,IGameManagerSendNotifyAble,IObserveObjective
{
    private GamePlayUICanvas gameplayCanvasUI => gameMaster.gamePlayUICanvas;
    private CrosshairController crosshair => gameMaster.crosshairController;
    private User user => gameMaster.user;

    private PauseUICanvas pauseCanvasUI => gameMaster.pauseCanvasUI; 

    private ObjectiveManager objectiveManager => gameMaster.objectiveManager;
    private Elimination eliminationObjective;
    private TravelingToDestination travelingToDestination;

    private Player player => gameMaster.player;

    public GameManager gameManager { get => gameMaster.gameManager; set { } }

    public InGameLevelGamplayGameMasterNodeLeaf(InGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        
    }

    public override void Enter()
    {
        gameMaster.NotifyObserver(gameMaster);

        this.eliminationObjective = new Elimination(gameMaster.targetEliminationQuest);
        this.eliminationObjective.AddNotifyUpdateObjective(this);
        this.travelingToDestination = new TravelingToDestination(player.transform, gameMaster.destination.position);

        this.objectiveManager.AddObjective(this.eliminationObjective);
        this.objectiveManager.AddObjective(this.travelingToDestination);

        gameplayCanvasUI.enabled = true;
        crosshair.EnableCrosshairVisable();

        Cursor.lockState = CursorLockMode.Locked;

        this.objectiveManager.StartPlayObjective();
        user.EnableInput();
    }

    public override void Exit()
    {
        this.eliminationObjective.RemoveNotifyUpdateObjective(this);
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

        if (objectiveManager.allQuestClear){
            gameMaster.curLevelHotelPhase = InGameLevelGameMaster.LevelHotelPhase.MissionComplete;
            return;
        }

        if (player.isDead) {
            gameMaster.curLevelHotelPhase = InGameLevelGameMaster.LevelHotelPhase.GameOver;
            return;
        }

        if (gameMaster.isTriggerExit) 
        {
            gameManager.OnNotify(this);
        }


    }
    public void GetNotifyObjectiveUpdate(Objective objective)
    {
        if (objective is Elimination eliminate)
            if (eliminate.status == Objective.ObjectiveStatus.Complete)
                this.eliminationObjective.RemoveNotifyUpdateObjective(this);

        if(objective is Elimination elimination && gameManager.curNodeLeaf is LevelHotelGameManagerNodeLeaf levelHotel)
        {
            if(levelHotel.isPlayMusic == false)
            {
                gameManager.soundTrackManager.TriggerSoundTrack(gameManager.gamePlaySoundTrack);
                levelHotel.isPlayMusic = true;
            }
        }
    }
}
