using UnityEngine;

public class EnemyWeaponManuver : WeaponManuverManager
{
    private Enemy enemy => weaponAdvanceUser as Enemy;
    public EnemyWeaponManuver(IWeaponAdvanceUser weaponAdvanceUser, Enemy enemy) : base(weaponAdvanceUser)
    {

    }

    public override PickUpWeaponNodeLeaf pickUpWeaponNodeLeaf { get; protected set; }

    public NodeSelector curWeaponManuverSelector { get; set; }
    public override DropWeaponManuverNodeLeaf dropWeaponManuverNodeLeaf { get; protected set; }
    public override SecondaryToPrimarySwitchWeaponManuverLeafNode secondaryToPrimarySwitchWeaponManuverLeafNode { get; protected set; }
    public override PrimaryToSecondarySwitchWeaponManuverLeafNode primaryToSecondarySwitchWeaponManuverLeafNode { get; protected set; }

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
            if (enemy.curNodeLeaf is EnemyStandIdleStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandMoveStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandTakeCoverStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandTakeAimStateNodeLeaf
                )
                return true;
            return false;
        }
    }

    public override bool isPullTriggerManuverAble
    {
        get
        {
            if (aimingWeight >= 1)
                return true;
            return false;
        }
    }

    public override bool isReloadManuverAble { 
        get 
        {
            if (enemy._currentWeapon == null)
                return false;

            if(dropWeaponManuverNodeLeaf.Precondition())
                return false;

            if(secondaryToPrimarySwitchWeaponManuverLeafNode.Precondition())
                return false;

            if(primaryToSecondarySwitchWeaponManuverLeafNode.Precondition())
                return false;

            if(holsterWeaponSelector.Precondition())
                return false;

            if (enemy.curNodeLeaf is EnemyStandIdleStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandMoveStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandTakeCoverStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandTakeAimStateNodeLeaf
                || enemy.curNodeLeaf is EnemySprintStateNodeLeaf
                )
                return true;
            return false;
        } 
    }

    public override bool isSwitchWeaponManuverAble 
    {
        get 
        {
            if (enemy.curNodeLeaf is EnemyStandIdleStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandMoveStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandTakeCoverStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandTakeAimStateNodeLeaf
                || enemy.curNodeLeaf is EnemySprintStateNodeLeaf
                )
                return true;
            return false;
        }
    }

    public override bool isPickingUpWeaponManuverAble 
    {
        get
        {
            if (enemy.curNodeLeaf is EnemyStandIdleStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandMoveStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandTakeCoverStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandTakeAimStateNodeLeaf
                || enemy.curNodeLeaf is EnemySprintStateNodeLeaf
                )
                return true;
            return false;
        }
    }

    public override bool isDropWeaponManuverAble 
    {
        get
        {
            if (enemy.curNodeLeaf is EnemyStandIdleStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandMoveStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandTakeCoverStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandTakeAimStateNodeLeaf
                || enemy.curNodeLeaf is EnemySprintStateNodeLeaf
                )
                return true;
            return false;
        }
    }

    public override void InitailizedNode()
    {
        pickUpWeaponNodeLeaf = new PickUpWeaponNodeLeaf(weaponAdvanceUser,
            ()=> 
            {

                if (isPickingUpWeaponManuverAble 
                && weaponAdvanceUser._isPickingUpWeaponCommand)
                {
                    if (weaponAdvanceUser._findingWeaponBehavior.FindingWeapon())
                    {
                        //Debug.Log("Enemy weaponAdvanceUser._findingWeaponBehavior.FindingWeapon()");
                        return true;
                    }
                }
                return false;
            } 
            );

        curWeaponManuverSelector = new NodeSelector (
            ()=> curWeapon != null);
        dropWeaponManuverNodeLeaf = new DropWeaponManuverNodeLeaf(weaponAdvanceUser,
            () => (isDropWeaponManuverAble && weaponAdvanceUser._isDropWeaponCommand) || (enemy.isDead && enemy._currentWeapon != null));
        secondaryToPrimarySwitchWeaponManuverLeafNode = new SecondaryToPrimarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
           () => weaponAdvanceUser._isDrawPrimaryWeaponCommand
           && isSwitchWeaponManuverAble
           && curWeapon == weaponAdvanceUser._weaponBelt.mySecondaryWeapon as Weapon 
           && weaponAdvanceUser._weaponBelt.myPrimaryWeapon != null);
        primaryToSecondarySwitchWeaponManuverLeafNode = new PrimaryToSecondarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => weaponAdvanceUser._isDrawSecondaryWeaponCommand
           && isSwitchWeaponManuverAble
           && curWeapon == weaponAdvanceUser._weaponBelt.myPrimaryWeapon as Weapon
           && weaponAdvanceUser._weaponBelt.mySecondaryWeapon != null);

        holsterWeaponSelector = new NodeSelector(
          () => weaponAdvanceUser._isHolsterWeaponCommand && isSwitchWeaponManuverAble);
        holsterPrimaryWeaponManuverNodeLeaf = new HolsterPrimaryWeaponManuverNodeLeaf(weaponAdvanceUser,
            () => weaponAdvanceUser._currentWeapon is PrimaryWeapon);
        holsterSecondaryWeaponManuverNodeLeaf = new HolsterSecondaryWeaponManuverNodeLeaf(weaponAdvanceUser,
            () =>true);

        reloadNodeAttachAbleSelector = new NodeAttachAbleSelector();
        aimDownSightWeaponManuverNodeLeaf = new AimDownSightWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => isAimingManuverAble && weaponAdvanceUser._isAimingCommand);
        lowReadyWeaponManuverNodeLeaf = new LowReadyWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => curWeapon != null);

        drawPrimaryWeaponManuverNodeLeaf = new DrawPrimaryWeaponManuverNodeLeaf(weaponAdvanceUser,
           () => weaponAdvanceUser._isDrawPrimaryWeaponCommand
           && isSwitchWeaponManuverAble
           && weaponAdvanceUser._weaponBelt.myPrimaryWeapon != null);
        drawSecondaryWeaponManuverNodeLeaf = new DrawSecondaryWeaponManuverNodeLeaf(weaponAdvanceUser,
            () => weaponAdvanceUser._isDrawSecondaryWeaponCommand
            && isSwitchWeaponManuverAble
            && weaponAdvanceUser._weaponBelt.mySecondaryWeapon != null);
        restWeaponManuverLeafNode = new RestWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => true);

        startNodeSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser, () => true);

        startNodeSelector.AddtoChildNode(pickUpWeaponNodeLeaf);
        startNodeSelector.AddtoChildNode(curWeaponManuverSelector);
        startNodeSelector.AddtoChildNode(drawPrimaryWeaponManuverNodeLeaf);
        startNodeSelector.AddtoChildNode(drawSecondaryWeaponManuverNodeLeaf);
        startNodeSelector.AddtoChildNode(restWeaponManuverLeafNode);

        curWeaponManuverSelector.AddtoChildNode(dropWeaponManuverNodeLeaf);
        curWeaponManuverSelector.AddtoChildNode(primaryToSecondarySwitchWeaponManuverLeafNode);
        curWeaponManuverSelector.AddtoChildNode(secondaryToPrimarySwitchWeaponManuverLeafNode);
        curWeaponManuverSelector.AddtoChildNode(holsterWeaponSelector);
        curWeaponManuverSelector.AddtoChildNode(reloadNodeAttachAbleSelector);
        curWeaponManuverSelector.AddtoChildNode(aimDownSightWeaponManuverNodeLeaf); 
        curWeaponManuverSelector.AddtoChildNode(lowReadyWeaponManuverNodeLeaf);

        holsterWeaponSelector.AddtoChildNode(holsterPrimaryWeaponManuverNodeLeaf);
        holsterWeaponSelector.AddtoChildNode(holsterSecondaryWeaponManuverNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }
    public override void UpdateNode()
    {
        base.UpdateNode();
    }
   
}
