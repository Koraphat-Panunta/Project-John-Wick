using Sirenix.OdinInspector;
using UnityEngine;

public abstract class DataScriptableObject : ScriptableObject 
{
    [SerializeField, ReadOnly]
    private string objectID;
    public string ObjectID => objectID;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(objectID))
        {
            objectID = System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif
}
