/// <summary>
/// How a <see cref="StatModifier"/>'s value combines with the base value of a stat.
/// Final = (base + sum(Flat)) * (1 + sum(PercentAdd)) * product(1 + PercentMult)
/// </summary>
public enum StatModifierType
{
    /// <summary>Added directly to the base value (e.g. +25 max HP).</summary>
    Flat,

    /// <summary>Added to a running sum, applied as a single percentage (e.g. +25% becomes ×1.25 with no other PctAdd).</summary>
    PercentAdd,

    /// <summary>Multiplied in sequence (e.g. two +25% PctMult become ×1.25 × 1.25 = ×1.5625).</summary>
    PercentMult
}
