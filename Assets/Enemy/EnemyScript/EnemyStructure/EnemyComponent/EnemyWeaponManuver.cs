using UnityEngine;

public class EnemyWeaponManuver : WeaponManuverManager
{
    private Enemy enemy;
    public EnemyWeaponManuver(IWeaponAdvanceUser weaponAdvanceUser, Enemy enemy) : base(weaponAdvanceUser)
    {
        this.enemy = enemy;
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

    public override void InitailizedNode()
    {
        pickUpWeaponNodeLeaf = new PickUpWeaponNodeLeaf(weaponAdvanceUser,
            ()=> 
            {

                if (isPickingUpWeaponManuverAble)
                {
                    if (weaponAdvanceUser.findingWeaponBehavior.FindingWeapon())
                    {
                        //Debug.Log("Enemy weaponAdvanceUser.findingWeaponBehavior.FindingWeapon()");
                        return true;
                    }
                }
                return false;
            } 
            );

        curWeaponManuverSelector = new WeaponManuverSelectorNode(weaponAdvanceUser,
            ()=> curWeapon != null);

        dropWeaponManuverNodeLeaf = new DropWeaponManuverNodeLeaf(weaponAdvanceUser,
            () => isDropWeaponManuverAble || (enemy.isDead && enemy._currentWeapon != null));
        swtichingWeaponManuverSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser,
             () => isSwitchWeaponManuverAble);
        primaryToSecondarySwitchWeaponManuverLeafNode = new PrimaryToSecondarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => curWeapon is PrimaryWeapon);
        secondaryToPrimarySwitchWeaponManuverLeafNode = new SecondaryToPrimarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => curWeapon is SecondaryWeapon);
        reloadNodeAttachAbleSelector = new NodeAttachAbleSelector();
        aimDownSightWeaponManuverNodeLeaf = new AimDownSightWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            () => isAimingManuverAble);
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
        OnserveEnemyStateNode(this.enemy);
        base.UpdateNode();
    }
    public void OnserveEnemyStateNode(Enemy enemy)
    {
        IWeaponAdvanceUser weaponAdvanceUser = enemy;
        EnemyStateLeafNode enemyActionNodeLeaf = enemy.enemyStateManagerNode.curNodeLeaf as EnemyStateLeafNode;

        if (enemy._isInPain 
            || enemyActionNodeLeaf is FallDown_EnemyState_NodeLeaf
            || enemy.isDead
            || enemy.enemyStateManagerNode.curNodeLeaf is IGotGunFuAttackAbleNode)
        {
            isAimingManuverAble = false;
            isPullTriggerManuverAble = false;
            isReloadManuverAble = false;
            isSwitchWeaponManuverAble = false;
            isPickingUpWeaponManuverAble = false;
            return;
        }

        if (enemyActionNodeLeaf is EnemySprintStateNode)
        {
            if (weaponAdvanceUser.isReloadCommand)
                isReloadManuverAble = true;
            if (weaponAdvanceUser.isSwitchWeaponCommand)
                isSwitchWeaponManuverAble = true;

            isAimingManuverAble = false;
            isPullTriggerManuverAble = false;

            return;
        }
        
            
        WeaponAdvanceCommanding(weaponAdvanceUser);
            return;
        
    }

    private void WeaponAdvanceCommanding(IWeaponAdvanceUser weaponAdvanceUser)
    {

        isAimingManuverAble = weaponAdvanceUser.isAimingCommand;

        isReloadManuverAble = weaponAdvanceUser.isReloadCommand;

        isSwitchWeaponManuverAble = weaponAdvanceUser.isSwitchWeaponCommand;

        isPullTriggerManuverAble = weaponAdvanceUser.isPullTriggerCommand;

        isPickingUpWeaponManuverAble = weaponAdvanceUser.isPickingUpWeaponCommand;
    }
}
