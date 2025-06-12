using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LevelHotelGameMaster : InGameLevelGameMaster
{

    public List<Enemy> targetEliminationQuest;
    public Transform destination;

    protected override void Start()
    {
        if(gameManager != null)
            gameManager.soundTrackManager.PlaySoundTrack(gameManager.soundTrackManager.theHotelTrack);
        base.Start();
    }
   

   
    public LevelHotelDelayOpeningGameMasterNodeLeaf levelHotelDelayOpeningGameMasterNodeLeaf { get; private set; }
    public override InGameLevelOpeningGameMasterNodeLeaf levelOpeningGameMasterNodeLeaf { get ; protected set; }
    public override InGameLevelMisstionCompleteGameMasterNodeLeaf levelMisstionCompleteGameMasterNodeLeaf { get ; protected set ; }
    public override InGameLevelGameOverGameMasterNodeLeaf levelGameOverGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelRestGameMasterNodeLeaf levelRestGameMasterNodeLeaf { get; protected set; }
    public LevelHotelGameplayGameMasterNodeLeaf levelHotelGamePlayGameMasterNodeLeaf { get; protected set; }
    public override PauseInGameGameMasterNodeLeaf pauseInGameGameMasterNodeLeaf { get; protected set; }
    public override void InitailizedNode()
    {
        startNodeSelector = new GameMasterNodeSelector<InGameLevelGameMaster>(this, () => true);

        levelHotelDelayOpeningGameMasterNodeLeaf = new LevelHotelDelayOpeningGameMasterNodeLeaf(this,()=> base.isCompleteLoad == false);
        levelOpeningGameMasterNodeLeaf = new InGameLevelOpeningGameMasterNodeLeaf(this, () => levelOpeningGameMasterNodeLeaf.isComplete == false);
        levelGameOverGameMasterNodeLeaf = new InGameLevelGameOverGameMasterNodeLeaf(this,gameOverUICanvas, () => player.isDead);
        levelHotelGamePlayGameMasterNodeLeaf = new LevelHotelGameplayGameMasterNodeLeaf(this, () => levelHotelGamePlayGameMasterNodeLeaf.isComplete == false);
        pauseInGameGameMasterNodeLeaf = new PauseInGameGameMasterNodeLeaf(this,pauseCanvasUI,
            () => pauseInGameGameMasterNodeLeaf.isPause );
        levelMisstionCompleteGameMasterNodeLeaf = new InGameLevelMisstionCompleteGameMasterNodeLeaf(this,missionCompleteUICanvas,()=> true);

        startNodeSelector.AddtoChildNode(levelOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelGameOverGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(pauseInGameGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelHotelGamePlayGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelMisstionCompleteGameMasterNodeLeaf);

        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf;
        curNodeLeaf.Enter();
    }
}
public class LevelHotelGameplayGameMasterNodeLeaf : InGameLevelGamplayGameMasterNodeLeaf<LevelHotelGameMaster>
{
    private Elimination eliminationObjective;
    private TravelingToDestination travelingToDestination;
    public bool isComplete { get; private set; }
    private enum Phase
    {
        eliminate,
        travel
    }
    private Phase curPhase;
    public LevelHotelGameplayGameMasterNodeLeaf(LevelHotelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }

    public override void Enter()
    {
        eliminationObjective = new Elimination(gameMaster.targetEliminationQuest);
        travelingToDestination = new TravelingToDestination(player.transform,gameMaster.destination.transform.position);
        curPhase = Phase.eliminate;

        gameMaster.curObjective = eliminationObjective;
        eliminationObjective.AddNotifyUpdateObjective(this);
        
        isComplete = false;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        if(curPhase == Phase.eliminate)
        {
            gameMaster.curObjective = eliminationObjective;
            if (gameMaster.curObjective.PerformedDone())
            {
                curPhase = Phase.travel;
            }
        }
        if(curPhase == Phase.travel)
        {
            gameMaster.curObjective = travelingToDestination;
            if (gameMaster.curObjective.PerformedDone())
            {
                isComplete = true;
            }
        }
        
        base.FixedUpdateNode();
    }

    public override void GetNotifyObjectiveUpdate(Objective objective)
    {
        if(curPhase == Phase.eliminate 
            && objective is Elimination elimination)
        {
            if (elimination.status == Objective.ObjectiveStatus.Complete)
            {
                curPhase = Phase.travel;
                gameMaster.curObjective = travelingToDestination;
            }
        }
        base.GetNotifyObjectiveUpdate(objective);
    }

    public override void UpdateNode()
    {
        
        base.UpdateNode();
    }
}
public class LevelHotelDelayOpeningGameMasterNodeLeaf : GameMasterNodeLeaf<LevelHotelGameMaster>
{
    public LevelHotelDelayOpeningGameMasterNodeLeaf(LevelHotelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }

    public override void Enter()
    {
        gameMaster.gamePlayUICanvas.DisableGameplayUI();
    }

    public override void Exit()
    {
       
    }

    public override void FixedUpdateNode()
    {
       
    }

    public override bool IsComplete()
    {
        throw new NotImplementedException();
    }

    public override void UpdateNode()
    {
        
    }
}
