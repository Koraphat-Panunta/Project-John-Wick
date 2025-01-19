using UnityEngine;

[CreateAssetMenu(fileName = "GunFuGotHitNode", menuName = "ScriptableObjects/GunFuGotHitNode")]
public class GunFu_GotHit_ScriptableObject : ScriptableObject
{
    public AnimationClip AnimationClip;
    public float ExitTime_Normalized;
    public string StateName;
}
