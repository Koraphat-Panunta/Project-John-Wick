using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSocket : MonoBehaviour
{
    [SerializeField] private Animator UserAnimator;
    public Weapon CurWeapon;
    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CurWeapon = GetComponentInChildren<Weapon>();
    }
    public Animator GetAnimator()
    {
        return UserAnimator;
    }
}
