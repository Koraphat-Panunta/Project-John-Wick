using UnityEngine;

public class PlayerDisplayStaminaGaugeUI : DisplayGaugeUI
{
    protected override float gaugeValueRefNormalized => this.playerInfo.staminaGauge._gauge / this.playerInfo.staminaGauge.maxGauge;
}
