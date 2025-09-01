using System.Collections.Generic;
using System;
using UnityEngine;

public class PrologueInGameLevelGameplayGameMasterNodeLeaf : InGameLevelGamplayGameMasterNodeLeaf<PrologueLevelGameMaster>
{

    public PrologueInGameLevelGameplayGameMasterNodeLeaf(PrologueLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
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
        return isComplete;
    }

    private void SpawnEnemyA1(Door doorA1)
    {
        if (doorA1.isOpen)
        {
            gameMaster.enemySpawnPoint_A1.SpawnEnemy(gameMaster.enemy_ObjectManager
                , null
                , gameMaster.glock17_weaponObjectManager
                , false
                , out Enemy firstEnemy);
            doorA1.doorTriggerEvent -= SpawnEnemyA1;
        }
    }
    private void SpawnGroupEnemyA2(Door doorA2)
    {
        if (doorA2.isOpen)
        {
            gameMaster.enemySpawnPoint_A2[0].SpawnEnemy(gameMaster.enemy_ObjectManager
                , null
                , gameMaster.glock17_weaponObjectManager
                , false
                , out Enemy enemy);
            gameMaster.enemySpawnPoint_A2[1].SpawnEnemy(gameMaster.enemy_ObjectManager
               , null
               , gameMaster.glock17_weaponObjectManager
               , false
               , out Enemy enemy2);
            gameMaster.enemySpawnPoint_A2[2].SpawnEnemy(gameMaster.enemyMask_ObjectManager
               , null
               , gameMaster.ar15Optic_weaponObjectManager
               , false
               , out Enemy enemy3);
            doorA2.doorTriggerEvent -= SpawnGroupEnemyA2;
        }
    }
    private void SpawnEnemyA3(Door doorA3)
    {
        if (doorA3.isOpen)
        { }
    }
}
