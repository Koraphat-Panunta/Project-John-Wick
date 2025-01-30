using UnityEngine;

public class WeaponManuverManager 
{
    public IWeaponAdvanceUser weaponAdvanceUser;
    public IMovementCompoent movementCompoent;
    public Weapon curWeapon;

    public WeaponManuverLeafNode curWeaponManuverLeafNode;
    public WeaponManuverSelectorNode startWeaponManuverSelectorNode;

    public float aimingWeight;

    public bool isAiming;
    public bool isPullTrigger;
    public bool isReload;
    public bool isSwitchWeapon;

    public WeaponManuverSelectorNode swtichingWeaponManuverSelector { get; protected set; }
    public PrimaryToSecondarySwitchWeaponManuverLeafNode primaryToSecondarySwitchWeaponManuverLeafNode { get; protected set; }
    public SecondaryToPrimarySwitchWeaponManuverLeafNode secondaryToPrimarySwitchWeaponManuverLeafNode{ get; protected set; }
    public AimDownSightWeaponManuverNodeLeaf aimDownSightWeaponManuverNodeLeaf { get; protected set; }
    public LowReadyWeaponManuverNodeLeaf lowReadyWeaponManuverNodeLeaf { get; protected set; }
    public RestWeaponManuverLeafNode restWeaponManuverLeafNode { get; protected set; }

    public WeaponManuverManager(IWeaponAdvanceUser weaponAdvanceUser)
    {
        this.weaponAdvanceUser = weaponAdvanceUser;
        curWeapon = weaponAdvanceUser.currentWeapon;

        InitailzedWeaponManuverNode();
    }
    protected virtual void InitailzedWeaponManuverNode()
    {
        swtichingWeaponManuverSelector = new WeaponManuverSelectorNode(this.weaponAdvanceUser, 
            () => isSwitchWeapon);
        primaryToSecondarySwitchWeaponManuverLeafNode = new PrimaryToSecondarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => curWeapon is PrimaryWeapon);
        secondaryToPrimarySwitchWeaponManuverLeafNode = new SecondaryToPrimarySwitchWeaponManuverLeafNode(this.weaponAdvanceUser,
            () => curWeapon is SecondaryWeapon);

        aimDownSightWeaponManuverNodeLeaf = new AimDownSightWeaponManuverNodeLeaf(this.weaponAdvanceUser,
            ()=> isAiming);
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
    public virtual void UpdateNode()
    {
        if (curWeaponManuverLeafNode.IsReset())
        {
            curWeaponManuverLeafNode.Exit();
            curWeaponManuverLeafNode = null;
            startWeaponManuverSelectorNode.FindingNode(out WeaponManuverLeafNode weaponManuverLeafNode);
            curWeaponManuverLeafNode = weaponManuverLeafNode;
            curWeaponManuverLeafNode.Enter();
        }

        if(curWeaponManuverLeafNode != null)
            curWeaponManuverLeafNode.UpdateNode();
    }
    public virtual void FixedUpdateNode()
    {
        if(curWeaponManuverLeafNode != null)
            curWeaponManuverLeafNode.FixedUpdateNode();
    }
    public virtual void LateUpdate()
    {

    }
    public void ChangeWeaponManuverNode(WeaponManuverLeafNode weaponManuverLeafNode)
    {
        curWeaponManuverLeafNode.Exit();
        curWeaponManuverLeafNode = weaponManuverLeafNode;
        curWeaponManuverLeafNode.Enter();

    }
    public virtual void WeaponCommanding()
    {
        if (isPullTrigger)
            curWeapon.PullTrigger();

        if(isReload)
            curWeapon.Reload();
    }
}
