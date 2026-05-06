using UnityEngine;

public class WeaponBelt 
{
    public PrimaryWeaponSocket primaryWeaponSocket;
    public SecondaryWeaponSocket secondaryWeaponSocket;

    public PrimaryWeapon myPrimaryWeapon;
    public SecondaryWeapon mySecondaryWeapon;

    public AmmoProuch ammoProuch;
    public WeaponBelt(PrimaryWeaponSocket primaryWeaponSocket, SecondaryWeaponSocket secondaryWeaponSocket,AmmoProuch ammoProuch)
    {
        this.primaryWeaponSocket = primaryWeaponSocket;
        this.secondaryWeaponSocket = secondaryWeaponSocket;
        this.ammoProuch = ammoProuch;
    }

}
