using UnityEngine;

public class RandomUtil 
{
    /// <summary>
    /// Returns true with probability <paramref name="trueChance"/> (0..1).
    /// </summary>
    /// <param name="trueChance">Probability of returning true. Clamped: 0 → always false, 1 → always true. Default 0.5 = coin flip.</param>
    /// <example>
    /// if (RandomUtil.Chance(0.25f)) {  ...25% of the time...  }
    /// bool willCrit = RandomUtil.Chance(critRate);
    /// </example>
    public static bool Chance(float trueChance = 0.5f)
    {
        if (trueChance <= 0f) return false;
        if (trueChance >= 1f) return true;
        return Random.value < trueChance;
    }

    /// <summary>
    /// Same as <see cref="Chance(float)"/> but takes a percent value (0..100) for readability.
    /// </summary>
    /// <example>
    /// if (RandomUtil.ChancePercent(15)) {  ...15% of the time...  }
    /// </example>
    public static bool ChancePercent(float percent)
    {
        return Chance(percent * 0.01f);
    }
}
