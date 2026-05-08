using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDecisionContextSCRP", menuName = "ScriptableObjects/Enemy/EnemyDecisionContextScriptableObject")]
public class EnemyDecisionContextScriptableObject : ScriptableObject
{
    [Range(0,50)]
    [SerializeField] public float raduisTargetZone;

    [Range(0, 100)]
    [SerializeField] public float lostSightTargetTime;
}
