using UnityEngine;

public class EnemyWeaponManuver : WeaponManuverManager
{
    private Enemy enemy;
    public EnemyWeaponManuver(IWeaponAdvanceUser weaponAdvanceUser, Enemy enemy) : base(weaponAdvanceUser)
    {
        this.enemy = enemy;
    }

    public override WeaponManuverSelectorNode swtichingWeaponManuverSelector { get; protected set; }
    public override PrimaryToSecondarySwitchWeaponManuverLeafNode primaryToSecondarySwitchWeaponManuverLeafNode { get; protected set; }
    public override SecondaryToPrimarySwitchWeaponManuverLeafNode secondaryToPrimarySwitchWeaponManuverLeafNode { get; protected set; }
    public override AimDownSightWeaponManuverNodeLeaf aimDownSightWeaponManuverNodeLeaf { get; protected set; }
    public override LowReadyWeaponManuverNodeLeaf lowReadyWeaponManuverNodeLeaf { get; protected set; }
    public override RestWeaponManuverLeafNode restWeaponManuverLeafNode { get; protected set; }

    public override void InitailizedNode()
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

        startNodeSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser, () => true);

        startNodeSelector.AddtoChildNode(swtichingWeaponManuverSelector);
        startNodeSelector.AddtoChildNode(aimDownSightWeaponManuverNodeLeaf);
        startNodeSelector.AddtoChildNode(lowReadyWeaponManuverNodeLeaf);
        startNodeSelector.AddtoChildNode(restWeaponManuverLeafNode);

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
        EnemyStateLeafNode enemyActionNodeLeaf = enemy.curStateLeaf;

        if (enemy._isInPain 
            || enemy.curStateLeaf is FallDown_EnemyState_NodeLeaf
            )
        {
            isAimingManuver = false;
            isPullTriggerManuver = false;
            isReloadManuver = false;
            isSwitchWeaponManuver = false;
            return;
        }

        if (enemy.curStateLeaf is IGunFuAttackedAbleNode)
        {
            Debug.Log("enemy.curStateLeaf is IGunFuAttackedAbleNode");
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
        else
        {
            WeaponAdvanceCommanding(weaponAdvanceUser);
            return;
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
