using UnityEngine;
using UnityEngine.UI;

public class OptionUICanvas : MonoBehaviour
{
    public Slider mouseSensitivitySlider;
    public Slider aimSensitivitySlider;
    public Slider volumeMasterSlider;
    public Slider volumeEffectSlider;
    public Slider volumeMusicSlider;

    private GameManager gameManager;

    [SerializeField] private CameraController cameraController;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        cameraController = FindAnyObjectByType<CameraController>();

        RegisterUIEvents();
    }
    private void OnEnable()
    {
        mouseSensitivitySlider.value = cameraController.standardCameraSensivity;
        aimSensitivitySlider.value = cameraController.aimDownSightCameraSensivity;
    }
    private void OnDisable()
    {
        
    }
   
    private void RegisterUIEvents()
    {
        mouseSensitivitySlider.onValueChanged.AddListener(val => 
        {
            cameraController.standardCameraSensivity = val;

            if(gameManager != null)
                gameManager.dataBased.settingData.mouseSensitivivty = cameraController.standardCameraSensivity;
        });
        aimSensitivitySlider.onValueChanged.AddListener(val => {

            cameraController.aimDownSightCameraSensivity = val;

            if(gameManager != null)
                gameManager.dataBased.settingData.mouseAimDownSightSensitivity = cameraController.aimDownSightCameraSensivity;
        });
        //volumeMasterSlider.onValueChanged.AddListener(val => {
        //    DataBased.Instance.settingData.volumeMaster = val;
        //});
        //volumeEffectSlider.onValueChanged.AddListener(val => {
        //    DataBased.Instance.settingData.volumeEffect = val;
        //});
        //volumeMusicSlider.onValueChanged.AddListener(val => {
        //    DataBased.Instance.settingData.volumeMusic = val;
        //});
    }

}
