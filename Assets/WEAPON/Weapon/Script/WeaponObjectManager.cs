using System.Collections.Generic;
using UnityEngine;

public class WeaponObjectManager 
{
    public Weapon originWeapon { get; protected set; }

    private ObjectPooling<Weapon> weaponObjPool;
    protected Camera mainCamera;

    public WeaponObjectManager(Weapon originWeapon,Camera mainCamera)
    {
        this.mainCamera = mainCamera;
        this.originWeapon = originWeapon;
        
    }
}
