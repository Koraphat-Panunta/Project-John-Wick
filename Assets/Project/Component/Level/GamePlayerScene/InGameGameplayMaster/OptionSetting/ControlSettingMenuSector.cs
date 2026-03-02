using UnityEngine;

public class ControlSettingMenuSector : OptionMenuSector
{
    public override OptionUIDisplayer optionUIDisplayer => this.controlSettingOptionDisplay;
    public ControlSettingOptionDisplay controlSettingOptionDisplay { get; protected set; }

    public ControlSettingMenuSector(OptionUICanvas optionUICanvas, ControlSettingOptionDisplay optionUIDisplayer, GameMaster gameMaster) : base(optionUICanvas, gameMaster)
    {
        this.controlSettingOptionDisplay = optionUIDisplayer;
        optionUIDisplayer.mouseSensitivity.onValueChanged.AddListener(OnMouseSensitivityChange);
        optionUIDisplayer.aimDownSightSensitivity.onValueChanged.AddListener(OnMouseAimDownSightSensitivityChange);
    }

    public override void ResetToDefault()
    {

    }

    private void OnMouseSensitivityChange(float value)
    {
        DynamicDataBased.Instance.settingDataScriptableObject.gameSetting.lookSensitivity = value;
    }
    private void OnMouseAimDownSightSensitivityChange(float value)
    {
        DynamicDataBased.Instance.settingDataScriptableObject.gameSetting.aimDownSightSensitivity = value;
    }

   
}
