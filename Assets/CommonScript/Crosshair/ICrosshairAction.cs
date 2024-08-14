using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICrosshairAction 
{
    public void Performed(Weapon weapon);

    public void Performed(PlayerStateManager playerStateManager);
    
}
