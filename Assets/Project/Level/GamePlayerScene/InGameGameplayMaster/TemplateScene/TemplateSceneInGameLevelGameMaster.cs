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

    public EnemySpawnPointRoom enemySpawnPointRoom_1;
    public EnemySpawnPointRoom enemySpawnPointRoom_2;
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
        private bool isSpawnWave1;
        private float delaySpawn = 3;

        private List<Enemy> enemies;
        public TemplateSceneInGameLevel_Gameplay_Nodeleaf(TemplateSceneInGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
        {
            isSpawnWave1 = false;
            enemies = new List<Enemy>();
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
            if (isSpawnWave1 == false && delaySpawn <= 0 && gameMaster.stopSpawning == false && gameMaster.enemyObjectManager.clearEnemyList.Count <= 0)
            {
                
                gameMaster.enemySpawnPointRoom_1.SpawnEnemy(
                    gameMaster.enemyObjectManager
                    , gameMaster.enemyDirector
                    , gameMaster.ar15_MK1_weaponObjManager, false, out Enemy enemy1);

                gameMaster.enemySpawnPointRoom_2.SpawnEnemy(
                    gameMaster.enemyObjectManager
                    , gameMaster.enemyDirector
                    , gameMaster.glock17_MK1_weaponobjManager, false, out Enemy enemy2);

                enemies.Add(enemy1);
                enemies.Add(enemy2);
                isSpawnWave1 = true;
            }
            else
                Debug.Log("Delay spawn 1 = " + delaySpawn);
            int deadNum = enemies.Count;
            if (enemies.Count > 0)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.isDead)
                        deadNum--;
                }
                if (deadNum <= 0)
                {
                    enemies.Clear();
                    isSpawnWave1 = false;
                    delaySpawn = 3;
                }
            }

            delaySpawn = Mathf.Clamp(delaySpawn - Time.deltaTime, 0, delaySpawn);

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

