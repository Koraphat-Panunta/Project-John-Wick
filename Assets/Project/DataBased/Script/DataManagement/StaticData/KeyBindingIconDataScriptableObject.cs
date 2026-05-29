using System;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyBindingIconData", menuName = "ScriptableObjects/GameData/KeyBindingIconDataSCRP")]
public class KeyBindingIconDataScriptableObject : ScriptableObject
{
    [Serializable]
    public struct KeyIconEntry
    {
        // Unity Input System binding path, e.g. "<Keyboard>/space", "<Mouse>/leftButton"
        public string bindingPath;
        public Sprite icon;
    }

    public KeyIconEntry[] keyIconEntries;

    public bool TryGetIcon(string bindingPath, out Sprite icon)
    {
        icon = null;
        if (keyIconEntries == null) return false;

        for (int i = 0; i < keyIconEntries.Length; i++)
        {
            if (keyIconEntries[i].bindingPath == bindingPath)
            {
                icon = keyIconEntries[i].icon;
                return true;
            }
        }

        return false;
    }
}
