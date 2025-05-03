using UnityEngine;
using UnityEngine.UI;

public class ControlSettingOptionUISector : OptionUISector
{

    public Slider mouseSensitivity;
    public Slider aimDownSightSensitivity;

    private GameManager gameManager;
    private CameraController cameraController;

    public ControlSettingOptionUISector(OptionUICanvas optionUICanvas, Button applyButton, GameObject canvasSector
        , GameManager gameManager
        , CameraController cameraController
        , Slider mouseSensitivity
        , Slider aimDownSightSensitivity) : base(optionUICanvas, applyButton, canvasSector)
    {
        this.gameManager = gameManager;
        this.cameraController = cameraController;
        this.mouseSensitivity = mouseSensitivity;
        this.aimDownSightSensitivity = aimDownSightSensitivity;
    }

    public override void Show()
    {
        canvasSector.SetActive(true);
        applyButton.gameObject.SetActive(true);
    }

    public override void Hide()
    {
        canvasSector.gameObject.SetActive(false);
        applyButton.gameObject.SetActive(false);
    }

    public override void Apply()
    {
        if (gameManager != null)
        {
            gameManager.dataBased.settingData.mouseSensitivivty = this.mouseSensitivity.value;
            gameManager.dataBased.settingData.mouseAimDownSightSensitivity = this.aimDownSightSensitivity.value;
        }
        else if (cameraController != null)
        {
            cameraController.standardCameraSensivity = gameManager.dataBased.settingData.mouseSensitivivty;
            cameraController.aimDownSightCameraSensivity = gameManager.dataBased.settingData.mouseAimDownSightSensitivity;
        }
        else
        {
            throw new System.Exception("None apply SettingData");
        }
    }

    public override void Load()
    {
        if(gameManager != null)
        {
            this.mouseSensitivity.value = gameManager.dataBased.settingData.mouseSensitivivty;
            this.aimDownSightSensitivity.value = gameManager.dataBased.settingData.mouseAimDownSightSensitivity;
        }
        else if(cameraController != null)
        {
            this.mouseSensitivity.value = cameraController.standardCameraSensivity;
            this.aimDownSightSensitivity.value = cameraController.aimDownSightCameraSensivity;
        }
        else
        {
            throw new System.Exception("None Load SettingData");
        }
    }
}
