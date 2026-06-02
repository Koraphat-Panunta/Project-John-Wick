using UnityEngine;

[CreateAssetMenu(fileName = "RestrictScriptableObject", menuName = "ScriptableObjects/GunFuObject/RestrictScriptableObject")]
public class RestrictScriptableObject : ScriptableObject
{
    public string stateName;

    public AnimationInteractScriptableObject enterInteractSCRP;
    public AnimationInteractScriptableObject exitInteractSCRP;

    public Vector3 holdTargetOffset;
    public Vector3 holdRotationOffset;

    [Range(0, 50)]
    public float holdDuration;

}
