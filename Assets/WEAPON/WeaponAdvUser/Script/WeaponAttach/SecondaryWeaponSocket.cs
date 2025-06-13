using UnityEngine;

public class SecondaryWeaponSocket : MonoBehaviour, IWeaponAttachingAble
{
    public Transform weaponAttachingAbleTransform => this.transform;
}
