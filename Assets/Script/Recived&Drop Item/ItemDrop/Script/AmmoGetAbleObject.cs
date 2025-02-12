using UnityEngine;

public class AmmoGetAbleObject : ItemObject<IAmmoRecivedAble>
{
    [Range(0, 100)]
    [SerializeField] private int amoutPrimaryAmmoAdd;

    [Range(0,100)]
    [SerializeField] private int amoutSecondaryAmmoAdd;

    protected override void SetVisitorClient(IAmmoRecivedAble client)
    {
        Weapon primaryWeapon = clent.weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon;
        Weapon secondaryWeapon = clent.weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon;

        Debug.Log("primaryWeapon ammo before =" + client.ammoProuch.amountOf_ammo[primaryWeapon.bullet.myType]);

        clent.ammoProuch.AddAmmo(primaryWeapon.bullet.myType, amoutPrimaryAmmoAdd);
        clent.ammoProuch.AddAmmo(secondaryWeapon.bullet.myType, amoutSecondaryAmmoAdd);


        Debug.Log("primaryWeapon ammo after =" + client.ammoProuch.amountOf_ammo[primaryWeapon.bullet.myType]);


        client.Recived(this);
    }
}
