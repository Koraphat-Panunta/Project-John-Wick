using UnityEngine;
using UnityEngine.UI;

public class ControlSettingOptionDisplay : OptionUIDisplayer
{

    public Slider mouseSensitivity;
    public Slider aimDownSightSensitivity;

    public ControlSettingOptionDisplay(GameObject canvasSector,Slider mouseSensitivity,Slider adsSensitivity) : base(canvasSector)
    {
        this.mouseSensitivity = mouseSensitivity;
        this.aimDownSightSensitivity = adsSensitivity;
    }

    protected override void Load(DataBased dataBased)
    {
        this.mouseSensitivity.value = dataBased.settingData.mouseSensitivivty;
        this.aimDownSightSensitivity.value = dataBased.settingData.mouseAimDownSightSensitivity;
    }
}
