using UnityEngine;

public class AmmoGetAbleObject : ItemObject<IAmmoRecivedAble>
{
    [Range(0, 100)]
    [SerializeField] public int amoutPrimaryAmmoAdd;

    [Range(0,100)]
    [SerializeField] public int amoutSecondaryAmmoAdd;

    protected override void SetVisitorClient(IAmmoRecivedAble client)
    {
        Weapon primaryWeapon = clent.weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon;
        Weapon secondaryWeapon = clent.weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon;

        clent.ammoProuch.AddAmmo(primaryWeapon.bullet.myType, amoutPrimaryAmmoAdd);
        clent.ammoProuch.AddAmmo(secondaryWeapon.bullet.myType, amoutSecondaryAmmoAdd);

        client.Recived(this);
    }
    protected override void Update()
    {
        base.Update();
    }
}
