using System;

public class WaveBasedSectionNodeLeaf : InGameLevelGameMasterNodeLeaf<InGameLevelGameMaster>,IGameMasterSectorNodeLeaf
{
    public EnemyWaveManager _enemyWaveManager { get; protected set; }
    public bool isEnable { get; set; }

    public WaveBasedSectionNodeLeaf(InGameLevelGameMaster gameMaster,EnemyWaveManager enemyWaveManager, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        _enemyWaveManager = enemyWaveManager;
    }
   
    public override bool IsComplete()
    {
        if(_enemyWaveManager.waveIsClear)
            return true;

        return false;
    }
    public override bool IsReset()
    {
        return IsComplete();
    }
    public override void UpdateNode()
    {
        _enemyWaveManager.EnemyWaveUpdate();
    }
}
