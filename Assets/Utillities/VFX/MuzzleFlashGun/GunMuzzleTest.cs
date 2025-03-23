using UnityEngine;
using UnityEngine.VFX;
public class GunMuzzleTest : MonoBehaviour
{
    public VisualEffect muzzleVFX;


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Fire();
        }
    }

    void Fire()
    {
        if (muzzleVFX != null)
        {
            muzzleVFX.SendEvent("OnPlay");
        }
    }
}
