using UnityEngine;

public class SecondaryWeaponSocket : MonoBehaviour, IWeaponAttachingAble
{
    [SerializeField] private Character character;
    public Transform weaponAttachingAbleTransform => this.transform;

    public IWeaponAdvanceUser weaponAdvanceUser => this.character as IWeaponAdvanceUser;

    public Weapon curWeaponAtSocket { get; set; }

    public void OnValidate()
    {
        if (this.character == null)
            this.character = GetComponentInParent<Character>();
    }
}
