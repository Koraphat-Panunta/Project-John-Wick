using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMovementControllerScriptableObject", menuName = "ScriptableObjects/CharacterMovementControllerScriptableObject")]
public class CharacterMovementControllerScriptableObject : ScriptableObject
{
    public Vector3 centerOffsetPosition;
    public float slopeAngle;
    public float height;
    public float raduis;
    public float maxStepHeight = .35f;
}
