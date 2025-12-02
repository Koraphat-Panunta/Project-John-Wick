using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyDebuger :MonoBehaviour,IInitializedAble
{
    public Enemy enemy;
    [SerializeField] private string CurrentEnemyState;
    [SerializeField] private float posture;

    [SerializeField,TextArea] 
    private string Debug;

    [SerializeField] private Weapon curWeapon;
    [SerializeField] private Weapon myPrimaryWeapon;
    [SerializeField] private Weapon mySecondaryWeapon;

    [SerializeField] private int curWeaponMagCount;
    [SerializeField] private int curWeaponBulletCapacity;
    // Start is called before the first frame update

    public void Initialized()
    {
        
    }
    // UpdateNode is called once per frame
    void Update()
    {
        this.curWeapon = enemy._currentWeapon;
        this.myPrimaryWeapon = enemy._weaponBelt.myPrimaryWeapon as Weapon;
        this.mySecondaryWeapon = enemy._weaponBelt.mySecondaryWeapon as Weapon;

        if (curWeapon != null)
        {
            curWeaponBulletCapacity = curWeapon.bulletCapacity;
            curWeaponMagCount = (int)(enemy._currentWeapon.bulletCapacity * 0.7f);
        }

       CurrentEnemyState = this.enemy.enemyStateManagerNode.GetCurNodeLeaf().ToString();
       posture = enemy._posture;


    }
    private void OnDrawGizmos()
    {
       
    }

  
}
