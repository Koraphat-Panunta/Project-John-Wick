using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HpRegenarate
{
    private Player player;
    private float regenarate_rate = 1;
    public float regenarate_countDown = 3;
    public HpRegenarate(Player player) 
    {
        this.player = player;
    }
    public void Regenarate()
    {
        if (regenarate_countDown > 0)
        {
            regenarate_countDown -= Time.deltaTime;
        }
        else
        {
            if(player.GetHP() < 100)
            {
                player.SetHP(Mathf.Clamp(player.GetHP() + regenarate_rate * Time.deltaTime, 0, 100));
            }
        }
    }
}