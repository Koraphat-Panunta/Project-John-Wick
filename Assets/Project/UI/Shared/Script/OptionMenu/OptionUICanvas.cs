using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUICanvas : MonoBehaviour,IInitializedAble
{

    [SerializeField] public Button backButton;
    [SerializeField] public Button applyButton;

    public OptionUIDisplayer curOptionUISector { get; private set; }

    [SerializeField] public SelectOptionSector[] selectOptionSectors;
    public Dictionary<OptionUIDisplayer, Button> buttonSelectOptionDisplayer;

    public void ChangeOptionUISector(OptionUIDisplayer optionUISector,SettingDataScriptableObject settingDataScriptableObject)
    {
        if(curOptionUISector != null)
            curOptionUISector.Hide();

        curOptionUISector = optionUISector;

        Debug.Log("curOptionUISector = " + curOptionUISector);
        Debug.Log("optionUISector = "+ optionUISector);

        curOptionUISector.Show(settingDataScriptableObject);
    }

    public bool GetOptionDisplayAs<T>(out T optionDisplay) where T : OptionUIDisplayer
    {
        optionDisplay = null;

        if(this.selectOptionSectors == null
            || this.selectOptionSectors.Length <=0 )
            return false;


        for(int i = 0;i < this.selectOptionSectors.Length; i++)
        {
            if (this.selectOptionSectors[i].optionUIDisplayer is T)
            {
                optionDisplay = this.selectOptionSectors[i].optionUIDisplayer as T;
                return true;
            }
        }

        return false;
    }

    [Serializable]
    public struct SelectOptionSector
    {
        public OptionUIDisplayer optionUIDisplayer;
        public Button settingSelectionButton;
    }
   
  

    public void Initialized()
    {
        this.buttonSelectOptionDisplayer = new Dictionary<OptionUIDisplayer, Button>();

        if (this.selectOptionSectors == null
           || this.selectOptionSectors.Length <= 0)
            return;


        for (int i = 0; i < this.selectOptionSectors.Length; i++)
        {
            this.buttonSelectOptionDisplayer.Add(
                this.selectOptionSectors[i].optionUIDisplayer
                , this.selectOptionSectors[i].settingSelectionButton
                );
        }

    }
}
