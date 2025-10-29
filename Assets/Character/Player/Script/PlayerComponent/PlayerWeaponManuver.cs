using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerWeaponManuver : WeaponManuverManager,IQuickSwitchWeaponManuverAble
{
    private Player player => weaponAdvanceUser as Player;
    public PlayerWeaponManuver(IWeaponAdvanceUser weaponAdvanceUser, Player player) : base(weaponAdvanceUser)
    {

    }
    #region Initailized WeaponManuver Property
    public override bool isAimingManuverAble
    {
        get
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

    public override bool isPullTriggerManuverAble
    {
        get
        {
            if ((weaponAdvanceUser._weaponManuverManager as INodeManager).GetCurNodeLeaf() is IReloadMagazineNode)
                return false;
            if (aimingWeight >= 1)
                return true;
            return false;
        }
    }

    public override bool isReloadManuverAble
    {
        get
        {

            if (player._currentWeapon == null)
                return false;

            if (weaponAdvanceUser._weaponManuverManager.isDropWeaponManuverAble
                && weaponAdvanceUser._isDropWeaponCommand)
                return false;

            if (secondaryToPrimarySwitchWeaponManuverLeafNode.preCondition.Invoke())
                return false;

            if (switchDrawSecondaryNodeSelector.preCondition.Invoke())
                return false;

            if (player.curNodeLeaf is PlayerCrouch_Idle_NodeLeaf
               || player.curNodeLeaf is PlayerCrouch_Move_NodeLeaf
               || player.curNodeLeaf is PlayerStandIdleNodeLeaf
               || player.curNodeLeaf is PlayerStandMoveNodeLeaf
               || player.curNodeLeaf is PlayerInCoverStandIdleNodeLeaf
               || player.curNodeLeaf is PlayerInCoverStandMoveNodeLeaf
               || player.curNodeLeaf is PlayerSprintNode
               || player.curNodeLeaf is PlayerDodgeRollStateNodeLeaf
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
    #region ImplementQuickSwitchWeaponManuver
    public bool isQuickSwtichWeaponManuverAble
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
        set { }
    }
    public IWeaponAdvanceUser _weaponAdvanceUser { get => weaponAdvanceUser; set { } }
    #endregion

    #endregion

    #region InitializedNode
    public override PickUpWeaponNodeLeaf pickUpWeaponNodeLeaf { get; protected set; }

    public NodeSelector curWeaponManuverSelectorNode { get; protected set; }
    public override DropWeaponManuverNodeLeaf dropWeaponManuverNodeLeaf { get; protected set; }
    public QuickSwitch_Draw_NodeLeaf quickSwitch_Draw_OnEmpty_NodeLeaf { get; protected set; }
    public NodeSelector quickSwitchExitSelector;
    public QuickSwitch_HolsterPrimaryWeapon_NodeLeaf quickSwitch_HolsterSecondHandWeapon_To_Reload;
    public QuickSwitch_Reload_NodeLeaf quickSwitch_Reload_NodeLeaf;
    public QuickSwitch_HolsterSecondaryWeapon_NodeLeaf quickSwitch_HolsterSecondary_NodeLeaf;
    public QuickSwitch_HolsterPrimaryWeapon_NodeLeaf quickSwitch_HolsterPrimary_NodeLeaf;
    public override SecondaryToPrimarySwitchWeaponManuverLeafNode secondaryToPrimarySwitchWeaponManuverLeafNode { get; protected set; }

    public NodeSelector switchDrawSecondaryNodeSelector { get; protected set; }
    public QuickSwitch_Draw_NodeLeaf quickSwitch_Draw_NodeLeaf { get; protected set; }
    public override PrimaryToSecondarySwitchWeaponManuverLeafNode primaryToSecondarySwitchWeaponManuverLeafNode { get; protected set; }

    public NodeSelector holsterSelector { get; protected set; }
    public override HolsterPrimaryWeaponManuverNodeLeaf holsterPrimaryWeaponManuverNodeLeaf { get; protected set; }
    public override HolsterSecondaryWeaponManuverNodeLeaf holsterSecondaryWeaponManuverNodeLeaf { get; protected set; }

    public NodeSelector handlingWeaponNodeSelector { get; protected set; }

    public QuickSwitch_AimDownSight_NodeLeaf quickSwitch_AimDownSight_NodeLeaf { get; protected set; }
    public QuickSwitch_LowReady_NodeLeaf quickSwitch_LowReady_NodeLeaf { get; protected set; }
    public override AimDownSightWeaponManuverNodeLeaf aimDownSightWeaponManuverNodeLeaf { get; protected set; }
    public override LowReadyWeaponManuverNodeLeaf lowReadyWeaponManuverNodeLeaf { get; protected set; }

    public override DrawSecondaryWeaponManuverNodeLeaf drawSecondaryWeaponManuverNodeLeaf { get; protected set; }
    public override DrawPrimaryWeaponManuverNodeLeaf drawPrimaryWeaponManuverNodeLeaf { get; protected set; }
    public override RestWeaponManuverLeafNode restWeaponManuverLeafNode { get; protected set; }

    #endregion
    #region InitialzedReloadNode

    public NodeManagerPortable reloadNodeManagerPortable { get; protected set; }
    public override NodeAttachAbleSelector reloadNodeAttachAbleSelector { get; protected set; }

    private void InitialzedReloadNode()
    {
        reloadNodeManagerPortable = new NodeManagerPortable();
        reloadNodeAttachAbleSelector = new NodeAttachAbleSelector();


        reloadNodeManagerPortable.InitialzedOuterNode(
            () =>
            {
                reloadNodeManagerPortable.startNodeSelector.AddtoChildNode(reloadNodeAttachAbleSelector);
            });
    }
    public override void InitailizedNode()
    {
        pickUpWeaponNodeLeaf = new PickUpWeaponNodeLeaf(weaponAdvanceUser,
            () =>
            {
                if (isPickingUpWeaponManuverAble == false)
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
                if (weaponAdvanceUser._isDropWeaponCommand)
                    return true;

                return false;
            });
        quickSwitch_Draw_OnEmpty_NodeLeaf = new QuickSwitch_Draw_NodeLeaf(weaponAdvanceUser, this,
            () => weaponAdvanceUser._weaponBelt.myPrimaryWeapon != null
            && weaponAdvanceUser._currentWeapon == weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon
            && weaponAdvanceUser._weaponBelt.mySecondaryWeapon != null
            && weaponAdvanceUser._currentWeapon.bulletStore[BulletStackType.Chamber] <= 0 && weaponAdvanceUser._currentWeapon.bulletStore[BulletStackType.Magazine] <= 0
            && isQuickSwtichWeaponManuverAble
            && weaponAdvanceUser._isPullTriggerCommand
            , player.quickSwitchDrawSCRP
            , player.quickSwitchHoldOffset);
        quickSwitchExitSelector = new NodeSelector(
            () => (weaponAdvanceUser._isDrawPrimaryWeaponCommand
            || weaponAdvanceUser._isDrawSecondaryWeaponCommand
            || weaponAdvanceUser._isHolsterWeaponCommand
            || weaponAdvanceUser._isReloadCommand)
            && isSwitchWeaponManuverAble
            && curWeapon == weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon
            && weaponAdvanceUser._secondHandSocket.curWeaponAtSocket != null
            , "QuickSwitchExitSelector");

        quickSwitch_HolsterSecondHandWeapon_To_Reload = new QuickSwitch_HolsterPrimaryWeapon_NodeLeaf(this.weaponAdvanceUser, this
            , () => weaponAdvanceUser._isReloadCommand
            , player.quickSiwthcHolsterPrimarySCRP);
        quickSwitch_Reload_NodeLeaf = new QuickSwitch_Reload_NodeLeaf(this.weaponAdvanceUser, this
            , () => true);

        quickSwitch_HolsterSecondary_NodeLeaf = new QuickSwitch_HolsterSecondaryWeapon_NodeLeaf(this.weaponAdvanceUser, this
            , () => weaponAdvanceUser._isDrawPrimaryWeaponCommand
            , player.quickSwitchHoslterSecondarySCRP);

        quickSwitch_HolsterPrimary_NodeLeaf = new QuickSwitch_HolsterPrimaryWeapon_NodeLeaf(this.weaponAdvanceUser, this
            , () => true
            , player.quickSiwthcHolsterPrimarySCRP);




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
        //quickDrawWeaponManuverLeafNode = new QuickDrawWeaponManuverLeafNodeLeaf(this.weaponAdvanceUser,
        //    () => isQuickDrawWeaponManuverAble 
        //    && isAimingManuverAble && weaponAdvanceUser._isAimingCommand);
        quickSwitch_Draw_NodeLeaf = new QuickSwitch_Draw_NodeLeaf(weaponAdvanceUser, this
            , () => isQuickSwtichWeaponManuverAble
            , player.quickSwitchDrawSCRP
            , player.quickSwitchHoldOffset);
        primaryToSecondarySwitchWeaponManuverLeafNode = new PrimaryToSecondarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => true);

        holsterSelector = new NodeSelector(
            () => weaponAdvanceUser._isHolsterWeaponCommand && isSwitchWeaponManuverAble);
        holsterPrimaryWeaponManuverNodeLeaf = new HolsterPrimaryWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => curWeapon == weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon);
        holsterSecondaryWeaponManuverNodeLeaf = new HolsterSecondaryWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => curWeapon == weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon);


        reloadNodeAttachAbleSelector = new NodeAttachAbleSelector();
        handlingWeaponNodeSelector = new NodeSelector(() => true);

        quickSwitch_AimDownSight_NodeLeaf = new QuickSwitch_AimDownSight_NodeLeaf(this.weaponAdvanceUser, this
            , () =>
            isAimingManuverAble
            && weaponAdvanceUser._isAimingCommand
            && weaponAdvanceUser._secondHandSocket.curWeaponAtSocket != null
            && isQuickSwtichWeaponManuverAble);
        quickSwitch_LowReady_NodeLeaf = new QuickSwitch_LowReady_NodeLeaf(
            this.weaponAdvanceUser
            , this
            , player.quickSwitchHoldOffset
            , () => weaponAdvanceUser._secondHandSocket.curWeaponAtSocket != null
            && isQuickSwtichWeaponManuverAble);

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
        curWeaponManuverSelectorNode.AddtoChildNode(quickSwitch_Draw_OnEmpty_NodeLeaf);
        curWeaponManuverSelectorNode.AddtoChildNode(quickSwitchExitSelector);
        curWeaponManuverSelectorNode.AddtoChildNode(secondaryToPrimarySwitchWeaponManuverLeafNode);
        curWeaponManuverSelectorNode.AddtoChildNode(switchDrawSecondaryNodeSelector);
        curWeaponManuverSelectorNode.AddtoChildNode(holsterSelector);
        curWeaponManuverSelectorNode.AddtoChildNode(handlingWeaponNodeSelector);

        quickSwitchExitSelector.AddtoChildNode(quickSwitch_HolsterSecondHandWeapon_To_Reload);
        quickSwitchExitSelector.AddtoChildNode(quickSwitch_HolsterSecondary_NodeLeaf);
        quickSwitchExitSelector.AddtoChildNode(quickSwitch_HolsterPrimary_NodeLeaf);

        quickSwitch_HolsterSecondHandWeapon_To_Reload.AddTransitionNode(quickSwitch_Reload_NodeLeaf);

        handlingWeaponNodeSelector.AddtoChildNode(quickSwitch_AimDownSight_NodeLeaf);
        handlingWeaponNodeSelector.AddtoChildNode(quickSwitch_LowReady_NodeLeaf);
        handlingWeaponNodeSelector.AddtoChildNode(aimDownSightWeaponManuverNodeLeaf);
        handlingWeaponNodeSelector.AddtoChildNode(lowReadyWeaponManuverNodeLeaf);

        switchDrawSecondaryNodeSelector.AddtoChildNode(quickSwitch_Draw_NodeLeaf);
        switchDrawSecondaryNodeSelector.AddtoChildNode(primaryToSecondarySwitchWeaponManuverLeafNode);

        holsterSelector.AddtoChildNode(holsterPrimaryWeaponManuverNodeLeaf);
        holsterSelector.AddtoChildNode(holsterSecondaryWeaponManuverNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);

        InitialzedReloadNode();
    }
    #endregion


    public override void UpdateNode()
    {
        base.UpdateNode();
        reloadNodeManagerPortable.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
        reloadNodeManagerPortable.FixedUpdateNode();
    }
}

