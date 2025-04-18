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
    public Door door_T2S2;

    public Door door_T3S1;

    public Canvas titleCanvas;
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

    public TutorialTitleGameMasterNodeLeaf tutorialTitleGameMasterNodeLeaf;
    public override InGameLevelMisstionCompleteGameMasterNodeLeaf levelMisstionCompleteGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelGameOverGameMasterNodeLeaf levelGameOverGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelRestGameMasterNodeLeaf levelRestGameMasterNodeLeaf { get; protected set; }

    public override void InitailizedNode()
    {
        startNodeSelector = new GameMasterNodeSelector<TutorialLevelGameMaster>(this, () => true);

        levelOpeningGameMasterNodeLeaf = new InGameLevelOpeningGameMasterNodeLeaf(this,()=>levelOpeningGameMasterNodeLeaf.isComplete == false);

        this.TutorialGameplayGameMasterNodeLeaf_T1S1 = new TutorialGameplayGameMasterNodeLeaf_T1S1(this, () => this.TutorialGameplayGameMasterNodeLeaf_T1S1.isComplete == false);
        this.TutorialGameplayGameMasterNodeLeaf_T1S2 = new TutorialGameplayGameMasterNodeLeaf_T1S2(this, () => this.TutorialGameplayGameMasterNodeLeaf_T1S2.isComplete == false);
        this.TutorialGameplayGameMasterNodeLeaf_T1S3 = new TutorialGameplayGameMasterNodeLeaf_T1S3(this, () => this.TutorialGameplayGameMasterNodeLeaf_T1S3.isComplete == false);
        this.tutorialTitleGameMasterNodeLeaf = new TutorialTitleGameMasterNodeLeaf(this, () => true);

        startNodeSelector.AddtoChildNode(levelOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(this.TutorialGameplayGameMasterNodeLeaf_T1S1);
        startNodeSelector.AddtoChildNode(this.TutorialGameplayGameMasterNodeLeaf_T1S2);
        startNodeSelector.AddtoChildNode(this.TutorialGameplayGameMasterNodeLeaf_T1S3);

        startNodeSelector.AddtoChildNode(this.tutorialTitleGameMasterNodeLeaf);

        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf;
        curNodeLeaf.Enter();

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
        gameMaster.eliminateAllEnemy.enabled = true;
        await Task.Delay(4500);
        gameMaster.eliminateAllEnemy.enabled = false;
    }
}
public class TutorialTitleGameMasterNodeLeaf : GameMasterNodeLeaf<TutorialLevelGameMaster>
{

    public TutorialTitleGameMasterNodeLeaf(TutorialLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
    }
    public override void Enter()
    {
        gameMaster.titleCanvas.gameObject.SetActive(true);
        gameMaster.gamePlayUICanvas.DisableGameplayUI();
        gameMaster.user.DisableInput();
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
}
