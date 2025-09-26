using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

public class PrologueLevelGameMaster : InGameLevelGameMaster,IGameLevelMasterObserver
{
    [SerializeField] private OpeningUICanvas openingUICanvas;
    [SerializeField] private GameOverUICanvas gameOverUICanvas;
    [SerializeField] private PauseUICanvas pauseCanvasUI;
    [SerializeField] private MissionCompleteUICanvas missionCompleteUICanvas;

    public NodeCombine gameMasterNodeCombine;
    public NodeSelector gameMasterModeNodeSelector;
    public NodeSelector gameMasterSectionNodeSelector;

    public WaveBasedSectionNodeLeaf waveBaseSection_1_Nodeleaf;
    public WaveBasedSectionNodeLeaf waveBaseSection_2_Nodeleaf;

    public InGameLevelGameMasterNodeLeaf<PrologueLevelGameMaster> freeRomeSectionNodeLeaf;
    public InGameLevelOpeningGameMasterNodeLeaf levelOpeningGameMasterNodeLeaf { get; protected set; }
    public InGameLevelMisstionCompleteGameMasterNodeLeaf levelMisstionCompleteGameMasterNodeLeaf { get; protected set; }
    public InGameLevelGameOverGameMasterNodeLeaf levelGameOverGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelRestGameMasterNodeLeaf levelRestGameMasterNodeLeaf { get; protected set; }
    public override PauseInGameGameMasterNodeLeaf pauseInGameGameMasterNodeLeaf { get ; protected set ; }
    public InGameLevelDelayOpeningLoad delayOpeningGameMasterNodeLeaf { get ; protected set ; }
    protected InGameLevelGamplayGameMasterNodeLeaf<PrologueLevelGameMaster> prologueInGameLevelGameplayGameMasterNodeLeaf;

    [SerializeField] private DoorKeyItem key;

    public Door door_A1;
    public Door door_A2_Enter;
    public Door door_A3_EnterBelow;
    public Door door_A3_Exit;
    public Door door_A4_Enter;
    public Door door_A4_2;
    public Door door_A4_3;
    public Door door_A4_4;
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
    private bool isTriggerBox4_1BeenTrigger;
    public TriggerBox triggerBoxA4_2;
    private bool isTriggerBox4_2BeenTrigger;

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
    public EnemySpawnerPoint[] enemySpawnPoint_A4_6;
    public EnemySpawnerPoint[] enemySpawnPoint_A5;
    public EnemySpawnPointRoom[] enemySpawnPointRooms_A5;

    [SerializeField] private Camera cameraMain;

    [SerializeField] private int Wave1;
    [SerializeField] private int numberOfEnemyWave1;
    [SerializeField] private int wave2;
    [SerializeField] private int numberOfEnemyWave2;

    public override void Initialized()
    {

        this.enemy_ObjectManager = new EnemyObjectManager(enemyOrigin, this.cameraMain);
        this.enemyMask_ObjectManager = new EnemyObjectManager(enemyMaskOrigin, this.cameraMain);
        this.enemyMaskArmored_ObjectManager = new EnemyObjectManager(enemyMaskArmordOrigin, this.cameraMain);

        this.glock17_weaponObjectManager = new WeaponObjectManager(glock17_Origin, this.cameraMain);
        this.ar15_weaponObjectManager = new WeaponObjectManager(ar15_Origin, this.cameraMain);
        this.ar15Optic_weaponObjectManager = new WeaponObjectManager(ar15Optic_Origin, this.cameraMain);
        this.ar15Redot_weaponObjectManager = new WeaponObjectManager(ar15Redot_Origin, this.cameraMain);
        this.ar15TacticalScope_weaponObjectManager = new WeaponObjectManager(ar15TacticalScope_Origin, this.cameraMain);


        this.InitializedWave();
        this.door_A1.doorTriggerEvent += this.OnDoorTriggerEvent;
        this.door_A2_Enter.doorTriggerEvent += this.OnDoorTriggerEvent;
        this.door_A3_EnterBelow.doorTriggerEvent += this.OnDoorTriggerEvent;
        this.door_A3_Exit.doorTriggerEvent += this.OnDoorTriggerEvent;
        this.door_A4_Enter.doorTriggerEvent += this.OnDoorTriggerEvent;
        this.door_A4_2.doorTriggerEvent += this.OnDoorTriggerEvent;
        this.door_A4_3.doorTriggerEvent += this.OnDoorTriggerEvent;
        this.door_A4_4.doorTriggerEvent += this.OnDoorTriggerEvent;
        this.door_A4_Exit.doorTriggerEvent += this.OnDoorTriggerEvent;
        this.door_A5_Enter.doorTriggerEvent += this.OnDoorTriggerEvent;

        this.triggerBoxA4_1.AddTriggerBoxEvent(this.OnTriggerBoxEvent);
        this.triggerBoxA4_2.AddTriggerBoxEvent(this.OnTriggerBoxEvent);

        this.InitialziedGameMasterEvent();

        this.AddObserver(this);

        base.Initialized();
    }
    
