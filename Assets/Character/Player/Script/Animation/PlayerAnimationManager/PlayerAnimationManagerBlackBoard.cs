using UnityEngine;

public partial class PlayerAnimationManager
{
    private INodeManager playerStateNodeMnager => player.playerStateNodeManager;
    private INodeManager playerWeaponManuverNodeManager => player.weaponAdvanceUser._weaponManuverManager;
    private bool isEnableUpperLayer { get 
        {
            if(playerWeaponManuverNodeManager.TryGetCurNodeLeaf<RestWeaponManuverLeafNode>())
                return false;

            if(playerStateNodeMnager.TryGetCurNodeLeaf<PlayerGunFuHitNodeLeaf>()
                || playerStateNodeMnager.TryGetCurNodeLeaf<PlayerDodgeRollStateNodeLeaf>()
                || (playerStateNodeMnager.TryGetCurNodeLeaf(out RestrictGunFuStateNodeLeaf nodeLeaf) && (nodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Enter || nodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Exit))
                || (playerStateNodeMnager.TryGetCurNodeLeaf(out HumanShield_GunFuInteraction_NodeLeaf humanShield)   && (humanShield.curPhase == PlayerStateNodeLeaf.NodePhase.Enter || humanShield.curPhase == PlayerStateNodeLeaf.NodePhase.Exit))
                || playerStateNodeMnager.TryGetCurNodeLeaf<WeaponDisarm_GunFuInteraction_NodeLeaf>()
                )
                return false;
            return true;
        } 
    }
    private bool isPerformGunFu { get 
        {
            if(playerStateNodeMnager.GetCurNodeLeaf() is IGunFuNode)
                return true;
            return false;
        } 
    }
    private bool isDrawSwitchWeapon { get 
        { 
            if(playerWeaponManuverNodeManager.TryGetCurNodeLeaf<PrimaryToSecondarySwitchWeaponManuverLeafNode>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<SecondaryToPrimarySwitchWeaponManuverLeafNode>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<DrawPrimaryWeaponManuverNodeLeaf>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<DrawSecondaryWeaponManuverNodeLeaf>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuickDrawWeaponManuverLeafNodeLeaf>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<HolsterPrimaryWeaponManuverNodeLeaf>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<HolsterSecondaryWeaponManuverNodeLeaf>()
                )
                return true;
            return false;
        } 
    }
    private bool isPerformReload { get 
        {
            if(playerWeaponManuverNodeManager.TryGetCurNodeLeaf<IReloadNode>())
                return true;
            return false;
        } }
}
