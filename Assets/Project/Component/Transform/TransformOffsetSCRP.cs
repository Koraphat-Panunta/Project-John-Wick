using UnityEngine;

[CreateAssetMenu(fileName = "TransformOffsetSCRP", menuName = "ScriptableObjects/TransformOffsetSCRP")]
public class TransformOffsetSCRP : ScriptableObject
{
    [SerializeField] public Vector3 postitionOffset;
    [SerializeField] public Vector3 rotationEulerOffset;
}
