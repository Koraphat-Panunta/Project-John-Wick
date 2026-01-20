using UnityEngine;
using UnityEngine.VFX;
public class GunMuzzleTest : MonoBehaviour,IObserverWeapon,IInitializedAble
{
    [SerializeField] private VisualEffect muzzleVFX;
    [SerializeField] private VisualEffect bulletShell;
    [SerializeField] private Weapon weapon;

    public void Initialized()
    {
        weapon.AddObserver(this);
    }
    public void Fire()
    {
        //Debug.Log("WeaponFireVFX");
           
        if(muzzleVFX != null)
        muzzleVFX.SendEvent("OnPlay");
        
        if(bulletShell != null)
        bulletShell.SendEvent("OnPlay");
    }

    public void OnNotify<T>(Weapon weapon, T weaponNotify)
    {
       if(weaponNotify is WeaponSubject.WeaponNotifyType weaponNotifyMassage
            && weaponNotifyMassage == WeaponSubject.WeaponNotifyType.Firing)
            Fire();
    }

   
}
