using UnityEngine;

[CreateAssetMenu(fileName = "HeadLookConstrainScriptableObject", menuName = "ScriptableObjects/ConstrainObject/HeadLookConstrainScriptableObject")]
public class HeadLookConstrainScriptableObject : ScriptableObject
{
    [Range(0, 1)]
    [SerializeField] public float weight;
    public Vector3 offsetLook;

    [Range(0, 500)]
    [SerializeField] public float offsetChangedRate;
}
