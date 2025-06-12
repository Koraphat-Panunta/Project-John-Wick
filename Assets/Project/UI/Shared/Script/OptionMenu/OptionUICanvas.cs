using UnityEngine;
using UnityEngine.UI;

public class OptionUICanvas : MonoBehaviour
{


    [SerializeField] private Slider volumeMasterSlider;
    [SerializeField] private Slider volumeEffectSlider;
    [SerializeField] private Slider volumeMusicSlider;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private Button backButton;
    [SerializeField] private Button applyButton;

    public OptionUISector curOptionUISector { get; private set; }

    public OptionSectorSelectorOptionUISector optionSectorSelectorOptionUISector { get; private set; }
    [SerializeField] private GameObject optionSelectorSectorGameObject;
    [SerializeField] private Button graphicSetting;
    [SerializeField] private Button controlSetting;
    [SerializeField] private Button audioSetting;
    public ControlSettingOptionUISector controlSettingOptionUISector { get; private set; }
    [SerializeField] private GameObject controlSectorObject;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider aimSensitivitySlider;

    
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        cameraController = FindAnyObjectByType<CameraController>();

        InitializedOptionUISector();
    }
    
    public void ChangeOptionUISector(OptionUISector optionUISector)
    {
        if(curOptionUISector != null)
            curOptionUISector.Hide();

        curOptionUISector = optionUISector;
        curOptionUISector.Show();
    }

    private void InitializedOptionUISector()
    {
        this.optionSectorSelectorOptionUISector = new OptionSectorSelectorOptionUISector(this
            ,this.applyButton
            ,this.optionSelectorSectorGameObject
            ,this.graphicSetting
            ,this.audioSetting
            ,this.controlSetting);

        this.controlSettingOptionUISector = new ControlSettingOptionUISector(this
            ,this.applyButton
            ,this.controlSectorObject
            ,this.gameManager
            ,this.cameraController
            ,this.mouseSensitivitySlider
            ,this.aimSensitivitySlider);

    }

}
