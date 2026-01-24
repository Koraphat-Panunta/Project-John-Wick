using System.Collections;
using UnityEngine;

public class DropMagWeaponAnimationEvent : MonoBehaviour
{
    [SerializeField] Rigidbody originMagazine;

    Coroutine disableMag;

    [SerializeField] Vector3 forceOnReleses;
    public void ReleasesMagazine()
    {
        if(disableMag != null)
            StopCoroutine(disableMag);
        this.originMagazine.gameObject.SetActive(true);
        this.originMagazine.AddRelativeForce(forceOnReleses,ForceMode.VelocityChange);
        this.disableMag = StartCoroutine(DisableMag());
    }

    public IEnumerator DisableMag()
    {
        yield return new WaitForSeconds(3);
        this.originMagazine.gameObject.SetActive(false);
        this.disableMag = null;
    }
}
