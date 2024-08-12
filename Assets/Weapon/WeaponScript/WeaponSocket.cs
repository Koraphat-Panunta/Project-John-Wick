using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSocket : MonoBehaviour
{
    public enum Socket
    {
        InHand,
        Holster
    }
    public Socket Thissocket;
    [SerializeField] private Animator UserAnimator;
    public Weapon CurWeapon;
    public WeaponSingleton weaponSingleton;
    [SerializeField] private GameObject WeaponUser;
    [SerializeField] private Camera camera;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        weaponSingleton = GetComponentInChildren<WeaponSingleton>();
        StartCoroutine(GetWeaponSingleton());
    }

    // Update is called once per frame
    public Animator GetAnimator()
    {
        return UserAnimator;
    }
    public GameObject GetWeaponUser()
    {
        return WeaponUser;
    }
    public Camera GetCamera()
    {
        return camera;
    }
    IEnumerator GetWeaponSingleton()
    {
       
        while(CurWeapon == null)
        {
            CurWeapon = weaponSingleton.GetWeapon();
            yield return null;
        }
    }
}
