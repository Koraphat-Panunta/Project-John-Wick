using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerHpRegenarate:IObserverPlayer
{
    private Player player;
    private float regenarate_rate = 8;
    public float regenarate_countDown { get; private set; }
    public float elapesTime = 0;
    public PlayerHpRegenarate(Player player) 
    {
        this.player = player;
        this.player.AddObserver(this);
        regenarate_countDown = 6;
    }
    public void Regenarate()
    {
        if(player.isDead)
            return;

        if (elapesTime > 0)
        {
            elapesTime -= Time.deltaTime;
        }
        else
        {
            player.NotifyObserver(player, SubjectPlayer.NotifyEvent.HealthRegen);
            player.SetHP(Mathf.Clamp(player.GetHP() + regenarate_rate * Time.deltaTime, 0, player.GetMaxHp()));
        }
    }

    public void OnNotify(Player player, SubjectPlayer.NotifyEvent playerAction)
    {
        if(playerAction == SubjectPlayer.NotifyEvent.GetDamaged)
            elapesTime = regenarate_countDown;
    }

    public void OnNotify(Player player)
    {
        
    }
}
