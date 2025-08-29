using System;
using System.Collections.Generic;
using UnityEngine;

public class TemplateSceneInGameLevelGameMaster : InGameLevelGameMaster
{
    public override InGameLevelRestGameMasterNodeLeaf levelRestGameMasterNodeLeaf { get ; protected set ; }
    private TemplateSceneInGameLevel_Gameplay_Nodeleaf templateSceneInGameLevel_Gameplay_Nodeleaf { get; set; }
    [SerializeField] private Camera cameraMain;

    [SerializeField] private EnemyDirector enemyDirector;
    [SerializeField] private Enemy enemyMK1;

    public EnemySpawnPointRoom[] enemySpawnPointRoomWave;
    public EnemyObjectManager enemyObjectManager { get; set; }
    public Weapon ar15_MK1_origin;
    public WeaponObjectManager ar15_MK1_weaponObjManager { get; set; }
    public Weapon glock17_MK1_origin;
    public WeaponObjectManager glock17_MK1_weaponobjManager { get; set; }
    [SerializeField] private bool stopSpawning;
    protected override void Awake()
    {
        enemyObjectManager = new EnemyObjectManager(enemyMK1,cameraMain);
        ar15_MK1_weaponObjManager = new WeaponObjectManager(ar15_MK1_origin, cameraMain);
        glock17_MK1_weaponobjManager = new WeaponObjectManager(glock17_MK1_origin, cameraMain);
        base.Awake();
    }

    protected override void LateUpdate()
    {
        enemyObjectManager.ClearCorpseEnemyUpdate();
        ar15_MK1_weaponObjManager.ClearWeaponUpdate();
        glock17_MK1_weaponobjManager.ClearWeaponUpdate();
        base.LateUpdate();
    }
    private class TemplateSceneInGameLevel_Gameplay_Nodeleaf : InGameLevelGamplayGameMasterNodeLeaf<TemplateSceneInGameLevelGameMaster>
    {
        public EnemyWaveManager enemyWaveManager { get; set; }
        public TemplateSceneInGameLevel_Gameplay_Nodeleaf(TemplateSceneInGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
        {
            enemyWaveManager = new EnemyWaveManager(gameMaster.player.transform, gameMaster.enemySpawnPointRoomWave, gameMaster.enemyDirector);

            EnemyWave enemyWave1 = new EnemyWave(
                () => true
                , new EnemyWave.EnemyListSpawn[]
                {
                    new EnemyWave.EnemyListSpawn(gameMaster.enemyObjectManager,gameMaster.glock17_MK1_weaponobjManager,4),
                    new EnemyWave.EnemyListSpawn(gameMaster.enemyObjectManager,gameMaster.ar15_MK1_weaponObjManager,2)
                }
                );

            EnemyWave enemyWave2 = new EnemyWave(
                () => enemyWaveManager.numberOfEnemy <= 2
                , new EnemyWave.EnemyListSpawn[]
                {
                    new EnemyWave.EnemyListSpawn(gameMaster.enemyObjectManager,gameMaster.glock17_MK1_weaponobjManager,4)
                }
                , 2);

            EnemyWave enemyWave3 = new EnemyWave(
               () => enemyWaveManager.numberOfEnemy <= 2
               , new EnemyWave.EnemyListSpawn[]
               {
                    new EnemyWave.EnemyListSpawn(gameMaster.enemyObjectManager,gameMaster.glock17_MK1_weaponobjManager,2),
                    new EnemyWave.EnemyListSpawn(gameMaster.enemyObjectManager,gameMaster.ar15_MK1_weaponObjManager,2)
               }
               , 2);

            EnemyWave enemyWave4 = new EnemyWave(
              () => enemyWaveManager.numberOfEnemy <= 0
              , new EnemyWave.EnemyListSpawn[]
              {
                    new EnemyWave.EnemyListSpawn(gameMaster.enemyObjectManager,gameMaster.glock17_MK1_weaponobjManager,2),
                    new EnemyWave.EnemyListSpawn(gameMaster.enemyObjectManager,gameMaster.ar15_MK1_weaponObjManager,4)
              }
              , 5);

            EnemyWave enemyWave5 = new EnemyWave(
              () => enemyWaveManager.numberOfEnemy <= 2
              , new EnemyWave.EnemyListSpawn[]
              {
                    new EnemyWave.EnemyListSpawn(gameMaster.enemyObjectManager,gameMaster.ar15_MK1_weaponObjManager,4),
              }
              , 3);

            enemyWaveManager.AddEnemyWave(enemyWave1);
            enemyWaveManager.AddEnemyWave(enemyWave2);
            enemyWaveManager.AddEnemyWave(enemyWave3);
            enemyWaveManager.AddEnemyWave(enemyWave4);
            enemyWaveManager.AddEnemyWave(enemyWave5);
        }
        public override void Enter()
        {

            base.Enter();
        }
        public override void FixedUpdateNode()
        {
            base.FixedUpdateNode();
        }
        public override void UpdateNode()
        {
            enemyWaveManager.EnemyWaveUpdate();
            base.UpdateNode();
        }
        
        public override void Exit()
        {
            base.Exit();
        }
    }
    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true);
        templateSceneInGameLevel_Gameplay_Nodeleaf = new TemplateSceneInGameLevel_Gameplay_Nodeleaf(this, () => true);

        startNodeSelector.AddtoChildNode(templateSceneInGameLevel_Gameplay_Nodeleaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }
}

