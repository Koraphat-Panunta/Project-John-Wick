using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBrounceOffGotAttackGunFuScriptableObject", menuName = "ScriptableObjects/PlayerScriptable/PlayerBrounceOffGotAttackGunFuScriptableObject")]
public class PlayerBrounceOffGotAttackGunFuScriptableObject : ScriptableObject
{
    public AnimationClip animationClip;
    [Range(0, 1)]
    public float onGroundNormalized;

    [Range(0, 100)]
    public float breakForcingOnGround;
}
