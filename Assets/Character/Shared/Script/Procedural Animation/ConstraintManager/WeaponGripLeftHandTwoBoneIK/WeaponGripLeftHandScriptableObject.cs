using UnityEngine;

[CreateAssetMenu(fileName = "WeaponGripLeftHandScriptableObject", menuName = "ScriptableObjects/ConstrainObject/WeaponGripLeftHandScriptableObject")]
public class WeaponGripLeftHandScriptableObject : ScriptableObject
{
    [SerializeField] public Vector3 hintTargetPositionAdditionOffset;

    [SerializeField] public Vector3 leftHandGripPositionOffset;
    [SerializeField] public Vector3 leftHandGripRotationOffset;
}