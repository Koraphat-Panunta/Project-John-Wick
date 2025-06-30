using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TutorialLevelGameMaster : InGameLevelGameMaster
{

    public Door door_T1S1;
    [SerializeField] public TextMeshProUGUI movementTutorial;

    public Door door_T1S2;
    [SerializeField] public TextMeshProUGUI gunFuDisarmTutorial;
    [SerializeField] public TextMeshProUGUI reloadTutorial;
    [SerializeField] public Enemy enemy_T1S2;

    public Door door_T1S3;
    public EnemyDirector enemyDirector_T1S3;
    public TextMeshProUGUI eliminateAllEnemy;

    public Door door_T2S1;
    public Enemy enemy_T2S1;
    public TextMeshProUGUI gunFuKnockDown;
    public TextMeshProUGUI restrict;
    public TextMeshProUGUI humanShield;

    public Door door_T2S2;
    public EnemyDirector enemyDirector_T2S2;

    public Door door_T3S1;
    public TextMeshProUGUI execute;
    public Enemy enemy_T3S1;

    public EnemyDirector enemyDirector_T3S2;
    public TextMeshProUGUI executeLastone;

    public Canvas titleCanvas;

    public PlayerHPDisplay playerHPDisplay;
    protected override void Awake()
    {

        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }
    public override InGameLevelOpeningGameMasterNodeLeaf levelOpeningGameMasterNodeLeaf { get ; protected set ; }

    public TutorialGameplayGameMasterNodeLeaf_T1S1 TutorialGameplayGameMasterNodeLeaf_T1S1;
    public TutorialGameplayGameMasterNodeLeaf_T1S2 TutorialGameplayGameMasterNodeLeaf_T1S2;
    public TutorialGameplayGameMasterNodeLeaf_T1S3 TutorialGameplayGameMasterNodeLeaf_T1S3;

    public TutorialGameplayGameMasterNodeLeaf_T2S1 TutorialGameplayGameMasterNodeLeaf_T2S1;
    public TutorialGameplayGameMasterNodeLeaf_T2S2 TutorialGameplayGameMasterNodeLeaf_T2S2;

    public TutorialGameplayGameMasterNodeLeaf_T3S1 TutorialGameplayGameMasterNodeLeaf_T3S1;
    public TutorialGameplayGameMasterNodeLeaf_T3S2 TutorialGameplayGameMasterNodeLeaf_T3S2;

    public TutorialTitleGameMasterNodeLeaf tutorialTitleGameMasterNodeLeaf;
    public override InGameLevelMisstionCompleteGameMasterNodeLeaf levelMisstionCompleteGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelGameOverGameMasterNodeLeaf levelGameOverGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelRestGameMasterNodeLeaf levelRestGameMasterNodeLeaf { get; protected set; }
    public override PauseInGameGameMasterNodeLeaf pauseInGameGameMasterNodeLeaf { get; protected set; }

    public override void InitailizedNode()
    {
        startNodeSelector = new GameMasterNodeSelector<TutorialLevelGameMaster>(this, () => true);

        levelOpeningGameMasterNodeLeaf = new TutorialOpeningGameMasterNodeLeaf(this,()=>levelOpeningGameMasterNodeLeaf.isComplete == false);

        pauseInGameGameMasterNodeLeaf = new PauseInGameGameMasterNodeLeaf(this, pauseCanvasUI, () => pauseInGameGameMasterNodeLeaf.isPause);

        this.TutorialGameplayGameMasterNodeLeaf_T1S1 = new TutorialGameplayGameMasterNodeLeaf_T1S1(this, () => this.TutorialGameplayGameMasterNodeLeaf_T1S1.isComplete == false);
        this.TutorialGameplayGameMasterNodeLeaf_T1S2 = new TutorialGameplayGameMasterNodeLeaf_T1S2(this, () => this.TutorialGameplayGameMasterNodeLeaf_T1S2.isComplete == false);
        this.TutorialGameplayGameMasterNodeLeaf_T1S3 = new TutorialGameplayGameMasterNodeLeaf_T1S3(this, () => this.TutorialGameplayGameMasterNodeLeaf_T1S3.isComplete == false);

        this.TutorialGameplayGameMasterNodeLeaf_T2S1 = new TutorialGameplayGameMasterNodeLeaf_T2S1(this, () => this.TutorialGameplayGameMasterNodeLeaf_T2S1.isComplete == false);
        this.TutorialGameplayGameMasterNodeLeaf_T2S2 = new TutorialGameplayGameMasterNodeLeaf_T2S2(this, () => this.TutorialGameplayGameMasterNodeLeaf_T2S2.isComplete == false);

        this.TutorialGameplayGameMasterNodeLeaf_T3S1 = new TutorialGameplayGameMasterNodeLeaf_T3S1(this, () => this.TutorialGameplayGameMasterNodeLeaf_T3S1.isComplete == false);
        this.TutorialGameplayGameMasterNodeLeaf_T3S2 = new TutorialGameplayGameMasterNodeLeaf_T3S2(this, () => this.TutorialGameplayGameMasterNodeLeaf_T3S2.isComplete == false);

        this.tutorialTitleGameMasterNodeLeaf = new TutorialTitleGameMasterNodeLeaf(this, () => true);

        startNodeSelector.AddtoChildNode(levelOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(pauseInGameGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(this.TutorialGameplayGameMasterNodeLeaf_T1S1);
        startNodeSelector.AddtoChildNode(this.TutorialGameplayGameMasterNodeLeaf_T1S2);
        startNodeSelector.AddtoChildNode(this.TutorialGameplayGameMasterNodeLeaf_T1S3);

        startNodeSelector.AddtoChildNode(this.TutorialGameplayGameMasterNodeLeaf_T2S1);
        startNodeSelector.AddtoChildNode(this.TutorialGameplayGameMasterNodeLeaf_T2S2);

        startNodeSelector.AddtoChildNode(this.TutorialGameplayGameMasterNodeLeaf_T3S1);
        startNodeSelector.AddtoChildNode(this.TutorialGameplayGameMasterNodeLeaf_T3S2);

        startNodeSelector.AddtoChildNode(this.tutorialTitleGameMasterNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);

    }
}

public class TutorialOpeningGameMasterNodeLeaf : InGameLevelOpeningGameMasterNodeLeaf
{
    public TutorialOpeningGameMasterNodeLeaf(InGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }
    protected async override void OpeningDelay()
    {
        await Task.Delay(3000);
        gameMaster.gamePlayUICanvas.EnableGameplayUI();
        this.isComplete = true;
        (gameMaster as TutorialLevelGameMaster).playerHPDisplay.gameObject.SetActive(false);
    }
}
public class TutorialGameplayGameMasterNodeLeaf_T1S1 : InGameLevelGamplayGameMasterNodeLeaf<TutorialLevelGameMaster>
{
    Door doorT1S1 => gameMaster.door_T1S1;
    public bool isComplete { get; private set; }
    public TutorialGameplayGameMasterNodeLeaf_T1S1(TutorialLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }
    public override void Enter()
    {
        gameMaster.movementTutorial.gameObject.SetActive(false);
        isComplete = false;
        MovementTutorialDisplayDelay();
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
        if (Vector3.Distance(player.transform.position, doorT1S1.transform.position) < 1.25f)
        {
            doorT1S1.Open();
            gameMaster.movementTutorial.gameObject.SetActive(false);
            isComplete = false;
            isComplete = true;
        }
        base.UpdateNode();
    }
    private async void MovementTutorialDisplayDelay()
    {
        await Task.Delay(2000);
        gameMaster.movementTutorial.gameObject.SetActive(true);
    }
}
public class TutorialGameplayGameMasterNodeLeaf_T1S2 : InGameLevelGamplayGameMasterNodeLeaf<TutorialLevelGameMaster>
{
    Door door => gameMaster.door_T1S2;
    Enemy enemy => gameMaster.enemy_T1S2;

    public bool isComplete { get; private set; }

    private enum Phase
    {
        beforeEnemyDead,
        afterEnemyDead
    }
    private Phase curPhase;

    public TutorialGameplayGameMasterNodeLeaf_T1S2(TutorialLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }

    public override void Enter()
    {
        isComplete = false;
        curPhase = Phase.beforeEnemyDead;
        enemy.gameObject.SetActive(true);
        DisarmDelay();
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
        switch (curPhase)
        {
            case Phase.beforeEnemyDead:
                {
                    if (enemy.isDead)
                    {
                        gameMaster.gunFuDisarmTutorial.gameObject.SetActive(false);
                        ReloadDelay();
                        curPhase = Phase.afterEnemyDead;
                    }
                }
                break;
            case Phase.afterEnemyDead:
                {
                    if (Vector3.Distance(door.transform.position, player.transform.position) < 1.25f)
                    {
                        door.Open();
                        isComplete = true;
                    }
                }
                break;
               
        }
        base.UpdateNode();
    }
    private async void DisarmDelay()
    {
        await Task.Delay(500);
        gameMaster.gunFuDisarmTutorial.gameObject.SetActive(true);
    }
    private async void ReloadDelay()
    {
        gameMaster.reloadTutorial.gameObject.SetActive(true);
        await Task.Delay(5000);
        gameMaster.reloadTutorial.gameObject.SetActive(false);
    }
}
public class TutorialGameplayGameMasterNodeLeaf_T1S3 : InGameLevelGamplayGameMasterNodeLeaf<TutorialLevelGameMaster>
{
    private Door door => gameMaster.door_T1S3;
    private EnemyDirector enemyDirector => gameMaster.enemyDirector_T1S3;
    public bool isComplete { get; private set; }
    public TutorialGameplayGameMasterNodeLeaf_T1S3(TutorialLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }

    public override void Enter()
    {
        enemyDirector.gameObject.SetActive(true);
        ObjectiveDescribeEvent();
        isComplete = false;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        if(enemyDirector.allEnemiesAliveCount <= 0)
        {
            if (Vector3.Distance(door.transform.position, player.transform.position) < 1.25f)
            {
                isComplete = true;
                door.Open();
            }
        }
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
    private async void ObjectiveDescribeEvent()
    {
        gameMaster.eliminateAllEnemy.gameObject.SetActive(true);
        await Task.Delay(4500);
        gameMaster.eliminateAllEnemy.gameObject.SetActive(false);
    }
}
public class TutorialGameplayGameMasterNodeLeaf_T2S1 : InGameLevelGamplayGameMasterNodeLeaf<TutorialLevelGameMaster>,IObserverPlayer
{
    private Door door_T2S1 => gameMaster.door_T2S1;
    private Enemy enemy_T2S1 => gameMaster.enemy_T2S1;
    private TextMeshProUGUI gunFuKnockDown => gameMaster.gunFuKnockDown;
    private TextMeshProUGUI restrict => gameMaster.restrict;
    private TextMeshProUGUI humanShield => gameMaster.humanShield;
    private TextMeshProUGUI eliminate => gameMaster.eliminateAllEnemy;
    private enum Phase
    {
        KnockDown,
        restrict,
        humanShield,
        Eliminate,
    }
    private Phase curPhase;

    public bool isComplete { get; private set; }
    public TutorialGameplayGameMasterNodeLeaf_T2S1(TutorialLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }

    public override void Enter()
    {
        isComplete = false;
        player.AddObserver(this);
        curPhase = Phase.KnockDown;
        gunFuKnockDown.gameObject.SetActive(true);
        enemy_T2S1.isImortal = true;
        base.Enter();
    }

    public override void Exit()
    {
        player.RemoveObserver(this);
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
    public void OnNotify(Player player, SubjectPlayer.NotifyEvent playerAction)
    {
        if(curPhase == Phase.Eliminate)
        {
            enemy_T2S1.isImortal = false;
            if (enemy_T2S1.isDead)
            {
                if (Vector3.Distance(door_T2S1.transform.position, player.transform.position) < 1.25f)
                {
                    door_T2S1.Open();
                    isComplete = true;
                }
            }
        }
    }
    public void OnNotify<T>(Player player, T node) where T : INode
    {
        if (node is PlayerStateNodeLeaf playerStateNodeLeaf)
            switch (playerStateNodeLeaf)
            {
                case KnockDown_GunFuNode knockDown_GunFuNode:
                    {
                        if (knockDown_GunFuNode.curGunFuHitPhase == PlayerGunFuHitNodeLeaf.GunFuHitPhase.Hit
                            && curPhase == Phase.KnockDown)
                        {
                            gunFuKnockDown.gameObject.SetActive(false);
                            restrict.gameObject.SetActive(true);
                            curPhase = Phase.restrict;
                        }
                        break;
                    }
                case RestrictGunFuStateNodeLeaf restrictGunFuStateNodeLeaf:
                    {
                        if (restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Stay
                            && curPhase == Phase.restrict)
                        {
                            restrict.gameObject.SetActive(false);
                            humanShield.gameObject.SetActive(true);
                            curPhase = Phase.humanShield;
                        }
                        break;
                    }
                case HumanShield_GunFuInteraction_NodeLeaf humanShieldGunFuInteractionNodeLeaf:
                    {
                        if (humanShieldGunFuInteractionNodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Stay
                            && curPhase == Phase.humanShield)
                        {
                            humanShield.gameObject.SetActive(false);
                            EliminateEnemy();
                            curPhase = Phase.Eliminate;
                        }
                        break;
                    }
            }
    }
    private async void EliminateEnemy()
    {
        eliminate.gameObject.SetActive(true);
        await Task.Delay(4000);
        eliminate.gameObject.SetActive(false);
    }
  
}
public class TutorialGameplayGameMasterNodeLeaf_T2S2 : InGameLevelGamplayGameMasterNodeLeaf<TutorialLevelGameMaster>
{
    private EnemyDirector enemyDirector_T2S2 => gameMaster.enemyDirector_T2S2;
    private Door door => gameMaster.door_T2S2;
    private TextMeshProUGUI elimination => gameMaster.eliminateAllEnemy;
    public bool isComplete { get; private set; }
    public TutorialGameplayGameMasterNodeLeaf_T2S2(TutorialLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }
    public override void Enter()
    {
        isComplete = false;
        EliminateEnemy();
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        if(enemyDirector_T2S2.allEnemiesAliveCount <= 0)
        {
            if (Vector3.Distance(door.transform.position, player.transform.position) < 1.25f)
            {
                door.Open();
                isComplete = true;
            }
        }
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }

    private async void EliminateEnemy()
    {
        elimination.gameObject.SetActive(true);
        await Task.Delay(4000);
        elimination.gameObject.SetActive(false);
    }
}
public class TutorialGameplayGameMasterNodeLeaf_T3S1 : InGameLevelGamplayGameMasterNodeLeaf<TutorialLevelGameMaster>
{
    private TextMeshProUGUI execute => gameMaster.execute;
    private Enemy enemy => gameMaster.enemy_T3S1;
    private Door door => gameMaster.door_T3S1;
    public bool isComplete { get; private set; }
    public TutorialGameplayGameMasterNodeLeaf_T3S1(TutorialLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }

    public override void Enter()
    {
        execute.gameObject.SetActive(true);
        isComplete = false;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        if (enemy.isDead)
        {
            execute.gameObject.SetActive(false);
            if (Vector3.Distance(player.transform.position, door.transform.position) < 1.25f)
            {
                isComplete = true;
                door.Open();
            }
        }
        base.FixedUpdateNode();
    }
    public override void UpdateNode()
    {
        base.UpdateNode();
    }
}
public class TutorialGameplayGameMasterNodeLeaf_T3S2 : InGameLevelGamplayGameMasterNodeLeaf<TutorialLevelGameMaster>,IObserverPlayer
{
    private EnemyDirector enemyDirector => gameMaster.enemyDirector_T3S2;
    private Enemy lastEnemy;
    private TextMeshProUGUI eliminate => gameMaster.eliminateAllEnemy;
    private TextMeshProUGUI executeLastone => gameMaster.executeLastone;
    public bool isComplete { get; private set; }
    private enum Phase
    {
        Encouter,
        ExecuteLastone
    }
    private Phase curPhase;
    public TutorialGameplayGameMasterNodeLeaf_T3S2(TutorialLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }

    public override void Enter()
    {
        curPhase = Phase.Encouter;
        this.EliminateEnemy();
        isComplete = false;
        player.AddObserver(this);
        base.Enter();
    }

    public override void Exit()
    {
        player.RemoveObserver(this);
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public void OnNotify(Player player, SubjectPlayer.NotifyEvent playerAction)
    {
       if(curPhase == Phase.ExecuteLastone 
            &&player.curGunFuNode is GunFuExecute_OnGround_Single_NodeLeaf)
        {
            lastEnemy.isImortal = false;
        }
    }
    public void OnNotify<T>(Player player, T node) where T : INode
    {
       if(node is GunFuExecute_OnGround_Single_NodeLeaf gunFuExecuteNodeLeaf && curPhase == Phase.ExecuteLastone)
            lastEnemy.isImortal = false;
    }

    public override void UpdateNode()
    {
        switch (curPhase)
        {
            case Phase.Encouter: 
                {
                    if (enemyDirector.allEnemiesAliveCount == 1)
                    {
                        curPhase = Phase.ExecuteLastone;
                        lastEnemy = enemyDirector.GetAllEnemyAlive()[0];
                        lastEnemy.isImortal = true;
                        ExecuteLastOneDisplay();
                    }
                } break;

            case Phase.ExecuteLastone: 
                { 
                    if(lastEnemy.isDead)
                        isComplete = true;
                        
                } break;
        }

        
        base.UpdateNode();
    }
    private async void EliminateEnemy()
    {
        eliminate.gameObject.SetActive(true);
        await Task.Delay(4000);
        eliminate.gameObject.SetActive(false);
    }

    private async void ExecuteLastOneDisplay()
    {
        executeLastone.gameObject.SetActive(true);
        await Task.Delay(4000);
        executeLastone.gameObject.SetActive(false);
    }

   
}
public class TutorialTitleGameMasterNodeLeaf : GameMasterNodeLeaf<TutorialLevelGameMaster>
{

    public TutorialTitleGameMasterNodeLeaf(TutorialLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }

    public GameManager gameManager { get => gameMaster.gameManager; set { }  }

    public override void Enter()
    {
        gameMaster.titleCanvas.gameObject.SetActive(true);
        gameMaster.gamePlayUICanvas.DisableGameplayUI();
        gameMaster.user.DisableInput();
        Delay();
        gameManager.soundTrackManager.StopSoundTrack(5);
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdateNode()
    {
       
    }

    public override bool IsComplete()
    {
        return false;
    }

    public override void UpdateNode()
    {
        
    }
    private async void Delay()
    {
        await Task.Delay(5000);
        gameManager.ContinueGameplayScene();
    }
}
