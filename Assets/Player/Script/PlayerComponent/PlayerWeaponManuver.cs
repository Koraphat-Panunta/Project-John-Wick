using System.Collections;
using UnityEngine;

public class PlayerWeaponManuver : WeaponManuverManager
{
    private Player player;
    public PlayerWeaponManuver(IWeaponAdvanceUser weaponAdvanceUser,Player player) : base(weaponAdvanceUser)
    {
        this.player = player;
    }

    public QuickDrawWeaponManuverLeafNode quickDrawWeaponManuverAtAmmoOutLeafNode { get; protected set; }
    public override WeaponManuverSelectorNode swtichingWeaponManuverSelector { get ;protected set; }
    public QuickDrawWeaponManuverLeafNode quickDrawWeaponManuverLeafNode { get ;protected set; }
    public override PrimaryToSecondarySwitchWeaponManuverLeafNode primaryToSecondarySwitchWeaponManuverLeafNode { get ; protected set ; }
    public override SecondaryToPrimarySwitchWeaponManuverLeafNode secondaryToPrimarySwitchWeaponManuverLeafNode { get; protected set; }
    public override AimDownSightWeaponManuverNodeLeaf aimDownSightWeaponManuverNodeLeaf { get; protected set    ; }
    public override LowReadyWeaponManuverNodeLeaf lowReadyWeaponManuverNodeLeaf { get; protected set; }
    public override RestWeaponManuverLeafNode restWeaponManuverLeafNode { get; protected set; }

    public override void InitailizedNode()
    {
        quickDrawWeaponManuverAtAmmoOutLeafNode = new QuickDrawWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => 
            {
                if(isAimingManuver 
                && curWeapon is PrimaryWeapon
                && curWeapon.bulletStore[BulletStackType.Chamber] + curWeapon.bulletStore[BulletStackType.Magazine] <= 0
                && curWeapon.triggerState == TriggerState.IsDown
                && ((player.playerStateNodeManager.curNodeLeaf is HumanShield_GunFuInteraction_NodeLeaf) == false && (player.playerStateNodeManager.curNodeLeaf is HumanThrowGunFuInteractionNodeLeaf) == false))
                    return true;

                return false;
            }); 
        swtichingWeaponManuverSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser,
             () => isSwitchWeaponManuver);
        quickDrawWeaponManuverLeafNode = new QuickDrawWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => 
            {
                if(isAimingManuver && curWeapon is PrimaryWeapon)
                    return true;

                return false;
            });
        primaryToSecondarySwitchWeaponManuverLeafNode = new PrimaryToSecondarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => curWeapon is PrimaryWeapon);
        secondaryToPrimarySwitchWeaponManuverLeafNode = new SecondaryToPrimarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => curWeapon is SecondaryWeapon);
        aimDownSightWeaponManuverNodeLeaf = new AimDownSightWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => isAimingManuver);
        lowReadyWeaponManuverNodeLeaf = new LowReadyWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => curWeapon != null);

        restWeaponManuverLeafNode = new RestWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => true);

        startNodeSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser, () => true);

        startNodeSelector.AddtoChildNode(quickDrawWeaponManuverAtAmmoOutLeafNode);
        startNodeSelector.AddtoChildNode(swtichingWeaponManuverSelector);
        startNodeSelector.AddtoChildNode(aimDownSightWeaponManuverNodeLeaf);
        startNodeSelector.AddtoChildNode(lowReadyWeaponManuverNodeLeaf);
        startNodeSelector.AddtoChildNode(restWeaponManuverLeafNode);

        swtichingWeaponManuverSelector.AddtoChildNode(quickDrawWeaponManuverLeafNode);
        swtichingWeaponManuverSelector.AddtoChildNode(primaryToSecondarySwitchWeaponManuverLeafNode);
        swtichingWeaponManuverSelector.AddtoChildNode(secondaryToPrimarySwitchWeaponManuverLeafNode);

        startNodeSelector.FindingNode(out INodeLeaf weaponManuverLeafNode);
        curNodeLeaf = weaponManuverLeafNode as WeaponManuverLeafNode;
    }
   
    public override void UpdateNode()
    {
        OnservePlayerStateNode(this.player);

        base.UpdateNode();
    }
    public void OnservePlayerStateNode(Player player)
    {

        IWeaponAdvanceUser weaponAdvanceUser = player ;
        PlayerStateNodeLeaf playerActionNodeLeaf = player.playerStateNodeManager.curNodeLeaf as PlayerStateNodeLeaf;

        if(player.isDead)
            return;

        if (player.playerStateNodeManager.curNodeLeaf is PlayerDodgeRollStateNodeLeaf)
        {
            isAimingManuver = false;
            isPullTriggerManuver = false;
            return;
        }

        if(player.isSprint)
        {
            isReloadManuver = weaponAdvanceUser.isReloadCommand;    
            isSwitchWeaponManuver = weaponAdvanceUser.isSwitchWeaponCommand;

            isAimingManuver = false;
            isPullTriggerManuver = false;

            return;
        }
        else
        {
            WeaponAdvanceCommanding(weaponAdvanceUser);
        }
       
    }

    private void WeaponAdvanceCommanding(IWeaponAdvanceUser weaponAdvanceUser)
    {
        isAimingManuver = weaponAdvanceUser.isAimingCommand;

        isReloadManuver = weaponAdvanceUser.isReloadCommand;

        isSwitchWeaponManuver = weaponAdvanceUser.isSwitchWeaponCommand;

        isPullTriggerManuver = weaponAdvanceUser.isPullTriggerCommand;
        
    }

}

