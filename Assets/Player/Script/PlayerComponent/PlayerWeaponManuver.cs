using System.Collections;
using UnityEngine;

public class PlayerWeaponManuver : WeaponManuverManager
{
    private Player player;
    public PlayerWeaponManuver(IWeaponAdvanceUser weaponAdvanceUser,Player player) : base(weaponAdvanceUser)
    {
        this.player = player;
    }

    public override WeaponManuverSelectorNode swtichingWeaponManuverSelector { get ;protected set; }
    public override PrimaryToSecondarySwitchWeaponManuverLeafNode primaryToSecondarySwitchWeaponManuverLeafNode { get ; protected set ; }
    public override SecondaryToPrimarySwitchWeaponManuverLeafNode secondaryToPrimarySwitchWeaponManuverLeafNode { get; protected set; }
    public override AimDownSightWeaponManuverNodeLeaf aimDownSightWeaponManuverNodeLeaf { get; protected set    ; }
    public override LowReadyWeaponManuverNodeLeaf lowReadyWeaponManuverNodeLeaf { get; protected set; }
    public override RestWeaponManuverLeafNode restWeaponManuverLeafNode { get; protected set; }

    protected override void InitailzedWeaponManuverNode()
    {
        swtichingWeaponManuverSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser,
             () => isSwitchWeaponManuver);
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

        startWeaponManuverSelectorNode = new WeaponManuverSelectorNode(this.weaponAdvanceUser, () => true);

        startWeaponManuverSelectorNode.AddtoChildNode(swtichingWeaponManuverSelector);
        startWeaponManuverSelectorNode.AddtoChildNode(aimDownSightWeaponManuverNodeLeaf);
        startWeaponManuverSelectorNode.AddtoChildNode(lowReadyWeaponManuverNodeLeaf);
        startWeaponManuverSelectorNode.AddtoChildNode(restWeaponManuverLeafNode);

        swtichingWeaponManuverSelector.AddtoChildNode(primaryToSecondarySwitchWeaponManuverLeafNode);
        swtichingWeaponManuverSelector.AddtoChildNode(secondaryToPrimarySwitchWeaponManuverLeafNode);

        startWeaponManuverSelectorNode.FindingNode(out WeaponManuverLeafNode weaponManuverLeafNode);
        curWeaponManuverLeafNode = weaponManuverLeafNode;
    }
   
    public override void UpdateNode()
    {
        Debug.Log("Debug from PlayerWeaponManuver" + curWeapon);
        OnservePlayerStateNode(this.player);
        base.UpdateNode();
    }
    public void OnservePlayerStateNode(Player player)
    {

        IWeaponAdvanceUser weaponAdvanceUser = player ;
        PlayerActionNodeLeaf playerActionNodeLeaf = player.curPlayerActionNode;

        if(player.isSprint)
        {
            if (weaponAdvanceUser.isReloadCommand)
                isReloadManuver = true;
            if (weaponAdvanceUser.isSwitchWeaponCommand)
                isSwitchWeaponManuver = true;

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

