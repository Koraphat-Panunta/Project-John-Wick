using System;
using UnityEngine;

public class OptionMenuSettingInGameGameMasterNodeLeaf : GameMasterNodeLeaf
{
    private OptionUICanvas optionUICanvas;
    private bool isTriggerExit;
    public OptionMenuSettingInGameGameMasterNodeLeaf(GameMaster gameMaster,OptionUICanvas optionUICanvas, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        this.optionUICanvas = optionUICanvas;
    }

    public override void Enter()
    {
        isTriggerExit = false;
        this.optionUICanvas.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        this.optionUICanvas.gameObject.SetActive(false);
    }
    public override void UpdateNode()
    {
       
    }
    public override void FixedUpdateNode()
    {
        
    }
    public override bool IsReset()
    {
        return isTriggerExit;
    }
    public override bool IsComplete()
    {
        return false;
    }

    protected class OptionMenuSector
    {
        private OptionUICanvas optionUICanvas;
        protected DataBased savedDataBased;
        public OptionMenuSector(OptionUICanvas optionUICanvas)
        {
            this.optionUICanvas = optionUICanvas;
        }
        public void Enter()
        {
            this.optionUICanvas.applyButton.onClick.AddListener(this.Apply);
        }

        public virtual void Exit()
        {
            this.optionUICanvas.applyButton.onClick.RemoveListener(this.Apply);
        }
        public virtual void Apply()
        {

        }
        public virtual void ResetToDefault()
        {

        }
    }
    protected class ControlSettingMenuSector : OptionMenuSector
    {
        public ControlSettingMenuSector(OptionUICanvas optionUICanvas) : base(optionUICanvas)
        {
        }
    }

}
