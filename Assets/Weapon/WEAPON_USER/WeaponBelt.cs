using UnityEngine;

public class WeaponBelt 
{
    public Transform primaryWeaponSocket;
    public Transform secondaryWeaponSocket;

    public PrimaryWeapon primaryWeapon;
    public SecondaryWeapon secondaryWeapon;

    public AmmoProuch ammoProuch;
    public WeaponBelt(Transform primaryWeaponSocket,Transform secondaryWeaponSocket,AmmoProuch ammoProuch)
    {
        this.primaryWeaponSocket = primaryWeaponSocket;
        this.secondaryWeaponSocket = secondaryWeaponSocket;
        this.ammoProuch = ammoProuch;
    }

}
