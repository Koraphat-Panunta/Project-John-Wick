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
        protected StaticDataBased.SettingData savedDataBased;
        protected GameMaster gameMaster;
        protected OptionUIDisplayer optionUIDisplayer;
        public OptionMenuSector(OptionUICanvas optionUICanvas,OptionUIDisplayer optionUIDisplayer,GameMaster gameMaster)
        {
            this.optionUICanvas = optionUICanvas;
            this.savedDataBased = new StaticDataBased.SettingData();
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
            optionUICanvas.controlSettingOptionDisplay.mouseSensitivity.onValueChanged.AddListener(OnMouseSensitivityChange);
            optionUICanvas.controlSettingOptionDisplay.aimDownSightSensitivity.onValueChanged.AddListener(OnMouseAimDownSightSensitivityChange);
        }

        public override void ResetToDefault()
        {
            
        }

        private void OnMouseSensitivityChange(float value)
        {
            StaticDataBased.Instance.settingData.mouseSensitivivty = value;
        }
        private void OnMouseAimDownSightSensitivityChange(float value)
        {
            StaticDataBased.Instance.settingData.mouseAimDownSightSensitivity = value;
        }

        protected override void Apply_GameMasterData_To_SaveData()
        {
            base.savedDataBased.mouseSensitivivty = StaticDataBased.Instance.settingData.mouseSensitivivty;
            base.savedDataBased.mouseAimDownSightSensitivity = StaticDataBased.Instance.settingData.mouseAimDownSightSensitivity;
        }

        protected override void Apply_SaveData_To_GameMasterData()
        {
            StaticDataBased.Instance.settingData.mouseSensitivivty = base.savedDataBased.mouseSensitivivty;
            StaticDataBased.Instance.settingData.mouseAimDownSightSensitivity = base.savedDataBased.mouseAimDownSightSensitivity;
        }
    }
    protected class AudioSettingMenuSector : OptionMenuSector
    {
        public AudioSettingMenuSector(OptionUICanvas optionUICanvas, OptionUIDisplayer optionUIDisplayer, GameMaster gameMaster) : base(optionUICanvas, optionUIDisplayer, gameMaster)
        {
            this.optionUICanvas.audioSettingOptionDisplay.masterVolume.onValueChanged.AddListener(this.OnMasterVolumeValueChange);
            this.optionUICanvas.audioSettingOptionDisplay.musicVolume.onValueChanged.AddListener(this.OnMusicVolumeValueChange);
            this.optionUICanvas.audioSettingOptionDisplay.sfxVolume.onValueChanged.AddListener(this.OnSoundEffectVolumeValueChange);
        }

        public override void ResetToDefault()
        {
            
        }

        protected override void Apply_GameMasterData_To_SaveData()
        {
            base.savedDataBased.volumeMaster = StaticDataBased.Instance.settingData.volumeMaster;
            base.savedDataBased.volumeMusic = StaticDataBased.Instance.settingData.volumeMusic;
            base.savedDataBased.volumeEffect = StaticDataBased.Instance.settingData.volumeEffect;
        }
        protected override void Apply_SaveData_To_GameMasterData()
        {
            StaticDataBased.Instance.settingData.volumeMaster = base.savedDataBased.volumeMaster;
            StaticDataBased.Instance.settingData.volumeMusic = base.savedDataBased.volumeMusic;
            StaticDataBased.Instance.settingData.volumeEffect = base.savedDataBased.volumeEffect;
        }

        protected void OnMasterVolumeValueChange(float value) => StaticDataBased.Instance.settingData.volumeMaster = value;
        protected void OnMusicVolumeValueChange(float value) => StaticDataBased.Instance.settingData.volumeMusic = value;
        protected void OnSoundEffectVolumeValueChange(float value) => StaticDataBased.Instance.settingData.volumeEffect = value;
    }
}
