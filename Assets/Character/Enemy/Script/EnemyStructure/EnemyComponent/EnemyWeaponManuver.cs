using UnityEngine;

public class EnemyWeaponManuver : WeaponNodeManuverManager
{
    private Enemy enemy => weaponAdvanceUser as Enemy;
    public EnemyWeaponManuver(IRangeWeaponAdvanceUser weaponAdvanceUser, Enemy enemy) : base(weaponAdvanceUser)
    {

    }

    public override PickUpWeaponNodeLeaf pickUpWeaponNodeLeaf { get; protected set; }

    public NodeSelector curWeaponManuverSelector { get; set; }
    public override DropWeaponManuverNodeLeaf dropWeaponManuverNodeLeaf { get; protected set; }

    public NodeSelector switchDrawSecondaryNodeSelector { get; set; }
    public NodeSelector switchDrawPrimaryNodeSelector { get; set; }

    public NodeSelector holsterWeaponSelector { get; set; }
    public override HolsterPrimaryWeaponManuverNodeLeaf holsterPrimaryWeaponManuverNodeLeaf { get; protected set; }
    public override HolsterSecondaryWeaponManuverNodeLeaf holsterSecondaryWeaponManuverNodeLeaf { get; protected set; }

    public override NodeAttachAbleSelector reloadNodeAttachAbleSelector { get; protected set; }
    public override AimDownSightWeaponManuverNodeLeaf aimDownSightWeaponManuverNodeLeaf { get; protected set; }
    public override LowReadyWeaponManuverNodeLeaf lowReadyWeaponManuverNodeLeaf { get; protected set; }

    public override DrawPrimaryWeaponManuverNodeLeaf drawPrimaryWeaponManuverNodeLeaf { get ; protected set ; }
    public override DrawSecondaryWeaponManuverNodeLeaf drawSecondaryWeaponManuverNodeLeaf { get ; protected set; }
    public override RestWeaponManuverLeafNode restWeaponManuverLeafNode { get; protected set; }

    public override bool isAimingManuverAble
    {
        get
        {
            if (enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandIdleStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandMoveStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>()
                )
                return true;
            return false;
        }
    }

    public override bool isPullTriggerManuverAble
    {
        get
        {
            if(this.enemy.isReactAble == false)
                return false;

            if (aimingWeight >= 1)
                return true;

            return false;
        }
    }

    public override bool isReloadManuverAble {
        get
        {
            if (enemy._isInPain)
                return false;

            if (enemy._currentWeapon == null)
                return false;

            if(dropWeaponManuverNodeLeaf.Precondition())
                return false;

            if(switchDrawPrimaryNodeSelector.preCondition.Invoke())
                return false;

            if(switchDrawSecondaryNodeSelector.preCondition.Invoke())
                return false;

            if(holsterWeaponSelector.Precondition())
                return false;

            if (enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandIdleStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandMoveStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemySprintStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>()
                )
                return true;
            return false;
        }
    }

    public override bool isSwitchWeaponManuverAble
    {
        get
        {
            if (enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandIdleStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandMoveStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemySprintStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>()
                )
                return true;
            return false;
        }
    }

    public override bool isPickingUpWeaponManuverAble
    {
        get
        {
            if (enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandIdleStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandMoveStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemySprintStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>()
                )
                return true;
            return false;
        }
    }

    public override bool isDropWeaponManuverAble
    {
        get
        {
            if (enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandIdleStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyStandMoveStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchIdleStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemyCrouchMoveStateNodeLeaf>()
                || enemy.stateManagerNode.TryGetCurNodeLeaf<EnemySprintStateNodeLeaf>()
                )
                return true;
            return false;
        }
    }

    public override INodeManager _reloadNodeManager => this;

