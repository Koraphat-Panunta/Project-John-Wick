using System;
using UnityEngine;

public class InGameLevelDelayOpeningLoad : GameMasterNodeLeaf<InGameLevelGameMaster>
{
    public InGameLevelDelayOpeningLoad(InGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }
    public override void Enter()
    {
        gameMaster.gamePlayUICanvas.DisableGameplayUI();
    }

    public override void Exit()
    {

    }
    public override void UpdateNode()
    {

    }
    public override void FixedUpdateNode()
    {

    }

    public override bool IsComplete()
    {
        return false;
    }

  
}
