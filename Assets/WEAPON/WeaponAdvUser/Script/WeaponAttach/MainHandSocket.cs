using UnityEngine;

public class MainHandSocket : MonoBehaviour, IWeaponAttachingAble
{
    [SerializeField] private Character character;
    public Transform weaponAttachingAbleTransform => this.transform;
    public IWeaponAdvanceUser weaponAdvanceUser => character as IWeaponAdvanceUser;
    public Weapon curWeaponAtSocket { get => weaponAdvanceUser._currentWeapon; set => weaponAdvanceUser._currentWeapon = value; }

    private void OnValidate()
    {
        if(character == null)
            character = GetComponentInParent<Character>();
    }
}
