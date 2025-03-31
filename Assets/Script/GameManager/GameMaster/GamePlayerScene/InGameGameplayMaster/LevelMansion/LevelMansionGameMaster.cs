using System;
using UnityEngine;

public class LevelMansionGameMaster : InGameLevelGameMaster
{
    public override InGameLevelOpeningGameMasterNodeLeaf levelHotelOpeningGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelMisstionCompleteGameMasterNodeLeaf levelHotelMisstionCompleteGameMasterNodeLeaf { get ; protected set ; }
    public override GameOverGameMasterNodeLeaf gameOverGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelRestGameMasterNodeLeaf levelHotelRestGameMasterNodeLeaf { get; protected set; }

    public override void InitailizedNode()
    {
        
    }
}
public class LevelMansionGamePlaySequence1 : InGameLevelGamplayGameMasterNodeLeaf
{
    public LevelMansionGamePlaySequence1(InGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
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
