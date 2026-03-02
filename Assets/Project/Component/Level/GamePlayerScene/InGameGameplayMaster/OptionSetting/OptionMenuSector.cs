using UnityEngine;

public abstract class OptionMenuSector
{
    protected GameMaster gameMaster;
    public abstract OptionUIDisplayer optionUIDisplayer { get; }
    protected OptionUICanvas optionUICanvas;
    public OptionMenuSector(OptionUICanvas optionUICanvas, GameMaster gameMaster)
    {
        this.gameMaster = gameMaster;
        this.optionUICanvas = optionUICanvas;
    }
    public virtual void Enter()
    {
        this.optionUIDisplayer.Show(DynamicDataBased.Instance.settingDataScriptableObject);
    }

    public virtual void Exit()
    {
        this.optionUIDisplayer.Hide();
    }

    public abstract void ResetToDefault();
}
