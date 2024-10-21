using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationAR15 : WeaponAnimation
{
    public override void SpawnMagDrop()
    {
        if (magazine != null)
        {
            GameObject mag = GameObject.Instantiate(magazine);
            mag.transform.position = gameObject.transform.position + Vector3.down * 0.1f;
            mag.GetComponent<Rigidbody>().AddForce(-gameObject.transform.right * 3.4f + gameObject.transform.up * 2.6f, ForceMode.Impulse);
            mag.GetComponent<Rigidbody>().AddForceAtPosition(-mag.transform.right * 5, magazine.transform.position - magazine.transform.up * magazine.transform.localScale.y);
            StartCoroutine(Removemag(mag));
        }
    }
}
