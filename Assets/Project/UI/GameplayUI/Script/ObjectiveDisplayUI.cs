using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ObjectiveDisplayUI : GameplayUI
{
    [SerializeField] private InGameLevelGameMaster gameMaster;
    [SerializeField] private TextMeshProUGUI m_TextMeshProUGUI;
    private Objective currentObjective => gameMaster.curObjective;

    public override void DisableUI() 
    { 
        if(isEnable == false)
            return;

        gameMaster.OnObjectiveUpdate -= UpdateObjectiveDescribtion;
        this.m_TextMeshProUGUI.enabled = false; 

        base.DisableUI();
    }

    public override void EnableUI() 
    {
        if(isEnable == true)
            return;

        gameMaster.OnObjectiveUpdate += UpdateObjectiveDescribtion;
        this.m_TextMeshProUGUI.enabled = true; 

        base.EnableUI();
    }
   

    private void UpdateObjectiveDescribtion(Objective objective)
    {
        if (currentObjective != null)
            this.m_TextMeshProUGUI.text = currentObjective.ObjDescribe;
        else
            this.m_TextMeshProUGUI.text = "";
    }

 
    private void OnValidate()
    {
        gameMaster = FindAnyObjectByType<InGameLevelGameMaster>();
        //SetCurObjective
    }

}
