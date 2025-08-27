using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LevelHotelGameMaster : InGameLevelGameMaster
{
    [SerializeField] private OpeningUICanvas openingUICanvas;
    [SerializeField] private GameOverUICanvas gameOverUICanvas;
    [SerializeField] private PauseUICanvas pauseCanvasUI;
    [SerializeField] private MissionCompleteUICanvas missionCompleteUICanvas;

    public List<Enemy> targetEliminationQuest;
    public Transform destination;

    protected override void Start()
    {
        if(gameManager != null)
            gameManager.soundTrackManager.PlaySoundTrack(gameManager.soundTrackManager.theHotelTrack);
        base.Start();
    }
    public InGameLevelOpeningGameMasterNodeLeaf levelOpeningGameMasterNodeLeaf { get ; protected set; }
    public InGameLevelMisstionCompleteGameMasterNodeLeaf levelMisstionCompleteGameMasterNodeLeaf { get ; protected set ; }
    public InGameLevelGameOverGameMasterNodeLeaf levelGameOverGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelRestGameMasterNodeLeaf levelRestGameMasterNodeLeaf { get; protected set; }
    public LevelHotelGameplayGameMasterNodeLeaf levelHotelGamePlayGameMasterNodeLeaf { get; protected set; }
    public PauseInGameGameMasterNodeLeaf pauseInGameGameMasterNodeLeaf { get; protected set; }
    public InGameLevelDelayOpeningLoad delayOpeningGameMasterNodeLeaf { get; protected set; }

    public override void InitailizedNode()
    {
        startNodeSelector = new GameMasterNodeSelector<InGameLevelGameMaster>(this, () => true);

        delayOpeningGameMasterNodeLeaf = new InGameLevelDelayOpeningLoad(this,()=> base.isCompleteLoad == false);
        levelOpeningGameMasterNodeLeaf = new InGameLevelOpeningGameMasterNodeLeaf(this,openingUICanvas, () => levelOpeningGameMasterNodeLeaf.isComplete == false);
        levelGameOverGameMasterNodeLeaf = new InGameLevelGameOverGameMasterNodeLeaf(this,gameOverUICanvas, () => player.isDead);
        levelHotelGamePlayGameMasterNodeLeaf = new LevelHotelGameplayGameMasterNodeLeaf(this, () => levelHotelGamePlayGameMasterNodeLeaf.isComplete == false);
        pauseInGameGameMasterNodeLeaf = new PauseInGameGameMasterNodeLeaf(this,pauseCanvasUI,
            () => pauseInGameGameMasterNodeLeaf.isPause );
        levelMisstionCompleteGameMasterNodeLeaf = new InGameLevelMisstionCompleteGameMasterNodeLeaf(this,missionCompleteUICanvas,()=> true);

        startNodeSelector.AddtoChildNode(delayOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelGameOverGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(pauseInGameGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelHotelGamePlayGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelMisstionCompleteGameMasterNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }
}
public class LevelHotelGameplayGameMasterNodeLeaf : InGameLevelGamplayGameMasterNodeLeaf<LevelHotelGameMaster>,IObserveObjective
{
    private Elimination eliminationObjective;
    private TravelingToDestination travelingToDestination;
    public bool isComplete { get; private set; }

    public Objective curObjective;
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

        curObjective = eliminationObjective;
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
            curObjective = eliminationObjective;
            if (curObjective.PerformedDone())
            {
                curPhase = Phase.travel;
            }
        }
        if(curPhase == Phase.travel)
        {
            curObjective = travelingToDestination;
            if (curObjective.PerformedDone())
            {
                isComplete = true;
            }
        }
        
        base.FixedUpdateNode();
    }

    public void GetNotifyObjectiveUpdate(Objective objective)
    {
        if(curPhase == Phase.eliminate 
            && objective is Elimination elimination)
        {
            if (elimination.status == Objective.ObjectiveStatus.Complete)
            {
                curPhase = Phase.travel;
                curObjective = travelingToDestination;
            }
        }
    }

    public override void UpdateNode()
    {
        
        base.UpdateNode();
    }
}
