using UnityEngine;

/// <summary>
/// Designer-facing power-up definition. Create instances in the Project window
/// (Create &gt; ScriptableObjects &gt; PowerUp). One asset = one power-up type.
/// </summary>
[CreateAssetMenu(fileName = "PowerUp", menuName = "ScriptableObjects/PowerUp")]
public class PowerUpScriptableObject : ScriptableObject
{
    [Tooltip("Unique identifier for save/load and lookups.")]
    public string id;

    [Tooltip("Player-facing name.")]
    public string displayName;

    [Tooltip("Icon for UI displays.")]
    public Sprite icon;

    [Tooltip("Stat changes applied when picked up.")]
    public PowerUpEffect[] effects;

    [Tooltip("Seconds before the effect is removed automatically. 0 = permanent until removed externally.")]
    public float duration = 0f;
}
