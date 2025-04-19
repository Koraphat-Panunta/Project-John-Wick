using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Animations;
using UnityEngine;

public class LevelHotelGameMaster : InGameLevelGameMaster
{

    public List<Enemy> targetEliminationQuest;
    public Transform destination;

    private bool isCompleteLoad = false;

    public async void DelaySceneLoaded()
    {
        await Task.Delay(1700);
        isCompleteLoad = true;
    }

   
    public LevelHotelDelayOpeningGameMasterNodeLeaf levelHotelDelayOpeningGameMasterNodeLeaf { get; private set; }
    public override InGameLevelOpeningGameMasterNodeLeaf levelOpeningGameMasterNodeLeaf { get ; protected set; }
    public override InGameLevelMisstionCompleteGameMasterNodeLeaf levelMisstionCompleteGameMasterNodeLeaf { get ; protected set ; }
    public override InGameLevelGameOverGameMasterNodeLeaf levelGameOverGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelRestGameMasterNodeLeaf levelRestGameMasterNodeLeaf { get; protected set; }
    public LevelHotelGameplayGameMasterNodeLeaf levelHotelGamePlayGameMasterNodeLeaf { get; protected set; }

    public override void InitailizedNode()
    {
        startNodeSelector = new GameMasterNodeSelector<InGameLevelGameMaster>(this, () => true);

        levelHotelDelayOpeningGameMasterNodeLeaf = new LevelHotelDelayOpeningGameMasterNodeLeaf(this,()=> isCompleteLoad == false);
        levelOpeningGameMasterNodeLeaf = new InGameLevelOpeningGameMasterNodeLeaf(this, () => levelOpeningGameMasterNodeLeaf.isComplete == false);
        levelGameOverGameMasterNodeLeaf = new InGameLevelGameOverGameMasterNodeLeaf(this, () => player.isDead);
        levelHotelGamePlayGameMasterNodeLeaf = new LevelHotelGameplayGameMasterNodeLeaf(this, () => levelHotelGamePlayGameMasterNodeLeaf.isComplete == false);
        levelMisstionCompleteGameMasterNodeLeaf = new InGameLevelMisstionCompleteGameMasterNodeLeaf(this,()=> true);

        startNodeSelector.AddtoChildNode(levelOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelGameOverGameMasterNodeLeaf);
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
        if(gameMaster.curObjective == travelingToDestination)
        {
            if (travelingToDestination.PerformedDone())
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
        gameMaster.DelaySceneLoaded();
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