    protected override void Start()
    {
        _ = DelayInitialized();
        base.Start();
    }
    protected override void Update()
    {
        Wave1 = this.enemyWaveManager1.enemyWaves.Count;
        this.numberOfEnemyWave1 = this.enemyWaveManager1.numberOfEnemy;
        wave2 = this.enemyWaveManager2.enemyWaves.Count;
        this.numberOfEnemyWave2 = this.enemyWaveManager2.numberOfEnemy;
        
        base.Update();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected void LateUpdate()
    {
        enemy_ObjectManager.ClearCorpseEnemyUpdate();
        enemyMask_ObjectManager.ClearCorpseEnemyUpdate();
        enemyMaskArmored_ObjectManager.ClearCorpseEnemyUpdate();
    }

    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(()=>true,"PrologueStartNodeSelector");

        gameMasterNodeCombine = new NodeCombine(()=> true);

        gameMasterModeNodeSelector = new NodeSelector(()=> true);
        gameMasterSectionNodeSelector = new NodeSelector(()=> true);

        waveBaseSection_1_Nodeleaf = new WaveBasedSectionNodeLeaf(this, enemyWaveManager1, 
            () => waveBaseSection_1_Nodeleaf.isEnable && enemyDirectirA3.allEnemiesAliveCount <= 2 
            && waveBaseSection_1_Nodeleaf._enemyWaveManager.waveIsClear == false);
        waveBaseSection_2_Nodeleaf = new WaveBasedSectionNodeLeaf(this, enemyWaveManager2, 
            () => waveBaseSection_2_Nodeleaf.isEnable && enemyDirectirA5.allEnemiesAliveCount <= 2 
            && waveBaseSection_2_Nodeleaf._enemyWaveManager.waveIsClear == false);
        freeRomeSectionNodeLeaf = new InGameLevelGameMasterNodeLeaf<PrologueLevelGameMaster>(this,()=> true);

        delayOpeningGameMasterNodeLeaf = new InGameLevelDelayOpeningLoad(this, () => base.isCompleteLoad == false);
        levelOpeningGameMasterNodeLeaf = new InGameLevelOpeningGameMasterNodeLeaf(this, openingUICanvas , () => levelOpeningGameMasterNodeLeaf.isComplete == false);
        levelGameOverGameMasterNodeLeaf = new InGameLevelGameOverGameMasterNodeLeaf(this, gameOverUICanvas, () => player.isDead);
        pauseInGameGameMasterNodeLeaf = new PauseInGameGameMasterNodeLeaf(this, pauseCanvasUI,
    () => pauseInGameGameMasterNodeLeaf.isPause);
        levelMisstionCompleteGameMasterNodeLeaf = new InGameLevelMisstionCompleteGameMasterNodeLeaf(this, missionCompleteUICanvas, () => enemyWaveManager2.waveIsClear);
        prologueInGameLevelGameplayGameMasterNodeLeaf = new InGameLevelGamplayGameMasterNodeLeaf<PrologueLevelGameMaster>(this,()=> true);

        startNodeSelector.AddtoChildNode(gameMasterNodeCombine);

        gameMasterNodeCombine.AddCombineNode(gameMasterSectionNodeSelector);
        gameMasterNodeCombine.AddCombineNode(gameMasterModeNodeSelector);

        gameMasterSectionNodeSelector.AddtoChildNode(waveBaseSection_1_Nodeleaf);
        gameMasterSectionNodeSelector.AddtoChildNode(waveBaseSection_2_Nodeleaf);
        gameMasterSectionNodeSelector.AddtoChildNode(freeRomeSectionNodeLeaf);

        gameMasterModeNodeSelector.AddtoChildNode(delayOpeningGameMasterNodeLeaf);
        gameMasterModeNodeSelector.AddtoChildNode(levelOpeningGameMasterNodeLeaf);
        gameMasterModeNodeSelector.AddtoChildNode(levelGameOverGameMasterNodeLeaf);
        gameMasterModeNodeSelector.AddtoChildNode(pauseInGameGameMasterNodeLeaf);
        gameMasterModeNodeSelector.AddtoChildNode(levelMisstionCompleteGameMasterNodeLeaf);
        gameMasterModeNodeSelector.AddtoChildNode(prologueInGameLevelGameplayGameMasterNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);

    }
    private async Task DelayInitialized() 
    {
        await Task.Yield();

        enemyDirectorA2.gameObject.SetActive(true);
        enemyDirectirA3.gameObject.SetActive(true);
        enemyDirectirA4.gameObject.SetActive(true);
        enemyDirectirA5.gameObject.SetActive(true);
    }

