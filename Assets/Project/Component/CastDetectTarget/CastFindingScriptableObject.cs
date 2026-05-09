using UnityEngine;

[CreateAssetMenu(fileName = "CastFindingSCRP", menuName = "ScriptableObjects/CastFindingScriptableObject")]
public class CastFindingScriptableObject : ScriptableObject
{
    [SerializeField] public LayerMask targetLayerMask;
    [SerializeField] public float castDistance;
    [SerializeField] public float casthalfAngleDegrees;

}
