using UnityEngine;
using UnityEngine.UI;

public class OptionSectorSelectorOptionUISector : OptionUISector
{
    public Button enterGraphicSettingButton;
    public Button enterAudioSettingButton;
    public Button enterControlSettingButton;
    public OptionSectorSelectorOptionUISector(OptionUICanvas optionUICanvas, Button applyButton, GameObject canvasSector
        ,Button enterGraphicSettingButton
        ,Button enterAudioSettingButton
        ,Button enterControlSettingButton) : base(optionUICanvas, applyButton, canvasSector)
    {
        this.enterGraphicSettingButton = enterGraphicSettingButton;
        this.enterAudioSettingButton = enterAudioSettingButton;
        this.enterControlSettingButton = enterControlSettingButton;
    }

    public override void Apply()
    {
        
    }

    public override void Hide()
    {
       base.optionUICanvas.gameObject.SetActive(false);
    }

    public override void Load()
    {
       
    }

    public override void Show()
    {
        base.optionUICanvas.gameObject.SetActive(true);
    }
}
