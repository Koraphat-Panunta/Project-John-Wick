using UnityEngine;
using UnityEngine.UI;

public class ControlSettingOptionDisplay : OptionUIDisplayer
{

    public Slider mouseSensitivity;
    public Slider aimDownSightSensitivity;

    protected override void Load(SettingDataScriptableObject dataBased)
    {
        this.mouseSensitivity.value = dataBased.gameSetting.lookSensitivity;
        this.aimDownSightSensitivity.value = dataBased.gameSetting.aimDownSightSensitivity;
    }
}
