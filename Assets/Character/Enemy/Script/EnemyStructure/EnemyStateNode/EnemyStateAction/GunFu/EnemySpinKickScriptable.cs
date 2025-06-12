using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpinKickScriptable", menuName = "ScriptableObjects/EnemySpinKick")]
public class EnemySpinKickScriptable : ScriptableObject
{
    public AnimationClip animationClip;

    [Range(0, 1)]
    public float _pushForwardTimeNormalized;
    [Range(0, 1)]
    public float _hitTimeEnterNormalized;
    [Range(0, 1)]
    public float _hitTimeExitNormalized;
    [Range(0, 1)]
    public float _onGroundTimeNormalized;
    [Range(0, 1)]
    public float _stopRotatingTimeNormalized;

    [Range(0, 50)]
    public float _pushSelfTowardForce;

    [Range(0, 100)]
    public float _spicKickRotateSpeed;

    [Range(0, 10)]
    public float _distanceCastVolume;

    [Range(0, 10)]
    public float _upperCastOffsetVolume;

    [Range(0, 10)]
    public float _raduisSphereVolume;

    [Range(0, 100)]
    public float _stopingForceOnGround;

    [Range(0, 100)]
    public float _stopForceBeginStance;

    [Range(0, 100)]
    public float _targetPushingForce;
}
