using UnityEngine;

[CreateAssetMenu(fileName = "ClimbParkourScriptableObject", menuName = "ScriptableObjects/Parkour/ClimbParkourScriptableObject")]
public class ParkourScriptableObject : ScriptableObject
{
    [Range(-10, 10)]
    public float forWardControlPoint_1_offset;
    [Range(-10, 10)]
    public float upWardControlPoint_1_offset;

    [Range(-10, 10)]
    public float forwardExitPoint_offset;
    [Range(-10, 10)]
    public float upWardExitPoint_offset;

    [Range(0, 10)]
    public float speed;

    [Range(0, 10)]
    public float hieght;

    public AnimationCurve curve;
    public AnimationClip clip;
    public string stateName;

}
