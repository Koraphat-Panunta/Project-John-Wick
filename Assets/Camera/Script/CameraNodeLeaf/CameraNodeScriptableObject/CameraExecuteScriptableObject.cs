using UnityEngine;

[CreateAssetMenu(fileName = "CameraExecuteScriptableObject",menuName = "ScriptableObjects/CameraScriptableObject/CameraExecuteScriptableObject")]
public class CameraExecuteScriptableObject : ScriptableObject
{
    [Range(0, 1)]
    public float weight;

    public Vector3 executorAnchorOffset;

    public Vector3 opponentExecutedAnchorOffset;

    public Vector3 cameraPosAnchorOffset;

    public Vector3 cameraOffset;

    [Range(0, 10)]
    public float transitionEnterDuration;

    [Range(0, 10)]
    public float transitionExitDuration;
}
