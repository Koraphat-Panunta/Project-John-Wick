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

            if(this.isWeaponSwitching)
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

    protected bool isProne
    {
        get 
        {
            return this.playerStateManager.TryGetCurNodeLeaf<PlayerProneStateNodeLeaf>()
            || (this.playerStateManager.TryGetCurNodeLeaf<PlayerDolphinDiveStateNodeLeaf>(out PlayerDolphinDiveStateNodeLeaf dolphinDiveNode)
            && dolphinDiveNode.isPassingJump);
        }
    }
    protected void EnableIK() => this.isEnableIK = true;
    protected void DisableIK() => this.isEnableIK = false;  

    protected bool isEnableIK;

   

    protected bool isWeaponSwitching
    {
        get
        {
            if(playerWeaponManuverStateManager.TryGetCurNodeLeaf<DrawPrimaryWeaponManuverNodeLeaf>()
                || playerWeaponManuverStateManager.TryGetCurNodeLeaf<DrawSecondaryWeaponManuverNodeLeaf>()
                || playerWeaponManuverStateManager.TryGetCurNodeLeaf<PrimaryToSecondarySwitchWeaponManuverLeafNode>()
                || playerWeaponManuverStateManager.TryGetCurNodeLeaf<SecondaryToPrimarySwitchWeaponManuverLeafNode>()
                || playerWeaponManuverStateManager.TryGetCurNodeLeaf<QuickSwitch_Draw_NodeLeaf>()
                || playerWeaponManuverStateManager.TryGetCurNodeLeaf<QuickSwitch_HolsterPrimaryWeapon_NodeLeaf>()
                || playerWeaponManuverStateManager.TryGetCurNodeLeaf<QuickSwitch_HolsterSecondaryWeapon_NodeLeaf>())
                return true;

            return false;
        }
    }
}
