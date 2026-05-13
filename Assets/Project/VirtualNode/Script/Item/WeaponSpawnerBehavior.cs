public static class WeaponSpawnerBehavior
{
    public static void SpawnAndAttach(WeaponPoolManager weaponPoolManager, WeaponSpawnerData spawnerData, IGrabAbleObject weaponSocket)
    {
        if (spawnerData.weaponData == null) return;

        if (spawnerData.weaponData is RangeWeaponDataScriptableObject
            || spawnerData.weaponData is MeleeWeaponDataScriptableObject)
        {
            Weapon weapon = weaponPoolManager.SpawnWeapon(spawnerData.weaponData, weaponSocket);
            if (weapon == null) return;
            WeaponSpawnerExtensionData.ApplyExtensions(weapon, spawnerData.extensions);
        }
    }
}
