using UnityEngine;

public class AmmoGetAbleObject : ItemObject
{
    [Range(0, 100)]
    [SerializeField] public int amoutPrimaryAmmoAdd;

    [Range(0,100)]
    [SerializeField] public int amoutSecondaryAmmoAdd;

    

    protected override void SetVisitorClient(IRecivedAble client)
    {
        Weapon primaryWeapon = (client as IAmmoRecivedAble).weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon;
        Weapon secondaryWeapon = (client as IAmmoRecivedAble).weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon;

        (client as IAmmoRecivedAble).ammoProuch.AddAmmo(primaryWeapon.bullet.myType, amoutPrimaryAmmoAdd);
        (client as IAmmoRecivedAble).ammoProuch.AddAmmo(secondaryWeapon.bullet.myType, amoutSecondaryAmmoAdd);

        (client as IAmmoRecivedAble).Recived(this);
    }

    protected override void Update()
    {
        base.Update();
    }
}
