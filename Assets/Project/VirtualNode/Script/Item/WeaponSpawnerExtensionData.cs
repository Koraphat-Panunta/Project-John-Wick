using UnityEngine;

[System.Serializable]
public abstract class WeaponSpawnerExtensionData
{
    public abstract void Apply<T>(Weapon weapon) where T : Weapon;

    public static void ApplyExtensions<T>(T weapon, WeaponSpawnerExtensionData[] extensions) where T : Weapon
    {
        if (extensions == null) return;
        foreach (var ext in extensions)
            ext?.Apply<T>(weapon);
    }
}
