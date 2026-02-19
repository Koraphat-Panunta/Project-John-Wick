using UnityEngine;

public class GraphicManager : MonoBehaviour, IInitializedAble
{

    public static GraphicManager Instance;
    public void Initialized()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ApplyGraphicSetting(GraphicSetting graphicSetting)
    {
        DynamicDataBased.Instance.settingDataScriptableObject.graphicSetting = graphicSetting;

        QualitySettings.SetQualityLevel((int)graphicSetting.quality);

        Screen.SetResolution(
            ResolutionDisplay.GetResolution(graphicSetting.resolution).width
            ,ResolutionDisplay.GetResolution(graphicSetting.resolution).height
            ,graphicSetting.screenMode
            );

        Application.targetFrameRate = ResolutionDisplay.GetLimitFPS(graphicSetting.targetFPS);
    }
}
