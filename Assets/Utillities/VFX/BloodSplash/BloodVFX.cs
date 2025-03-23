using UnityEngine;
using UnityEngine.VFX;

public class BloodVFX : MonoBehaviour
{
    public VisualEffect BloodSplashVFX;


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Whatever Prox Method >w<
        {
            Hit();
        }
    }

    void Hit()
    {
        if (BloodSplashVFX != null)
        {
            BloodSplashVFX.SendEvent("OnPlay");
        }
    }
}
