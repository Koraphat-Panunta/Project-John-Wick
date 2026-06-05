using UnityEngine;

[CreateAssetMenu(fileName = "CameraSnapLookScriptableObject",
    menuName = "ScriptableObjects/CameraScriptableObject/CameraSnapLookScriptableObject")]
public class CameraSnapLookScriptableObject : ScriptableObject
{
    [Range(0.01f, 5f)]
    public float duration = 0.3f;
    public AnimationCurve transitionCurve;
    public CameraThirdPersonControllerViewScriptableObject viewSCRP;
}
