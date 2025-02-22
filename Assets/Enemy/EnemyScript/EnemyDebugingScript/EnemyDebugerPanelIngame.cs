using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDebugerPanelIngame : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyTacticDecision enemyTacticDecision;

    [SerializeField] TextMeshProUGUI stateDisplay;
    private string stateText;

    [SerializeField] TextMeshProUGUI tacticDisplay;
    private string tacticText;

    [SerializeField] Image hpDisplay;
    private float hpNormalized;

    [SerializeField] Image postureDisplay;
    private float postureNormalized;

    private void Awake()
    {
        canvas.worldCamera = Camera.main;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(canvas.worldCamera.transform,Vector3.up);

        stateText = "State = " + enemy.enemyStateManagerNode.curNodeLeaf;
        tacticText = "Tactic = " + enemyTacticDecision.curTacticDecision;
        hpNormalized = Mathf.Clamp01(enemy.GetHP()/100);
        postureNormalized = enemy._posture / 100;

        stateDisplay.text = stateText;
        tacticDisplay.text = tacticText;
        hpDisplay.rectTransform.localScale = new Vector3 (hpNormalized, hpDisplay.rectTransform.localScale.y, hpDisplay.rectTransform.localScale.z);

        if(enemy.isDead)
            canvas.enabled = false;
    }
}
