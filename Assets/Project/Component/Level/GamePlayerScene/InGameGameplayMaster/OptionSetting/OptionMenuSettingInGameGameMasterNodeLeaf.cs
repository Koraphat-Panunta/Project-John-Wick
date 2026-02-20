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
            
        this.optionUICanvas.backButton.onClick.AddListener(this.TriggerExit);

        this.nodeManager = gameMaster;
        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
    }

    public override void Enter()
    {

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
    

    
   
}
