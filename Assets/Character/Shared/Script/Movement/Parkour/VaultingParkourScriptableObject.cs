using UnityEngine;

[CreateAssetMenu(fileName = "VaultingParkourScriptableObject", menuName = "ScriptableObjects/Parkour/VaultingParkourScriptableObject")]
public class VaultingParkourScriptableObject : ParkourScriptableObject
{
    [Range(-10, 10)]
    public float forwardControlPoint_2_offset;
    [Range(-10, 10)]
    public float upWardControlPoint_2_offset;

    [Range(0, 10)]
    public float vaultingLenght;
}
