using UnityEngine;

/// <summary>
/// Generic loot spawner. Reads a <see cref="LootTableScriptableObject"/> and instantiates
/// each entry's <see cref="PickupableDefinition.worldPrefab"/> at this transform's position
/// with a random sideways + upward physics impulse — same feel as the legacy
/// DropAbleObjectClient.
///
/// Usage: drop on any source (enemy death listener, chest, level event), assign a
/// LootTable, and call <see cref="Drop"/> when the source decides it's time.
/// </summary>
public class Dropper : MonoBehaviour
{
    [SerializeField] private LootTableScriptableObject lootTable;

    [Range(0, 100)] [SerializeField] private float spawnForceUp = 5f;
    [Range(0, 100)] [SerializeField] private float spawnForceSide = 3f;

    [Tooltip("Optional override for the table — set to drop something specific without changing the field.")]
    private LootTableScriptableObject overrideTable;

    public void SetLootTable(LootTableScriptableObject table) => lootTable = table;

    /// <summary>Drop everything in the assigned loot table once.</summary>
    public void Drop()
    {
        var table = overrideTable != null ? overrideTable : lootTable;
        if (table == null || table.entries == null) return;

        for (int i = 0; i < table.entries.Count; i++)
        {
            var entry = table.entries[i];
            if (entry == null || entry.definition == null) continue;
            if (entry.definition.worldPrefab == null)
            {
                Debug.LogWarning($"[Dropper] '{entry.definition.name}' has no worldPrefab assigned — skipped.", this);
                continue;
            }
            for (int n = 0; n < entry.count; n++)
                SpawnOne(entry.definition.worldPrefab);
        }
    }

    /// <summary>Drop a one-off table without changing the assigned one.</summary>
    public void Drop(LootTableScriptableObject table)
    {
        var prev = overrideTable;
        overrideTable = table;
        Drop();
        overrideTable = prev;
    }

    private void SpawnOne(GameObject prefab)
    {
        var instance = Instantiate(prefab, transform.position, transform.rotation);
        var rb = instance.GetComponent<Rigidbody>();
        if (rb == null) return;

        Vector3 sideForce = Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.right * spawnForceSide;
        Vector3 upForce = Vector3.up * spawnForceUp;
        rb.AddForce(sideForce + upForce, ForceMode.Impulse);
    }
}
