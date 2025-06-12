using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDebugerPanelIngame : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyRoleBasedDecision enemyRole;

    [SerializeField] TextMeshProUGUI stateDisplay;
    private string stateText;

    [SerializeField] TextMeshProUGUI roleDisplay;
    private string curRole;

    [SerializeField] TextMeshProUGUI actionDisplay;
    private string actionText;

    [SerializeField] TextMeshProUGUI combatPhaseDisplay;
    private string combatPhaseText;

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
        curRole = "Role = " + enemyRole.enemyActionNodeManager;
        actionText = "Action = "+enemyRole.enemyActionNodeManager.curNodeLeaf;
        combatPhaseText = "CombatPhase = "+enemyRole._curCombatPhase;
        hpNormalized = Mathf.Clamp01(enemy.GetHP()/100);
        postureNormalized = enemy._posture / 100;

        stateDisplay.text = stateText;
        roleDisplay.text = curRole;
        actionDisplay.text = actionText;
        combatPhaseDisplay.text = combatPhaseText;

        hpDisplay.rectTransform.localScale = new Vector3 (hpNormalized, hpDisplay.rectTransform.localScale.y, hpDisplay.rectTransform.localScale.z);

        if (enemyRole.enemyActionNodeManager == enemyRole.chaserRoleNodeManager)

            roleDisplay.color = Color.red;
        else if (enemyRole.enemyActionNodeManager == enemyRole.overwatchRoleNodeManager)
            roleDisplay.color = new Color(1, 0.565f,0);

        if (enemy.isDead)
            canvas.enabled = false;
    }
}
