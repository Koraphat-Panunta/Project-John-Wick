using System;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenuSettingInGameGameMasterNodeLeaf : GameMasterNodeLeaf,INodeLeafTransitionAble
{
    private OptionUICanvas optionUICanvas;
    private bool isTriggerExit;
    private OptionMenuSector curMenuSector;
    private OptionMenuSector controlMenuSector;
    private OptionMenuSector audioMenuSector;

    public INodeManager nodeManager { get; set; }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    public OptionMenuSettingInGameGameMasterNodeLeaf(GameMaster gameMaster,OptionUICanvas optionUICanvas, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        this.optionUICanvas = optionUICanvas;
        
        this.controlMenuSector = new ControlSettingMenuSector(this.optionUICanvas,this.optionUICanvas.controlSettingOptionDisplay,gameMaster);
        this.audioMenuSector = new AudioSettingMenuSector(this.optionUICanvas,this.optionUICanvas.audioSettingOptionDisplay,gameMaster);

        this.optionUICanvas.controlSettingSelectButton.onClick.AddListener(this.SelectControlSetting);
        this.optionUICanvas.audioSettingSelectButton.onClick.AddListener(this.SelectAudioSetting);
        this.optionUICanvas.backButton.onClick.AddListener(this.TriggerExit);

        this.nodeManager = gameMaster;
        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
    }

    public override void Enter()
    {
        Debug.Log("Option Enter");

        Cursor.lockState = CursorLockMode.None;
        
        this.optionUICanvas.gameObject.SetActive(true);
        this.nodeLeafTransitionBehavior.TransitionAbleAll(this);
        this.SelectControlSetting();
    }

    public override void Exit()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isTriggerExit = false;
        if (curMenuSector != null)
            curMenuSector.Exit();

        this.optionUICanvas.gameObject.SetActive(false);
    }
    public override void UpdateNode()
    {
        this.TransitioningCheck();
    }
    public override void FixedUpdateNode()
    {
        
    }
    protected void TriggerExit() => isTriggerExit = true;
    public override bool IsReset()
    {
        return isTriggerExit;
    }
    public override bool IsComplete()
    {
        return false;
    }
    protected void ChangeOptionSettingSector(OptionMenuSector optionMenuSector)
    {
        if(curMenuSector != null)
            curMenuSector.Exit();

        curMenuSector = optionMenuSector;

        curMenuSector.Enter();
    }
    protected void SelectControlSetting() => this.ChangeOptionSettingSector(this.controlMenuSector);
    protected void SelectAudioSetting() => this.ChangeOptionSettingSector(this.audioMenuSector);

    public bool TransitioningCheck() => nodeLeafTransitionBehavior.TransitioningCheck(this);


    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);
    

    protected abstract class OptionMenuSector
    {
        protected OptionUICanvas optionUICanvas;
        protected DataBased savedDataBased;
        protected GameMaster gameMaster;
        protected OptionUIDisplayer optionUIDisplayer;
        public OptionMenuSector(OptionUICanvas optionUICanvas,OptionUIDisplayer optionUIDisplayer,GameMaster gameMaster)
        {
            this.optionUICanvas = optionUICanvas;
            this.savedDataBased = new DataBased();
            this.optionUIDisplayer = optionUIDisplayer;
            this.gameMaster = gameMaster;
        }
        public void Enter()
        {
            this.Apply_GameMasterData_To_SaveData();
            this.optionUICanvas.ChangeOptionUISector(this.optionUIDisplayer,this.savedDataBased);
            this.optionUICanvas.applyButton.onClick.AddListener(this.Apply_GameMasterData_To_SaveData);
        }

        public void Exit()
        {
            this.Apply_SaveData_To_GameMasterData();
            this.optionUICanvas.applyButton.onClick.RemoveListener(this.Apply_GameMasterData_To_SaveData);
        }
        protected abstract void Apply_GameMasterData_To_SaveData();
        protected abstract void Apply_SaveData_To_GameMasterData();

        public abstract void ResetToDefault();
       
    }
    protected class ControlSettingMenuSector : OptionMenuSector
    {
        public ControlSettingMenuSector(OptionUICanvas optionUICanvas, OptionUIDisplayer optionUIDisplayer, GameMaster gameMaster) : base(optionUICanvas, optionUIDisplayer, gameMaster)
        {
            optionUICanvas.mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChange);
            optionUICanvas.aimSensitivitySlider.onValueChanged.AddListener(OnMouseAimDownSightSensitivityChange);
        }

        public override void ResetToDefault()
        {
            
        }

        private void OnMouseSensitivityChange(float value)
        {
            gameMaster.dataBased.settingData.mouseSensitivivty = value;
        }
        private void OnMouseAimDownSightSensitivityChange(float value)
        {
            gameMaster.dataBased.settingData.mouseAimDownSightSensitivity = value;
        }

        protected override void Apply_GameMasterData_To_SaveData()
        {
            base.savedDataBased.settingData.mouseSensitivivty = gameMaster.dataBased.settingData.mouseSensitivivty;
            base.savedDataBased.settingData.mouseAimDownSightSensitivity = gameMaster.dataBased.settingData.mouseAimDownSightSensitivity;
        }

        protected override void Apply_SaveData_To_GameMasterData()
        {
            base.gameMaster.dataBased.settingData.mouseSensitivivty = base.savedDataBased.settingData.mouseSensitivivty;
            base.gameMaster.dataBased.settingData.mouseAimDownSightSensitivity = base.savedDataBased.settingData.mouseAimDownSightSensitivity;
        }
    }
    protected class AudioSettingMenuSector : OptionMenuSector
    {
        public AudioSettingMenuSector(OptionUICanvas optionUICanvas, OptionUIDisplayer optionUIDisplayer, GameMaster gameMaster) : base(optionUICanvas, optionUIDisplayer, gameMaster)
        {
            this.optionUICanvas.volumeMasterSlider.onValueChanged.AddListener(this.OnMasterVolumeValueChange);
            this.optionUICanvas.volumeMusicSlider.onValueChanged.AddListener(this.OnMusicVolumeValueChange);
            this.optionUICanvas.volumeEffectSlider.onValueChanged.AddListener(this.OnSoundEffectVolumeValueChange);
        }

        public override void ResetToDefault()
        {
            
        }

        protected override void Apply_GameMasterData_To_SaveData()
        {
            base.savedDataBased.settingData.volumeMaster = base.gameMaster.dataBased.settingData.volumeMaster;
            base.savedDataBased.settingData.volumeMusic = base.gameMaster.dataBased.settingData.volumeMusic;
            base.savedDataBased.settingData.volumeEffect = base.gameMaster.dataBased.settingData.volumeEffect;
        }
        protected override void Apply_SaveData_To_GameMasterData()
        {
            base.gameMaster.dataBased.settingData.volumeMaster = base.savedDataBased.settingData.volumeMaster;
            base.gameMaster.dataBased.settingData.volumeMusic = base.savedDataBased.settingData.volumeMusic;
            base.gameMaster.dataBased.settingData.volumeEffect = base.savedDataBased.settingData.volumeEffect;
        }

        protected void OnMasterVolumeValueChange(float value) => gameMaster.dataBased.settingData.volumeMaster = value;
        protected void OnMusicVolumeValueChange(float value) => gameMaster.dataBased.settingData.volumeMusic = value;
        protected void OnSoundEffectVolumeValueChange(float value) => gameMaster.dataBased.settingData.volumeEffect = value;
    }
}
