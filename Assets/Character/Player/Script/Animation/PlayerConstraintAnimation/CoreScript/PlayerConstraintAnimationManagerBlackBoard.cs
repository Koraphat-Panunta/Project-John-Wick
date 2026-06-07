using UnityEngine;

public partial class PlayerConstrainAnimationManager 
{
    protected INodeManager playerStateManager => player.playerStateNodeManager;
    protected INodeManager playerWeaponManuverStateManager => player._weaponManuverManager;
   
    protected bool isConstraintEnable
    {
        get
        {

            if (playerStateManager.TryGetCurNodeLeaf<OCM_Hit_NodeLeaf>()
                || playerStateManager.TryGetCurNodeLeaf<IGunFuExecuteNodeLeaf>()
                || playerStateManager.TryGetCurNodeLeaf<PlayerDodgeRollStateNodeLeaf>()
                || playerStateManager.TryGetCurNodeLeaf<PlayerSprintNode>() 
                || playerStateManager.TryGetCurNodeLeaf<PlayerSprintChangeDirectionNode>())
                return false;

            return true;
        } 
    }
    protected bool isWeaponGripConstraitEnable
    {
        get 
        {
            if (this.player._currentWeapon == null)
                return false;

            if ((this.player._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<IReloadNode>())
                return false;

            if ((this.player._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<IQuickSwitchNode>())
                return false;

            if ((this.player._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<DrawPrimaryWeaponManuverNodeLeaf>()
                || (this.player._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<DrawSecondaryWeaponManuverNodeLeaf>()
                || (this.player._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<HolsterPrimaryWeaponManuverNodeLeaf>()
                || (this.player._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<HolsterSecondaryWeaponManuverNodeLeaf>())
                return false;

            if (this.player.stateNodeManager.TryGetCurNodeLeaf<PlayerStandIdleNodeLeaf>()
                || this.player.stateNodeManager.TryGetCurNodeLeaf<PlayerStandMoveNodeLeaf>()
                || this.player.stateNodeManager.TryGetCurNodeLeaf<PlayerCrouch_Idle_NodeLeaf>()
                || this.player.stateNodeManager.TryGetCurNodeLeaf<PlayerCrouch_Move_NodeLeaf>()
                || this.player.stateNodeManager.TryGetCurNodeLeaf<PlayerProneStateNodeLeaf>()
                || this.player.stateNodeManager.TryGetCurNodeLeaf<PlayerDolphinDiveStateNodeLeaf>()
                || this.player.stateNodeManager.TryGetCurNodeLeaf<PlayerSprintNode>()
                || this.player.stateNodeManager.TryGetCurNodeLeaf<PlayerSprintChangeDirectionNode>()
                )
                return true;

            return false;

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
                || playerWeaponManuverStateManager.TryGetCurNodeLeaf<QuickSwitch_Draw_NodeLeaf>()
                || playerWeaponManuverStateManager.TryGetCurNodeLeaf<QuickSwitch_HolsterPrimaryWeapon_NodeLeaf>()
                || playerWeaponManuverStateManager.TryGetCurNodeLeaf<QuickSwitch_HolsterSecondaryWeapon_NodeLeaf>())
                return true;

            return false;
        }
    }
}
