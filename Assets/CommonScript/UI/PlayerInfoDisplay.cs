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
        if(this.playerInfo != null)
        {
            this.hud.StartCoroutine(BeginAddPlayerObserver());
        }
    }
    private IEnumerator BeginAddPlayerObserver()
    {
        yield return new WaitForEndOfFrame();
        if (playerInfo != null)
        {
            playerInfo.AddObserver(this);
        }
    }
    public void AddPlayerObserver()
    {
        this.playerInfo.AddObserver(this);
    }
    public void RemovePlayerObserver()
    {
        this.playerInfo.AddObserver(this);
    }
}
