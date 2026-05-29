using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyBindingSettingOptionDisplay : OptionUIDisplayer
{
    [SerializeField] public KeyBindingEntryUI[] entryUIs;
    [SerializeField] public Button resetToDefaultButton;

    // Assign in the inspector when key icon images are ready.
    // When null, entries show text labels instead of icons.
    [SerializeField] public KeyBindingIconDataScriptableObject iconData;

    private List<KeyBindingSettingMenuSector.RebindEntry> _entries;

    public void SetEntries(List<KeyBindingSettingMenuSector.RebindEntry> entries)
    {
        _entries = entries;
    }

    protected override void Load(SettingDataScriptableObject dataBased)
    {
        RefreshAllEntries();
    }

    public void RefreshAllEntries()
    {
        if (_entries == null) return;
        for (int i = 0; i < entryUIs.Length && i < _entries.Count; i++)
            RefreshEntry(i);
    }

    public void RefreshEntry(int index)
    {
        if (_entries == null || index >= _entries.Count || index >= entryUIs.Length) return;

        var entry = _entries[index];
        string path = entry.action.bindings[entry.bindingIndex].effectivePath;
        string display = InputControlPath.ToHumanReadableString(
            path,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        entryUIs[index].SetBinding(entry.displayName, path, display, iconData);
    }
}
