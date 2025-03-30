using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ObjectiveManager))]
public class ObjectiveDisplay : GameplayUI
{
    [SerializeField] private TextMeshProUGUI m_TextMeshProUGUI;
    //private string objectiveDescripstion;
    [SerializeField] private ObjectiveManager m_ObjectiveManager;
    private Objective currentObjective => m_ObjectiveManager.curObjective;

    public override void DisableUI() => this.m_TextMeshProUGUI.enabled = false;

    public override void EnableUI() => this.m_TextMeshProUGUI.enabled = true;
    
    void Update()
    {
        if(currentObjective != null)
            this.m_TextMeshProUGUI.text = currentObjective.ObjDescribe;
        else
            this.m_TextMeshProUGUI.text = "";
    }
   

   
}
