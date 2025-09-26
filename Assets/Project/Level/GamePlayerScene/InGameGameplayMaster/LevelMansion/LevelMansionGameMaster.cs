using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelMansionGameMaster : InGameLevelGameMaster
{
    [SerializeField] private OpeningUICanvas openingUICanvas;
    [SerializeField] private GameOverUICanvas gameOverUICanvas;
    [SerializeField] private PauseUICanvas pauseCanvasUI;
    [SerializeField] private MissionCompleteUICanvas missionCompleteUICanvas;

    public InGameLevelOpeningGameMasterNodeLeaf levelOpeningGameMasterNodeLeaf { get; protected set; }
    public LevelMansionGamePlaySequence1 levelMansionGamePlaySequence1 { get; protected set; }
    public InGameLevelMisstionCompleteGameMasterNodeLeaf levelMisstionCompleteGameMasterNodeLeaf { get; protected set; }
    public InGameLevelGameOverGameMasterNodeLeaf levelGameOverGameMasterNodeLeaf { get; protected set; }
    public override PauseInGameGameMasterNodeLeaf pauseInGameGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelRestGameMasterNodeLeaf levelRestGameMasterNodeLeaf { get; protected set; }
    public InGameLevelDelayOpeningLoad delayOpeningGameMasterNodeLeaf { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

    public List<Enemy> target;

    public Transform reaceDestinate;

    protected override void Start()
    {
        gameManager.soundTrackManager.PlaySoundTrack(gameManager.soundTrackManager.theMansionTrack);
        base.Start();
    }
  
    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true);

        this.levelOpeningGameMasterNodeLeaf = new InGameLevelOpeningGameMasterNodeLeaf(this, openingUICanvas, () => levelOpeningGameMasterNodeLeaf.isComplete == false);
        this.pauseInGameGameMasterNodeLeaf = new PauseInGameGameMasterNodeLeaf(this, pauseCanvasUI, () => this.pauseInGameGameMasterNodeLeaf.isPause);
        this.levelMansionGamePlaySequence1 = new LevelMansionGamePlaySequence1(this, () => this.levelMansionGamePlaySequence1.isComplete == false);
        this.levelMisstionCompleteGameMasterNodeLeaf = new InGameLevelMisstionCompleteGameMasterNodeLeaf(this, missionCompleteUICanvas, () => true);
        this.levelGameOverGameMasterNodeLeaf = new InGameLevelGameOverGameMasterNodeLeaf(this, gameOverUICanvas, () => player.isDead);
        this.levelRestGameMasterNodeLeaf = new InGameLevelRestGameMasterNodeLeaf(this, () => true);

        startNodeSelector.AddtoChildNode(this.levelOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(this.pauseInGameGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(this.levelMansionGamePlaySequence1);
        startNodeSelector.AddtoChildNode(this.levelMisstionCompleteGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(this.levelGameOverGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(this.levelRestGameMasterNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }

  

    
}
public class LevelMansionGamePlaySequence1 : InGameLevelGamplayGameMasterNodeLeaf<LevelMansionGameMaster>, IObserveObjective
{
    protected Elimination elimination;
    protected TravelingToDestination destination;
    public bool isComplete { get; private set; }
    private Objective curObjective;
    public LevelMansionGamePlaySequence1(LevelMansionGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        elimination = new Elimination(gameMaster.target);
        destination = new TravelingToDestination(player.transform, gameMaster.reaceDestinate.position);
        elimination.AddNotifyUpdateObjective(this);

        isComplete = false;
    }
    public override void Enter()
    {
        curObjective = elimination;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        if (destination.PerformedDone() && curObjective == destination)
        {
            destination.RemoveNotifyUpdateObjective(this);
            isComplete = true;
        }
        base.FixedUpdateNode();
    }

    public void GetNotifyObjectiveUpdate(Objective objective)
    {
        Debug.Log("GetNotifyObjectiveUpdate");
        switch (objective)
        {
            case Elimination elimination:
                {
                    Debug.Log("GetNotifyObjectiveUpdate Elimination");
                    if (elimination != this.elimination)
                        throw new Exception(" Unmatch objective Notify ");

                    if (elimination.status == Objective.ObjectiveStatus.Complete)
                    {
                        curObjective = destination;
                        elimination.RemoveNotifyUpdateObjective(this);

                        destination.AddNotifyUpdateObjective(this);
                    }

                }
                break;

        }

    }

    public override bool IsComplete()
    {
        return isComplete;
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }

    public override void RestartCheckPoint()
    {
        throw new NotImplementedException();
    }
}
public class LevelMansionGameOlaySequence2 : InGameLevelGamplayGameMasterNodeLeaf<LevelMansionGameMaster>
{
    public LevelMansionGameOlaySequence2(LevelMansionGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }


    public override bool IsComplete()
    {
        return base.IsComplete();
    }

    public override void RestartCheckPoint()
    {
        throw new NotImplementedException();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
