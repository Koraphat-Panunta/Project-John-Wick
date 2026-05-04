using UnityEngine;

public class PlayerDisplayStaminaGaugeUI : DisplayGaugeUI
{
    protected override float gaugeValueRefNormalized => this.playerInfo.staminaGauge._gauge / PlayerStatsScriptableObject.totalMaxStamina;
    public override void OnNotify<T>(Player player, T node)
    {
        base.bg_HP_bar_image.fillAmount = (this.playerInfo.staminaGauge.maxGauge / PlayerStatsScriptableObject.totalMaxStamina) * base.maxAmount;
        base.OnNotify(player, node);
    }
}
