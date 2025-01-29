using UnityEngine;

public class WeaponManuverManager 
{
    public IWeaponAdvanceUser weaponAdvanceUser;
    public Weapon curWeapon;

    public WeaponManuverLeafNode curWeaponManuverLeafNode;
    public WeaponManuverSelectorNode startWeaponManuverSelectorNode;

    public float aimingWeight;

    public bool isAiming;
    public bool isPullTrigger;
    public bool isReload;
    public bool isSwitchWeapon;

    public WeaponManuverManager(IWeaponAdvanceUser weaponAdvanceUser)
    {
        this.weaponAdvanceUser = weaponAdvanceUser;
        curWeapon = weaponAdvanceUser.currentWeapon;

        InitailzedWeaponManuverNode();
    }
    private void InitailzedWeaponManuverNode()
    {

    }
    public void UpdateNode()
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
    public void FixedUpdateNode()
    {
        if(curWeaponManuverLeafNode != null)
            curWeaponManuverLeafNode.FixedUpdateNode();
    }
    public void ChangeWeaponManuverNode(WeaponManuverLeafNode weaponManuverLeafNode)
    {
        curWeaponManuverLeafNode.Exit();
        curWeaponManuverLeafNode = weaponManuverLeafNode;
        curWeaponManuverLeafNode.Enter();

    }
}
