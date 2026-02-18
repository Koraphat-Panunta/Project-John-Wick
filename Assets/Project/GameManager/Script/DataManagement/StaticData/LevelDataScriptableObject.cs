using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataScriptableObject", menuName = "ScriptableObjects/LevelDataScriptableObject")]
public class LevelDataScriptableObject : DataScriptableObject
{
   

    public string levelName;
    public int checkPoints;


}
