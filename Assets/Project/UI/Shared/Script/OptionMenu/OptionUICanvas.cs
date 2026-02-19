using UnityEngine;
using UnityEngine.UI;

public class OptionUICanvas : MonoBehaviour,IInitializedAble
{

    [SerializeField] public Button backButton;
    [SerializeField] public Button applyButton;

    public OptionUIDisplayer curOptionUISector { get; private set; }

    public AudioSettingOptionDisplay audioSettingOptionDisplay { get; private set; }
    [SerializeField] public Button audioSettingSelectButton;

    public ControlSettingOptionDisplay controlSettingOptionDisplay { get; private set; }
    [SerializeField] public Button controlSettingSelectButton;



    public void ChangeOptionUISector(OptionUIDisplayer optionUISector,StaticDataBased.SettingData loadDataBased)
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

      

    }

    public void Initialized()
    {
        this.InitializedOptionUISector();
    }
}
