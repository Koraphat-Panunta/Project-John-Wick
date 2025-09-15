using UnityEngine;

public partial class PlayerConstrainAnimationManager 
{
    protected INodeManager playerStateManager => player.playerStateNodeManager;
    protected INodeManager playerWeaponManuverStateManager => player._weaponManuverManager;
   
    protected bool isConstraintEnable
    {
        get
        {

            if (playerStateManager.TryGetCurNodeLeaf<GunFuHitNodeLeaf>()
                || playerStateManager.TryGetCurNodeLeaf<IGunFuExecuteNodeLeaf>()
                || playerStateManager.TryGetCurNodeLeaf<PlayerDodgeRollStateNodeLeaf>())
                return false;

            if(player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0)
                return true;

            return false;
        } 
    }
}
