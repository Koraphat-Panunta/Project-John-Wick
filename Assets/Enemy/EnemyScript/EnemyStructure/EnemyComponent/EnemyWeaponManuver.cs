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

    protected override void InitailzedWeaponManuverNode()
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

        startWeaponManuverSelectorNode = new WeaponManuverSelectorNode(this.weaponAdvanceUser, () => true);

        startWeaponManuverSelectorNode.AddtoChildNode(swtichingWeaponManuverSelector);
        startWeaponManuverSelectorNode.AddtoChildNode(aimDownSightWeaponManuverNodeLeaf);
        startWeaponManuverSelectorNode.AddtoChildNode(lowReadyWeaponManuverNodeLeaf);
        startWeaponManuverSelectorNode.AddtoChildNode(restWeaponManuverLeafNode);

        swtichingWeaponManuverSelector.AddtoChildNode(primaryToSecondarySwitchWeaponManuverLeafNode);
        swtichingWeaponManuverSelector.AddtoChildNode(secondaryToPrimarySwitchWeaponManuverLeafNode);

        startWeaponManuverSelectorNode.FindingNode(out WeaponManuverLeafNode weaponManuverLeafNode);
        curWeaponManuverLeafNode = weaponManuverLeafNode;
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

        if (enemy._isInPain)
        {
            isAimingManuver = false;
            isPullTriggerManuver = false;
            isReloadManuver = false;
            isSwitchWeaponManuver = false;
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
