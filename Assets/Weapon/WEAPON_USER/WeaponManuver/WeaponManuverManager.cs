using UnityEngine;

public abstract class WeaponManuverManager 
{
    public IWeaponAdvanceUser weaponAdvanceUser;
    public IMovementCompoent movementCompoent;
    public Weapon curWeapon => weaponAdvanceUser.currentWeapon;

    public WeaponManuverLeafNode curWeaponManuverLeafNode;
    public WeaponManuverSelectorNode startWeaponManuverSelectorNode;

    public float aimingWeight;

    public bool isAimingManuver;
    public bool isPullTriggerManuver;
    public bool isReloadManuver;
    public bool isSwitchWeaponManuver;

    public abstract WeaponManuverSelectorNode swtichingWeaponManuverSelector { get; protected set; }
    public abstract PrimaryToSecondarySwitchWeaponManuverLeafNode primaryToSecondarySwitchWeaponManuverLeafNode { get; protected set; }
    public abstract SecondaryToPrimarySwitchWeaponManuverLeafNode secondaryToPrimarySwitchWeaponManuverLeafNode { get; protected set; }
    public abstract AimDownSightWeaponManuverNodeLeaf aimDownSightWeaponManuverNodeLeaf { get; protected set; }
    public abstract LowReadyWeaponManuverNodeLeaf lowReadyWeaponManuverNodeLeaf { get; protected set; }
    public abstract RestWeaponManuverLeafNode restWeaponManuverLeafNode { get; protected set; }

    public WeaponManuverManager(IWeaponAdvanceUser weaponAdvanceUser)
    {
        this.weaponAdvanceUser = weaponAdvanceUser;

        startWeaponManuverSelectorNode = new WeaponManuverSelectorNode(weaponAdvanceUser, () => true);
        InitailzedWeaponManuverNode();
    }
    protected abstract void InitailzedWeaponManuverNode();

    public virtual void UpdateNode()
    {

        if (curWeaponManuverLeafNode.IsReset())
        {
            Debug.Log(curWeaponManuverLeafNode + " IsReset");
            curWeaponManuverLeafNode.Exit();
            curWeaponManuverLeafNode = null;
            startWeaponManuverSelectorNode.FindingNode(out INodeLeaf weaponManuverLeafNode);
            curWeaponManuverLeafNode = weaponManuverLeafNode as WeaponManuverLeafNode;
            curWeaponManuverLeafNode.Enter();
        }

        if (curWeaponManuverLeafNode != null)
            curWeaponManuverLeafNode.UpdateNode();
    }
    public virtual void FixedUpdateNode()
    {
        if (curWeaponManuverLeafNode != null)
            curWeaponManuverLeafNode.FixedUpdateNode();
    }
    public void ChangeWeaponManuverNode(WeaponManuverLeafNode weaponManuverLeafNode)
    {
        curWeaponManuverLeafNode.Exit();
        curWeaponManuverLeafNode = weaponManuverLeafNode;
        curWeaponManuverLeafNode.Enter();

    }
    public virtual void WeaponCommanding()
    {
        if (isPullTriggerManuver)
            curWeapon.PullTrigger();

        if (isReloadManuver)
            curWeapon.Reload();
    }
}
