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
            if((weaponAdvanceUser._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<IReloadMagazineNode>())
                return false;
            if(aimingWeight >= 1)
                return true;
            return false;
        }
    }

    public override bool isReloadManuverAble
    {
        get {

            if(player._currentWeapon == null)
                return false;
            
            if(weaponAdvanceUser._weaponManuverManager.isDropWeaponManuverAble 
                && weaponAdvanceUser._isDropWeaponCommand)
                return false;

            if(secondaryToPrimarySwitchWeaponManuverLeafNode.preCondition.Invoke())
                return false;

            if(switchDrawSecondaryNodeSelector.preCondition.Invoke())
                return false;

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
    public override PickUpWeaponNodeLeaf pickUpWeaponNodeLeaf { get; protected set; }

    public NodeSelector curWeaponManuverSelectorNode { get; protected set; }
    public override DropWeaponManuverNodeLeaf dropWeaponManuverNodeLeaf { get; protected set; }
    public override SecondaryToPrimarySwitchWeaponManuverLeafNode secondaryToPrimarySwitchWeaponManuverLeafNode { get; protected set; }

    public NodeSelector switchDrawSecondaryNodeSelector { get; protected set; }
    public QuickDrawWeaponManuverLeafNodeLeaf quickDrawWeaponManuverLeafNode { get; protected set; }
    public override PrimaryToSecondarySwitchWeaponManuverLeafNode primaryToSecondarySwitchWeaponManuverLeafNode { get; protected set; }

    public NodeSelector holsterSelector { get; protected set; }
    public override HolsterPrimaryWeaponManuverNodeLeaf holsterPrimaryWeaponManuverNodeLeaf { get; protected set; }
    public override HolsterSecondaryWeaponManuverNodeLeaf holsterSecondaryWeaponManuverNodeLeaf { get; protected set; }

    public NodeCombine weaponHandling_Reload_NodeCombine { get; protected set; }
    public override NodeAttachAbleSelector reloadNodeAttachAbleSelector { get; protected set; }
    public NodeSelector handlingWeaponNodeSelector { get; protected set; }

    public override AimDownSightWeaponManuverNodeLeaf aimDownSightWeaponManuverNodeLeaf { get; protected set    ; }
    public override LowReadyWeaponManuverNodeLeaf lowReadyWeaponManuverNodeLeaf { get; protected set; }

    public override DrawSecondaryWeaponManuverNodeLeaf drawSecondaryWeaponManuverNodeLeaf { get; protected set; }
    public override DrawPrimaryWeaponManuverNodeLeaf drawPrimaryWeaponManuverNodeLeaf { get; protected set; }
    public override RestWeaponManuverLeafNode restWeaponManuverLeafNode { get; protected set; }

    #endregion
    public override void InitailizedNode()
    {
        pickUpWeaponNodeLeaf = new PickUpWeaponNodeLeaf(weaponAdvanceUser,
            ()=> 
            {
                if(isPickingUpWeaponManuverAble == false)
                    return false;

                if (weaponAdvanceUser._isPickingUpWeaponCommand &&
                player.currentInteractable != null &&
                player.currentInteractable is Weapon selectFindingWeapon
               )
                {
                    weaponAdvanceUser._findingWeaponBehavior.SetWeaponFindingSelecting(selectFindingWeapon);
                    return true;
                }
                weaponAdvanceUser._findingWeaponBehavior.SetWeaponFindingSelecting(null);
                return false;
            });  

        curWeaponManuverSelectorNode = new NodeSelector( 
            () => curWeapon != null);
        dropWeaponManuverNodeLeaf = new DropWeaponManuverNodeLeaf(this.weaponAdvanceUser, 
            () => 
            {
                if (isDropWeaponManuverAble == false)
                    return false;
                if(weaponAdvanceUser._isDropWeaponCommand)
                    return true;

                return false;
                });

       
        secondaryToPrimarySwitchWeaponManuverLeafNode = new SecondaryToPrimarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
           () => weaponAdvanceUser._isDrawPrimaryWeaponCommand 
           && isSwitchWeaponManuverAble
           && weaponAdvanceUser._currentWeapon == weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon 
           && weaponAdvanceUser._weaponBelt.myPrimaryWeapon != null);

        switchDrawSecondaryNodeSelector = new NodeSelector(
            () => weaponAdvanceUser._isDrawSecondaryWeaponCommand 
            && isSwitchWeaponManuverAble
            && weaponAdvanceUser._currentWeapon == weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon
            && weaponAdvanceUser._weaponBelt.mySecondaryWeapon != null);
        quickDrawWeaponManuverLeafNode = new QuickDrawWeaponManuverLeafNodeLeaf(this.weaponAdvanceUser,
            () => isQuickDrawWeaponManuverAble 
            && isAimingManuverAble && weaponAdvanceUser._isAimingCommand);
        primaryToSecondarySwitchWeaponManuverLeafNode = new PrimaryToSecondarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => true );

        holsterSelector = new NodeSelector(
            () => weaponAdvanceUser._isHolsterWeaponCommand && isSwitchWeaponManuverAble);
        holsterPrimaryWeaponManuverNodeLeaf = new HolsterPrimaryWeaponManuverNodeLeaf(this.weaponAdvanceUser, 
            () => curWeapon == weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon);
        holsterSecondaryWeaponManuverNodeLeaf = new HolsterSecondaryWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => curWeapon == weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon);

        weaponHandling_Reload_NodeCombine = new NodeCombine(()=> true);
        reloadNodeAttachAbleSelector = new NodeAttachAbleSelector();
        handlingWeaponNodeSelector = new NodeSelector(()=>true);

        aimDownSightWeaponManuverNodeLeaf = new AimDownSightWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => isAimingManuverAble && weaponAdvanceUser._isAimingCommand);
        lowReadyWeaponManuverNodeLeaf = new LowReadyWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => true);

        drawPrimaryWeaponManuverNodeLeaf = new DrawPrimaryWeaponManuverNodeLeaf(this.weaponAdvanceUser,
         () => weaponAdvanceUser._isDrawPrimaryWeaponCommand 
         && isSwitchWeaponManuverAble 
         && weaponAdvanceUser._weaponBelt.myPrimaryWeapon != null);
        drawSecondaryWeaponManuverNodeLeaf = new DrawSecondaryWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => weaponAdvanceUser._isDrawSecondaryWeaponCommand
         && isSwitchWeaponManuverAble
         && weaponAdvanceUser._weaponBelt.mySecondaryWeapon != null);

        restWeaponManuverLeafNode = new RestWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => true);

        startNodeSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser, () => true);

        startNodeSelector.AddtoChildNode(pickUpWeaponNodeLeaf);
        startNodeSelector.AddtoChildNode(curWeaponManuverSelectorNode);
        startNodeSelector.AddtoChildNode(drawPrimaryWeaponManuverNodeLeaf);
        startNodeSelector.AddtoChildNode(drawSecondaryWeaponManuverNodeLeaf);
        startNodeSelector.AddtoChildNode(restWeaponManuverLeafNode);

        curWeaponManuverSelectorNode.AddtoChildNode(dropWeaponManuverNodeLeaf);
        curWeaponManuverSelectorNode.AddtoChildNode(secondaryToPrimarySwitchWeaponManuverLeafNode);
        curWeaponManuverSelectorNode.AddtoChildNode(switchDrawSecondaryNodeSelector);
        curWeaponManuverSelectorNode.AddtoChildNode(holsterSelector);
        curWeaponManuverSelectorNode.AddtoChildNode(weaponHandling_Reload_NodeCombine);

        weaponHandling_Reload_NodeCombine.AddCombineNode(reloadNodeAttachAbleSelector);
        weaponHandling_Reload_NodeCombine.AddCombineNode(handlingWeaponNodeSelector);

        handlingWeaponNodeSelector.AddtoChildNode(aimDownSightWeaponManuverNodeLeaf);
        handlingWeaponNodeSelector.AddtoChildNode(lowReadyWeaponManuverNodeLeaf);

        switchDrawSecondaryNodeSelector.AddtoChildNode(quickDrawWeaponManuverLeafNode);
        switchDrawSecondaryNodeSelector.AddtoChildNode(primaryToSecondarySwitchWeaponManuverLeafNode);

        holsterSelector.AddtoChildNode(holsterPrimaryWeaponManuverNodeLeaf);
        holsterSelector.AddtoChildNode(holsterSecondaryWeaponManuverNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }
   
    public override void UpdateNode()
    {

        base.UpdateNode();
    }
  
}

