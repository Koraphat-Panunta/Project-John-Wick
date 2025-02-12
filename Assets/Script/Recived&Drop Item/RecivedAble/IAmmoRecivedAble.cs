using UnityEngine;

public interface IAmmoRecivedAble : IRecivedAble
{
    public IWeaponAdvanceUser weaponAdvanceUser { get; }
    public AmmoProuch ammoProuch { get => weaponAdvanceUser.weaponBelt.ammoProuch; }

    public void Recived(AmmoGetAbleObject ammoGetAbleObject);
    
}
