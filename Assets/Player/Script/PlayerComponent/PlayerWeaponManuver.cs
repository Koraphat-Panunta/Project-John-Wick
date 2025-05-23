using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerWeaponManuver : WeaponManuverManager
{
    private Player player;
    public PlayerWeaponManuver(IWeaponAdvanceUser weaponAdvanceUser,Player player) : base(weaponAdvanceUser)
    {
        this.player = player;
    }

    public WeaponManuverSelectorNode curWeaponManuverSelectorNode { get; protected set; }
    public QuickDrawWeaponManuverLeafNode quickDrawWeaponManuverAtAmmoOutLeafNode { get; protected set; }
    public override WeaponManuverSelectorNode swtichingWeaponManuverSelector { get ;protected set; }
    public RestWeaponManuverLeafNode restWeaponSwitchManuver { get; set; }
    public QuickDrawWeaponManuverLeafNode quickDrawWeaponManuverLeafNode { get ;protected set; }
    public override PrimaryToSecondarySwitchWeaponManuverLeafNode primaryToSecondarySwitchWeaponManuverLeafNode { get ; protected set ; }
    public override SecondaryToPrimarySwitchWeaponManuverLeafNode secondaryToPrimarySwitchWeaponManuverLeafNode { get; protected set; }
    public override AimDownSightWeaponManuverNodeLeaf aimDownSightWeaponManuverNodeLeaf { get; protected set    ; }
    public override LowReadyWeaponManuverNodeLeaf lowReadyWeaponManuverNodeLeaf { get; protected set; }
    public override RestWeaponManuverLeafNode restWeaponManuverLeafNode { get; protected set; }
    public override PickUpWeaponNodeLeaf pickUpWeaponNodeLeaf { get;protected set; }
    public override DropWeaponManuverNodeLeaf dropWeaponManuverNodeLeaf { get; protected set ; }

    public WeaponManuverSelectorNode drawWeaponManuverSelectorNode { get; protected set; }
    public override DrawPrimaryWeaponManuverNodeLeaf drawPrimaryWeaponManuverNodeLeaf { get; protected set; }
    public DrawPrimaryWeaponManuverNodeLeaf drawPrimaryWeaponInWeaponManuvering;
    public override DrawSecondaryWeaponManuverNodeLeaf drawSecondaryWeaponManuverNodeLeaf { get; protected set; }
    public DrawSecondaryWeaponManuverNodeLeaf drawSecondaryWeaponInWeaponManuveringNodeLeaf;
    public override HolsterPrimaryWeaponManuverNodeLeaf holsterPrimaryWeaponManuverNodeLeaf { get ; protected set ; }
    public override HolsterSecondaryWeaponManuverNodeLeaf holsterSecondaryWeaponManuverNodeLeaf { get ; protected set ; }

    public override void InitailizedNode()
    {
        pickUpWeaponNodeLeaf = new PickUpWeaponNodeLeaf(weaponAdvanceUser,
            ()=> 
            {
                if(isPickingUpWeaponManuver)
                    if(weaponAdvanceUser.findingWeaponBehavior.FindingWeapon())
                        return true;
                else return false;
                else return false;
            }
           
            );  
        curWeaponManuverSelectorNode = new WeaponManuverSelectorNode(this.weaponAdvanceUser, () => curWeapon != null);
        dropWeaponManuverNodeLeaf = new DropWeaponManuverNodeLeaf(this.weaponAdvanceUser, () => isDropWeaponManuver);
        quickDrawWeaponManuverAtAmmoOutLeafNode = new QuickDrawWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => 
            {
                if(curWeapon == null)
                    return false;

                if (weaponAdvanceUser.weaponBelt.secondaryWeapon == null)
                    return false;

                if(curWeapon is PrimaryWeapon && isAimingManuver && curWeapon.bulletStore[BulletStackType.Chamber]<=0 && curWeapon.bulletStore[BulletStackType.Magazine] <= 0 && curWeapon.triggerState == TriggerState.IsDown)
                    return true;

                return false;
            }); 
        swtichingWeaponManuverSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser,
             () =>
             {
                if(isSwitchWeaponManuver)
                     return true;
                return false;
             }
            
             );
       
        quickDrawWeaponManuverLeafNode = new QuickDrawWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => 
            {
                if(curWeapon == null)
                    return false;

                if(isAimingManuver && curWeapon is PrimaryWeapon && weaponAdvanceUser.weaponBelt.secondaryWeapon != null)
                    return true;

                return false;
            });
        drawPrimaryWeaponInWeaponManuvering = new DrawPrimaryWeaponManuverNodeLeaf(weaponAdvanceUser,
            () =>
            {
                if(weaponAdvanceUser._currentWeapon == weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon)
                    return false;

                if(weaponAdvanceUser._currentWeapon != weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon && weaponAdvanceUser._currentWeapon != weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon
                &&weaponAdvanceUser.weaponBelt.primaryWeapon != null)
                    return true;

                return false;
            });

        drawSecondaryWeaponInWeaponManuveringNodeLeaf = new DrawSecondaryWeaponManuverNodeLeaf(weaponAdvanceUser,
            () => 
            {
                if (weaponAdvanceUser._currentWeapon == weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon)
                    return false;

                if (weaponAdvanceUser._currentWeapon != weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon && weaponAdvanceUser._currentWeapon != weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon
               && weaponAdvanceUser.weaponBelt.secondaryWeapon != null)
                    return true;
                return false;
            }
            );
        primaryToSecondarySwitchWeaponManuverLeafNode = new PrimaryToSecondarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => curWeapon == weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon && weaponAdvanceUser.weaponBelt.secondaryWeapon != null);
        secondaryToPrimarySwitchWeaponManuverLeafNode = new SecondaryToPrimarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => curWeapon == weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon && weaponAdvanceUser.weaponBelt.primaryWeapon != null);
        holsterPrimaryWeaponManuverNodeLeaf = new HolsterPrimaryWeaponManuverNodeLeaf(this.weaponAdvanceUser, 
            () => curWeapon == weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon);
        holsterSecondaryWeaponManuverNodeLeaf = new HolsterSecondaryWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => curWeapon == weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon);
        restWeaponSwitchManuver = new RestWeaponManuverLeafNode(weaponAdvanceUser,()=> true);

        aimDownSightWeaponManuverNodeLeaf = new AimDownSightWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => isAimingManuver);
        lowReadyWeaponManuverNodeLeaf = new LowReadyWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => true);

        drawWeaponManuverSelectorNode = new WeaponManuverSelectorNode(this.weaponAdvanceUser,
            () => isSwitchWeaponManuver && (weaponAdvanceUser.weaponBelt.primaryWeapon != null || weaponAdvanceUser.weaponBelt.secondaryWeapon != null));
        drawPrimaryWeaponManuverNodeLeaf = new DrawPrimaryWeaponManuverNodeLeaf(this.weaponAdvanceUser,
         () => weaponAdvanceUser.weaponBelt.primaryWeapon != null);
        drawSecondaryWeaponManuverNodeLeaf = new DrawSecondaryWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => weaponAdvanceUser.weaponBelt.secondaryWeapon != null);

        restWeaponManuverLeafNode = new RestWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => true);

        startNodeSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser, () => true);

        startNodeSelector.AddtoChildNode(pickUpWeaponNodeLeaf);
        startNodeSelector.AddtoChildNode(curWeaponManuverSelectorNode);

        curWeaponManuverSelectorNode.AddtoChildNode(dropWeaponManuverNodeLeaf);
        curWeaponManuverSelectorNode.AddtoChildNode(quickDrawWeaponManuverAtAmmoOutLeafNode);
        curWeaponManuverSelectorNode.AddtoChildNode(swtichingWeaponManuverSelector);
        curWeaponManuverSelectorNode.AddtoChildNode(aimDownSightWeaponManuverNodeLeaf);
        curWeaponManuverSelectorNode.AddtoChildNode(lowReadyWeaponManuverNodeLeaf);

        startNodeSelector.AddtoChildNode(drawWeaponManuverSelectorNode);
        startNodeSelector.AddtoChildNode(restWeaponManuverLeafNode);

        swtichingWeaponManuverSelector.AddtoChildNode(quickDrawWeaponManuverLeafNode);
        swtichingWeaponManuverSelector.AddtoChildNode(drawPrimaryWeaponInWeaponManuvering);
        swtichingWeaponManuverSelector.AddtoChildNode(drawSecondaryWeaponInWeaponManuveringNodeLeaf);
        swtichingWeaponManuverSelector.AddtoChildNode(primaryToSecondarySwitchWeaponManuverLeafNode);
        swtichingWeaponManuverSelector.AddtoChildNode(secondaryToPrimarySwitchWeaponManuverLeafNode);
        swtichingWeaponManuverSelector.AddtoChildNode(holsterPrimaryWeaponManuverNodeLeaf);
        swtichingWeaponManuverSelector.AddtoChildNode(holsterSecondaryWeaponManuverNodeLeaf);
        swtichingWeaponManuverSelector.AddtoChildNode(restWeaponSwitchManuver);

        drawWeaponManuverSelectorNode.AddtoChildNode(drawPrimaryWeaponManuverNodeLeaf);
        drawWeaponManuverSelectorNode.AddtoChildNode(drawSecondaryWeaponManuverNodeLeaf);

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

        if(player.playerStateNodeManager.curNodeLeaf is PlayerGunFuHitNodeLeaf || player.playerStateNodeManager.curNodeLeaf is PlayerGunFu_Interaction_NodeLeaf)
        {
            if (player.playerStateNodeManager.curNodeLeaf is HumanShield_GunFuInteraction_NodeLeaf humanShield
                && humanShield.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Stay)
            {
                isAimingManuver = player.isAimingCommand;
                isPullTriggerManuver = player.isPullTriggerCommand;
                return;
            }
            if(player.playerStateNodeManager.curNodeLeaf is HumanThrowGunFuInteractionNodeLeaf)
            {
                isAimingManuver = player.isAimingCommand;
                isPullTriggerManuver = false;
                return;
            }
            //else if (player.playerStateNodeManager.curNodeLeaf is WeaponDisarm_GunFuInteraction_NodeLeaf)
            //{
               
            //    return;
            //}
            return;
        }

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
        
            WeaponAdvanceCommanding(weaponAdvanceUser);
        
       
    }

    private void WeaponAdvanceCommanding(IWeaponAdvanceUser weaponAdvanceUser)
    {
        isAimingManuver = weaponAdvanceUser.isAimingCommand;

        isReloadManuver = weaponAdvanceUser.isReloadCommand;

        isSwitchWeaponManuver = weaponAdvanceUser.isSwitchWeaponCommand;

        isPullTriggerManuver = weaponAdvanceUser.isPullTriggerCommand;

        isPickingUpWeaponManuver = weaponAdvanceUser.isPickingUpWeaponCommand;

        isDropWeaponManuver = weaponAdvanceUser.isDropWeaponCommand;
    }

}

