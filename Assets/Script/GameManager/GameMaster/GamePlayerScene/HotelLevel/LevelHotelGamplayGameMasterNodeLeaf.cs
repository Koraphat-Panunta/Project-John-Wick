using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelHotelGamplayGameMasterNodeLeaf : GameMasterNodeLeaf<LevelHotelGameMaster>
{
    private Canvas gameplayCanvasUI => gameMaster.gameplayCanvasUI;
    private CrosshairController crosshair => gameMaster.crosshairController;
    private RawImage hpBar => gameMaster.hpBar;
    private TextMeshProUGUI weaponInfo => gameMaster.weaponInfo;
    private User user => gameMaster.user;

    private ObjectiveManager objectiveManager => gameMaster.objectiveManager;
    private Elimination eliminationObjective;
    private TravelingToDestination travelingToDestination;

    private Player player => gameMaster.player;
    public LevelHotelGamplayGameMasterNodeLeaf(LevelHotelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        
    }

    public override void Enter()
    {
        this.eliminationObjective = new Elimination(gameMaster.targetEliminationQuest);
        this.travelingToDestination = new TravelingToDestination(player.transform, gameMaster.destination.position);

        this.objectiveManager.AddObjective(this.eliminationObjective);
        this.objectiveManager.AddObjective(this.travelingToDestination);

        gameplayCanvasUI.enabled = true;
        crosshair.EnableCrosshairVisable();
        hpBar.enabled = true;
        weaponInfo.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;

        this.objectiveManager.StartPlayObjective();
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
        if (objectiveManager.allQuestClear){
            gameMaster.curLevelHotelPhase = LevelHotelGameMaster.LevelHotelPhase.MissionComplete;
            return;
        }

        if (player.isDead) {
            gameMaster.curLevelHotelPhase = LevelHotelGameMaster.LevelHotelPhase.GameOver;
            return;
        }

        
    }
}
