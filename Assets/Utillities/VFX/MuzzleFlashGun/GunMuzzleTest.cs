using UnityEngine;
using UnityEngine.VFX;
public class GunMuzzleTest : MonoBehaviour
{
    [SerializeField] private VisualEffect muzzleVFX;
    [SerializeField] private VisualEffect bulletShell;


   
    public void Fire()
    {
        //Debug.Log("WeaponFireVFX");
           
        if(muzzleVFX != null)
        muzzleVFX.SendEvent("OnPlay");
        
        if(bulletShell != null)
        bulletShell.SendEvent("OnPlay");
    }

  

   
}
