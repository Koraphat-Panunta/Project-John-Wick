using System;

/// <summary>
/// One stat change carried by a <see cref="PowerUpScriptableObject"/>.
/// A single power-up can carry multiple effects (e.g. +25 MaxHP AND +10% Reload Speed).
/// </summary>
[Serializable]
public class PowerUpEffect
{
    public StatType targetStat;
    public StatModifierType modType;
    public float value;
}
