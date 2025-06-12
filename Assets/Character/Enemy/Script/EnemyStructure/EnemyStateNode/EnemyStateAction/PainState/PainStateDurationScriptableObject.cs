using UnityEngine;

[CreateAssetMenu(fileName = "PainStateDuration", menuName = "ScriptableObjects/PainStateDuration")]
public class PainStateDurationScriptableObject : ScriptableObject
{
    public float head_LightHit;
    public float head_HeavyHit;

    public float bodyFront_LightHit;
    public float bodyFront_HeavyHit;
    public float bodyFornt_MediumHit;

    public float bodyBack_LightHit;
    public float bodyBack_HeavyHit;

    public float armLeft_LightHit;
    public float armLeft_HeavyHit;

    public float armRight_LightHit;
    public float armRight_HeavyHit;

    public float legLeft_LightHit;
    public float legLeft_HeavyHit;

    public float legRight_LightHit;
    public float legRight_HeavyHit;
}
