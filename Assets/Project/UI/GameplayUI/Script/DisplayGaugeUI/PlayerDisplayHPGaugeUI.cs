using UnityEngine;

public class PlayerDisplayHPGaugeUI : DisplayGaugeUI
{
    protected override float gaugeValueRefNormalized => this.playerInfo.GetHP() / this.playerInfo.GetMaxHp();
}