    public override void InitailizedNode()
    {
        pickUpWeaponNodeLeaf = new PickUpWeaponNodeLeaf(weaponAdvanceUser,
            () =>
            {
                if (isPickingUpWeaponManuverAble
                && weaponAdvanceUser._isPickingUpWeaponCommand)
                {
                    if (weaponAdvanceUser._findingWeaponBehavior.FindingWeapon())
                        return true;
                }
                return false;
            });

        curWeaponManuverSelector = new NodeSelector(() => curWeapon != null);
        dropWeaponManuverNodeLeaf = new DropWeaponManuverNodeLeaf(weaponAdvanceUser,
            () => (isDropWeaponManuverAble && weaponAdvanceUser._isDropWeaponCommand) || (enemy.isDead && enemy._currentWeapon != null));

        switchDrawSecondaryNodeSelector = new NodeSelector(
            () => weaponAdvanceUser._isDrawSecondaryWeaponCommand
            && isSwitchWeaponManuverAble
            && curWeapon == weaponAdvanceUser._weaponBelt.myPrimaryWeapon as RangeWeapon
            && weaponAdvanceUser._weaponBelt.mySecondaryWeapon != null);

        switchDrawPrimaryNodeSelector = new NodeSelector(
            () => weaponAdvanceUser._isDrawPrimaryWeaponCommand
            && isSwitchWeaponManuverAble
            && curWeapon == weaponAdvanceUser._weaponBelt.mySecondaryWeapon as RangeWeapon
            && weaponAdvanceUser._weaponBelt.myPrimaryWeapon != null);

        holsterWeaponSelector = new NodeSelector(
            () => weaponAdvanceUser._isHolsterWeaponCommand && isSwitchWeaponManuverAble);
 

        holsterPrimaryWeaponManuverNodeLeaf = new HolsterPrimaryWeaponManuverNodeLeaf(weaponAdvanceUser,
            () => weaponAdvanceUser._currentWeapon is PrimaryWeapon,
            enemy.holsterPrimaryWeaponSCRP,
            this.enemy.humanoidBone._leftHandBone,
            enemy.LeftHandHoldWeaponOffset);

        holsterSecondaryWeaponManuverNodeLeaf = new HolsterSecondaryWeaponManuverNodeLeaf(weaponAdvanceUser,
            () => true,
            enemy.holsterSecondaryWeaponSCRP);

        reloadNodeAttachAbleSelector = new NodeAttachAbleSelector();
        aimDownSightWeaponManuverNodeLeaf = new AimDownSightWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => isAimingManuverAble && weaponAdvanceUser._isAimingCommand);
        lowReadyWeaponManuverNodeLeaf = new LowReadyWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => curWeapon != null);

        drawPrimaryWeaponManuverNodeLeaf = new DrawPrimaryWeaponManuverNodeLeaf(weaponAdvanceUser,
           () => weaponAdvanceUser._isDrawPrimaryWeaponCommand
           && isSwitchWeaponManuverAble
           && weaponAdvanceUser._weaponBelt.myPrimaryWeapon != null,
           enemy.drawPrimaryWeaponSCRP,
           this.enemy.humanoidBone._leftHandBone,
           enemy.LeftHandHoldWeaponOffset);

        drawSecondaryWeaponManuverNodeLeaf = new DrawSecondaryWeaponManuverNodeLeaf(weaponAdvanceUser,
            () => weaponAdvanceUser._isDrawSecondaryWeaponCommand
            && isSwitchWeaponManuverAble
            && weaponAdvanceUser._weaponBelt.mySecondaryWeapon != null,
            enemy.drawSecondaryWeaponSCRP);
        restWeaponManuverLeafNode = new RestWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => true);

        // Wire INodeLeafTransitionAble transitions on holster nodes
        holsterPrimaryWeaponManuverNodeLeaf.nodeManager = this;
        (holsterPrimaryWeaponManuverNodeLeaf as INodeLeafTransitionAble).AddTransitionNode(
            drawSecondaryWeaponManuverNodeLeaf,
            () => this.weaponAdvanceUser._weaponBelt.mySecondaryWeapon != null);

        holsterSecondaryWeaponManuverNodeLeaf.nodeManager = this;
        (holsterSecondaryWeaponManuverNodeLeaf as INodeLeafTransitionAble).AddTransitionNode(
            drawPrimaryWeaponManuverNodeLeaf,
            () => this.weaponAdvanceUser._weaponBelt.myPrimaryWeapon != null);

        startNodeSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser, () => true);

        startNodeSelector.AddtoChildNode(pickUpWeaponNodeLeaf);
        startNodeSelector.AddtoChildNode(curWeaponManuverSelector);
        startNodeSelector.AddtoChildNode(drawPrimaryWeaponManuverNodeLeaf);
        startNodeSelector.AddtoChildNode(drawSecondaryWeaponManuverNodeLeaf);
        startNodeSelector.AddtoChildNode(restWeaponManuverLeafNode);

        curWeaponManuverSelector.AddtoChildNode(dropWeaponManuverNodeLeaf);
        curWeaponManuverSelector.AddtoChildNode(switchDrawSecondaryNodeSelector);
        curWeaponManuverSelector.AddtoChildNode(switchDrawPrimaryNodeSelector);
        curWeaponManuverSelector.AddtoChildNode(holsterWeaponSelector);
        curWeaponManuverSelector.AddtoChildNode(reloadNodeAttachAbleSelector);
        curWeaponManuverSelector.AddtoChildNode(aimDownSightWeaponManuverNodeLeaf);
        curWeaponManuverSelector.AddtoChildNode(lowReadyWeaponManuverNodeLeaf);

        switchDrawSecondaryNodeSelector.AddtoChildNode(holsterPrimaryWeaponManuverNodeLeaf);
        switchDrawPrimaryNodeSelector.AddtoChildNode(holsterSecondaryWeaponManuverNodeLeaf);

        holsterWeaponSelector.AddtoChildNode(holsterPrimaryWeaponManuverNodeLeaf);
        holsterWeaponSelector.AddtoChildNode(holsterSecondaryWeaponManuverNodeLeaf);

        _nodeManagerBehavior.SearchingNewNode(this);
    }
    public override void UpdateNode()
    {
        base.UpdateNode();
    }

}
