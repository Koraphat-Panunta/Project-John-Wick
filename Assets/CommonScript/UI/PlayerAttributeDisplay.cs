using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributeDisplay : PlayerInfoDisplay
{
    private RawImage HP_bar;
    public PlayerAttributeDisplay(Player player, HUD hud,RawImage HP_bar) : base(player, hud)
    {
        base.hud = hud;
        base.playerInfo = player;
        this.HP_bar = HP_bar;
    }

    public override void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
       if(playerAction == SubjectPlayer.PlayerAction.GetShoot)
        {
            HP_bar.rectTransform.sizeDelta = new Vector2(HP_bar.rectTransform.sizeDelta.x, player.GetHP());
        }
       if(playerAction == SubjectPlayer.PlayerAction.HealthRegen)
        {
            HP_bar.rectTransform.sizeDelta = new Vector2(HP_bar.rectTransform.sizeDelta.x, player.GetHP());
        }
    }
}
