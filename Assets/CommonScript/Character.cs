using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IWeaponSenses,IDamageAble
{
    protected float HP;
    //Call When Weapon performed Event
    public abstract void Aiming(Weapon weapon);


    public abstract void Firing(Weapon weapon);

    public abstract void LowReadying(Weapon weapon);

    public abstract void Reloading(Weapon weapon, Reload.ReloadType reloadType);
    

    public void TakeDamage(float Damage)
    {
        HP -= Damage;
    }
}
