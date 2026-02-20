using UnityEditor.SettingsManagement;
using UnityEngine;

public abstract class OptionUIDisplayer : MonoBehaviour
{
    public GameObject optionCanvasSector;
  
    public void Show(SettingDataScriptableObject dataBased)
    {
        this.optionCanvasSector.SetActive(true);
        this.Load(dataBased);
    }
    public void Hide()
    {
        this.optionCanvasSector?.SetActive(false);
    }
    protected abstract void Load(SettingDataScriptableObject dataBased);
}
