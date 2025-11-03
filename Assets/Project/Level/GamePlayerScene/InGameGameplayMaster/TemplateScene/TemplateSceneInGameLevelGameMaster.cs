using System;
using System.Collections.Generic;
using UnityEngine;

public class TemplateSceneInGameLevelGameMaster : InGameLevelGameMaster
{
    private TemplateSceneInGameLevel_Gameplay_Nodeleaf templateSceneInGameLevel_Gameplay_Nodeleaf { get; set; }
    [SerializeField] private CameraController cameraController;

    [SerializeField] private EnemyDirector enemyDirector;
    [SerializeField] private Enemy enemyMK1;

    [SerializeField] private List<EnemyWave> enemyWaves;

    public EnemySpawnPointRoom[] enemySpawnPointRoomWave;
    public EnemyObjectManager enemyObjectManager { get; set; }
    public Weapon ar15_MK1_origin;
    public WeaponObjectManager ar15_MK1_weaponObjManager { get; set; }
    public Weapon glock17_MK1_origin;
    public WeaponObjectManager glock17_MK1_weaponobjManager { get; set; }

    [SerializeField] private bool stopSpawning;

    [SerializeField] private ThirdPersonCinemachineCamera thirdPersonCinemachineCamera;
    public override void Initialized()
    {


        this.thirdPersonCinemachineCamera = cameraController.thirdPersonCinemachineCamera;

        base.Initialized();
    }

    [SerializeField] private float lookSensitivity;
    [SerializeField] private float adsSensitivity;

    [SerializeField] private float masterVolume;
    [SerializeField] private float musicVolume;
    [SerializeField] private float sfxVolume;
   
   
    protected  void LateUpdate()
    {
        lookSensitivity = dataBased.settingData.mouseSensitivivty;
        adsSensitivity = dataBased.settingData.mouseAimDownSightSensitivity;

        masterVolume = dataBased.settingData.volumeMaster;
        musicVolume = dataBased.settingData.volumeMusic;
        sfxVolume = dataBased.settingData.volumeEffect;

 
    }
    private class TemplateSceneInGameLevel_Gameplay_Nodeleaf : InGameLevelGamplayGameMasterNodeLeaf<TemplateSceneInGameLevelGameMaster>
    {

        public TemplateSceneInGameLevel_Gameplay_Nodeleaf(TemplateSceneInGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
        {
            
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
            //enemyWaveManager.EnemyWaveUpdate();
            
            base.UpdateNode();
        }
        
        public override void Exit()
        {
            base.Exit();
        }

        public override void RestartCheckPoint()
        {
            throw new NotImplementedException();
        }
    }
    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true);

        pausingSelector = new NodeSelector(() => this.menuInGameGameMasterNodeLeaf.isMenu);
        menuInGameGameMasterNodeLeaf = new MenuInGameGameMasterNodeLeaf(this, pauseCanvasUI, () => true);
        optionMenuSettingInGameGameMasterNode = new OptionMenuSettingInGameGameMasterNodeLeaf(this, optionCanvasUI, () => menuInGameGameMasterNodeLeaf.isTriggerToSetting);

        templateSceneInGameLevel_Gameplay_Nodeleaf = new TemplateSceneInGameLevel_Gameplay_Nodeleaf(this, () => true);



        startNodeSelector.AddtoChildNode(pausingSelector);
        startNodeSelector.AddtoChildNode(templateSceneInGameLevel_Gameplay_Nodeleaf);

        pausingSelector.AddtoChildNode(optionMenuSettingInGameGameMasterNode);
        pausingSelector.AddtoChildNode(menuInGameGameMasterNodeLeaf);

        _nodeManagerBehavior.SearchingNewNode(this);
    }

   

   
}