    #region WaveEvent
    private Enemy[] enemyA3After = new Enemy[4];

    private EnemyWaveManager enemyWaveManager1;

    private EnemyWaveManager enemyWaveManager2;
    private void InitializedWave()
    {
        EnemyWave[] enemyWaves = new EnemyWave[2];

        enemyWaves[0] = new EnemyWave(() => enemyDirectirA3.allEnemiesAliveCount <= 2, new EnemyWave.EnemyListSpawn[]
        {
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemy_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager,numberSpawn = 2 },
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemyMask_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager, numberSpawn = 2 }
        }
        , 3);

        enemyWaves[1] = new EnemyWave(() => enemyDirectirA3.allEnemiesAliveCount <= 2, new EnemyWave.EnemyListSpawn[]
        {
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemy_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager,numberSpawn = 2 },
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemyMaskArmored_ObjectManager, weaponObjectManager = this.ar15_weaponObjectManager,numberSpawn = 1 }
        }, 3);

        enemyWaveManager1 = new EnemyWaveManager(player,this.enemySpawnPointRooms_A3, enemyDirectirA3);
        enemyWaveManager1.AddEnemyWave(enemyWaves[0]);
        enemyWaveManager1.AddEnemyWave(enemyWaves[1]);

        EnemyWave[] enemyWaves2 = new EnemyWave[5];

        enemyWaves2[0] = new EnemyWave(() => enemyDirectirA5.allEnemiesAliveCount <= 2, new EnemyWave.EnemyListSpawn[]    
        {
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemy_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager,numberSpawn = 3 },
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemyMask_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager, numberSpawn = 2 },
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemyMaskArmored_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager, numberSpawn = 1 }
        }
        , 3);

        enemyWaves2[1] = new EnemyWave(() => enemyDirectirA5.allEnemiesAliveCount <= 2, new EnemyWave.EnemyListSpawn[]
        {
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemy_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager,numberSpawn = 1 },
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemyMask_ObjectManager, weaponObjectManager = this.ar15_weaponObjectManager, numberSpawn = 2 },
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemyMaskArmored_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager, numberSpawn = 1 }
        }
        , 3);

        enemyWaves2[2] = new EnemyWave(() => enemyDirectirA5.allEnemiesAliveCount <= 2, new EnemyWave.EnemyListSpawn[]
        {
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemy_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager,numberSpawn = 3 },
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemyMaskArmored_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager, numberSpawn = 2 }
        }
        , 3);

        enemyWaves2[3] = new EnemyWave(() => enemyDirectirA5.allEnemiesAliveCount <= 0, new EnemyWave.EnemyListSpawn[]
        {
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemy_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager,numberSpawn = 3 },
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemyMask_ObjectManager, weaponObjectManager = this.ar15Redot_weaponObjectManager, numberSpawn = 3 }
        }
        , 3);
        enemyWaves2[4] = new EnemyWave(() => enemyDirectirA5.allEnemiesAliveCount <= 1, new EnemyWave.EnemyListSpawn[]
       {
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemyMask_ObjectManager, weaponObjectManager = this.ar15_weaponObjectManager, numberSpawn = 1 },
            new EnemyWave.EnemyListSpawn{enemyObjectManager = this.enemyMaskArmored_ObjectManager, weaponObjectManager = this.ar15TacticalScope_weaponObjectManager,numberSpawn = 3 },
       }
       , 3);

        this.enemyWaveManager2 = new EnemyWaveManager(player,enemySpawnPointRooms_A5,enemyDirectirA5);
        this.enemyWaveManager2.AddEnemyWave(enemyWaves2[0]);
        this.enemyWaveManager2.AddEnemyWave(enemyWaves2[1]);
        this.enemyWaveManager2.AddEnemyWave(enemyWaves2[2]);
        this.enemyWaveManager2.AddEnemyWave(enemyWaves2[3]);
        this.enemyWaveManager2.AddEnemyWave(enemyWaves2[4]);


    }
    #endregion

    private void OnDoorTriggerEvent(Door door)
    {
        this.UpdateingEvent();
    }
    private void OnTriggerBoxEvent(Collider collider,TriggerBox triggerBox)
    {
        Debug.Log("BoxTrigger");
        if (triggerBox == this.triggerBoxA4_1)
        {
            Debug.Log("triggerBox == this.triggerBoxA4_1");
            isTriggerBox4_1BeenTrigger = true;
        }

        if(triggerBox == this.triggerBoxA4_2)
            isTriggerBox4_2BeenTrigger = true;



        this.UpdateingEvent();
    }
    protected void InitialziedGameMasterEvent()
    {
        gameMasterEvent.Add(() => door_A1.isOpen
        ,() => {
            enemySpawnPoint_A1.SpawnEnemy(enemy_ObjectManager,glock17_weaponObjectManager);
        });

        gameMasterEvent.Add(() => door_A2_Enter.isOpen
        , () => {
            enemySpawnPoint_A2[0].SpawnEnemy(enemy_ObjectManager, enemyDirectorA2 , glock17_weaponObjectManager);
            enemySpawnPoint_A2[1].SpawnEnemy(enemy_ObjectManager, enemyDirectorA2, glock17_weaponObjectManager);
            enemySpawnPoint_A2[2].SpawnEnemy(enemyMask_ObjectManager, enemyDirectorA2, glock17_weaponObjectManager);
        });

        gameMasterEvent.Add(()=> door_A3_EnterBelow.isOpen
        ,  () => {
            enemySpawnerPoint_A3_Before.SpawnEnemy(enemy_ObjectManager, ar15Optic_weaponObjectManager);
        });

        gameMasterEvent.Add(() => door_A4_Enter.isOpen
        , () => {
            enemySpawnPoint_A4_1.SpawnEnemy(enemyMask_ObjectManager, glock17_weaponObjectManager);
        });

        gameMasterEvent.Add(() => isTriggerBox4_1BeenTrigger
        , () =>
        {
            enemySpawnPoint_A4_2[0].SpawnEnemy(enemy_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_2[1].SpawnEnemy(enemy_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_2[2].SpawnEnemy(enemyMask_ObjectManager, enemyDirectirA4, ar15_weaponObjectManager);
        });

        gameMasterEvent.Add(() => door_A4_2.isOpen
        , () => {
            enemySpawnPoint_A4_3[0].SpawnEnemy(enemy_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_3[1].SpawnEnemy(enemy_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_3[2].SpawnEnemy(enemyMask_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_3[3].SpawnEnemy(enemyMask_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
        });

        gameMasterEvent.Add(() => isTriggerBox4_2BeenTrigger
        , () => {
            enemySpawnPoint_A4_4[0].SpawnEnemy(enemy_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_4[1].SpawnEnemy(enemyMask_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
        });

        gameMasterEvent.Add(() => door_A4_3.isOpen
        , () => {
            enemySpawnPoint_A4_5[0].SpawnEnemy(enemyMaskArmored_ObjectManager, enemyDirectirA4, ar15Redot_weaponObjectManager);
            enemySpawnPoint_A4_5[1].SpawnEnemy(enemyMaskArmored_ObjectManager, enemyDirectirA4, ar15TacticalScope_weaponObjectManager);
        });

        gameMasterEvent.Add(() => door_A4_4.isOpen
        , () =>{
            enemySpawnPoint_A4_6[0].SpawnEnemy(enemy_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_6[1].SpawnEnemy(enemy_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_6[2].SpawnEnemy(enemy_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
        });

        gameMasterEvent.Add(() => door_A4_Exit.isOpen
        , () => {
            enemySpawnerPoint_A3_After[0].SpawnEnemy(enemy_ObjectManager, enemyDirectirA3, glock17_weaponObjectManager,out enemyA3After[0]);
            enemySpawnerPoint_A3_After[1].SpawnEnemy(enemyMask_ObjectManager, enemyDirectirA3, glock17_weaponObjectManager, out enemyA3After[1]);
            enemySpawnerPoint_A3_After[2].SpawnEnemy(enemy_ObjectManager, enemyDirectirA3, glock17_weaponObjectManager, out enemyA3After[2]);
            enemySpawnerPoint_A3_After[3].SpawnEnemy(enemy_ObjectManager, enemyDirectirA3, glock17_weaponObjectManager, out enemyA3After[3]);
            waveBaseSection_1_Nodeleaf.isEnable = true;
            this.door_A3_Exit.isBeenInteractAble = false;
        });

        gameMasterEvent.Add(() => this.enemyWaveManager1.waveIsClear, 
            () => this.door_A3_Exit.isBeenInteractAble = true );

        gameMasterEvent.Add(() => this.door_A5_Enter.isOpen
        , () => 
        {
            enemySpawnPoint_A5[0].SpawnEnemy(enemy_ObjectManager, enemyDirectirA5, glock17_weaponObjectManager);
            enemySpawnPoint_A5[1].SpawnEnemy(enemy_ObjectManager, enemyDirectirA5, glock17_weaponObjectManager);
            enemySpawnPoint_A5[2].SpawnEnemy(enemy_ObjectManager, enemyDirectirA5, ar15_weaponObjectManager);
            enemySpawnPoint_A5[3].SpawnEnemy(enemy_ObjectManager, enemyDirectirA5, ar15_weaponObjectManager);
            waveBaseSection_2_Nodeleaf.isEnable = true;
        });
    }
   
    public void OnNotify<T>(InGameLevelGameMaster inGameLevelGameMaster, T var)
    {
        if(inGameLevelGameMaster == this 
            && var is WaveBasedSectionNodeLeaf enemyWavebase 
            )
        {
            if(enemyWavebase == waveBaseSection_1_Nodeleaf && enemyWavebase.curPhase == InGameLevelGameMasterNodeLeaf<InGameLevelGameMaster>.InGameLevelPhase.Exit)
                this.door_A3_Exit.isBeenInteractAble = true;
        }
    }

    
}

