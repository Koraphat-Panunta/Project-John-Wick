using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.Video;
using TMPro;

public class PrologueLevelGameMaster : InGameLevelGameMaster,IGameLevelMasterObserver,IObserverEnemy
{
    [SerializeField] private OpeningUICanvas openingUICanvas;
    [SerializeField] private GameOverUICanvas gameOverUICanvas;
    [SerializeField] private MissionCompleteUICanvas missionCompleteUICanvas;

    public NodeManagerPortable gameMasterSectionNodeManagerPortable;

    public WaveBasedSectionNodeLeaf waveBaseSection_1_Nodeleaf;
    public WaveBasedSectionNodeLeaf waveBaseSection_2_Nodeleaf;

    public InGameLevelGameMasterNodeLeaf<PrologueLevelGameMaster> freeRomeSectionNodeLeaf;
    public InGameLevelOpeningGameMasterNodeLeaf levelOpeningGameMasterNodeLeaf { get; protected set; }
    public VideoTutorialPlayGameMasterNodeLeaf videoTutorialPlayGameMasterNodeLeaf { get; protected set; }
    public InGameLevelMisstionCompleteGameMasterNodeLeaf levelMisstionCompleteGameMasterNodeLeaf { get; protected set; }
    public InGameLevelGameOverGameMasterNodeLeaf levelGameOverGameMasterNodeLeaf { get; protected set; }
    public InGameLevelDelayOpeningLoad delayOpeningGameMasterNodeLeaf { get ; protected set ; }
    protected InGameLevelGamplayGameMasterNodeLeaf<PrologueLevelGameMaster> prologueInGameLevelGameplayGameMasterNodeLeaf;

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

    public Enemy enemyTutorialOrigin;
    public Enemy enemyMaskTutorialOrigin;

    public Enemy enemyOrigin;
    public Enemy enemyMaskOrigin;
    public Enemy enemyMaskArmordOrigin;

    public EnemyObjectManager enemy_ObjectManager;
    public EnemyObjectManager enemyMask_ObjectManager;
    public EnemyObjectManager enemyMaskArmored_ObjectManager;

    public EnemyObjectManager enemy_Tutorial_ObjectManager;
    public EnemyObjectManager enemy_Mask_Tutorial_ObjectManager;

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



    public TriggerBox triggerBox_Area1_Tutorial_MoveCrouchSprint;
    [SerializeField] private bool isTriggerBox_Area1_Tutorial_MoveCrouchSprint_BeenTrigger;
    public TriggerBox triggerBox_Area1_Tutorial_AimShootReload;
    [SerializeField] private bool isTriggerBox_Area1_Tutorial_AimShootReload_BeenTrigger;
    public TriggerBox triggerBox_Area2_Tutorial_DodgeVaulting;
    [SerializeField] private bool isTriggerBox_Area2_Tutorial_DodgeVaulting_BeenTrigger;
    public TriggerBox triggerBox_Area3_Tutorial_GunFuHit3;
    [SerializeField] private bool isTriggerBox_Area3_Tutorial_GunFuHit3_BeenTrigger;
    public TriggerBox triggerBox_Area4_Tutorial_GunFuRestrict;
    [SerializeField] private bool isTriggerBox_Area4_Tutorial_GunFuRestrict_BeenTrigger;
    public TriggerBox triggerBox_Area4_SpawnEnemy_1;
    [SerializeField] private bool isTriggerBox_Area4_SpawnEnemy_1_BeenTrigger;
    public TriggerBox triggerBox_Area4_Tutorial_WeaponDisarm;
    [SerializeField] private bool isTriggerBox_Area4_Tutorial_WeaponDisarm_BeenTrigger;

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

    [SerializeField] private CameraController cameraController;

    [SerializeField] private int Wave1;
    [SerializeField] private int numberOfEnemyWave1;
    [SerializeField] private int wave2;
    [SerializeField] private int numberOfEnemyWave2;

    [SerializeField] private VideoTutorialUI videoTutorialUI;

