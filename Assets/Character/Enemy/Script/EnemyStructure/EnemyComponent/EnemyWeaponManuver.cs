using UnityEngine;

public class EnemyWeaponManuver : WeaponManuverManager
{
    private Enemy enemy => weaponAdvanceUser as Enemy;
    public EnemyWeaponManuver(IWeaponAdvanceUser weaponAdvanceUser, Enemy enemy) : base(weaponAdvanceUser)
    {

    }

    public override NodeAttachAbleSelector reloadNodeAttachAbleSelector { get ; protected set ; }
    public WeaponManuverSelectorNode curWeaponManuverSelector { get; set; }
    public override WeaponManuverSelectorNode swtichingWeaponManuverSelector { get; protected set; }
    public override PrimaryToSecondarySwitchWeaponManuverLeafNode primaryToSecondarySwitchWeaponManuverLeafNode { get; protected set; }
    public override SecondaryToPrimarySwitchWeaponManuverLeafNode secondaryToPrimarySwitchWeaponManuverLeafNode { get; protected set; }
    public override AimDownSightWeaponManuverNodeLeaf aimDownSightWeaponManuverNodeLeaf { get; protected set; }
    public override LowReadyWeaponManuverNodeLeaf lowReadyWeaponManuverNodeLeaf { get; protected set; }
    public override RestWeaponManuverLeafNode restWeaponManuverLeafNode { get; protected set; }

    public override PickUpWeaponNodeLeaf pickUpWeaponNodeLeaf { get;protected set; }
    public override DropWeaponManuverNodeLeaf dropWeaponManuverNodeLeaf { get; protected set ; }
    public override DrawPrimaryWeaponManuverNodeLeaf drawPrimaryWeaponManuverNodeLeaf { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override DrawSecondaryWeaponManuverNodeLeaf drawSecondaryWeaponManuverNodeLeaf { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override HolsterPrimaryWeaponManuverNodeLeaf holsterPrimaryWeaponManuverNodeLeaf { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override HolsterSecondaryWeaponManuverNodeLeaf holsterSecondaryWeaponManuverNodeLeaf { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }

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
            if (enemy.curNodeLeaf is EnemyStandIdleStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandMoveStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandTakeCoverStateNodeLeaf
                || enemy.curNodeLeaf is EnemyStandTakeAimStateNodeLeaf
                || enemy.curNodeLeaf is EnemySprintStateNode
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
                || enemy.curNodeLeaf is EnemySprintStateNode
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
                || enemy.curNodeLeaf is EnemySprintStateNode
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
                || enemy.curNodeLeaf is EnemySprintStateNode
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

        curWeaponManuverSelector = new WeaponManuverSelectorNode(weaponAdvanceUser,
            ()=> curWeapon != null);

        dropWeaponManuverNodeLeaf = new DropWeaponManuverNodeLeaf(weaponAdvanceUser,
            () => (isDropWeaponManuverAble && weaponAdvanceUser._isDropWeaponCommand) || (enemy.isDead && enemy._currentWeapon != null));
        swtichingWeaponManuverSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser,
             () => isSwitchWeaponManuverAble && weaponAdvanceUser._isSwitchWeaponCommand);
        primaryToSecondarySwitchWeaponManuverLeafNode = new PrimaryToSecondarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => curWeapon is PrimaryWeapon);
        secondaryToPrimarySwitchWeaponManuverLeafNode = new SecondaryToPrimarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => curWeapon is SecondaryWeapon);
        reloadNodeAttachAbleSelector = new NodeAttachAbleSelector();
        aimDownSightWeaponManuverNodeLeaf = new AimDownSightWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => isAimingManuverAble && weaponAdvanceUser._isAimingCommand);
        lowReadyWeaponManuverNodeLeaf = new LowReadyWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => curWeapon != null);

        restWeaponManuverLeafNode = new RestWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => true);

        startNodeSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser, () => true);

        startNodeSelector.AddtoChildNode(pickUpWeaponNodeLeaf);
        startNodeSelector.AddtoChildNode(curWeaponManuverSelector);
        startNodeSelector.AddtoChildNode(restWeaponManuverLeafNode);

        curWeaponManuverSelector.AddtoChildNode(dropWeaponManuverNodeLeaf);
        curWeaponManuverSelector.AddtoChildNode(swtichingWeaponManuverSelector);
        curWeaponManuverSelector.AddtoChildNode(reloadNodeAttachAbleSelector);
        curWeaponManuverSelector.AddtoChildNode(aimDownSightWeaponManuverNodeLeaf); 
        curWeaponManuverSelector.AddtoChildNode(lowReadyWeaponManuverNodeLeaf);

        swtichingWeaponManuverSelector.AddtoChildNode(primaryToSecondarySwitchWeaponManuverLeafNode);
        swtichingWeaponManuverSelector.AddtoChildNode(secondaryToPrimarySwitchWeaponManuverLeafNode);

        nodeManagerBehavior.SearchingNewNode(this);
    }
    public override void UpdateNode()
    {
        base.UpdateNode();
    }
   
}
