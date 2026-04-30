using UnityEngine;

[CreateAssetMenu(fileName = "ClimbParkourScriptableObject", menuName = "ScriptableObjects/Parkour/ClimbParkourScriptableObject")]
public class ClimbParkourScriptableObject : ParkourScriptableObject
{
    [Range(-10, 10)]
    public float forwardStartClimbPoint_offset;
    [Range(-10, 10)]
    public float upWardStartClimbPoint_offset;

    [Range(0, 1)]
    public float catchEdgeTimeNormalized;
}
