using System.Collections.Generic;
using System;
using UnityEngine;

public class PrologueInGameLevelGameplayGameMaster_Part1_NodeLeaf : InGameLevelGamplayGameMasterNodeLeaf<PrologueLevelGameMaster>,IObserverEnemy
{

    private bool isFirstEnemySpawn;
    public PrologueInGameLevelGameplayGameMaster_Part1_NodeLeaf(PrologueLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }
    public override void Enter()
    {
        if (isFirstEnemySpawn == false) 
        {
            gameMaster.enemySpawnPoint_A1.SpawnEnemy(gameMaster.enemy_ObjectManager,null,gameMaster.glock17_weaponObjectManager,false,out Enemy firstEnemy);
            firstEnemy.AddObserver(this);
            isFirstEnemySpawn = true;
        }
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

    public void Notify<T>(Enemy enemy, T node)
    {
        if (node is EnemyDeadStateNode deadStateNode && deadStateNode.curstate == EnemyStateLeafNode.Curstate.Enter)
        {
            enemy.RemoveObserver(this);
            isComplete = true;
        }
    }

    public override void RestartCheckPoint()
    {
        
    }

    //private class TutorialHintTasking : ITaskingExecute
    //{
    //    private GameObject tutorialGUI;
    //    private Func<bool> isCompleteShow;
    //    private Action fixedUpdate;
    //    public TutorialHintTasking(GameObject textMeshProUGUI, Func<bool> isCompleteShow)
    //    {
    //        this.tutorialGUI = textMeshProUGUI;
    //        this.isCompleteShow = isCompleteShow;
    //    }
    //    public TutorialHintTasking(GameObject textMeshProUGUI, Func<bool> isCompleteShow, Action fixUpdate)
    //    {
    //        this.tutorialGUI = textMeshProUGUI;
    //        this.isCompleteShow = isCompleteShow;
    //        this.fixedUpdate = fixUpdate;
    //    }
    //    public void FixedUpdate()
    //    {
    //        if (IsComplete())
    //            this.tutorialGUI.gameObject.SetActive(false);

    //        if (this.tutorialGUI.gameObject.activeSelf == false)
    //            this.tutorialGUI.gameObject.SetActive(true);

    //        if (fixedUpdate != null)
    //            fixedUpdate.Invoke();
    //    }

    //    public bool IsComplete()
    //    {
    //        if (this.isCompleteShow.Invoke())
    //        {
    //            this.tutorialGUI.gameObject.SetActive(false);
    //            return true;
    //        }
    //        return false;
    //    }

    //    public void Update()
    //    {

    //    }
    //}
}
