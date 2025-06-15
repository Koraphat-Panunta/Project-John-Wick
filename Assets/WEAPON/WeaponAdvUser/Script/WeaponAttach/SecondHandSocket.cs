using UnityEngine;

public class SecondHandSocket : MonoBehaviour, IWeaponAttachingAble
{
    [SerializeField] private Character character;
    public Transform weaponAttachingAbleTransform => this.transform;
    public IWeaponAdvanceUser weaponAdvanceUser => character.GetComponent<IWeaponAdvanceUser>();

    public Weapon curWeaponAtSocket { get ; set ; }

   

    private void OnValidate()
    {
        if (character == null)
            character = GetComponentInParent<Character>();
    }
}
