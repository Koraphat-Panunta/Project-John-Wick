using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerWeaponManuver : WeaponManuverManager
{
    private Player player => weaponAdvanceUser as Player;
    public PlayerWeaponManuver(IWeaponAdvanceUser weaponAdvanceUser,Player player) : base(weaponAdvanceUser)
    {

    }
    #region Initailized WeaponManuver Property
    public override bool isAimingManuverAble { get 
        {
            if (player.curNodeLeaf is PlayerCrouch_Idle_NodeLeaf
                || player.curNodeLeaf is PlayerCrouch_Move_NodeLeaf
                || player.curNodeLeaf is PlayerStandIdleNodeLeaf
                || player.curNodeLeaf is PlayerStandMoveNodeLeaf
                || player.curNodeLeaf is PlayerInCoverStandIdleNodeLeaf
                || player.curNodeLeaf is PlayerInCoverStandMoveNodeLeaf
                || player.curNodeLeaf is RestrictGunFuStateNodeLeaf
                || player.curNodeLeaf is HumanShield_GunFuInteraction_NodeLeaf
                )
                return true;
            return false; 
        } 
    }

    public override bool isPullTriggerManuverAble {
        get 
        {
            if(aimingWeight >= 1)
                return true;
            return false;
        }
    }

    public override bool isReloadManuverAble
    {
        get {
            if (player.curNodeLeaf is PlayerCrouch_Idle_NodeLeaf
               || player.curNodeLeaf is PlayerCrouch_Move_NodeLeaf
               || player.curNodeLeaf is PlayerStandIdleNodeLeaf
               || player.curNodeLeaf is PlayerStandMoveNodeLeaf
               || player.curNodeLeaf is PlayerInCoverStandIdleNodeLeaf
               || player.curNodeLeaf is PlayerInCoverStandMoveNodeLeaf
               || player.curNodeLeaf is PlayerSprintNode
               )
                return true;
            return false;
        }
    }

    public override bool isSwitchWeaponManuverAble 
    {
        get
        {
            if (player.curNodeLeaf is PlayerCrouch_Idle_NodeLeaf
               || player.curNodeLeaf is PlayerCrouch_Move_NodeLeaf
               || player.curNodeLeaf is PlayerStandIdleNodeLeaf
               || player.curNodeLeaf is PlayerStandMoveNodeLeaf
               || player.curNodeLeaf is PlayerInCoverStandIdleNodeLeaf
               || player.curNodeLeaf is PlayerInCoverStandMoveNodeLeaf
               || player.curNodeLeaf is PlayerSprintNode
               )
                return true;
            return false;
        }
    }

    public override bool isPickingUpWeaponManuverAble 
    {
        get
        {
            if (player.curNodeLeaf is PlayerCrouch_Idle_NodeLeaf
               || player.curNodeLeaf is PlayerCrouch_Move_NodeLeaf
               || player.curNodeLeaf is PlayerStandIdleNodeLeaf
               || player.curNodeLeaf is PlayerStandMoveNodeLeaf
               || player.curNodeLeaf is PlayerInCoverStandIdleNodeLeaf
               || player.curNodeLeaf is PlayerInCoverStandMoveNodeLeaf
               || player.curNodeLeaf is PlayerSprintNode
               )
                return true;
            return false;
        }
    }

    public override bool isDropWeaponManuverAble 
    {
        get
        {
            if (player.curNodeLeaf is PlayerCrouch_Idle_NodeLeaf
               || player.curNodeLeaf is PlayerCrouch_Move_NodeLeaf
               || player.curNodeLeaf is PlayerStandIdleNodeLeaf
               || player.curNodeLeaf is PlayerStandMoveNodeLeaf
               || player.curNodeLeaf is PlayerInCoverStandIdleNodeLeaf
               || player.curNodeLeaf is PlayerInCoverStandMoveNodeLeaf
               || player.curNodeLeaf is PlayerSprintNode
               )
                return true;
            return false;
        }
    }
    private bool isQuickDrawWeaponManuverAble
    {
        get 
        {
            if (player.curNodeLeaf is PlayerCrouch_Idle_NodeLeaf
                || player.curNodeLeaf is PlayerCrouch_Move_NodeLeaf
                || player.curNodeLeaf is PlayerStandIdleNodeLeaf
                || player.curNodeLeaf is PlayerStandMoveNodeLeaf
                || player.curNodeLeaf is PlayerInCoverStandIdleNodeLeaf
                || player.curNodeLeaf is PlayerInCoverStandMoveNodeLeaf
                )
                return true;
            return false;
        }
    }

    #endregion

    #region InitializedNode
    public override NodeAttachAbleSelector reloadNodeAttachAbleSelector { get ; protected set ; }
    public WeaponManuverSelectorNode curWeaponManuverSelectorNode { get; protected set; }
    public override WeaponManuverSelectorNode swtichingWeaponManuverSelector { get ;protected set; }
    public RestWeaponManuverLeafNode restWeaponSwitchManuver { get; set; }
    public QuickDrawWeaponManuverLeafNodeLeaf quickDrawWeaponManuverLeafNode { get ;protected set; }
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

    #endregion
    public override void InitailizedNode()
    {
        pickUpWeaponNodeLeaf = new PickUpWeaponNodeLeaf(weaponAdvanceUser,
            ()=> 
            {

                if(isPickingUpWeaponManuverAble == false)
                    return false;
                   
                if(weaponAdvanceUser._isPickingUpWeaponCommand && 
                weaponAdvanceUser._findingWeaponBehavior.FindingWeapon())
                    return true;
                else return false;

            }
           
            );  
        curWeaponManuverSelectorNode = new WeaponManuverSelectorNode(this.weaponAdvanceUser, () => curWeapon != null);
        dropWeaponManuverNodeLeaf = new DropWeaponManuverNodeLeaf(this.weaponAdvanceUser, 
            () => 
            {
                if (isDropWeaponManuverAble == false)
                    return false;

                if(weaponAdvanceUser._isDropWeaponCommand)
                    return true;

                return false;
                }
            );
        
        swtichingWeaponManuverSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser,
             () =>
             {
                if(isSwitchWeaponManuverAble == false)
                     return false;

                 if (weaponAdvanceUser._isSwitchWeaponCommand)
                     return true;

                 return false;
             }
            
             );
       
        quickDrawWeaponManuverLeafNode = new QuickDrawWeaponManuverLeafNodeLeaf(this.weaponAdvanceUser,
            () => 
            {
                if(this.isQuickDrawWeaponManuverAble == false)
                    return false;

                if (weaponAdvanceUser._isSwitchWeaponCommand == false)
                    return false;

                if (curWeapon == null)
                    return false;

                if(isAimingManuverAble && curWeapon is PrimaryWeapon && weaponAdvanceUser._weaponBelt.mySecondaryWeapon != null)
                    return true;

                return false;
            });
        drawPrimaryWeaponInWeaponManuvering = new DrawPrimaryWeaponManuverNodeLeaf(weaponAdvanceUser,
            () =>
            {
                if(this.isSwitchWeaponManuverAble == false)
                    return false;

                if (weaponAdvanceUser._isSwitchWeaponCommand == false)
                    return false;

                if (weaponAdvanceUser._currentWeapon == weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon)
                    return false;

                if(weaponAdvanceUser._currentWeapon != weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon && weaponAdvanceUser._currentWeapon != weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon
                &&weaponAdvanceUser._weaponBelt.myPrimaryWeapon != null)
                    return true;

                return false;
            });

        drawSecondaryWeaponInWeaponManuveringNodeLeaf = new DrawSecondaryWeaponManuverNodeLeaf(weaponAdvanceUser,
            () => 
            {
                if (this.isSwitchWeaponManuverAble == false)
                    return false;

                if (weaponAdvanceUser._isSwitchWeaponCommand == false)
                    return false;

                if (weaponAdvanceUser._currentWeapon == weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon)
                    return false;

                if (weaponAdvanceUser._currentWeapon != weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon && weaponAdvanceUser._currentWeapon != weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon
               && weaponAdvanceUser._weaponBelt.mySecondaryWeapon != null)
                    return true;
                return false;
            }
            );
        primaryToSecondarySwitchWeaponManuverLeafNode = new PrimaryToSecondarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => 
            {
                if (this.isSwitchWeaponManuverAble == false)
                    return false;

                if (weaponAdvanceUser._isSwitchWeaponCommand == false)
                    return false;

                if (curWeapon == weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon && weaponAdvanceUser._weaponBelt.mySecondaryWeapon != null)
                    return true;

                return false;
                }
            );
        secondaryToPrimarySwitchWeaponManuverLeafNode = new SecondaryToPrimarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => 
            {
                if (this.isSwitchWeaponManuverAble == false)
                    return false;

                if (weaponAdvanceUser._isSwitchWeaponCommand == false)
                    return false;

                if (curWeapon == weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon && weaponAdvanceUser._weaponBelt.myPrimaryWeapon != null)
                    return true;
                
                return false;
                }
            );
        holsterPrimaryWeaponManuverNodeLeaf = new HolsterPrimaryWeaponManuverNodeLeaf(this.weaponAdvanceUser, 
            () => 
            {
                if( this.isSwitchWeaponManuverAble == false)
                    return false;

                if (weaponAdvanceUser._isSwitchWeaponCommand == false)
                    return false;

                if (curWeapon == weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon)
                    return true;

                return false;
            }
            );
        holsterSecondaryWeaponManuverNodeLeaf = new HolsterSecondaryWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => 
            {
                if(this.isSwitchWeaponManuverAble == false)
                    return false;

                if (weaponAdvanceUser._isSwitchWeaponCommand == false)
                    return false;

                if (curWeapon == weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon)
                    return true;

                return false;
            });
        restWeaponSwitchManuver = new RestWeaponManuverLeafNode(weaponAdvanceUser,()=> true);

        reloadNodeAttachAbleSelector = new NodeAttachAbleSelector();

        aimDownSightWeaponManuverNodeLeaf = new AimDownSightWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => isAimingManuverAble && weaponAdvanceUser._isAimingCommand);
        lowReadyWeaponManuverNodeLeaf = new LowReadyWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => true);

        drawWeaponManuverSelectorNode = new WeaponManuverSelectorNode(this.weaponAdvanceUser,
            () => isSwitchWeaponManuverAble && weaponAdvanceUser._isSwitchWeaponCommand
            && (weaponAdvanceUser._weaponBelt.myPrimaryWeapon != null || weaponAdvanceUser._weaponBelt.mySecondaryWeapon != null));
        drawPrimaryWeaponManuverNodeLeaf = new DrawPrimaryWeaponManuverNodeLeaf(this.weaponAdvanceUser,
         () => weaponAdvanceUser._weaponBelt.myPrimaryWeapon != null);
        drawSecondaryWeaponManuverNodeLeaf = new DrawSecondaryWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => weaponAdvanceUser._weaponBelt.mySecondaryWeapon != null);

        restWeaponManuverLeafNode = new RestWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => true);

        startNodeSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser, () => true);

        startNodeSelector.AddtoChildNode(pickUpWeaponNodeLeaf);
        startNodeSelector.AddtoChildNode(curWeaponManuverSelectorNode);
        startNodeSelector.AddtoChildNode(drawWeaponManuverSelectorNode);
        startNodeSelector.AddtoChildNode(restWeaponManuverLeafNode);

        curWeaponManuverSelectorNode.AddtoChildNode(dropWeaponManuverNodeLeaf);
        curWeaponManuverSelectorNode.AddtoChildNode(swtichingWeaponManuverSelector);
        curWeaponManuverSelectorNode.AddtoChildNode(reloadNodeAttachAbleSelector);
        curWeaponManuverSelectorNode.AddtoChildNode(aimDownSightWeaponManuverNodeLeaf);
        curWeaponManuverSelectorNode.AddtoChildNode(lowReadyWeaponManuverNodeLeaf);



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

        nodeManagerBehavior.SearchingNewNode(this);
    }
   
    public override void UpdateNode()
    {

        base.UpdateNode();
    }
  
}

