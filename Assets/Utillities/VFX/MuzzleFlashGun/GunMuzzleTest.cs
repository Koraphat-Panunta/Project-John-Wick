using UnityEngine;
using UnityEngine.VFX;
public class GunMuzzleTest : MonoBehaviour,IObserverWeapon
{
    [SerializeField] private VisualEffect muzzleVFX;
    [SerializeField] private VisualEffect bulletShell;
    [SerializeField] private Weapon weapon;

    private void Awake()
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

    public void OnNotify(Weapon weapon, WeaponSubject.WeaponNotifyType weaponNotify)
    {
       if(weaponNotify == WeaponSubject.WeaponNotifyType.Firing)
            Fire();
    }
}
