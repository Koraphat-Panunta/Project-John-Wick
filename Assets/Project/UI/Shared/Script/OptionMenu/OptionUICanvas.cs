using UnityEngine;
using UnityEngine.UI;

public class OptionUICanvas : MonoBehaviour,IInitializedAble
{

    [SerializeField] public Button backButton;
    [SerializeField] public Button applyButton;

    public OptionUIDisplayer curOptionUISector { get; private set; }

    public AudioSettingOptionDisplay audioSettingOptionDisplay { get; private set; }
    [SerializeField] public Button audioSettingSelectButton;
    [SerializeField] public GameObject audioSettingCanvas;
    [SerializeField] public Slider volumeMasterSlider;
    [SerializeField] public Slider volumeEffectSlider;
    [SerializeField] public Slider volumeMusicSlider;

    public ControlSettingOptionDisplay controlSettingOptionDisplay { get; private set; }
    [SerializeField] public Button controlSettingSelectButton;
    [SerializeField] public GameObject controlSettingCanvas;
    [SerializeField] public Slider mouseSensitivitySlider;
    [SerializeField] public Slider aimSensitivitySlider;


    public void ChangeOptionUISector(OptionUIDisplayer optionUISector,DataBased loadDataBased)
    {
        if(curOptionUISector != null)
            curOptionUISector.Hide();



        curOptionUISector = optionUISector;

        Debug.Log("curOptionUISector = " + curOptionUISector);
        Debug.Log("optionUISector = "+ optionUISector);

        curOptionUISector.Show(loadDataBased);
    }

    private void InitializedOptionUISector()
    {

        this.controlSettingOptionDisplay = new ControlSettingOptionDisplay(
            this.controlSettingCanvas
            ,this.mouseSensitivitySlider
            ,this.aimSensitivitySlider);

        this.audioSettingOptionDisplay = new AudioSettingOptionDisplay(
            this.audioSettingCanvas
            , this.volumeMasterSlider
            , this.volumeMusicSlider
            , this.volumeEffectSlider);

    }

    public void Initialized()
    {
        this.InitializedOptionUISector();
    }
}
