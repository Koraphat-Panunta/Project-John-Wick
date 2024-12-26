using UnityEngine;
using System.Collections.Generic;
using System;

public class WeaponDataBased : MonoBehaviour
{
    public Dictionary<Weapon,bool> weaponUnlockable = new Dictionary<Weapon, bool>();
    public Dictionary<Weapon,IWeaponAttachmentData> weaponAttachmentData = new Dictionary<Weapon,IWeaponAttachmentData>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
