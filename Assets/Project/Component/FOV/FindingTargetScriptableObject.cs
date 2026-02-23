using UnityEngine;
[CreateAssetMenu(fileName = "FindingTargetScriptableObject", menuName = "ScriptableObjects/FieldOfViewScriptableObject/FindingTargetScriptableObject")]
public class FindingTargetScriptableObject : FieldOfViewScriptableObject
{
    [SerializeField] public LayerMask targetLayer;
}
