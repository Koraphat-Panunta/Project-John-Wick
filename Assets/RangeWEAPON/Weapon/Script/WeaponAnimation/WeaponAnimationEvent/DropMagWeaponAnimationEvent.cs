using System.Collections;
using UnityEngine;

public class DropMagWeaponAnimationEvent : MonoBehaviour
{
    [SerializeField] Transform realMag;
    [SerializeField] GameObject originMagazine;




    public void ReleasesMagazine()
    {

        this.originMagazine.transform.position = this.realMag.position;
        this.originMagazine.transform.rotation = this.realMag.rotation;
        GameObject dropedMag = GameObject.Instantiate(this.originMagazine.gameObject);
        dropedMag.gameObject.SetActive(true);
        dropedMag.transform.position = this.realMag.position;
        dropedMag.transform.rotation = this.realMag.rotation;
        dropedMag.GetComponent<Rigidbody>().AddRelativeForce(realMag.up * -3,ForceMode.Impulse);
        StartCoroutine(DisableMag(dropedMag));

    }

    public IEnumerator DisableMag(GameObject magDroped)
    {
        yield return new WaitForSeconds(3);
        GameObject.Destroy(magDroped);

    }
}
