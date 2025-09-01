using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading.Tasks;

public class PrologueLevelGameMaster : InGameLevelGameMaster
{
    [SerializeField] private OpeningUICanvas openingUICanvas;
    [SerializeField] private GameOverUICanvas gameOverUICanvas;
    [SerializeField] private PauseUICanvas pauseCanvasUI;
    [SerializeField] private MissionCompleteUICanvas missionCompleteUICanvas;

    public InGameLevelOpeningGameMasterNodeLeaf levelOpeningGameMasterNodeLeaf { get; protected set; }
    public InGameLevelMisstionCompleteGameMasterNodeLeaf levelMisstionCompleteGameMasterNodeLeaf { get; protected set; }
    public InGameLevelGameOverGameMasterNodeLeaf levelGameOverGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelRestGameMasterNodeLeaf levelRestGameMasterNodeLeaf { get; protected set; }
    public PauseInGameGameMasterNodeLeaf pauseInGameGameMasterNodeLeaf { get ; protected set ; }
    public InGameLevelDelayOpeningLoad delayOpeningGameMasterNodeLeaf { get ; protected set ; }
    protected PrologueInGameLevelGameplayGameMaster_Part1_NodeLeaf prologueInGameLevelGameplayGameMasterNodeLeaf;

    [SerializeField] private DoorKeyItem key;

    public Door door_A1;
    public Door door_A2_Enter;
    public Door door_A3_EnterBelow;
    public Door door_A3_EnterSide;
    public Door door_A4_Enter;
    public Door door_A4_2;
    public Door door_A4_3;
    public Door door_A4_Exit;
    public Door door_A5_Enter;

    public Enemy enemyOrigin;
    public Enemy enemyMaskOrigin;
    public Enemy enemyMaskArmordOrigin;

    public EnemyObjectManager enemy_ObjectManager;
    public EnemyObjectManager enemyMask_ObjectManager;
    public EnemyObjectManager enemyMaskArmored_ObjectManager;

    public Weapon glock17_Origin;
    public Weapon ar15_Origin;
    public Weapon ar15Redot_Origin;
    public Weapon ar15Optic_Origin;
    public Weapon ar15TacticalScope_Origin;

    public WeaponObjectManager glock17_weaponObjectManager;
    public WeaponObjectManager ar15_weaponObjectManager;
    public WeaponObjectManager ar15Redot_weaponObjectManager;
    public WeaponObjectManager ar15Optic_weaponObjectManager;
    public WeaponObjectManager ar15TacticalScope_weaponObjectManager;

    public EnemyDirector enemyDirectorA2;
    public EnemyDirector enemyDirectirA3 ;
    public EnemyDirector enemyDirectirA4;
    public EnemyDirector enemyDirectirA5;

    public TriggerBox triggerBoxA4_1;
    public TriggerBox triggerBoxA4_2;

    public EnemySpawnerPoint enemySpawnPoint_A1;
    public EnemySpawnerPoint[] enemySpawnPoint_A2;
    public EnemySpawnerPoint enemySpawnerPoint_A3_Before;
    public EnemySpawnerPoint[] enemySpawnerPoint_A3_After;
    public EnemySpawnPointRoom[] enemySpawnPointRooms_A3;
    public EnemySpawnerPoint enemySpawnPoint_A4_1;
    public EnemySpawnerPoint[] enemySpawnPoint_A4_2;
    public EnemySpawnerPoint[] enemySpawnPoint_A4_3;
    public EnemySpawnerPoint[] enemySpawnPoint_A4_4;
    public EnemySpawnerPoint[] enemySpawnPoint_A4_5;
    public EnemySpawnerPoint[] enemySpawnPoint_A5;
    public EnemySpawnPointRoom[] enemySpawnPointRooms_A5;


    protected override void Start()
    {
        _ = DelayInitialized();
        base.Start();
    }
    protected override void FixedUpdate()
    {
       
        base.FixedUpdate();
    }

    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(()=>true,"PrologueStartNodeSelector");

        delayOpeningGameMasterNodeLeaf = new InGameLevelDelayOpeningLoad(this, () => base.isCompleteLoad == false);
        levelOpeningGameMasterNodeLeaf = new InGameLevelOpeningGameMasterNodeLeaf(this, openingUICanvas , () => levelOpeningGameMasterNodeLeaf.isComplete == false);
        levelGameOverGameMasterNodeLeaf = new InGameLevelGameOverGameMasterNodeLeaf(this, gameOverUICanvas, () => player.isDead);
        prologueInGameLevelGameplayGameMasterNodeLeaf = new PrologueInGameLevelGameplayGameMaster_Part1_NodeLeaf(this,()=> prologueInGameLevelGameplayGameMasterNodeLeaf.IsComplete() == false);
        pauseInGameGameMasterNodeLeaf = new PauseInGameGameMasterNodeLeaf(this, pauseCanvasUI,
            () => pauseInGameGameMasterNodeLeaf.isPause);
        levelMisstionCompleteGameMasterNodeLeaf = new InGameLevelMisstionCompleteGameMasterNodeLeaf(this, missionCompleteUICanvas, () => true);

        startNodeSelector.AddtoChildNode(delayOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelGameOverGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(pauseInGameGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(prologueInGameLevelGameplayGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelMisstionCompleteGameMasterNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);

    }

    private void OnValidate()
    {
     
    }
    private async Task DelayInitialized() 
    {
        await Task.Yield();

        enemyDirectorA2.gameObject.SetActive(false);
        enemyDirectirA3.gameObject.SetActive(false);
        enemyDirectirA4.gameObject.SetActive(false);
        enemyDirectirA5.gameObject.SetActive(false);
    }

}

