using System;
using UnityEngine;

public class WaveBasedSectionNodeLeaf : GameMasterNodeLeaf<InGameLevelGameMaster>
{
    private EnemyWaveManager _enemyWaveManager;
    private InGameLevelGameMaster gameLevelGameMaster;

    public WaveBasedSectionNodeLeaf(InGameLevelGameMaster gameMaster,EnemyWaveManager enemyWaveManager, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }

    public override void Enter()
    {
        this.gameLevelGameMaster.NotifyObserver<WaveBasedSectionNodeLeaf>(gameLevelGameMaster, this);
    }

    public override void Exit()
    {
        this.gameLevelGameMaster.NotifyObserver<WaveBasedSectionNodeLeaf>(gameLevelGameMaster, this);
    }

    public override void FixedUpdateNode()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsComplete()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateNode()
    {
        _enemyWaveManager.EnemyWaveUpdate();
    }
}
