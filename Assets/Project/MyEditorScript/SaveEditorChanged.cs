using UnityEditor;
using UnityEngine;

public static class SaveEditorChanged 
{
   public static void SaveEditorChangedObject(Object objectChanged)
    {
        Undo.RecordObject(objectChanged, "Modify Property");
        EditorUtility.SetDirty(objectChanged);
    }
}
