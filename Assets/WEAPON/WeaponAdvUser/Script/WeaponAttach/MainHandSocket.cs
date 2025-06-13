using UnityEngine;

public class MainHandSocket : MonoBehaviour, IWeaponAttachingAble
{
    [SerializeField] private Character character;
    public Transform weaponAttachingAbleTransform => this.transform;
    public IWeaponAdvanceUser weaponAdvanceUser => character.GetComponent<IWeaponAdvanceUser>();

    private void OnValidate()
    {
        if(character == null)
            character = GetComponentInParent<Character>();
    }
}
