using UnityEngine;


[CreateAssetMenu(fileName = "CameraThirdPersonView", menuName = "ScriptableObjects/CameraScriptableObject/CameraThirdPersonView")]
public class CameraThirdPersonControllerViewScriptableObject : ScriptableObject
{
    public Vector3 viewOffsetRight;
    [Range(45, 120)]
    public float fov;
    public AnimationCurve transitionCurve;

}
