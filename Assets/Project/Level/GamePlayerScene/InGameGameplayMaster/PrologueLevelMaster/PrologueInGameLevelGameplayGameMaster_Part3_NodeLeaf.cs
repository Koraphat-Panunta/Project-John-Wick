using System;
using UnityEngine;

public class PrologueInGameLevelGameplayGameMaster_Part3_NodeLeaf : InGameLevelGamplayGameMasterNodeLeaf<PrologueLevelGameMaster>
{
    private bool isGroup1Spawn;
    private bool isGroup2Spawn;
    private bool isGroup3Spawn;
    private bool isGroup4Spawn;
    private bool isGroup5Spawn;
    private bool isGroup6Spawn;
    public PrologueInGameLevelGameplayGameMaster_Part3_NodeLeaf(PrologueLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }

    public override void UpdateNode()
    {
        //Group1
        if(gameMaster.door_A3_EnterBelow.isOpen && isGroup1Spawn == false)
        {
            gameMaster.enemySpawnerPoint_A3_Before.SpawnEnemy(
                gameMaster.enemyMask_ObjectManager, null, gameMaster.ar15Optic_weaponObjectManager, false, out Enemy enemy);
            isGroup1Spawn = true;
        }
        //Group2
        if (gameMaster.door_A4_Enter.isOpen && isGroup2Spawn == false)
        {
            gameMaster.enemySpawnPoint_A4_1.SpawnEnemy(
                gameMaster.enemy_ObjectManager, null, gameMaster.glock17_weaponObjectManager, false, out Enemy enemy);
            isGroup2Spawn = true;
        }
        //Group3
        base.UpdateNode();
    }
    public void OnTriggerBoxGroup3(Collider collider)
    {
        if(isGroup3Spawn == false 
            && collider.gameObject.TryGetComponent<Player>(out Player player))
        {
            isGroup3Spawn = true;
        }
    }
    
}
