using UnityEngine;

public class PrimaryWeaponSocket : MonoBehaviour, IWeaponAttachingAble
{
    public Transform weaponAttachingAbleTransform => this.transform;
}
