using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fixed (non-random) loot table. Designer specifies a list of entries; every entry
/// is dropped exactly <see cref="LootEntry.count"/> times when the table is used.
///
/// Centralizes "what does this enemy / chest / event drop" so spawn counts no longer
/// live in code. Add a new entry, save asset, drop counts change — no recompile.
/// </summary>
[CreateAssetMenu(fileName = "LootTable", menuName = "ScriptableObjects/LootTable")]
public class LootTableScriptableObject : ScriptableObject
{
    [Serializable]
    public class LootEntry
    {
        public PickupableDefinition definition;
        [Min(0)] public int count = 1;
    }

    public List<LootEntry> entries = new List<LootEntry>();
}
