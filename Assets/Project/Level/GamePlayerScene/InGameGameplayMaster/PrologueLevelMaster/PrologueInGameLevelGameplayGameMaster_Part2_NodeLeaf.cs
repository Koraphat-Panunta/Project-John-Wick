using System;
using UnityEngine;

public class PrologueInGameLevelGameplayGameMaster_Part2_NodeLeaf : InGameLevelGamplayGameMasterNodeLeaf<PrologueLevelGameMaster>,IObserverEnemy
{
    private int killedEnemyCount;
    public override bool isComplete { get => this.killedEnemyCount >= gameMaster.enemySpawnPoint_A2.Length; set => base.isComplete = value; }
    private bool isSpawnedEnemy;


    public PrologueInGameLevelGameplayGameMaster_Part2_NodeLeaf(PrologueLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
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
    public override void UpdateNode()
    {
        if(gameMaster.door_A2_Enter.isOpen && isSpawnedEnemy == false)
        {
            for (int i = 0; i < gameMaster.enemySpawnPoint_A2.Length; i++)
            {
                if (i >= gameMaster.enemySpawnPoint_A2.Length - 1)
                {
                    gameMaster.enemySpawnPoint_A2[i].SpawnEnemy(gameMaster.enemyMask_ObjectManager, null, gameMaster.glock17_weaponObjectManager, false, out Enemy enemy);
                    enemy.AddObserver(this);
                }
                else
                {
                    gameMaster.enemySpawnPoint_A2[i].SpawnEnemy(gameMaster.enemy_ObjectManager, null, gameMaster.glock17_weaponObjectManager, false, out Enemy enemy);
                    enemy.AddObserver(this);
                }

            }
            isSpawnedEnemy = true;
        }
        base.UpdateNode();
    }
    public override bool IsComplete()
    {
        return base.IsComplete();
    }

    public override void RestartCheckPoint()
    {

    }
    public void Notify<T>(Enemy enemy, T node)
    {
        if(node is EnemyDeadStateNode deadStateNode 
            && deadStateNode.curstate == EnemyStateLeafNode.Curstate.Enter)
        {
            enemy.RemoveObserver(this);
            this.killedEnemyCount++;
        }
    }
}
