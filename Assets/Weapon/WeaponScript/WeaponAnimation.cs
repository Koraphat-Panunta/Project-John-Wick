using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour,IObserverWeapon
{
    [SerializeField] public Weapon weapon;
    [SerializeField] public Animator animator;
    [SerializeField] public GameObject magazine;

    public void OnNotify(Weapon weapon, WeaponSubject.WeaponNotifyType weaponNotify)
    {
       if(weaponNotify == WeaponSubject.WeaponNotifyType.Reloading)
       {
            animator.SetTrigger("Reloading");
       }
       else if(weaponNotify == WeaponSubject.WeaponNotifyType.TacticalReload)
       {
            animator.SetTrigger("Reloading");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(weapon == null)
        {
            weapon = GetComponent<Weapon>();
        }
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
        weapon.AddObserver(this);
    }
    private void OnDisable()
    {
        weapon.Remove(this);
    }
    public void SpawnMagDrop()
    {
        GameObject mag = GameObject.Instantiate(magazine);
        mag.transform.position = gameObject.transform.position + Vector3.down*0.1f;
        mag.GetComponent<Rigidbody>().AddForce(gameObject.transform.right*4+gameObject.transform.up*3, ForceMode.Impulse);
        mag.GetComponent<Rigidbody>().AddForceAtPosition(mag.transform.right*5, magazine.transform.position - magazine.transform.up * magazine.transform.localScale.y);
        StartCoroutine(Removemag(mag));
    }
    IEnumerator Removemag(GameObject mag)
    {
        yield return new WaitForSeconds(3);
        GameObject.Destroy(mag);
    }
}
