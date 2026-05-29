using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionMenuSettingInGameGameMasterNodeLeaf : GameMasterNodeLeaf,INodeLeafTransitionAble
{
    private OptionUICanvas optionUICanvas;
    public bool isTriggerExit { get; protected set;}
    public bool isTriggerEnter { get; protected set; }
    private OptionMenuSector curMenuSector;
    private OptionMenuSector controlMenuSector;
    private OptionMenuSector audioMenuSector;
    private OptionMenuSector keyBindingMenuSector;

    public INodeManager nodeManager { get; set; }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    // userInput is optional: in-game passes the live UserInputActor.userInput so rebinds take effect immediately;
    // front-scene callers can omit it and a temporary instance with saved overrides is created instead.
    public OptionMenuSettingInGameGameMasterNodeLeaf(GameMaster gameMaster, OptionUICanvas optionUICanvas, Func<bool> preCondition, UserInput userInput = null) : base(gameMaster, preCondition)
    {
        this.optionUICanvas = optionUICanvas;

        if(this.optionUICanvas.GetOptionDisplayAs<ControlSettingOptionDisplay>(out ControlSettingOptionDisplay controlSettingOptionDisplay))
        {
            this.controlMenuSector = new ControlSettingMenuSector(this.optionUICanvas, controlSettingOptionDisplay, this.gameMaster);
            this.optionUICanvas.buttonSelectOptionDisplayer[controlSettingOptionDisplay].onClick.AddListener(this.SelectControlSetting);
        }

        if(this.optionUICanvas.GetOptionDisplayAs<AudioSettingOptionDisplay>(out AudioSettingOptionDisplay audioSettingOptionDisplay))
        {
            this.audioMenuSector = new AudioSettingMenuSector(this.optionUICanvas, audioSettingOptionDisplay, this.gameMaster);
            this.optionUICanvas.buttonSelectOptionDisplayer[audioSettingOptionDisplay].onClick.AddListener(this.SelectAudioSetting);
        }

        if(this.optionUICanvas.GetOptionDisplayAs<KeyBindingSettingOptionDisplay>(out KeyBindingSettingOptionDisplay keyBindingDisplay))
        {
            UserInput activeUserInput = userInput ?? CreateTempUserInputWithOverrides();
            this.keyBindingMenuSector = new KeyBindingSettingMenuSector(this.optionUICanvas, keyBindingDisplay, activeUserInput, this.gameMaster);

            if (this.optionUICanvas.buttonSelectOptionDisplayer != null
                && this.optionUICanvas.buttonSelectOptionDisplayer.ContainsKey(keyBindingDisplay))
                this.optionUICanvas.buttonSelectOptionDisplayer[keyBindingDisplay].onClick.AddListener(this.SelectKeyBindingSetting);
            else
                Debug.LogWarning("[OptionMenuSetting] Key Binding tab button not found in buttonSelectOptionDisplayer — assign it via OptionUICanvas.selectOptionSectors in the prefab.");
        }

        this.optionUICanvas.backButton.onClick.AddListener(this.TriggerExit);

        this.nodeManager = gameMaster;
        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
    }

    public override void Enter()
    {
        Debug.Log("OptionMenuSettingInGameGameMasterNodeLeaf Enter");

        Cursor.lockState = CursorLockMode.None;
        this.isTriggerEnter = false;
        this.optionUICanvas.gameObject.SetActive(true);
        this.nodeLeafTransitionBehavior.TransitionAbleAll(this);
        this.SelectControlSetting();
    }

    public override void Exit()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("OptionMenuSettingInGameGameMasterNodeLeaf Exit");

        this.isTriggerEnter = false;
        this.isTriggerExit = false;
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
    protected void TriggerEnter() => this.isTriggerEnter = true;
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
    protected void SelectAudioSetting()
    {
        Debug.Log("SelectAudioSetting");
        this.ChangeOptionSettingSector(this.audioMenuSector);
    }
    protected void SelectKeyBindingSetting() => this.ChangeOptionSettingSector(this.keyBindingMenuSector);

    public bool TransitioningCheck() => nodeLeafTransitionBehavior.TransitioningCheck(this);

    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);

    private UserInput CreateTempUserInputWithOverrides()
    {
        var temp = new UserInput();
        string savedJson = DynamicDataBased.Instance?.settingDataScriptableObject?.keyBindingSetting.bindingOverridesJson;
        if (!string.IsNullOrEmpty(savedJson))
            temp.LoadBindingOverridesFromJson(savedJson);
        return temp;
    }
}
