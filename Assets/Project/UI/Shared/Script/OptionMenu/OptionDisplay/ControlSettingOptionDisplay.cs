using UnityEngine;
using UnityEngine.UI;

public class ControlSettingOptionDisplay : OptionUIDisplayer
{

    public Slider mouseSensitivity;
    public Slider aimDownSightSensitivity;

    protected override void Load(StaticDataBased.SettingData dataBased)
    {
        this.mouseSensitivity.value = dataBased.mouseSensitivivty;
        this.aimDownSightSensitivity.value = dataBased.mouseAimDownSightSensitivity;
    }
}
