using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInfoDisplay : MonoBehaviour,IObserverPlayer
{
    public Player playerInfo { get; protected set; }

    public abstract void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction);
   

    protected virtual void Start()
    {
        playerInfo = FindAnyObjectByType<Player>().GetComponent<Player>();
    }


}
