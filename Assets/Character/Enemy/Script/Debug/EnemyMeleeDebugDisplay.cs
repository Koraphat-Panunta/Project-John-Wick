using UnityEngine;

/// <summary>
/// Debug overlay for the enemy melee guard/counter system. Renders five labels above
/// the enemy's head while in Play mode:
///   - Current INodeLeaf type name      (yellow)
///   - Guard mode active/inactive       (green / gray)
///   - Guard gauge current / max        (white)
///   - Hits remaining until guard       (orange)
///   - Blocks remaining until counter   (cyan)
///
/// Drop on the enemy GameObject (or any child). Enemy and EnemyCommandAPI are
/// auto-found via GetComponentInParent in Awake.
/// </summary>
[DisallowMultipleComponent]
public class EnemyMeleeDebugDisplay : MonoBehaviour
{
    [Header("Source")]
    [Tooltip("Auto-found via GetComponentInParent if left empty.")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyCommandAPI enemyCommandAPI;

    [Tooltip("Where the labels float. Falls back to this transform.")]
    [SerializeField] private Transform headAnchor;

    [Header("Layout")]
    [SerializeField] private float verticalOffset = 3.2f;
    [SerializeField] private int fontSize = 11;

    [Header("Toggle")]
    [SerializeField] private bool showInGameView = true;

    private static GUIStyle _style;

    private void Awake()
    {
        if (enemy == null)           enemy           = GetComponentInParent<Enemy>();
        if (enemyCommandAPI == null) enemyCommandAPI = GetComponentInParent<EnemyCommandAPI>();
        if (headAnchor == null)      headAnchor      = transform;
    }

    private void OnGUI()
    {
        if (!showInGameView || enemy == null || enemyCommandAPI == null) return;
        var cam = Camera.main;
        if (cam == null) return;

        if (_style == null)
            _style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        _style.fontSize = fontSize;

        Vector3 worldPos = (headAnchor != null ? headAnchor.position : transform.position)
                           + Vector3.up * verticalOffset;
        Vector3 screen = cam.WorldToScreenPoint(worldPos);
        if (screen.z <= 0f) return;
        float guiY = Screen.height - screen.y;

        var defending = enemyCommandAPI.enemyAutoDefendCommand;
        string stateName    = enemy.stateManagerNode.GetCurNodeLeaf()?.GetType().Name ?? "(none)";
        string guardMode    = enemy.isGuardModeEnabled ? "GUARD ON" : "guard off";
        Color  guardColor   = enemy.isGuardModeEnabled ? Color.green : Color.gray;
        string gauge        = $"GuardGauge: {enemy.guardGauge._gauge:0.0} / {enemy.maxGuardGauge:0.0}";
        string hitToGuard   = $"HitToGuard: {defending._guardingDecision.gotHitReactionGuardRate:0.0}";
        string hitToCounter = $"HitToCounter: {defending._guardingDecision.counterReactionRate:0.0}";

        float lineH = fontSize * 1.4f;
        DrawLabel(screen.x, guiY - lineH * 4f, stateName,    Color.yellow);
        DrawLabel(screen.x, guiY - lineH * 3f, guardMode,    guardColor);
        DrawLabel(screen.x, guiY - lineH * 2f, gauge,        Color.white);
        DrawLabel(screen.x, guiY - lineH,      hitToGuard,   new Color(1f, 0.55f, 0.15f));
        DrawLabel(screen.x, guiY,              hitToCounter, Color.cyan);
    }

    private void DrawLabel(float x, float y, string text, Color color)
    {
        var prev = GUI.color;
        GUI.color = color;
        Vector2 size = _style.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(x - size.x * 0.5f, y, size.x, size.y), text, _style);
        GUI.color = prev;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (enemy == null || enemyCommandAPI == null || !Application.isPlaying) return;

        Vector3 worldPos = (headAnchor != null ? headAnchor.position : transform.position)
                           + Vector3.up * verticalOffset;

        var defending = enemyCommandAPI.enemyAutoDefendCommand;
        string stateName = enemy.stateManagerNode.GetCurNodeLeaf()?.GetType().Name ?? "(none)";
        string label = $"{stateName}\n" +
                       $"{(enemy.isGuardModeEnabled ? "GUARD ON" : "guard off")}\n" +
                       $"Gauge: {enemy.guardGauge._gauge:0.0}/{enemy.maxGuardGauge:0.0}\n" +
                       $"HitToGuard: {defending._guardingDecision.gotHitReactionGuardRate:0.0}\n" +
                       $"HitToCounter: {defending._guardingDecision.counterReactionRate:0.0}";

        var style = new GUIStyle { alignment = TextAnchor.MiddleCenter };
        style.normal.textColor = Color.white;
        UnityEditor.Handles.Label(worldPos, label, style);
    }
#endif
}
