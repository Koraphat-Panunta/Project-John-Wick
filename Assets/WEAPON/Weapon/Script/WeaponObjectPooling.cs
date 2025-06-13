using System.Collections.Generic;
using UnityEngine;

public class WeaponObjectPooling : MonoBehaviour
{
    [SerializeField] List<Weapon> weapons = new List<Weapon>();
     private Queue<Weapon> weaponsQ;

    private void Awake()
    {
        this.weaponsQ = new Queue<Weapon>();
        this.weapons.ForEach(w => weaponsQ.Enqueue(w));
    }
    public Weapon Pull()
    {
        Weapon weapon = weaponsQ.Dequeue();
        return weapon;
    }

    public void Return(Weapon weapon)
    {
        this.weaponsQ.Enqueue(weapon);
    }
}