    [SerializeField] private VideoClip moveCrouchSprint_Tutorial_Video;
    [SerializeField] private string moveCrouchSprint_Tutorial_Text;
    [SerializeField] private VideoClip aimShootReload_Tutorial_Video;
    [SerializeField] private string aimShootReload_Tutorial_Text;
    [SerializeField] private VideoClip execute_Tutorial_Video;
    [SerializeField] private string execute_Tutorial_Text;
    [SerializeField] private VideoClip dodgeVaulting_Tutorial_Video;
    [SerializeField] private string dodgeVaulting_Tutorial_Text;
    [SerializeField] private VideoClip gunFuHit3_Tutorial_Video;
    [SerializeField] private string gunFuHit3_Tutorial_Text;
    [SerializeField] private VideoClip gunFuRestrict_Tutorial_Video;
    [SerializeField] private string gunFuRestrict_Tutorial_Text;
    //[SerializeField] private VideoClip groundControl_Tutorial_Video;
    //[SerializeField] private string groundControl_Tutorial_Text;
    [SerializeField] private VideoClip weaponDisarm_Tutorial_Video;
    [SerializeField] private string weaponDisarm_Tutorial_Text;

    [SerializeField] private Enemy executedEnemyTutorial;
    public override void Initialized()
    {

        this.enemy_Tutorial_ObjectManager = new EnemyObjectManager(enemyTutorialOrigin,this.cameraController.cameraMain,2,5);
        this.enemy_Mask_Tutorial_ObjectManager = new EnemyObjectManager(enemyMaskTutorialOrigin,this.cameraController.cameraMain,2,5);

        this.enemy_ObjectManager = new EnemyObjectManager(enemyOrigin, this.cameraController.cameraMain);
        this.enemyMask_ObjectManager = new EnemyObjectManager(enemyMaskOrigin, this.cameraController.cameraMain);
        this.enemyMaskArmored_ObjectManager = new EnemyObjectManager(enemyMaskArmordOrigin, this.cameraController.cameraMain);

        this.glock17_weaponObjectManager = new WeaponObjectManager(glock17_Origin, this.cameraController.cameraMain);
        this.ar15_weaponObjectManager = new WeaponObjectManager(ar15_Origin, this.cameraController.cameraMain);
        this.ar15Optic_weaponObjectManager = new WeaponObjectManager(ar15Optic_Origin, this.cameraController.cameraMain);
        this.ar15Redot_weaponObjectManager = new WeaponObjectManager(ar15Redot_Origin, this.cameraController.cameraMain);
        this.ar15TacticalScope_weaponObjectManager = new WeaponObjectManager(ar15TacticalScope_Origin, this.cameraController.cameraMain);


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


        this.triggerBox_Area4_SpawnEnemy_1.AddTriggerBoxEvent(this.OnTriggerBoxEvent);
        this.triggerBox_Area1_Tutorial_MoveCrouchSprint.AddTriggerBoxEvent(this.OnTriggerBoxEvent);
        this.triggerBox_Area1_Tutorial_AimShootReload.AddTriggerBoxEvent(this.OnTriggerBoxEvent);
        this.triggerBox_Area2_Tutorial_DodgeVaulting.AddTriggerBoxEvent(this.OnTriggerBoxEvent);
        this.triggerBox_Area3_Tutorial_GunFuHit3.AddTriggerBoxEvent(this.OnTriggerBoxEvent);
        this.triggerBox_Area4_Tutorial_GunFuRestrict.AddTriggerBoxEvent(this.OnTriggerBoxEvent);
        this.triggerBox_Area4_Tutorial_WeaponDisarm.AddTriggerBoxEvent(this.OnTriggerBoxEvent);

        this.door_A3_Exit.isLocked = true;
        this.door_A4_Exit.isLocked = true;

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
        this.gameMasterSectionNodeManagerPortable.UpdateNode();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        this.gameMasterSectionNodeManagerPortable.FixedUpdateNode();
    }
    protected void LateUpdate()
    {
        this.enemy_Tutorial_ObjectManager.ClearCorpseEnemyUpdate();
        this.enemy_Mask_Tutorial_ObjectManager.ClearCorpseEnemyUpdate();

        this.enemy_ObjectManager.ClearCorpseEnemyUpdate();
        this.enemyMask_ObjectManager.ClearCorpseEnemyUpdate();
        this.enemyMaskArmored_ObjectManager.ClearCorpseEnemyUpdate();
    }

    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(()=>true,"PrologueStartNodeSelector");


