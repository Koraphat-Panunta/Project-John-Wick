using UnityEngine;

public class EnemyWeaponManuver : WeaponManuverManager
{
    private Enemy enemy;
    public EnemyWeaponManuver(IWeaponAdvanceUser weaponAdvanceUser, Enemy enemy) : base(weaponAdvanceUser)
    {
        this.enemy = enemy;
    }

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
                if (isPickingUpWeaponManuver)
                {
                    if(weaponAdvanceUser.findingWeaponBehavior.FindingWeapon())
                        return true;
                }
                return false;
            } 
            );

        curWeaponManuverSelector = new WeaponManuverSelectorNode(weaponAdvanceUser,
            ()=> curWeapon != null);

        dropWeaponManuverNodeLeaf = new DropWeaponManuverNodeLeaf(weaponAdvanceUser,
            () => isDropWeaponManuver || (enemy.isDead && enemy._currentWeapon != null));
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

        startNodeSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser, () => true);

        startNodeSelector.AddtoChildNode(pickUpWeaponNodeLeaf);
        startNodeSelector.AddtoChildNode(curWeaponManuverSelector);
        startNodeSelector.AddtoChildNode(restWeaponManuverLeafNode);

        curWeaponManuverSelector.AddtoChildNode(dropWeaponManuverNodeLeaf);
        curWeaponManuverSelector.AddtoChildNode(swtichingWeaponManuverSelector);
        curWeaponManuverSelector.AddtoChildNode(aimDownSightWeaponManuverNodeLeaf);
        curWeaponManuverSelector.AddtoChildNode(lowReadyWeaponManuverNodeLeaf);

        swtichingWeaponManuverSelector.AddtoChildNode(primaryToSecondarySwitchWeaponManuverLeafNode);
        swtichingWeaponManuverSelector.AddtoChildNode(secondaryToPrimarySwitchWeaponManuverLeafNode);

        startNodeSelector.FindingNode(out INodeLeaf weaponManuverLeafNode);
        curNodeLeaf = weaponManuverLeafNode as WeaponManuverLeafNode;
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
            || enemy.enemyStateManagerNode.curNodeLeaf is IGunFuAttackedAbleNode)
        {
            isAimingManuver = false;
            isPullTriggerManuver = false;
            isReloadManuver = false;
            isSwitchWeaponManuver = false;
            return;
        }

        if (enemyActionNodeLeaf is EnemySprintStateNode)
        {
            if (weaponAdvanceUser.isReloadCommand)
                isReloadManuver = true;
            if (weaponAdvanceUser.isSwitchWeaponCommand)
                isSwitchWeaponManuver = true;

            isAimingManuver = false;
            isPullTriggerManuver = false;

            return;
        }
        
            
        WeaponAdvanceCommanding(weaponAdvanceUser);
            return;
        
    }

    private void WeaponAdvanceCommanding(IWeaponAdvanceUser weaponAdvanceUser)
    {

        isAimingManuver = weaponAdvanceUser.isAimingCommand;

        isReloadManuver = weaponAdvanceUser.isReloadCommand;

        isSwitchWeaponManuver = weaponAdvanceUser.isSwitchWeaponCommand;

        isPullTriggerManuver = weaponAdvanceUser.isPullTriggerCommand;

    }
}
