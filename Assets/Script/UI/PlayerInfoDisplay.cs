using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInfoDisplay : IObserverPlayer
{
    public Player playerInfo;
    protected HUD hud;
    public abstract void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction);
    public PlayerInfoDisplay(Player player,HUD hud)
    {
        this.playerInfo = player;
        this.hud = hud;
        this.playerInfo.AddObserver(this);
    }
   
    public void AddPlayerObserver()
    {
        this.playerInfo.AddObserver(this);
    }
    public void RemovePlayerObserver()
    {
        this.playerInfo.RemoveObserver(this);
    }

    public abstract void UpdateInfo();  

    public void OnNotify(Player player)
    {
       
    }
}
