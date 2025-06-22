using UnityEngine;

public abstract class WeaponManuverManager : INodeManager
{
    public NodeManagerBehavior nodeManagerBehavior { get; set ; }
    INodeLeaf INodeManager.curNodeLeaf { get => curNodeLeaf; set => curNodeLeaf = value; }
    private INodeLeaf curNodeLeaf;
    public INodeSelector startNodeSelector { get; set; }

    public IWeaponAdvanceUser weaponAdvanceUser;
    public IMovementCompoent movementCompoent;
    public Weapon curWeapon => weaponAdvanceUser._currentWeapon;

    public float aimingWeight;

    public abstract bool isAimingManuverAble { get; }
    public abstract bool isPullTriggerManuverAble { get; }
    public abstract bool isReloadManuverAble { get; }
    public abstract bool isDropWeaponManuverAble { get; }
    public abstract bool isPickingUpWeaponManuverAble { get; }
    public abstract bool isSwitchWeaponManuverAble { get; }

    public abstract PickUpWeaponNodeLeaf pickUpWeaponNodeLeaf { get;protected set; }
    public abstract DropWeaponManuverNodeLeaf dropWeaponManuverNodeLeaf { get;protected set; }
    public abstract DrawPrimaryWeaponManuverNodeLeaf drawPrimaryWeaponManuverNodeLeaf { get; protected set; }
    public abstract DrawSecondaryWeaponManuverNodeLeaf drawSecondaryWeaponManuverNodeLeaf { get; protected set; }
    public abstract HolsterPrimaryWeaponManuverNodeLeaf holsterPrimaryWeaponManuverNodeLeaf { get; protected set; }
    public abstract HolsterSecondaryWeaponManuverNodeLeaf holsterSecondaryWeaponManuverNodeLeaf { get; protected set; }
    public abstract PrimaryToSecondarySwitchWeaponManuverLeafNode primaryToSecondarySwitchWeaponManuverLeafNode { get; protected set; }
    public abstract SecondaryToPrimarySwitchWeaponManuverLeafNode secondaryToPrimarySwitchWeaponManuverLeafNode { get; protected set; }
    public abstract AimDownSightWeaponManuverNodeLeaf aimDownSightWeaponManuverNodeLeaf { get; protected set; }
    public abstract LowReadyWeaponManuverNodeLeaf lowReadyWeaponManuverNodeLeaf { get; protected set; }
    public abstract RestWeaponManuverLeafNode restWeaponManuverLeafNode { get; protected set; }
    public abstract NodeAttachAbleSelector reloadNodeAttachAbleSelector { get; protected set; }


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
    }
    public virtual void FixedUpdateNode() => nodeManagerBehavior.FixedUpdateNode(this);

    
}
