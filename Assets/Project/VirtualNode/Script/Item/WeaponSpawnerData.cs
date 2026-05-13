using UnityEngine;

[System.Serializable]
public struct WeaponSpawnerData
{
    public WeaponDataScriptableObject weaponData;

    [SerializeReference]
    public WeaponSpawnerExtensionData[] extensions;
}
