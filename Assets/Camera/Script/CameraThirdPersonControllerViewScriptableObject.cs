using UnityEngine;


[CreateAssetMenu(fileName = "CameraThirdPersonView", menuName = "ScriptableObjects/CameraScriptableObject/CameraThirdPersonView")]
public class CameraThirdPersonControllerViewScriptableObject : ScriptableObject
{
    public Vector3 viewOffsetRight;
    [Range(45, 120)]
    public float fov;
    public AnimationCurve transitionCurve;

    [Range(0, 10)]
    public float transitionInSpeed;

    [Range(0, 1)]
    public float minNormalized;
    [Range(0, 1)]
    public float maxNormalized;

}
