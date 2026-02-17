using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataScriptableObject", menuName = "ScriptableObjects/LevelDataScriptableObject")]
public class LevelDataScriptableObject : ScriptableObject
{
    public string levelID;
    private string LevelID => this.levelID;

    public string levelName;
    public int checkPoints;

#if UNITY_EDITOR

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(levelID))
        {
            string path = AssetDatabase.GetAssetPath(this);
            levelID = AssetDatabase.AssetPathToGUID(path);
            EditorUtility.SetDirty(this);
        }
    }
#endif
}
