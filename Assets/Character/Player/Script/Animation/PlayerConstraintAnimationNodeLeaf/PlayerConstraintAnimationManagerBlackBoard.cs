using UnityEngine;

public partial class PlayerConstrainAnimationManager 
{
    protected INodeManager playerStateManaher => player.playerStateNodeManager;
    protected INodeManager playerWeaponManuverStateManager => player._weaponManuverManager;
   
    protected bool isConstraintEnable 
    { 
        get 
        {
            if(playerWeaponManuverStateManager.TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>())
                return true;
            return false;
        } 
    }
}
