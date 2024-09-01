using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverPlayer
{
    public void OnNotify(Player player,SubjectPlayer.PlayerAction playerAction);
    

    
}
