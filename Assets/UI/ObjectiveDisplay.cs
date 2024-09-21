using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveDisplay : MonoBehaviour,IObseverLevel
{
    [SerializeField] private TextMeshProUGUI m_TextMeshProUGUI;
    //private string objectiveDescripstion;
    [SerializeField] private LevelManager level;
    private Objective currentObjective;
    void Start()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        currentObjective = level.GetListObjective()[0];
        if(currentObjective == null)
        {
            SetTextObjectiveDynamicaly(m_TextMeshProUGUI, "");
        }
        else
        {
            SetTextObjective(m_TextMeshProUGUI, currentObjective);
        }
    }
    // Update is called once per frame
    private void OnEnable()
    {
        level.AddObserver(this);
    }
    private void OnDisable()
    {
        level.RemoveObserver(this);
    }
    void Update()
    {
        
    }
    private void SetTextObjective(TextMeshProUGUI textMeshProUGUI,Objective objective)
    {
        textMeshProUGUI.text = objective.ObjDescribe;
    }
    private void RemoveTextObjective(TextMeshProUGUI textMeshProUGUI)
    {
        textMeshProUGUI.text = "";
    }
    private void SetTextObjectiveDynamicaly(TextMeshProUGUI textMeshProUGUI,string describe)
    {
        textMeshProUGUI.text = describe;
    }

    public void OnNotify(LevelManager level, LevelSubject.LevelEvent levelEvent)
    {
        if(levelEvent == LevelSubject.LevelEvent.ObjectiveComplete)
        {
            if (level.GetListObjective().Count > 0)
            {
                currentObjective = level.GetListObjective()[0];
                SetTextObjective(this.m_TextMeshProUGUI, currentObjective);
            }
            else
            {
                RemoveTextObjective(this.m_TextMeshProUGUI);
                //All objective done.
            }
        }
       
    }
}
