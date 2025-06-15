using UnityEngine;

public interface IWeaponAttachingAble
{
    public Transform weaponAttachingAbleTransform { get; }
    public IWeaponAdvanceUser weaponAdvanceUser { get; }
    public Weapon curWeaponAtSocket { get; set; }
    
}
