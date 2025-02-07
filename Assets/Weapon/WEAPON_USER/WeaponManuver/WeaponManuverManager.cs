using UnityEngine;

public abstract class WeaponManuverManager : INodeManager
{
    public NodeManagerBehavior nodeManagerBehavior { get; set ; }
    public INodeLeaf curNodeLeaf { get; set; }
    public INodeSelector startNodeSelector { get; set; }

    public IWeaponAdvanceUser weaponAdvanceUser;
    public IMovementCompoent movementCompoent;
    public Weapon curWeapon => weaponAdvanceUser.currentWeapon;

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
        this.startNodeSelector = new WeaponManuverSelectorNode(weaponAdvanceUser, () => true);

        nodeManagerBehavior = new NodeManagerBehavior();
        InitailizedNode();
    }
    public abstract void InitailizedNode();

    public virtual void UpdateNode()
    {
        
        nodeManagerBehavior.UpdateNode(this);
        Debug.Log("Call in WeaponManager curNodeLeaf = " + curNodeLeaf);

    }
    public virtual void FixedUpdateNode() => nodeManagerBehavior.FixedUpdateNode(this);
 
    public virtual void WeaponCommanding()
    {
        if (isPullTriggerManuver)
            curWeapon.PullTrigger();

        if (isReloadManuver)
            curWeapon.Reload();
    }
}