        delayOpeningGameMasterNodeLeaf = new InGameLevelDelayOpeningLoad(this, () => base.isCompleteLoad == false);
        levelOpeningGameMasterNodeLeaf = new InGameLevelOpeningGameMasterNodeLeaf(this, openingUICanvas , () => levelOpeningGameMasterNodeLeaf.isComplete == false);
        videoTutorialPlayGameMasterNodeLeaf = new VideoTutorialPlayGameMasterNodeLeaf(this, () => videoTutorialPlayGameMasterNodeLeaf.isPlaying, this.videoTutorialUI);
        levelGameOverGameMasterNodeLeaf = new InGameLevelGameOverGameMasterNodeLeaf(this, gameOverUICanvas, () => player.isDead);
        pausingSelector = new NodeSelector(() => this.menuInGameGameMasterNodeLeaf.isMenu);
        menuInGameGameMasterNodeLeaf = new MenuInGameGameMasterNodeLeaf(this, pauseCanvasUI, () => true);
        optionMenuSettingInGameGameMasterNode = new OptionMenuSettingInGameGameMasterNodeLeaf(this, optionCanvasUI, () => menuInGameGameMasterNodeLeaf.isTriggerToSetting);

        levelMisstionCompleteGameMasterNodeLeaf = new InGameLevelMisstionCompleteGameMasterNodeLeaf(this, missionCompleteUICanvas, () => enemyWaveManager2.waveIsClear);
        prologueInGameLevelGameplayGameMasterNodeLeaf = new InGameLevelGamplayGameMasterNodeLeaf<PrologueLevelGameMaster>(this,()=> true);

