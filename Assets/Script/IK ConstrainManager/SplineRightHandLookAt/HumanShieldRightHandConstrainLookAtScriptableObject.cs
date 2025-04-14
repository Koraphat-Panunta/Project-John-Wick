using UnityEngine;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "HumanShieldRightHandConstrainLookAtScriptableObject",menuName = "ScriptableObjects/ConstrainObject/HumanShieldRightHandConstrainLookAtScriptableObject")]
public class HumanShieldRightHandConstrainLookAtScriptableObject : ScriptableObject
{
    
    [Range(0, 1)]
    [SerializeField] public float weightRightHandAim;
    public Vector3 offset_RightHandAim;

    [Range(0, 1)]
    [SerializeField] public float weightRightForeArmAim;
    public Vector3 offset_RightForeArmAim;

    [Range(0, 1)]
    [SerializeField] public float weightRightArmAim;
    public Vector3 offset_RightArmAim;

    [Range(0, 500)]
    [SerializeField] public float offsetChangedRate;
}
