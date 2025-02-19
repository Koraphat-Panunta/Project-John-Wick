using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HpRegenarate
{
    private Player player;
    private float regenarate_rate = 30;
    public float regenarate_countDown = 3;
    public HpRegenarate(Player player) 
    {
        this.player = player;
    }
    public void Regenarate()
    {
        if(player.isDead)
            return;

        if (regenarate_countDown > 0)
        {
            regenarate_countDown -= Time.deltaTime;
        }
        else
        {
            if(player.GetHP() < 45)
            {
                player.NotifyObserver(player, SubjectPlayer.PlayerAction.HealthRegen);
                player.SetHP(Mathf.Clamp(player.GetHP() + regenarate_rate * Time.deltaTime, 0, 100));
            }
        }
    }
}
