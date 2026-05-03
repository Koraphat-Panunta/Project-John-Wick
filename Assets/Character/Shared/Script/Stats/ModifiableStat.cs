using System;
using System.Collections.Generic;

/// <summary>
/// A numeric stat composed from a base value plus a list of <see cref="StatModifier"/>s.
/// Recomputes whenever modifiers are added/removed/replaced and notifies listeners
/// via <see cref="OnValueChanged"/>.
/// </summary>
public class ModifiableStat
{
    private float baseValue;
    private readonly List<StatModifier> mods = new List<StatModifier>();

    /// <summary>Fired with the new value whenever it changes.</summary>
    public event Action<float> OnValueChanged;

    public float Value { get; private set; }

    public ModifiableStat(float baseValue)
    {
        this.baseValue = baseValue;
        Recompute();
    }

    public void SetBase(float value)
    {
        baseValue = value;
        Recompute();
    }

    public void AddModifier(StatModifier m)
    {
        if (m == null) return;
        mods.Add(m);
        Recompute();
    }

    public void RemoveModifier(StatModifier m)
    {
        if (m == null) return;
        if (mods.Remove(m)) Recompute();
    }

    /// <summary>
    /// Remove every modifier whose <see cref="StatModifier.source"/> equals <paramref name="source"/>.
    /// This is the primary removal path used by the power-up system.
    /// </summary>
    public void RemoveModifiersFromSource(object source)
    {
        if (source == null) return;
        if (mods.RemoveAll(m => ReferenceEquals(m.source, source)) > 0)
            Recompute();
    }

    public void ClearModifiers()
    {
        if (mods.Count == 0) return;
        mods.Clear();
        Recompute();
    }

    public bool HasModifierFromSource(object source)
    {
        if (source == null) return false;
        return mods.Exists(m => ReferenceEquals(m.source, source));
    }

    private void Recompute()
    {
        float flat = baseValue;
        float pctAdd = 0f;
        float pctMult = 1f;

        for (int i = 0; i < mods.Count; i++)
        {
            var m = mods[i];
            switch (m.type)
            {
                case StatModifierType.Flat: flat += m.value; break;
                case StatModifierType.PercentAdd: pctAdd += m.value; break;
                case StatModifierType.PercentMult: pctMult *= 1f + m.value; break;
            }
        }

        Value = flat * (1f + pctAdd) * pctMult;
        OnValueChanged?.Invoke(Value);
    }
}
