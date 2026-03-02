using UnityEngine;

public class PlayerDisplayExecuteGaugeUI : DisplayGaugeUI
{
    protected override float gaugeValueRefNormalized => this.playerInfo.executeGauge._gauge / this.playerInfo.executeGauge.maxGauge;
}
