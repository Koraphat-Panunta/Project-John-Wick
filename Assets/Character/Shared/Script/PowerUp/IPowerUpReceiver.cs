/// <summary>
/// Capability interface for any character that can receive power-ups.
/// The <c>source</c> argument identifies which power-up applied which effect,
/// so removal can target a specific contributor without touching others.
/// </summary>
public interface IPowerUpReceiver
{
    /// <summary>
    /// Apply every effect in <paramref name="def"/> to this receiver, all tagged with <paramref name="source"/>.
    /// Idempotent for a given source — calling twice with the same source does nothing the second time.
    /// </summary>
    void ApplyPowerUp(PowerUpScriptableObject def, object source);

    /// <summary>Remove every modifier previously applied with this exact <paramref name="source"/>.</summary>
    void RemovePowerUp(object source);

    /// <summary>True if any effects from <paramref name="source"/> are currently active.</summary>
    bool HasPowerUpFromSource(object source);
}
