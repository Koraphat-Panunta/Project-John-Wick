using UnityEngine;

[System.Serializable]
public abstract class EnemySpawnerExtensionData
{
    public abstract void Apply(Enemy enemy);

    public static void ApplyExtensions(Enemy enemy, EnemySpawnerExtensionData[] extensions)
    {
        if (extensions == null) return;
        foreach (var ext in extensions)
            ext.Apply(enemy);
    }
}
