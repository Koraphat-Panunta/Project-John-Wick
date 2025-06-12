using UnityEngine;

[CreateAssetMenu(fileName = "GotRestrictScriptableObject", menuName = "ScriptableObjects/GunFuObject/GotRestrictScriptableObject")]
public class GotRestrictScriptableObject : ScriptableObject
{
    public AnimationClip gotRestrictExitClip;

    [Range(0, 1)]
    public float gotRestrictEnter_enterNormalized;

    [Range(0, 1)]
    public float gotRestrictEnter_exitNormalized;

    [Range(0, 1)]
    public float gotRestrictExit_enterNormalized;

    [Range(0, 1)]
    public float gotRestrictExit_exitNormalized;

    [Range(0, 50)]
    public float gotRestrictExit_BreakForce;


}
