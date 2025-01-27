using UnityEngine;

public class WeaponManuverManager 
{
    public IWeaponAdvanceUser weaponAdvanceUser;
    public Weapon curWeapon;

    public WeaponManuverLeafNode curWeaponManuverLeafNode;
    public WeaponManuverSelectorNode weaponManuverSelectorNode;

    public float aimingWeight;

    public bool isAiming;
    public bool isPullTrigger;
    public bool isReload;
    public bool isSwitchWeapon;

    public WeaponManuverManager(IWeaponAdvanceUser weaponAdvanceUser)
    {
        this.weaponAdvanceUser = weaponAdvanceUser;
        curWeapon = weaponAdvanceUser.currentWeapon;
    }
    private void InitailzedWeaponManuverNode()
    {

    }
    public void UpdateNode()
    {
        
    }
    public void FixedUpdateNode()
    {

    }
}
