using UnityEngine;

[CreateAssetMenu(fileName = "VaultingParkourScriptableObject", menuName = "ScriptableObjects/Parkour/VaultingParkourScriptableObject")]
public class VaultingParkourScriptableObject : ParkourScriptableObject
{
    [Range(-10, 10)]
    public float forwardControlPoint_2_offset;
    [Range(-10, 10)]
    public float rightwardControlPoint_2_offset;
}
