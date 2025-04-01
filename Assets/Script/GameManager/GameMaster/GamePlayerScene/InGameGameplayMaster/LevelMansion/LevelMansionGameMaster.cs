using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelMansionGameMaster : InGameLevelGameMaster
{
    public override InGameLevelOpeningGameMasterNodeLeaf levelOpeningGameMasterNodeLeaf { get; protected set; }
    public LevelMansionGamePlaySequence1 levelMansionGamePlaySequence1 { get; protected set; }
    public override InGameLevelMisstionCompleteGameMasterNodeLeaf levelMisstionCompleteGameMasterNodeLeaf { get ; protected set ; }
    public override InGameLevelGameOverGameMasterNodeLeaf levelGameOverGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelRestGameMasterNodeLeaf levelRestGameMasterNodeLeaf { get; protected set; }

    public List<Enemy> target;

    protected override void Awake()
    {
        curLevelMansionPhase = LevelMansionPhase.Gameplay1;
        base.Awake();
    }
    public enum LevelMansionPhase
    {
        Gameplay1,
        MissionComplete,
        MissionFailed
    }
    public LevelMansionPhase curLevelMansionPhase;
    public override void InitailizedNode()
    {
        startNodeSelector = new GameMasterNodeSelector<LevelMansionGameMaster>(this, () => true);

        this.levelOpeningGameMasterNodeLeaf = new InGameLevelOpeningGameMasterNodeLeaf(this,()=> levelOpeningGameMasterNodeLeaf.isComplete == false);
        this.levelMansionGamePlaySequence1 = new LevelMansionGamePlaySequence1(this,()=> curLevelMansionPhase == LevelMansionPhase.Gameplay1);
        this.levelMisstionCompleteGameMasterNodeLeaf = new InGameLevelMisstionCompleteGameMasterNodeLeaf(this, () => curLevelMansionPhase == LevelMansionPhase.MissionComplete);
        this.levelGameOverGameMasterNodeLeaf = new InGameLevelGameOverGameMasterNodeLeaf(this, () => curLevelMansionPhase == LevelMansionPhase.MissionFailed);
        this.levelRestGameMasterNodeLeaf = new InGameLevelRestGameMasterNodeLeaf(this, () => true);

        startNodeSelector.AddtoChildNode(this.levelOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(this.levelMansionGamePlaySequence1);
        startNodeSelector.AddtoChildNode(this.levelMisstionCompleteGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(this.levelGameOverGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(this.levelRestGameMasterNodeLeaf);

        startNodeSelector.FindingNode(out INodeLeaf curNodeLeaf);
        this.curNodeLeaf = curNodeLeaf;
        this.curNodeLeaf.Enter();
    }
}
public class LevelMansionGamePlaySequence1 : InGameLevelGamplayGameMasterNodeLeaf<LevelMansionGameMaster>
{
    protected Elimination elimination;
    public LevelMansionGamePlaySequence1(LevelMansionGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        elimination = new Elimination(gameMaster.target);
        elimination.AddNotifyUpdateObjective(this);


    }
    public override void Enter()
    {
        gameMaster.curObjective = elimination;
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

    public override void GetNotifyObjectiveUpdate(Objective objective)
    {
        Debug.Log("GetNotifyObjectiveUpdate");
        switch (objective)
        {
            case Elimination elimination: 
                {
                    Debug.Log("GetNotifyObjectiveUpdate Elimination");
                    if (elimination != this.elimination)
                        throw new Exception(" Unmatch objective Notify ");

                    if(elimination.status == Objective.ObjectiveStatus.Complete)
                    {
                        elimination.RemoveNotifyUpdateObjective(this);
                        gameMaster.curLevelMansionPhase = LevelMansionGameMaster.LevelMansionPhase.MissionComplete;
                    }
                        
                }
                break;
        }

        base.GetNotifyObjectiveUpdate(objective);
    }

    public override bool IsComplete()
    {
        return base.IsComplete();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
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

    public override void GetNotifyObjectiveUpdate(Objective objective)
    {

    }

    public override bool IsComplete()
    {
        return base.IsComplete();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
