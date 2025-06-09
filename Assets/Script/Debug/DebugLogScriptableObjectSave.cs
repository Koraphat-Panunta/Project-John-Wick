using UnityEngine;

[CreateAssetMenu(fileName = "DebugLog", menuName = "Debug/Log Scriptable Object")]
public class DebugLogScriptableObjectSave : ScriptableObject
{
    [SerializeField, TextArea(10, 20)] public string debugLog;
}
