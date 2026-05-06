using UnityEngine;

/// <summary>
/// Debug overlay for an <see cref="EnemyDirectedDecision"/>. Renders three labels above
/// the enemy's head while in Play mode:
///   - Current INodeLeaf type name           (default: yellow)
///   - EnemyDecisionContext.combatPhase      (default: orange)
///   - EnemyDecisionContext.roleCommand      (default: cyan)
///
/// Drop on the enemy GameObject (or any child of it). The decision is auto-found
/// via GetComponentInParent in Awake; the head anchor falls back to this transform.
///
/// Renders to Game view via OnGUI (WorldToScreenPoint). Skips drawing when the
/// anchor is behind the camera or off-screen.
/// </summary>
[DisallowMultipleComponent]
public class EnemyDecisionDebugDisplay : MonoBehaviour
{
    [Header("Source")]
    [Tooltip("Decision to display. Auto-found in Awake if left empty.")]
    [SerializeField] private EnemyDirectedDecision decision;

    [Tooltip("Where the labels float above. Falls back to this transform.")]
    [SerializeField] private Transform headAnchor;

    [Header("Layout")]
    [Tooltip("World-space meters above the head anchor where the bottom label sits.")]
    [SerializeField] private float verticalOffset = 2.2f;

    [SerializeField] private int fontSize = 12;

    [Header("Colors")]
    [SerializeField] private Color nodeLeafColor = Color.yellow;
    [SerializeField] private Color combatPhaseColor = new Color(1f, 0.55f, 0.15f); // orange
    [SerializeField] private Color roleCommandColor = Color.cyan;

    [Header("Toggle")]
    [SerializeField] private bool showInGameView = true;

    // Reused style to avoid re-allocating every OnGUI tick.
    private static GUIStyle _style;

    private void Awake()
    {
        if (decision == null) decision = GetComponentInParent<EnemyDirectedDecision>();
        if (headAnchor == null) headAnchor = transform;
    }

    private void OnGUI()
    {
        if (!showInGameView) return;
        if (decision == null || decision.enemyDecisionContext == null) return;

        var cam = Camera.main;
        if (cam == null) return;

        // Lazy-init style and keep fontSize in sync if Inspector value changed.
        if (_style == null)
            _style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        _style.fontSize = fontSize;

        Vector3 worldPos = (headAnchor != null ? headAnchor.position : transform.position)
                           + Vector3.up * verticalOffset;
        Vector3 screen = cam.WorldToScreenPoint(worldPos);
        if (screen.z <= 0f) return;            // anchor is behind the camera
        float guiY = Screen.height - screen.y; // OnGUI Y is top-down; screen Y is bottom-up

        // Pull current state through the INodeManager interface contract.
        var nodeLeaf = (decision as INodeManager).GetCurNodeLeaf();
        string nodeLeafName = nodeLeaf != null ? nodeLeaf.GetType().Name : "(none)";
        string combatPhase = decision.enemyDecisionContext.combatPhase.ToString();
        string roleCommand = decision.enemyDecisionContext.roleCommand.ToString();

        // Stack three labels: node leaf on top, combat phase middle, role command bottom.
        float lineH = fontSize * 1.25f;
        DrawCenteredLabel(screen.x, guiY - lineH * 2f, nodeLeafName, nodeLeafColor);
        DrawCenteredLabel(screen.x, guiY - lineH,      combatPhase,  combatPhaseColor);
        DrawCenteredLabel(screen.x, guiY,              roleCommand,  roleCommandColor);
    }

    private void DrawCenteredLabel(float x, float y, string text, Color color)
    {
        var prev = GUI.color;
        GUI.color = color;
        Vector2 size = _style.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(x - size.x * 0.5f, y, size.x, size.y), text, _style);
        GUI.color = prev;
    }

#if UNITY_EDITOR
    // Mirror in Scene view as well, so it's visible while editing/inspecting.
    private void OnDrawGizmos()
    {
        if (decision == null || decision.enemyDecisionContext == null) return;
        if (!Application.isPlaying) return;

        Vector3 worldPos = (headAnchor != null ? headAnchor.position : transform.position)
                           + Vector3.up * verticalOffset;

        var nodeLeaf = (decision as INodeManager).GetCurNodeLeaf();
        string label = $"{(nodeLeaf != null ? nodeLeaf.GetType().Name : "(none)")}\n" +
                       $"{decision.enemyDecisionContext.combatPhase}\n" +
                       $"{decision.enemyDecisionContext.roleCommand}";

        var style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
        UnityEditor.Handles.Label(worldPos, label, style);
    }
#endif
}
