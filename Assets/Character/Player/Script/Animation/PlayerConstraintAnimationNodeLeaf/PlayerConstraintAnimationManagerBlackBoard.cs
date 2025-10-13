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
                || playerStateManager.TryGetCurNodeLeaf<PlayerDodgeRollStateNodeLeaf>()
                || playerStateManager.TryGetCurNodeLeaf<PlayerSprintNode>())
                return false;

            return true;
        } 
    }
    protected bool isWeaponGripConstraitEnable
    {
        get 
        {
            if(player._currentWeapon == null)
                return false;

            if(playerStateManager.TryGetCurNodeLeaf<IGunFuNode>())
                return false;

            if(playerStateManager.TryGetCurNodeLeaf<IParkourNodeLeaf>())
                return false;

            if(playerWeaponManuverStateManager.TryGetCurNodeLeaf<IReloadNode>())
                return false;

            if (playerWeaponManuverStateManager.TryGetCurNodeLeaf<IQuickSwitchNode>())
                return false;

            

            return true;
        }
    }

    protected bool isHeadLookEnable 
    { 
        get 
        {
            if(playerWeaponManuverStateManager.TryGetCurNodeLeaf<IReloadNode>())
                return false;

            if(playerStateManager.TryGetCurNodeLeaf<IGunFuNode>())
                return false;

            return true;
        } 
    }
}
