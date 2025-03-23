using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributeDisplay : PlayerInfoDisplay
{
    private RawImage HP_bar;
    private float maxHP_BAR_Lenght;
    public PlayerAttributeDisplay(Player player, HUD hud,RawImage HP_bar) : base(player, hud)
    {
        base.hud = hud;
        base.playerInfo = player;
        this.HP_bar = HP_bar;
        this.maxHP_BAR_Lenght = HP_bar.rectTransform.sizeDelta.y;
        Debug.Log("HP_bar.rectTransform.sizeDelta.x = " + HP_bar.rectTransform.sizeDelta.y);
        UpdateInfo();

    }

    public override void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if (playerAction == SubjectPlayer.PlayerAction.GetDamaged)
        {
           UpdateInfo();
        }
        if (playerAction == SubjectPlayer.PlayerAction.HealthRegen)
        {
            UpdateInfo();
        }
        if (playerAction == SubjectPlayer.PlayerAction.RecivedHp)
        {
            UpdateInfo();
        }
    }

    public override void UpdateInfo()
    {
        HP_bar.rectTransform.sizeDelta = new Vector2(HP_bar.rectTransform.sizeDelta.x, this.maxHP_BAR_Lenght*(playerInfo.GetHP()/playerInfo.GetMaxHp()));
    }
}