        startNodeSelector.AddtoChildNode(delayOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(videoTutorialPlayGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelGameOverGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(pausingSelector);
        startNodeSelector.AddtoChildNode(levelMisstionCompleteGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(prologueInGameLevelGameplayGameMasterNodeLeaf);

        pausingSelector.AddtoChildNode(optionMenuSettingInGameGameMasterNode);
        pausingSelector.AddtoChildNode(menuInGameGameMasterNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);

        this.InitialziedGameMasterSectionNodeManager();
    }

    private void InitialziedGameMasterSectionNodeManager()
    {
        this.gameMasterSectionNodeManagerPortable = new NodeManagerPortable();
        this.gameMasterSectionNodeManagerPortable.InitialzedOuterNode(
            () => 
            {
                waveBaseSection_1_Nodeleaf = new WaveBasedSectionNodeLeaf(this, enemyWaveManager1,
                    () => waveBaseSection_1_Nodeleaf.isEnable && enemyDirectirA3.allEnemiesAliveCount <= 2
                    && waveBaseSection_1_Nodeleaf._enemyWaveManager.waveIsClear == false);
                waveBaseSection_2_Nodeleaf = new WaveBasedSectionNodeLeaf(this, enemyWaveManager2,
                    () => waveBaseSection_2_Nodeleaf.isEnable && enemyDirectirA5.allEnemiesAliveCount <= 2
                    && waveBaseSection_2_Nodeleaf._enemyWaveManager.waveIsClear == false);
                freeRomeSectionNodeLeaf = new InGameLevelGameMasterNodeLeaf<PrologueLevelGameMaster>(this, () => true);

                this.gameMasterSectionNodeManagerPortable.startNodeSelector.AddtoChildNode(waveBaseSection_1_Nodeleaf);
                this.gameMasterSectionNodeManagerPortable.startNodeSelector.AddtoChildNode(waveBaseSection_2_Nodeleaf);
                this.gameMasterSectionNodeManagerPortable.startNodeSelector.AddtoChildNode(freeRomeSectionNodeLeaf);
            });

        base.parallelNodeManahger.Add(gameMasterSectionNodeManagerPortable);

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

        enemyWaves[0] = new EnemyWave(() => enemyDirectirA3.allEnemiesAliveCount <= 1, new EnemyListSpawn[]
        {
            new EnemyListSpawn{enemyObjectManager = this.enemy_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager,numberSpawn = 2 },
            new EnemyListSpawn{enemyObjectManager = this.enemyMask_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager, numberSpawn = 1 }
        }
        , 3);

        enemyWaves[1] = new EnemyWave(() => enemyDirectirA3.allEnemiesAliveCount <= 1, new EnemyListSpawn[]
        {
            new EnemyListSpawn{enemyObjectManager = this.enemy_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager,numberSpawn = 2 },
            new EnemyListSpawn{enemyObjectManager = this.enemyMaskArmored_ObjectManager, weaponObjectManager = this.ar15_weaponObjectManager,numberSpawn = 1 }
        }, 3);

        enemyWaveManager1 = new EnemyWaveManager(player,this.enemySpawnPointRooms_A3, enemyDirectirA3);
        enemyWaveManager1.AddEnemyWave(enemyWaves[0]);
        enemyWaveManager1.AddEnemyWave(enemyWaves[1]);

        EnemyWave[] enemyWaves2 = new EnemyWave[8];

        enemyWaves2[0] = new EnemyWave(() => enemyDirectirA5.allEnemiesAliveCount <= 2, new EnemyListSpawn[]    
        {
            new EnemyListSpawn{enemyObjectManager = this.enemy_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager,numberSpawn = 2 },
            new EnemyListSpawn{enemyObjectManager = this.enemyMask_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager, numberSpawn = 1 },
        }
        , 1);

        enemyWaves2[1] = new EnemyWave(() => enemyDirectirA5.allEnemiesAliveCount <= 2, new EnemyListSpawn[]
        {
            new EnemyListSpawn{enemyObjectManager = this.enemy_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager,numberSpawn = 1 },
            new EnemyListSpawn{enemyObjectManager = this.enemyMask_ObjectManager, weaponObjectManager = this.ar15_weaponObjectManager, numberSpawn = 1 },
            
        }
        , 0);

        enemyWaves2[2] = new EnemyWave(() => enemyDirectirA5.allEnemiesAliveCount <= 2, new EnemyListSpawn[]
        {
            new EnemyListSpawn{enemyObjectManager = this.enemy_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager,numberSpawn = 1 },
            new EnemyListSpawn{enemyObjectManager = this.enemyMask_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager,numberSpawn = 1 },
            new EnemyListSpawn{enemyObjectManager = this.enemyMaskArmored_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager, numberSpawn = 1 }
        }
        , 1);

        enemyWaves2[3] = new EnemyWave(() => enemyDirectirA5.allEnemiesAliveCount <= 0, new EnemyListSpawn[]
        {
            new EnemyListSpawn{enemyObjectManager = this.enemy_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager,numberSpawn = 3 },
            new EnemyListSpawn{enemyObjectManager = this.enemyMask_ObjectManager, weaponObjectManager = this.ar15Redot_weaponObjectManager, numberSpawn = 2 }
        }
        , 3);
        enemyWaves2[4] = new EnemyWave(() => enemyDirectirA5.allEnemiesAliveCount <= 2, new EnemyListSpawn[]
       {
            new EnemyListSpawn{enemyObjectManager = this.enemyMask_ObjectManager, weaponObjectManager = this.ar15_weaponObjectManager, numberSpawn = 3 },
            new EnemyListSpawn{enemyObjectManager = this.enemyMask_ObjectManager, weaponObjectManager = this.ar15TacticalScope_weaponObjectManager,numberSpawn = 1 },
       }
       , 3);
        enemyWaves2[5] = new EnemyWave(() => enemyDirectirA5.allEnemiesAliveCount <= 1, new EnemyListSpawn[]
       {
            new EnemyListSpawn{enemyObjectManager = this.enemyMask_ObjectManager, weaponObjectManager = this.ar15_weaponObjectManager, numberSpawn = 2 },
            new EnemyListSpawn{enemyObjectManager = this.enemyMaskArmored_ObjectManager, weaponObjectManager = this.ar15TacticalScope_weaponObjectManager,numberSpawn = 1 },
       }
       , 2);
        enemyWaves2[6] = new EnemyWave(() => enemyDirectirA5.allEnemiesAliveCount <= 1, new EnemyListSpawn[]
       {
            new EnemyListSpawn{enemyObjectManager = this.enemy_ObjectManager, weaponObjectManager = this.glock17_weaponObjectManager, numberSpawn = 3 },
            new EnemyListSpawn{enemyObjectManager = this.enemyMask_ObjectManager, weaponObjectManager = this.ar15Redot_weaponObjectManager, numberSpawn = 2 },
            new EnemyListSpawn{enemyObjectManager = this.enemyMaskArmored_ObjectManager, weaponObjectManager = this.ar15TacticalScope_weaponObjectManager,numberSpawn = 1 },
       }
       , 3);
        enemyWaves2[7] = new EnemyWave(() => enemyDirectirA5.allEnemiesAliveCount <= 1, new EnemyListSpawn[]
      {
            new EnemyListSpawn{enemyObjectManager = this.enemyMask_ObjectManager, weaponObjectManager = this.ar15Redot_weaponObjectManager, numberSpawn = 2 },
            new EnemyListSpawn{enemyObjectManager = this.enemyMaskArmored_ObjectManager, weaponObjectManager = this.ar15TacticalScope_weaponObjectManager,numberSpawn = 3 },
      }
      , 3);

        this.enemyWaveManager2 = new EnemyWaveManager(player,enemySpawnPointRooms_A5,enemyDirectirA5);
        this.enemyWaveManager2.AddEnemyWave(enemyWaves2[0]);
        this.enemyWaveManager2.AddEnemyWave(enemyWaves2[1]);
        this.enemyWaveManager2.AddEnemyWave(enemyWaves2[2]);
        this.enemyWaveManager2.AddEnemyWave(enemyWaves2[3]);
        this.enemyWaveManager2.AddEnemyWave(enemyWaves2[4]);
        this.enemyWaveManager2.AddEnemyWave(enemyWaves2[5]);
        this.enemyWaveManager2.AddEnemyWave(enemyWaves2[6]);
        this.enemyWaveManager2.AddEnemyWave(enemyWaves2[7]);


    }
    #endregion

    private void OnDoorTriggerEvent(Door door)
    {
        this.UpdateingEvent();
    }
    private void OnTriggerBoxEvent(Collider collider,TriggerBox triggerBox)
    {
        
       
        if(triggerBox == this.triggerBox_Area1_Tutorial_MoveCrouchSprint)
            this.isTriggerBox_Area1_Tutorial_MoveCrouchSprint_BeenTrigger = true;

        if(triggerBox == this.triggerBox_Area1_Tutorial_AimShootReload)
            this.isTriggerBox_Area1_Tutorial_AimShootReload_BeenTrigger = true;

        if(triggerBox == this.triggerBox_Area2_Tutorial_DodgeVaulting)
            this.isTriggerBox_Area2_Tutorial_DodgeVaulting_BeenTrigger = true;

        if(triggerBox == this.triggerBox_Area3_Tutorial_GunFuHit3)
            this.isTriggerBox_Area3_Tutorial_GunFuHit3_BeenTrigger = true;

        if(triggerBox == this.triggerBox_Area4_Tutorial_GunFuRestrict)
            this.isTriggerBox_Area4_Tutorial_GunFuRestrict_BeenTrigger = true;

        if(triggerBox == this.triggerBox_Area4_Tutorial_WeaponDisarm)
            this.isTriggerBox_Area4_Tutorial_WeaponDisarm_BeenTrigger = true;

        if(triggerBox == this.triggerBox_Area4_SpawnEnemy_1)
            isTriggerBox_Area4_SpawnEnemy_1_BeenTrigger = true;



        this.UpdateingEvent();
    }
    protected void InitialziedGameMasterEvent()
    {
        gameMasterEvent.Add(() => door_A1.isOpen
        ,() => {
            enemySpawnPoint_A1.SpawnEnemy(enemy_Tutorial_ObjectManager,glock17_weaponObjectManager);
            if(gameManager != null)
                gameManager.soundTrackManager.PlaySoundTrack(gameManager.soundTrackManager.prologueTrack);
        });

        gameMasterEvent.Add(() => door_A2_Enter.isOpen
        , () => {
            enemySpawnPoint_A2[0].SpawnEnemy(enemy_ObjectManager, enemyDirectorA2 , glock17_weaponObjectManager);
            enemySpawnPoint_A2[1].SpawnEnemy(enemy_ObjectManager, enemyDirectorA2, glock17_weaponObjectManager);
            enemySpawnPoint_A2[2].SpawnEnemy(enemy_Mask_Tutorial_ObjectManager, enemyDirectorA2, glock17_weaponObjectManager,out executedEnemyTutorial);
            this.executedEnemyTutorial.AddObserver(this);
        });

        gameMasterEvent.Add(()=> door_A3_EnterBelow.isOpen
        ,  () => {
            enemySpawnerPoint_A3_Before.SpawnEnemy(enemy_Mask_Tutorial_ObjectManager, ar15Optic_weaponObjectManager);
        });

        gameMasterEvent.Add(() => door_A4_Enter.isOpen
        , () => {
            enemySpawnPoint_A4_1.SpawnEnemy(enemy_Tutorial_ObjectManager, glock17_weaponObjectManager);
        });
        gameMasterEvent.Add(() => this.isTriggerBox_Area1_Tutorial_MoveCrouchSprint_BeenTrigger
        ,() =>    
        {
            videoTutorialPlayGameMasterNodeLeaf.SetVideoPlayer(this.moveCrouchSprint_Tutorial_Video);
            videoTutorialPlayGameMasterNodeLeaf.SetTextTutorial(this.moveCrouchSprint_Tutorial_Text);
            videoTutorialPlayGameMasterNodeLeaf.isPlaying = true;
        });
        gameMasterEvent.Add(() => this.isTriggerBox_Area1_Tutorial_AimShootReload_BeenTrigger
        , () =>
        {
            videoTutorialPlayGameMasterNodeLeaf.SetVideoPlayer(this.aimShootReload_Tutorial_Video);
            videoTutorialPlayGameMasterNodeLeaf.SetTextTutorial(this.aimShootReload_Tutorial_Text);
            videoTutorialPlayGameMasterNodeLeaf.isPlaying = true;
        });
        gameMasterEvent.Add(() => executedEnemyTutorial != null && executedEnemyTutorial.isStagger
        , () => 
        {
            videoTutorialPlayGameMasterNodeLeaf.SetVideoPlayer(this.execute_Tutorial_Video);
            videoTutorialPlayGameMasterNodeLeaf.SetTextTutorial(this.execute_Tutorial_Text);
            videoTutorialPlayGameMasterNodeLeaf.isPlaying = true;
        });
        gameMasterEvent.Add(()=> this.isTriggerBox_Area2_Tutorial_DodgeVaulting_BeenTrigger
        , () => 
        {
            videoTutorialPlayGameMasterNodeLeaf.SetVideoPlayer(this.dodgeVaulting_Tutorial_Video);
            videoTutorialPlayGameMasterNodeLeaf.SetTextTutorial(this.dodgeVaulting_Tutorial_Text);
            videoTutorialPlayGameMasterNodeLeaf.isPlaying = true;
        });
        gameMasterEvent.Add(() => this.isTriggerBox_Area3_Tutorial_GunFuHit3_BeenTrigger
        , () => 
        {
            videoTutorialPlayGameMasterNodeLeaf.SetVideoPlayer(this.gunFuHit3_Tutorial_Video);
            videoTutorialPlayGameMasterNodeLeaf.SetTextTutorial(this.gunFuHit3_Tutorial_Text);
            videoTutorialPlayGameMasterNodeLeaf.isPlaying = true;
        });
        gameMasterEvent.Add(() => this.isTriggerBox_Area4_Tutorial_GunFuRestrict_BeenTrigger
        , () => 
        {
            enemySpawnPoint_A4_2[0].SpawnEnemy(enemy_Mask_Tutorial_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_2[1].SpawnEnemy(enemy_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_2[2].SpawnEnemy(enemy_Tutorial_ObjectManager, enemyDirectirA4, ar15_weaponObjectManager);

            videoTutorialPlayGameMasterNodeLeaf.SetVideoPlayer(this.gunFuRestrict_Tutorial_Video);
            videoTutorialPlayGameMasterNodeLeaf.SetTextTutorial(this.gunFuRestrict_Tutorial_Text);
            videoTutorialPlayGameMasterNodeLeaf.isPlaying = true;
        });
     

        gameMasterEvent.Add(() => door_A4_2.isOpen
        , () => {

            //videoTutorialPlayGameMasterNodeLeaf.SetVideoPlayer(groundControl_Tutorial_Video);
            //videoTutorialPlayGameMasterNodeLeaf.SetTextTutorial(groundControl_Tutorial_Text);
            //videoTutorialPlayGameMasterNodeLeaf.isPlaying = true;

            enemySpawnPoint_A4_3[0].SpawnEnemy(enemy_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_3[1].SpawnEnemy(enemy_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_3[2].SpawnEnemy(enemyMask_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_3[3].SpawnEnemy(enemyMask_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
        });

        gameMasterEvent.Add(() => isTriggerBox_Area4_SpawnEnemy_1_BeenTrigger
        , () => {
            enemySpawnPoint_A4_4[0].SpawnEnemy(enemy_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_4[1].SpawnEnemy(enemyMask_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
        });

        gameMasterEvent.Add(() => door_A4_3.isOpen
        , () => {

            videoTutorialPlayGameMasterNodeLeaf.SetVideoPlayer(weaponDisarm_Tutorial_Video);
            videoTutorialPlayGameMasterNodeLeaf.SetTextTutorial(weaponDisarm_Tutorial_Text);
            videoTutorialPlayGameMasterNodeLeaf.isPlaying = true;

            enemySpawnPoint_A4_5[0].SpawnEnemy(enemyMaskArmored_ObjectManager, enemyDirectirA4, ar15Redot_weaponObjectManager);
            enemySpawnPoint_A4_5[1].SpawnEnemy(enemyMaskArmored_ObjectManager, enemyDirectirA4, ar15TacticalScope_weaponObjectManager);
        });

        gameMasterEvent.Add(() => door_A4_4.isOpen
        , () =>{
            enemySpawnPoint_A4_6[0].SpawnEnemy(enemy_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_6[1].SpawnEnemy(enemy_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            enemySpawnPoint_A4_6[2].SpawnEnemy(enemy_ObjectManager, enemyDirectirA4, glock17_weaponObjectManager);
            this.door_A4_Exit.isLocked = false;
        });

        gameMasterEvent.Add(() => door_A4_Exit.isOpen
        , () => {
            enemySpawnerPoint_A3_After[0].SpawnEnemy(enemy_ObjectManager, enemyDirectirA3, glock17_weaponObjectManager,out enemyA3After[0]);
            enemySpawnerPoint_A3_After[1].SpawnEnemy(enemyMask_ObjectManager, enemyDirectirA3, glock17_weaponObjectManager, out enemyA3After[1]);
            enemySpawnerPoint_A3_After[2].SpawnEnemy(enemy_ObjectManager, enemyDirectirA3, glock17_weaponObjectManager, out enemyA3After[2]);
            enemySpawnerPoint_A3_After[3].SpawnEnemy(enemy_ObjectManager, enemyDirectirA3, glock17_weaponObjectManager, out enemyA3After[3]);
            waveBaseSection_1_Nodeleaf.isEnable = true;
        });

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
        if(var is WaveBasedSectionNodeLeaf waveBasedSectionNodeLeaf
            && waveBasedSectionNodeLeaf == waveBaseSection_1_Nodeleaf 
            && waveBasedSectionNodeLeaf.curPhase == InGameLevelGameMasterNodeLeaf<InGameLevelGameMaster>.InGameLevelPhase.Exit)
        {
            if (alreadyNotify == false)
            {
                this.door_A3_Exit.isLocked = false;
                this.door_A3_Exit.Open();
                StartCoroutine(LookAtUnlockDoor());
                alreadyNotify = true;
            }
        }

    }
    private bool alreadyNotify = false;
    private IEnumerator LookAtUnlockDoor()
    {
        float timer = 2.5f;
        float l = 0;
        Vector3 lookPosition = cameraController.thirdPersonCinemachineCamera.targetLookTarget.position;
        while(timer > 0)
        {
            lookPosition = Vector3.Lerp(lookPosition,this.door_A3_Exit.transform.position,Mathf.Clamp01(l));
            cameraController.thirdPersonCinemachineCamera.InputRotateCamera(lookPosition, Vector3.up);
            l += Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    public void Notify<T>(Enemy enemy, T node)
    {
        if(enemy == executedEnemyTutorial && enemy.isStagger)
        {
            UpdateingEvent();
            enemy.RemoveObserver(this);
        }
    }
}

