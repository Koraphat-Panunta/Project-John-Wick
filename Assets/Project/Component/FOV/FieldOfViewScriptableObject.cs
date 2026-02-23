using UnityEngine;
[CreateAssetMenu(fileName = "FieldOfViewScriptableObject", menuName = "ScriptableObjects/FieldOfViewScriptableObject/FieldOfViewScriptableObject")]
public class FieldOfViewScriptableObject : ScriptableObject
{
    [SerializeField] public float distance;
    [SerializeField] public float angleInDegrees;
    [SerializeField] public LayerMask collideLayerMask;
}
