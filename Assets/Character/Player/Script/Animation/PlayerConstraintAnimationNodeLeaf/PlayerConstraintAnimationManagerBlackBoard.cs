using UnityEngine;

public partial class PlayerConstrainAnimationManager 
{
    protected INodeManager playerStateManager => player.playerStateNodeManager;
    protected INodeManager playerWeaponManuverStateManager => player._weaponManuverManager;
   
    protected bool isConstraintEnable 
    { 
        get 
        {
            if(player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0)
                return true;
            return false;
        } 
    }
}
