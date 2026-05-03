/// <summary>
/// One adjustment applied to a <see cref="ModifiableStat"/>.
/// The <see cref="source"/> is the identity used by removal — typically the
/// power-up pickup instance or any other unique reference that owns this modifier.
/// </summary>
public class StatModifier
{
    public readonly object source;
    public readonly float value;
    public readonly StatModifierType type;

    public StatModifier(object source, float value, StatModifierType type)
    {
        this.source = source;
        this.value = value;
        this.type = type;
    }
}
