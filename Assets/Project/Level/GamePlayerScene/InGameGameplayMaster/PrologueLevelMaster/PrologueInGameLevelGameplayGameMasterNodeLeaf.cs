using System.Collections.Generic;
using System;
using UnityEngine;

public class PrologueInGameLevelGameplayGameMasterNodeLeaf : InGameLevelGamplayGameMasterNodeLeaf<PrologueLevelGameMaster>, IObserverPlayer
{
    public Queue<ITaskingExecute> tutorialHintTask;

    public ITaskingExecute movementTutorial;
    public ITaskingExecute pickUpTutorial;
    public ITaskingExecute openTheDoorTutorial;
    public ITaskingExecute shootTutorial;
    public ITaskingExecute reloadTutorial;
    public ITaskingExecute executeTutorial;
    public ITaskingExecute executeRefillHP;
    public ITaskingExecute showGunFuTutorial;

    private Door doorA21 => gameMaster.door_A21;
    private bool isMove;
    private bool isSprint;
    private bool isPickingUpWeapon;
    private int shootCount = 5;
    private bool isReload;
    private bool isExecute;
    private float showExecuteRefill_time = 6;
    public PrologueInGameLevelGameplayGameMasterNodeLeaf(PrologueLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {

        tutorialHintTask = new Queue<ITaskingExecute>();

        gameMaster.enemyDirectorA2.gameObject.SetActive(false);
        gameMaster.enemyDirectirA3.gameObject.SetActive(false);
        gameMaster.enemyDirectirA4.gameObject.SetActive(false);
        gameMaster.enemyDirectirA5.gameObject.SetActive(false);

        this.InitializedTaskingTutotialHint();

        tutorialHintTask.Enqueue(movementTutorial);
        tutorialHintTask.Enqueue(pickUpTutorial);
        tutorialHintTask.Enqueue(openTheDoorTutorial);
        tutorialHintTask.Enqueue(shootTutorial);
        tutorialHintTask.Enqueue(reloadTutorial);
        tutorialHintTask.Enqueue(executeTutorial);
        tutorialHintTask.Enqueue(executeRefillHP);
        tutorialHintTask.Enqueue(showGunFuTutorial);
    }

    public override void Enter()
    {
        gameMaster.player.AddObserver(this);
        base.Enter();
    }

    public override void Exit()
    {
        gameMaster.player.RemoveObserver(this);
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        this.UpdateTutorialHint();
        this.UpdateEnemySpawnEvent();

        base.FixedUpdateNode();
    }


    public void OnNotify<T>(Player player, T node)
    {
        if(node is PlayerStandMoveNodeLeaf)
            isMove = true;

        if(node is PlayerSprintNode)
            isSprint = true;

        if(player._currentWeapon != null)
            isPickingUpWeapon = true;

        if(node is SubjectPlayer.NotifyEvent.Firing)
        {
            shootCount -= 1;
        }

        if(node is IReloadNode)
            isReload = true;

        if(node is IGunFuExecuteNodeLeaf)
            isExecute = true;

        
    }

    private void UpdateEnemySpawnEvent()
    {
        if(gameMaster.enemyDirectorA2.gameObject.activeSelf == false)
        {
            if(gameMaster.door_A21.isOpen)
                gameMaster.enemyDirectorA2.gameObject.SetActive(true);
        }
        if(gameMaster.enemyDirectirA3.gameObject.activeSelf == false)
        {
            if(gameMaster.door_A31.isOpen)
                gameMaster.enemyDirectirA3.gameObject.SetActive(true);
        }
        if(gameMaster.enemyDirectirA4.gameObject.activeSelf == false)
        {
            if(gameMaster.door_A42.isOpen)
                gameMaster.enemyDirectirA4.gameObject.SetActive(true);
        }
        if(gameMaster.enemyDirectirA5.gameObject.activeSelf == false)
        {
            if(gameMaster.door_A52.isOpen)
                gameMaster.enemyDirectirA5.gameObject.SetActive(true);
        }
    }

    private void UpdateTutorialHint()
    {
        if (tutorialHintTask.Count <= 0)
            return;

        tutorialHintTask.Peek().FixedUpdate();

        if (tutorialHintTask.Peek().IsComplete())
            tutorialHintTask.Dequeue();

    }
    private void InitializedTaskingTutotialHint()
    {

        movementTutorial = new TutorialHintTasking(gameMaster.movementHint,
            () =>
            {
                if ((isMove && isSprint)
                || (isPickingUpWeapon))
                    return true;
                return false;
            });

        pickUpTutorial = new TutorialHintTasking(gameMaster.pickUpWeaponHint,
            () =>
            {
                if (isPickingUpWeapon)
                    return true;
                return false;
            });
        openTheDoorTutorial = new TutorialHintTasking(gameMaster.openTheDoorHint,
            () =>
            {
                if(doorA21.isOpen)
                    return true;
                return false;
            });
        shootTutorial = new TutorialHintTasking(gameMaster.shootHint,
            () =>
            {
                if(shootCount <= 0
                || isReload
                || isExecute)
                    return true;
                return false;
            });
        reloadTutorial = new TutorialHintTasking(gameMaster.reloadHint,
            () => isReload || isExecute);

        executeTutorial = new TutorialHintTasking(gameMaster.executeHint,
            () => isExecute);

        executeRefillHP = new TutorialHintTasking(gameMaster.executeHP_refillHint,
            () => this.showExecuteRefill_time <= 0,
            ()=> this.showExecuteRefill_time -= Time.deltaTime);

        showGunFuTutorial = new TutorialHintTasking(gameMaster.showGunFuCommboHint,
            () => 
            {
                foreach(EnemyDecision enemyA3 in gameMaster.enemyA3)
                {
                    if (enemyA3.enemy.isDead == false)
                        return false;
                }

                foreach(EnemyDecision enemyA4 in gameMaster.enemyA4)
                {
                    if(enemyA4.enemy.isDead == false)
                        return false;
                }
                return true;
            });
    }

    private class TutorialHintTasking : ITaskingExecute
    {
        private GameObject tutorialGUI;
        private Func<bool> isCompleteShow;
        private Action fixedUpdate;
        public TutorialHintTasking(GameObject textMeshProUGUI, Func<bool> isCompleteShow)
        {
            this.tutorialGUI = textMeshProUGUI;
            this.isCompleteShow = isCompleteShow;
        }
        public TutorialHintTasking(GameObject textMeshProUGUI, Func<bool> isCompleteShow,Action fixUpdate)
        {
            this.tutorialGUI = textMeshProUGUI;
            this.isCompleteShow = isCompleteShow;
            this.fixedUpdate = fixUpdate;
        }
        public void FixedUpdate()
        {
            if(IsComplete())
                this.tutorialGUI.gameObject.SetActive(false);

            if (this.tutorialGUI.gameObject.activeSelf == false)
                this.tutorialGUI.gameObject.SetActive(true);

            if(fixedUpdate != null)
                fixedUpdate.Invoke();
        }

        public bool IsComplete()
        {
            if (this.isCompleteShow.Invoke())
            {
                this.tutorialGUI.gameObject.SetActive(false);
                return true;
            }
            return false;
        }

        public void Update()
        {

        }
    }
}
