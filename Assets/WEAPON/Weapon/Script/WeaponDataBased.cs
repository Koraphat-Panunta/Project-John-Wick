using UnityEngine;

public class WeaponDataBased : MonoBehaviour
{
    public static WeaponDataBased weaponDataBased;

    [SerializeField] public WeaponDataScriptableObject[] weaponDataScriptableObject;

    public void Awake()
    {
        weaponDataBased = this;
    }
}
