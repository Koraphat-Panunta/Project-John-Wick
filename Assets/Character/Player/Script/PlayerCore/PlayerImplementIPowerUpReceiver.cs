using System.Collections.Generic;
using UnityEngine;

public partial class Player : IPowerUpReceiver
{
    /// <summary>Modifiable max-HP. Drives _hpGauge.maxGauge through OnValueChanged.</summary>
    public ModifiableStat maxHpStat { get; private set; }
    public ModifiableStat maxStamina { get; private set; }
    public ModifiableStat ammoProuchTier { get; private set; }

    /// <summary>Per-source list of currently-applied modifiers, for fast removal.</summary>
    private readonly Dictionary<object, List<(StatType stat, StatModifier mod)>> activePowerUps
        = new Dictionary<object, List<(StatType, StatModifier)>>();

    /// <summary>
    /// Call from <see cref="Initialized"/> AFTER _hpGauge is created.
    /// Wires the modifier system to the gauge and clamps current HP if max drops.
    /// </summary>
    public void InitializePowerUpReceiver()
    {
        maxHpStat = new ModifiableStat(this._hpGauge.maxGauge);
        maxHpStat.OnValueChanged += newMax =>
        {
            this._hpGauge.SetMaxGauge(newMax);
            if (this._hpGauge._gauge > newMax)
                this._hpGauge.SetGauge(newMax); // clamp current HP if max dropped
        };

        this.maxStamina = new ModifiableStat(this.staminaGauge.maxGauge);
        this.maxStamina.OnValueChanged += newMax =>
        {
            this.staminaGauge.SetMaxGauge(newMax);
            if (this.staminaGauge._gauge > newMax)
                this.staminaGauge.SetGauge(newMax);
        };

        this.ammoProuchTier = new ModifiableStat(this._weaponBelt.ammoProuch.ammoProuchTier);
        this.ammoProuchTier.OnValueChanged += newTier =>
        {
            this._weaponBelt.ammoProuch.SetMaximumAmmoTier((int)newTier);
            this._weaponBelt.ammoProuch.RefillAmmo();
        };
    }

    public void ApplyPowerUp(PowerUpScriptableObject def, object source)
    {
        if (def == null || source == null)
        {
            Debug.LogWarning("[Player.ApplyPowerUp] Null def or source — ignored.", this);
            return;
        }
        if (activePowerUps.ContainsKey(source))
            return; // idempotent

        var applied = new List<(StatType, StatModifier)>();
        if (def.effects != null)
        {
            for (int i = 0; i < def.effects.Length; i++)
            {
                var effect = def.effects[i];
                var stat = GetStatForType(effect.targetStat);
                if (stat == null) continue;

                var mod = new StatModifier(source, effect.value, effect.modType);
                stat.AddModifier(mod);
                applied.Add((effect.targetStat, mod));


            }
        }
        activePowerUps[source] = applied;
        this.NotifyObserver<SubjectPlayer.NotifyEvent>(this, NotifyEvent.AppliedPowerUp);
    }

    public void RemovePowerUp(object source)
    {
        if (source == null) return;
        if (!activePowerUps.TryGetValue(source, out var applied)) return;

        for (int i = 0; i < applied.Count; i++)
        {
            var (statType, mod) = applied[i];
            var stat = GetStatForType(statType);
            stat?.RemoveModifier(mod);
        }
        activePowerUps.Remove(source);
        this.NotifyObserver<SubjectPlayer.NotifyEvent>(this, NotifyEvent.RemovePowerUp);
    }

    public bool HasPowerUpFromSource(object source)
    {
        return source != null && activePowerUps.ContainsKey(source);
    }

    /// <summary>Resolves a <see cref="StatType"/> to the underlying <see cref="ModifiableStat"/> on this player.</summary>
    private ModifiableStat GetStatForType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHP: return maxHpStat;
            case StatType.MaxStamina: return maxStamina;
            case StatType.AmmoProuch: return ammoProuchTier;
            // Add cases here as new StatType entries are introduced.
            default:
                Debug.LogWarning($"[Player.GetStatForType] No stat wired for {type}.", this);
                return null;
        }
    }
}
