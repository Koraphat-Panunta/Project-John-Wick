using UnityEngine;

public class PlayerDisplayHPGaugeUI : DisplayGaugeUI
{
    protected override float gaugeValueRefNormalized => this.playerInfo.GetHP() / PlayerStatsScriptableObject.totalMaxHP;
    protected float bgValueRefNormalized => (this.playerInfo.GetMaxHp() / PlayerStatsScriptableObject.totalMaxHP) * base.maxAmount;

  
    public override void OnNotify<T>(Player player, T node)
    {
        base.bg_HP_bar_image.fillAmount = this.bgValueRefNormalized;
        base.OnNotify(player, node);
    }


}
