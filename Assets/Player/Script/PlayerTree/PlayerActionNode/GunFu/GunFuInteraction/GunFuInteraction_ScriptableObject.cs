using UnityEngine;

[CreateAssetMenu(fileName = "GunFuInteract", menuName = "ScriptableObjects/GunFuInteract")]
public class GunFuInteraction_ScriptableObject : ScriptableObject
{
    public float TransitionAbleTime_Normalized;
    public AnimationClip AinimnationClip;
    public string StateName;
}
